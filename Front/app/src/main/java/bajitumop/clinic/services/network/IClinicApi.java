package bajitumop.clinic.services.network;

import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorShortModel;
import bajitumop.clinic.models.User;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.PUT;

public interface IClinicApi {

    @GET("default/fallback")
    public Call<String> defaultFallback();

    @GET("doctors")
    public Call<ApiResult<DoctorShortModel[]>> getDoctors();

    @PUT("update")
    public Call<ApiResult> updateUser(@Body User user);
}
