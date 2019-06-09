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
import bajitumop.clinic.models.ServiceModel;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.views.BaseListFragment;

public class ServicesFragment extends BaseListFragment<ServicesFragment.IServicesListInteractionListener> {

    private ServicesListAdapter adapter;
    private ArrayList<ServiceModel> services = new ArrayList<>();

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_services_list, container, false);

        Context context = view.getContext();
        RecyclerView recyclerView = view.findViewById(R.id.servicesList);
        recyclerView.setLayoutManager(new LinearLayoutManager(context));
        adapter = new ServicesListAdapter(context, services, listener);
        recyclerView.setAdapter(adapter);

        return view;
    }

    @Override
    public void onResume() {
        super.onResume();
        reload();
    }

    private void reload() {
        setProgress();
        sendRequest(clinicApi.getServices(), new IOnResponseCallback<ServiceModel[]>() {
            @Override
            public void onResponse(ApiResult<ServiceModel[]> result) {
                if (result.isSuccess()) {
                    services.clear();
                    services.addAll(Arrays.asList(result.getData()));
                    adapter.notifyDataSetChanged();
                    setContent();
                } else {
                    setConnectionError();
                }
            }
        });
    }

    public interface IServicesListInteractionListener {
        void onServiceClick(ServiceModel service);
    }
}
