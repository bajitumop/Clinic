package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

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

        User user = getUser();
        if (user == null) {
            finish();
            return;
        }

        if (user.getFirstName() != null) { firstNameEditText.setText(user.getFirstName()); }
        if (user.getSecondName() != null) { secondNameEditText.setText(user.getSecondName()); }
        if (user.getThirdName() != null) { thirdNameEditText.setText(user.getThirdName()); }
        if (user.getUsername() != null) { usernameEditText.setText(user.getUsername()); }
        if (user.getPasswordHash() != null) { firstNameEditText.setText(user.getFirstName()); }
    }

    private void submit() {
        submitButton.setEnabled(false);
        final User user = new User();
        user.setFirstName(firstNameEditText.getText().toString());
        user.setSecondName(secondNameEditText.getText().toString());
        user.setThirdName(thirdNameEditText.getText().toString());
        user.setUsername(usernameEditText.getText().toString());
        user.setPasswordHash(firstNameEditText.getText().toString());
        user.setAccessToken(getUser().getAccessToken());

        clinicApi.updateUser(user)
                .enqueue(new Callback<ApiResult>() {
                    @Override
                    public void onResponse(@NonNull Call<ApiResult> call, @NonNull Response<ApiResult> response) {
                        onUserUpdateResponse(response.body(), user);
                        submitButton.setEnabled(true);
                    }

                    @Override
                    public void onFailure(@NonNull Call<ApiResult> call, @NonNull Throwable t) {
                        submitButton.setEnabled(true);
                    }
                });
    }

    private void onUserUpdateResponse(ApiResult result, User user){
        if (result == null) {
            onUserUpdateFailure();
        } else if (result.isSuccess()) {
            updateUser(user);
            snackbar(submitButton, getString(R.string.successfully_user_update));
        } else {
            snackbar(submitButton, result.getMessage());
        }
    }

    private void onUserUpdateFailure(){
        snackbar(submitButton, getString(R.string.something_went_wrong));
    }
}
