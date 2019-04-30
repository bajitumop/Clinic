package bajitumop.clinic.services.network;

import android.content.Context;
import android.content.SharedPreferences;

import bajitumop.clinic.models.User;

public class UserStorage {
    private static UserStorage userStorage;
    private SharedPreferences sharedPreferences;

    private UserStorage(SharedPreferences prefs) {
        sharedPreferences = prefs;
    }

    public static UserStorage Create(Context context) {
        if (userStorage == null){
            userStorage = new UserStorage(context.getSharedPreferences("SharedPrefs", Context.MODE_PRIVATE));
        }

        return userStorage;
    }

    public void updateUser(User user) {
        SharedPreferences.Editor editor = sharedPreferences.edit();
        if (user == null) {
            editor.putString("username", null);
            editor.putString("passwordHash", null);
            editor.putString("firstName", null);
            editor.putString("secondName", null);
            editor.putString("thirdName", null);
            editor.putString("accessToken", null);
        } else {
            editor.putString("username", user.getUsername());
            editor.putString("passwordHash", user.getPasswordHash());
            editor.putString("firstName", user.getFirstName());
            editor.putString("secondName", user.getSecondName());
            editor.putString("thirdName", user.getThirdName());
            editor.putString("accessToken", user.getAccessToken());
        }

        editor.apply();
    }

    public User getUser(){
        String accessToken = getAccessToken();
        if (accessToken == null){
            return null;
        }

        User user = new User();
        user.setUsername(sharedPreferences.getString("username", null));
        user.setPasswordHash(sharedPreferences.getString("passwordHash", null));
        user.setFirstName(sharedPreferences.getString("firstName", null));
        user.setSecondName(sharedPreferences.getString("secondName", null));
        user.setThirdName(sharedPreferences.getString("thirdName", null));
        user.setAccessToken(accessToken);
        return user;
    }

    public String getAccessToken() {
        return sharedPreferences.getString("accessToken", null);
    }
}
