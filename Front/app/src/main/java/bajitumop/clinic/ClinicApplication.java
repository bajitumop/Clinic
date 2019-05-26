package bajitumop.clinic;

import android.app.Application;

import bajitumop.clinic.services.network.UserStorage;

public class ClinicApplication extends Application {
    private static UserStorage userStorage;

    public static UserStorage getUserStorage() {
        return userStorage;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        userStorage = UserStorage.Create(this);
    }
}
