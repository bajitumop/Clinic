package bajitumop.clinic.views.MainFragments;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import java.util.List;
import bajitumop.clinic.R;
import bajitumop.clinic.models.VisitModel;
import bajitumop.clinic.services.DateTime;

public class VisitsListAdapter extends RecyclerView.Adapter<VisitsListAdapter.ViewHolder> {

        private final VisitsFragment.IVisitsListInteractionListener listener;
        private List<VisitModel> visits;

        public VisitsListAdapter(List<VisitModel> visits, VisitsFragment.IVisitsListInteractionListener listener) {
            this.visits = visits;
            this.listener = listener;
        }

        @Override
        public VisitsListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_visit_item, parent, false);
            return new VisitsListAdapter.ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(final VisitsListAdapter.ViewHolder holder, int position) {
            holder.setService(visits.get(position));
            holder.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (listener != null) {
                        listener.onVisitClick(holder.getVisit());
                    }
                }
            });
        }

        @Override
        public int getItemCount() {
            return visits.size();
        }

        public class ViewHolder extends RecyclerView.ViewHolder {
            private View mView;
            TextView serviceDescription;
            TextView dateTime;
            private VisitModel visit;

            public ViewHolder(View view) {
                super(view);
                mView = view;
                serviceDescription = view.findViewById(R.id.serviceDescription);
                dateTime = view.findViewById(R.id.dateTime);
            }

            public void setService(VisitModel visit){
                this.visit = visit;
                serviceDescription.setText(visit.getServiceDescription());
                dateTime.setText(DateTime.formatFullDate(visit.getDateTime()));
            }

            public VisitModel getVisit() {
                return visit;
            }

            public void setOnClickListener(View.OnClickListener clickListener) {
                mView.setOnClickListener(clickListener);
            }
        }
    }
