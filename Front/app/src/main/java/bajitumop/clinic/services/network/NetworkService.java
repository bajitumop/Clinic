package bajitumop.clinic.services.network;

import bajitumop.clinic.BuildConfig;
import io.reactivex.schedulers.Schedulers;
import okhttp3.OkHttpClient;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;

public class NetworkService {
    public static final String HOST = "http://192.168.1.38:8888";
    private static IClinicApi clinicApi;

    public static IClinicApi Create(){
        if (clinicApi == null) {

            OkHttpClient okHttpClient = new OkHttpClient.Builder()
                    .addInterceptor(new HttpLoggingInterceptor().setLevel((BuildConfig.DEBUG) ? HttpLoggingInterceptor.Level.BODY : HttpLoggingInterceptor.Level.NONE))
                    .build();

            clinicApi = new Retrofit.Builder()
                    .baseUrl(HOST+"/api/")
                    .client(okHttpClient)
                    .addConverterFactory(GsonConverterFactory.create())
                    .addCallAdapterFactory(RxJava2CallAdapterFactory.createWithScheduler(Schedulers.io()))
                    .build()
                    .create(IClinicApi.class);
        }

        return clinicApi;
    }
}
