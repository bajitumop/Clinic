package bajitumop.clinic.views.MainFragments;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import java.util.ArrayList;
import java.util.Arrays;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorModel;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.views.BaseListFragment;
import bajitumop.clinic.views.MainActivity;

public class DoctorsFragment extends BaseListFragment<DoctorsFragment.IDoctorsListInteractionListener> {

    private DoctorsListAdapter adapter;
    private ArrayList<DoctorModel> doctors = new ArrayList<>();

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        ((MainActivity)getActivity()).getSupportActionBar().setTitle(R.string.doctors_title);
        View view = inflater.inflate(R.layout.fragment_doctors_list, container, false);

        Context context = view.getContext();
        RecyclerView recyclerView = view.findViewById(R.id.doctorsList);
        recyclerView.setLayoutManager(new LinearLayoutManager(context));
        adapter = new DoctorsListAdapter(context, doctors, listener);
        recyclerView.setAdapter(adapter);

        return view;
    }

    @Override
    public void onResume() {
        super.onResume();
        setProgress();
        sendRequest(clinicApi.getDoctors(), new IOnResponseCallback<DoctorModel[]>() {
            @Override
            public void onResponse(ApiResult<DoctorModel[]> result) {
                if (result.isSuccess()) {
                    DoctorModel[] data = result.getData();
                    doctors.clear();
                    doctors.addAll(Arrays.asList(data));
                    adapter.notifyDataSetChanged();
                    if (data.length == 0) {
                        setEmpty();
                    } else {
                        setContent();
                    }
                } else {
                    setConnectionError();
                }
            }
        });
    }

    public interface IDoctorsListInteractionListener {
        void onDoctorClick(DoctorModel doctor);
    }
}
