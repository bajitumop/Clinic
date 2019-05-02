package bajitumop.clinic.views.MainFragments;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;

import bajitumop.clinic.R;
import bajitumop.clinic.models.DoctorShortModel;
import bajitumop.clinic.services.network.NetworkService;

import java.util.List;

public class DoctorsListAdapter extends RecyclerView.Adapter<DoctorsListAdapter.ViewHolder> {

    private final Context context;
    private final DoctorsFragment.IDoctorsListInteractionListener listener;
    private List<DoctorShortModel> doctors;

    public DoctorsListAdapter(Context context, List<DoctorShortModel> doctors, DoctorsFragment.IDoctorsListInteractionListener listener) {
        this.context = context;
        this.doctors = doctors;
        this.listener = listener;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_doctor_item, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(final ViewHolder holder, int position) {
        holder.setDoctor(doctors.get(position), context);
        holder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (listener != null) {
                    listener.onDoctorClick(holder.getDoctor());
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return doctors.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        private View mView;
        TextView doctorName;
        TextView doctorPositions;
        ImageView doctorImage;
        private DoctorShortModel doctor;

        public ViewHolder(View view) {
            super(view);
            mView = view;
            doctorName = view.findViewById(R.id.doctorName);
            doctorPositions = view.findViewById(R.id.doctorPositions);
            doctorImage = view.findViewById(R.id.doctorImage);
        }

        public void setDoctor(DoctorShortModel doctor, Context context){
            this.doctor = doctor;
            doctorName.setText(String.format("%s %s %s", doctor.getSecondName(), doctor.getFirstName(), doctor.getThirdName()));
            doctorPositions.setText(doctor.getSpecialties()[0]);
            Glide.with(context)
                    .applyDefaultRequestOptions(new RequestOptions()
                    .placeholder(R.drawable.ic_doctor_48dp)
                    .error(R.drawable.ic_doctor_48dp))
                    .load(NetworkService.HOST + doctor.getImageUrl())
                    .apply(RequestOptions.circleCropTransform())
                    .into(doctorImage);
        }

        public DoctorShortModel getDoctor() {
            return doctor;
        }

        public void setOnClickListener(View.OnClickListener clickListener) {
            mView.setOnClickListener(clickListener);
        }
    }
}
