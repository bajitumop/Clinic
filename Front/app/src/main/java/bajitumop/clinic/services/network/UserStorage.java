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
            editor.putString("username", "");
            editor.putString("passwordHash", "");
            editor.putString("firstName", "");
            editor.putString("secondName", "");
            editor.putString("thirdName", "");
            editor.putString("accessToken", "");
        } else {
            editor.putString("username", user.getUsername());
            editor.putString("passwordHash", user.getPasswordHash());
            editor.putString("firstName", user.getFirstName());
            editor.putString("secondName", user.getSecondName());
            editor.putString("thirdName", user.getThirdName());
            editor.putString("accessToken", user.getAccessToken());
        }
    }

    public User getUser(){
        String accessToken = getAccessToken();
        if (accessToken == null){
            return null;
        }

        User user = new User();
        user.setUsername(sharedPreferences.getString("username", ""));
        user.setPasswordHash(sharedPreferences.getString("passwordHash", ""));
        user.setFirstName(sharedPreferences.getString("firstName", ""));
        user.setSecondName(sharedPreferences.getString("secondName", ""));
        user.setThirdName(sharedPreferences.getString("thirdName", ""));
        user.setAccessToken(accessToken);
        return user;
    }

    public String getAccessToken() {
        return sharedPreferences.getString("accessToken", null);
    }
}
