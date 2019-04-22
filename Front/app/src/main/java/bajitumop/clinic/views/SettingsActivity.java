package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.google.gson.Gson;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.User;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class SettingsActivity extends BaseActivity {
    TextView loginTitleTextView;

    EditText firstNameEditText;
    EditText secondNameEditText;
    EditText thirdNameEditText;
    EditText phoneEditText;
    EditText usernameEditText;
    EditText passwordEditText;

    Button submitButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_settings);

        loginTitleTextView = findViewById(R.id.loginTitleTextView);

        firstNameEditText = findViewById(R.id.firstNameEditText);
        secondNameEditText = findViewById(R.id.secondNameEditText);
        thirdNameEditText = findViewById(R.id.thirdNameEditText);
        usernameEditText = findViewById(R.id.usernameEditText);
        passwordEditText = findViewById(R.id.passwordEditText);

        submitButton = findViewById(R.id.submitButton);
        submitButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                submit();
            }
        });

        Intent intent = getIntent();
        String serializedUser = intent.getStringExtra("user");
        User user = new Gson().fromJson(serializedUser, User.class);

        if (user.getFirstName() != null) { firstNameEditText.setText(user.getFirstName()); }
        if (user.getSecondName() != null) { secondNameEditText.setText(user.getSecondName()); }
        if (user.getThirdName() != null) { thirdNameEditText.setText(user.getThirdName()); }
        if (user.getUsername() != null) { usernameEditText.setText(user.getUsername()); }
        if (user.getPasswordHash() != null) { firstNameEditText.setText(user.getFirstName()); }
    }

    private void submit() {
        submitButton.setEnabled(false);
        User user = new User();
        user.setFirstName(firstNameEditText.getText().toString());
        user.setSecondName(secondNameEditText.getText().toString());
        user.setThirdName(thirdNameEditText.getText().toString());
        user.setUsername(usernameEditText.getText().toString());
        user.setPasswordHash(firstNameEditText.getText().toString());

        clinicApi.updateUser(user)
                .enqueue(new Callback<ApiResult>() {
                    @Override
                    public void onResponse(@NonNull Call<ApiResult> call, @NonNull Response<ApiResult> response) {
                        ApiResult body = response.body();
                        if (body.isSuccess()) {
                            snackbar(submitButton, "success");
                        } else {
                            snackbar(submitButton, body.getMessage());
                        }
                        submitButton.setEnabled(true);
                        // more logic
                    }

                    @Override
                    public void onFailure(@NonNull Call<ApiResult> call, @NonNull Throwable t) {
                        snackbar(submitButton, "Something went wrong");
                        submitButton.setEnabled(true);
                    }
                });
    }
}
