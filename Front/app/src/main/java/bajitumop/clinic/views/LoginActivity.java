package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.LoginModel;
import bajitumop.clinic.models.LoginResponse;
import bajitumop.clinic.models.RegistrationModel;
import bajitumop.clinic.models.User;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.services.Sha256HashAlgorithm;

public class LoginActivity extends BaseActivity {

    private Sha256HashAlgorithm hashAlgorithm = Sha256HashAlgorithm.Create();

    private TextView loginTitle;
    private TextView loginSecondPage;
    private EditText usernameEditText;
    private EditText passwordEditText;
    private EditText firstNameEditText;
    private EditText secondNameEditText;
    private EditText thirdNameEditText;
    private Button submitButton;
    private LinearLayout additionalBlockLinearLayout;

    private boolean isLogin = true;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        loginTitle = findViewById(R.id.loginTitle);
        loginSecondPage = findViewById(R.id.loginSecondPage);
        usernameEditText = findViewById(R.id.usernameEditText);
        passwordEditText = findViewById(R.id.passwordEditText);
        firstNameEditText = findViewById(R.id.firstNameEditText);
        secondNameEditText = findViewById(R.id.secondNameEditText);
        thirdNameEditText = findViewById(R.id.thirdNameEditText);
        submitButton = findViewById(R.id.loginSubmitButton);
        additionalBlockLinearLayout = findViewById(R.id.additional_block);

        loginSecondPage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                toggleView(!isLogin);
            }
        });

        submitButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onSubmit();
            }
        });

        toggleView(isLogin);
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (getUser() != null) {
            startMainActivity();
        }
    }

    private void toggleView(boolean isLogin){
        loginTitle.setText(getString(isLogin ? R.string.enter : R.string.registration));
        loginSecondPage.setText(getString(isLogin ? R.string.registration : R.string.enter));

        additionalBlockLinearLayout.setVisibility(isLogin ? View.GONE : View.VISIBLE);
        submitButton.setText(getString(isLogin ? R.string.login : R.string.register));
        this.isLogin = isLogin;
    }

    private void onSubmit(){
        submitButton.setEnabled(false);
        final String username = usernameEditText.getText().toString();
        final String password = passwordEditText.getText().toString();
        final String hash = hashAlgorithm.hash(password);

        if (isLogin) {
            LoginModel loginModel = new LoginModel(username, hash);
            sendRequest(clinicApi.login(loginModel), new IOnResponseCallback<LoginResponse>() {
                @Override
                public void onResponse(ApiResult<LoginResponse> result) {
                    if (result.isSuccess()) {
                        LoginResponse response = result.getData();
                        User user = new User();
                        user.setUsername(username);
                        user.setPasswordHash(hash);
                        user.setAccessToken(response.getAccessToken());
                        user.setFirstName(response.getFirstName());
                        user.setSecondName(response.getSecondName());
                        user.setThirdName(response.getThirdName());
                        updateUser(user);
                        startMainActivity();
                    } else {
                        snackbar(submitButton, result.getMessage());
                    }

                    submitButton.setEnabled(true);
                }
            });

        } else {
            final String firstName = firstNameEditText.getText().toString();
            final String secondName = secondNameEditText.getText().toString();
            final String thirdName = thirdNameEditText.getText().toString();
            RegistrationModel registrationModel = new RegistrationModel(username, hash, firstName, secondName, thirdName);

            sendRequest(clinicApi.register(registrationModel), new IOnResponseCallback<String>() {
                @Override
                public void onResponse(ApiResult<String> result) {
                    if (result.isSuccess()) {
                        String accessToken = result.getData();
                        User user = new User();
                        user.setUsername(username);
                        user.setPasswordHash(hash);
                        user.setAccessToken(accessToken);
                        user.setFirstName(firstName);
                        user.setSecondName(secondName);
                        user.setThirdName(thirdName);
                        updateUser(user);
                        startMainActivity();
                    } else {
                        snackbar(submitButton, result.getMessage());
                    }

                    submitButton.setEnabled(true);
                }
            });
        }
    }

    private void startMainActivity() {
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
    }
}
