package bajitumop.clinic;

import android.app.Application;
import android.content.res.Configuration;

import java.util.Locale;

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
        Locale locale = new Locale("ru","RU");
        Locale.setDefault(locale);
        Configuration config = new Configuration();
        config.locale = locale;
        getApplicationContext().getResources().updateConfiguration(config, null);
    }
}
