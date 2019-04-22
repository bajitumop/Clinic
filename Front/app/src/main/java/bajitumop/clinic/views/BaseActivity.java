package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.view.View;

import bajitumop.clinic.models.User;
import bajitumop.clinic.services.network.IClinicApi;
import bajitumop.clinic.services.network.NetworkService;
import bajitumop.clinic.services.network.UserStorage;

public abstract class BaseActivity extends AppCompatActivity {
    protected IClinicApi clinicApi = NetworkService.Create();
    private UserStorage userStorage;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        userStorage = UserStorage.Create(getApplicationContext());
    }

    protected void snackbar(View v, String text, int time) {
        Snackbar.make(v, text, time).show();
    }

    protected void snackbar(View v, String text) {
        snackbar(v, text, Snackbar.LENGTH_LONG);
    }

    protected User getUser() {
        return userStorage.getUser();
    }

    protected void updateUser(User user) {
        userStorage.updateUser(user);
    }
}
