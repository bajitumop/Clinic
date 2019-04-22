package bajitumop.clinic.services.network;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class NetworkService {
    private static IClinicApi clinicApi;

    public static IClinicApi Create(){
        if (clinicApi == null) {
            clinicApi = new Retrofit.Builder()
                    .baseUrl("http://192.168.1.38:8888/api/")
                    .addConverterFactory(GsonConverterFactory.create())
                    .build()
                    .create(IClinicApi.class);
        }

        return clinicApi;
    }
}
