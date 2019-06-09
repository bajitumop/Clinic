package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import java.text.SimpleDateFormat;
import java.util.Locale;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.Empty;
import bajitumop.clinic.models.VisitModel;
import bajitumop.clinic.services.network.IOnResponseCallback;

public class VisitActivity extends BaseActivity {

    private VisitModel visit;
    private static final SimpleDateFormat dateTimeFormat = new SimpleDateFormat("dd.MM.yyyy в HH:mm", Locale.getDefault());

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_visit);
        Intent intent = getIntent();
        visit = (VisitModel) intent.getSerializableExtra("visit");
        initFields();
    }

    private void initFields(){
        ((TextView)findViewById(R.id.dateTime)).setText(dateTimeFormat.format(visit.getDateTime()));
        ((TextView)findViewById(R.id.specialty)).setText(String.format("Специальность: %s", visit.getSpecialty()));
        ((TextView)findViewById(R.id.serviceDescription)).setText(visit.getServiceDescription());
        ((TextView)findViewById(R.id.doctor)).setText(String.format(
                "Врач: %s %s %s", visit.getDoctorSecondName(),
                visit.getDoctorFirstName(),
                visit.getDoctorThirdName()));

        findViewById(R.id.deleteVisit).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deleteVisit();
            }
        });
    }

    private void deleteVisit(){
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
}
