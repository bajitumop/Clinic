package bajitumop.clinic.views;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;

import bajitumop.clinic.R;
import bajitumop.clinic.models.DoctorModel;
import bajitumop.clinic.services.network.NetworkService;

public class DoctorActivity extends BaseActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getSupportActionBar().setTitle(R.string.details_activity_title);
        setContentView(R.layout.activity_doctor);
        Intent intent = getIntent();
        final DoctorModel doctor = (DoctorModel)intent.getSerializableExtra("doctor");

        ((TextView)findViewById(R.id.name)).setText(doctor.getFullName());
        ((TextView)findViewById(R.id.specialty)).setText(doctor.getSpecialty());
        ((TextView)findViewById(R.id.info)).setText(doctor.getInfo());

        findViewById(R.id.makeRecord).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(DoctorActivity.this, RecordActivity.class);
                intent.putExtra("doctorId", doctor.getId());
                intent.putExtra("specialty", doctor.getSpecialty());
                startActivity(intent);
                finish();
            }
        });

        Glide.with(this)
                .applyDefaultRequestOptions(new RequestOptions()
                        .placeholder(R.drawable.ic_doctors_48dp)
                        .error(R.drawable.ic_doctors_48dp))
                .load(NetworkService.HOST + doctor.getImageUrl())
                .apply(RequestOptions.circleCropTransform())
                .into(((ImageView)findViewById(R.id.image)));
    }
}
