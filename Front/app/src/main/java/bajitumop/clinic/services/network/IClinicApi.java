package bajitumop.clinic.services.network;

import retrofit2.Call;
import retrofit2.http.GET;

public interface IClinicApi {

    @GET("default/fallback")
    public Call<String> defaultFallback();

}
