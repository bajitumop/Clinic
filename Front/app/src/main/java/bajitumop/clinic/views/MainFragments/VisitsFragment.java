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
import bajitumop.clinic.models.VisitModel;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.views.BaseListFragment;

public class VisitsFragment extends BaseListFragment<VisitsFragment.IVisitsListInteractionListener> {

    private VisitsListAdapter adapter;
    private ArrayList<VisitModel> visits = new ArrayList<>();

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_visits_list, container, false);

        Context context = view.getContext();
        RecyclerView recyclerView = view.findViewById(R.id.visitsList);
        recyclerView.setLayoutManager(new LinearLayoutManager(context));
        adapter = new VisitsListAdapter(context, visits, listener);
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
        sendRequest(clinicApi.getVisits(), new IOnResponseCallback<VisitModel[]>() {
            @Override
            public void onResponse(ApiResult<VisitModel[]> result) {
                if (result.isSuccess()) {
                    visits.clear();
                    visits.addAll(Arrays.asList(result.getData()));
                    adapter.notifyDataSetChanged();
                    setContent();
                } else {
                    setConnectionError();
                }
            }
        });
    }

    public interface IVisitsListInteractionListener {
            void onVisitClick(VisitModel visit);
        }
    }
