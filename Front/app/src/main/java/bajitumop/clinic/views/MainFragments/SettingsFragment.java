package bajitumop.clinic.views.MainFragments;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.Empty;
import bajitumop.clinic.models.User;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.services.security.Sha256HashAlgorithm;
import bajitumop.clinic.views.BaseFragment;

public class SettingsFragment extends BaseFragment {
    private Sha256HashAlgorithm hashAlgorithm = Sha256HashAlgorithm.Create();
    private IUserStorageProvider userStorageProvider;

    private EditText firstNameEditText;
    private EditText secondNameEditText;
    private EditText thirdNameEditText;
    private EditText passwordEditText;
    private EditText usernameEditText;
    private String lastAccessToken;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_settings, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        firstNameEditText = view.findViewById(R.id.firstNameEditText);
        secondNameEditText = view.findViewById(R.id.secondNameEditText);
        thirdNameEditText = view.findViewById(R.id.thirdNameEditText);
        passwordEditText = view.findViewById(R.id.passwordEditText);
        usernameEditText = view.findViewById(R.id.usernameEditText);

        User user = userStorageProvider.readUser();

        if (user.getFirstName() != null) { firstNameEditText.setText(user.getFirstName()); }
        if (user.getSecondName() != null) { secondNameEditText.setText(user.getSecondName()); }
        if (user.getThirdName() != null) { thirdNameEditText.setText(user.getThirdName()); }
        if (user.getUsername() != null) { usernameEditText.setText(user.getUsername()); }
        if (user.getPasswordHash() != null) { firstNameEditText.setText(user.getFirstName()); }
        if (user.getAccessToken() != null) { lastAccessToken = user.getAccessToken(); }

        Button submitButton = view.findViewById(R.id.submitButton);
        submitButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveUser();
            }
        });
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        userStorageProvider = (IUserStorageProvider) context;
    }

    @Override
    public void onDetach() {
        super.onDetach();
        userStorageProvider = null;
    }

    private void saveUser() {
        String passwordText = passwordEditText.getText().toString();
        String hash = hashAlgorithm.hash(passwordText);
        String firstName = firstNameEditText.getText().toString();
        String secondName = secondNameEditText.getText().toString();
        String thirdName = thirdNameEditText.getText().toString();
        String username = usernameEditText.getText().toString();
        final User user = new User();
        user.setPasswordHash(hash);
        user.setUsername(username);
        user.setFirstName(firstName);
        user.setSecondName(secondName);
        user.setThirdName(thirdName);
        user.setAccessToken(lastAccessToken);

        sendRequest(clinicApi.updateUser(user), new IOnResponseCallback<Empty>() {
            @Override
            public void onResponse(ApiResult<Empty> result) {
                snackbar(usernameEditText, getString(result.isSuccess()
                        ? R.string.user_data_was_successfully_saved
                        : R.string.user_data_was_not_successfully_saved));

                if (result.isSuccess()) {
                    userStorageProvider.writeUser(user);
                }
            }
        });
    }

    public interface IUserStorageProvider {
        User readUser();
        void writeUser(User user);
    }
}
