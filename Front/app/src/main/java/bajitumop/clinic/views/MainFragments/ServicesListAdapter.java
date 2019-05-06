package bajitumop.clinic.views.MainFragments;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.List;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ServiceModel;

public class ServicesListAdapter extends RecyclerView.Adapter<ServicesListAdapter.ViewHolder> {

    private final Context context;
    private final ServicesFragment.IServicesListInteractionListener listener;
    private List<ServiceModel> services;

    public ServicesListAdapter(Context context, List<ServiceModel> services, ServicesFragment.IServicesListInteractionListener listener) {
        this.context = context;
        this.services = services;
        this.listener = listener;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_service_item, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(final ViewHolder holder, int position) {
        holder.setService(services.get(position));
        holder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (listener != null) {
                    listener.onServiceClick(holder.getService());
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return services.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        private View mView;
        TextView serviceDescription;
        private ServiceModel service;

        public ViewHolder(View view) {
            super(view);
            mView = view;
            serviceDescription = view.findViewById(R.id.serviceDescription);
        }

        public void setService(ServiceModel service){
            this.service = service;
            serviceDescription.setText(service.getDescription());
        }

        public ServiceModel getService() {
            return service;
        }

        public void setOnClickListener(View.OnClickListener clickListener) {
            mView.setOnClickListener(clickListener);
        }
    }
}
