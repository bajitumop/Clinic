package bajitumop.clinic.services.network;

import java.io.IOException;

import bajitumop.clinic.BuildConfig;
import bajitumop.clinic.ClinicApplication;
import io.reactivex.schedulers.Schedulers;
import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;

public class NetworkService {
    public static final String HOST = "http://10.0.2.2:5000";
    private static IClinicApi clinicApi;

    public static IClinicApi Create(){
        if (clinicApi == null) {

            OkHttpClient okHttpClient = new OkHttpClient.Builder()
                    .addInterceptor(new HttpLoggingInterceptor().setLevel((BuildConfig.DEBUG) ? HttpLoggingInterceptor.Level.BODY : HttpLoggingInterceptor.Level.NONE))
                    .addInterceptor(new Interceptor() {
                        @Override
                        public Response intercept(Chain chain) throws IOException {
                            String accessToken = ClinicApplication.getUserStorage().getAccessToken();
                            if (accessToken == null) {
                                return chain.proceed(chain.request());
                            }

                            Request request = chain.request().newBuilder().addHeader("Authorization", String.format("Bearer %s", accessToken)).build();
                            return chain.proceed(request);
                        }
                    })
                    .build();

            clinicApi = new Retrofit.Builder()
                    .baseUrl(HOST + "/api/")
                    .client(okHttpClient)
                    .addConverterFactory(GsonConverterFactory.create())
                    .addCallAdapterFactory(RxJava2CallAdapterFactory.createWithScheduler(Schedulers.io()))
                    .build()
                    .create(IClinicApi.class);
        }

        return clinicApi;
    }
}
