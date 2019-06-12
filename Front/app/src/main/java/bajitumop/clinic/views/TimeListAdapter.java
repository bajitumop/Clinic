package bajitumop.clinic.views;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.List;

import bajitumop.clinic.R;
import bajitumop.clinic.models.VisitInfoStatusModel;
import bajitumop.clinic.models.VisitStatus;
import bajitumop.clinic.services.DateTime;

public class TimeListAdapter extends RecyclerView.Adapter<TimeListAdapter.ViewHolder> {

    private Context context;
    private final OnTimeClickListener listener;
    private List<VisitInfoStatusModel> visitStatuses;

    public TimeListAdapter(List<VisitInfoStatusModel> visits, OnTimeClickListener listener, Context context) {
        this.visitStatuses = visits;
        this.context = context;
        this.listener = listener;
    }

    @Override
    public TimeListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_time_item, parent, false);
        return new TimeListAdapter.ViewHolder(view, context);
    }

    @Override
    public void onBindViewHolder(final TimeListAdapter.ViewHolder holder, int position) {
        holder.setService(visitStatuses.get(position));
        holder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (listener != null) {
                    listener.onTimeClick(holder.getVisit());
                }
            }
        });
    }

    @Override
    public int getItemCount() { return visitStatuses.size(); }

    public class ViewHolder extends RecyclerView.ViewHolder {
        private Context context;
        private View mView;
        private TextView time;
        private VisitInfoStatusModel visit;

        public ViewHolder(View view, Context context) {
            super(view);
            mView = view;
            this.context = context;
            time = view.findViewById(R.id.time);
        }

        public void setService(VisitInfoStatusModel visit){
            this.visit = visit;
            time.setText(DateTime.formatTime(visit.getDate()));
            boolean openedStatus = visit.getStatus() == VisitStatus.Opened;
            mView.setEnabled(openedStatus);
            time.setBackgroundColor(context.getResources().getColor(openedStatus ? R.color.default_background : R.color.disabled_background));
            time.setTextColor(context.getResources().getColor(openedStatus ? R.color.default_text : R.color.disabled_text));
        }

        public VisitInfoStatusModel getVisit() {
            return visit;
        }

        public void setOnClickListener(View.OnClickListener clickListener) {
            mView.setOnClickListener(clickListener);
        }
    }
}
