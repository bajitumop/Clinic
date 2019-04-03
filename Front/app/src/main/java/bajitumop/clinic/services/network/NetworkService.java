package bajitumop.clinic.services.network;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

// ToDo: make as singleton
public class NetworkService {
    private static final String BASE_URL = "http://192.168.1.38:8888/api/";
    private Retrofit mRetrofit;

    public NetworkService() {
        mRetrofit = new Retrofit.Builder()
                .baseUrl(BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
    }

    public IClinicApi getClinicApi() {
        return mRetrofit.create(IClinicApi.class);
    }
}
