package bajitumop.clinic.views.MainFragments;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import java.util.ArrayList;
import java.util.Arrays;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorShortModel;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.views.BaseListFragment;

public class DoctorsFragment extends BaseListFragment<DoctorsFragment.IDoctorsListInteractionListener> {

    private DoctorsListAdapter adapter;
    private ArrayList<DoctorShortModel> doctors = new ArrayList<>();

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_doctors_list, container, false);

        Context context = view.getContext();
        RecyclerView recyclerView = view.findViewById(R.id.doctorsList);
        recyclerView.setLayoutManager(new LinearLayoutManager(context));
        adapter = new DoctorsListAdapter(context, doctors, listener);
        recyclerView.setAdapter(adapter);

        return view;
    }

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        setProgress();
        sendRequest(clinicApi.getDoctors(), new IOnResponseCallback<DoctorShortModel[]>() {
            @Override
            public void onResponse(ApiResult<DoctorShortModel[]> result) {
                if (result.isSuccess()) {
                    doctors.clear();
                    doctors.addAll(Arrays.asList(result.getData()));
                    adapter.notifyDataSetChanged();
                    setContent();
                } else {
                    setConnectionError();
                }
            }
        });
    }

    public interface IDoctorsListInteractionListener {
        void onDoctorClick(DoctorShortModel doctor);
    }
}
