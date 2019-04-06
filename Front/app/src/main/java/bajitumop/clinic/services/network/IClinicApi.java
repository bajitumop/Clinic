package bajitumop.clinic.services.network;

import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorShortModel;
import retrofit2.Call;
import retrofit2.http.GET;

public interface IClinicApi {

    @GET("default/fallback")
    public Call<String> defaultFallback();

    @GET("doctors")
    public Call<ApiResult<DoctorShortModel[]>> getDoctors();

}
