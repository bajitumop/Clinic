package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.Empty;
import bajitumop.clinic.models.VisitModel;
import bajitumop.clinic.services.DateTime;
import bajitumop.clinic.services.network.IOnResponseCallback;

public class VisitActivity extends BaseActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getSupportActionBar().setTitle(R.string.details_activity_title);
        setContentView(R.layout.activity_visit);
        Intent intent = getIntent();
        final VisitModel visit = (VisitModel) intent.getSerializableExtra("visit");

        ((TextView)findViewById(R.id.dateTime)).setText(DateTime.formatFullDate(visit.getDateTime()));
        ((TextView)findViewById(R.id.specialty)).setText(String.format("Специальность: %s", visit.getSpecialty()));
        ((TextView)findViewById(R.id.serviceDescription)).setText(visit.getServiceDescription());
        ((TextView)findViewById(R.id.doctor)).setText(String.format("Врач: %s", visit.getFullDoctorName()));

        findViewById(R.id.deleteVisit).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                sendRequest(clinicApi.DeleteVisit(visit.getId()), new IOnResponseCallback<Empty>() {
                    @Override
                    public void onResponse(ApiResult<Empty> result) {
                        if (result.isSuccess()){
                            finish();
                        } else {
                            snackbar(findViewById(R.id.deleteVisit), result.getMessage());
                        }
                    }
                });
            }
        });
    }
}
