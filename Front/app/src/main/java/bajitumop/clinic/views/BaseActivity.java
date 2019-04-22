package bajitumop.clinic.views;

import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.view.View;

import bajitumop.clinic.services.network.IClinicApi;
import bajitumop.clinic.services.network.NetworkService;

public abstract class BaseActivity extends AppCompatActivity {
    protected IClinicApi clinicApi = NetworkService.Create();

    protected void snackbar(View v, String text, int time) {
        Snackbar.make(v, text, time).show();
    }

    protected void snackbar(View v, String text) {
        snackbar(v, text, Snackbar.LENGTH_LONG);
    }
}
