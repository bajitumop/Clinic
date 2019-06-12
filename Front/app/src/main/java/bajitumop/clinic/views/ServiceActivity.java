package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.TextView;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ServiceModel;

public class ServiceActivity extends BaseActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getSupportActionBar().setTitle(R.string.details_activity_title);
        setContentView(R.layout.activity_service);
        Intent intent = getIntent();
        final ServiceModel service = (ServiceModel)intent.getSerializableExtra("service");

        ((TextView)findViewById(R.id.description)).setText(service.getDescription());
        ((TextView)findViewById(R.id.additionalInfo)).setText(service.getAdditionalInfo());
        ((TextView)findViewById(R.id.specialty)).setText(service.getSpecialty());
        ((TextView)findViewById(R.id.price)).setText(String.format("%.0f \u20BD", service.getPrice()));

        findViewById(R.id.makeRecord).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(ServiceActivity.this, RecordActivity.class);
                intent.putExtra("serviceId", service.getId());
                intent.putExtra("specialty", service.getSpecialty());
                startActivity(intent);
                finish();
            }
        });
    }
}
