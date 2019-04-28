package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;

import bajitumop.clinic.services.network.UserStorage;

public class StartActivity extends AppCompatActivity {
    private final static int LOGIN_REQUEST_CODE = 467;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        UserStorage userStorage = UserStorage.Create(getApplicationContext());
        if (userStorage.getAccessToken() != null) {
            startMainActivity();
        } else {
            Intent intent = new Intent(this, LoginActivity.class);
            startActivityForResult(intent, LOGIN_REQUEST_CODE);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        if (requestCode != LOGIN_REQUEST_CODE) {
            return;
        }

        if (resultCode == RESULT_OK) {
            startMainActivity();
            return;
        }

        finish();
    }

    private void startMainActivity() {
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }
}
