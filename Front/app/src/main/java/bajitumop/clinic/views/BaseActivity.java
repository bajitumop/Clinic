package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.Snackbar;
import android.support.v4.app.FragmentActivity;
import android.support.v7.app.AppCompatActivity;
import android.view.View;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import bajitumop.clinic.ClinicApplication;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.User;
import bajitumop.clinic.services.network.IClinicApi;
import bajitumop.clinic.services.network.IOnResponseCallback;
import bajitumop.clinic.services.network.NetworkService;
import io.reactivex.Single;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.functions.BiConsumer;
import retrofit2.HttpException;

public abstract class BaseActivity extends AppCompatActivity {
    protected IClinicApi clinicApi = NetworkService.Create();
    protected CompositeDisposable compositeDisposable = new CompositeDisposable();

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        compositeDisposable.clear();
    }

    protected void snackbar(View v, String text, int time) {
        Snackbar.make(v, text, time).show();
    }

    protected void snackbar(View v, String text) {
        snackbar(v, text, Snackbar.LENGTH_LONG);
    }

    protected User getUser() {
        return ClinicApplication.getUserStorage().getUser();
    }

    protected void updateUser(User user) {
        ClinicApplication.getUserStorage().updateUser(user);
    }

    protected <T> void sendRequest(@NonNull Single<ApiResult<T>> single, @NonNull final IOnResponseCallback<T> onResponse) {
        compositeDisposable.add(single
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new BiConsumer<ApiResult<T>, Throwable>() {
                    @Override
                    public void accept(ApiResult<T> result, Throwable throwable) throws Exception {
                        if (throwable instanceof HttpException) {
                            try {
                                String response = ((HttpException) throwable).response().errorBody().string();
                                result = new Gson().fromJson(response, new TypeToken<ApiResult<T>>() {}.getType());
                            } catch (Exception e) {
                                // ignore
                            }
                        }

                        if (result == null) {
                            result = new ApiResult<>();
                            result.setSuccess(false);
                            result.setMessage("Ошибка выполнения запроса :(");
                        }

                        onResponse.onResponse(result);
                    }
                }));
    }
}
