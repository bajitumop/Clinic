package bajitumop.clinic.services.network;

import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorModel;
import bajitumop.clinic.models.Empty;
import bajitumop.clinic.models.LoginModel;
import bajitumop.clinic.models.LoginResponse;
import bajitumop.clinic.models.RegistrationModel;
import bajitumop.clinic.models.User;
import io.reactivex.Single;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;

public interface IClinicApi {

    @GET("doctors")
    public Single<ApiResult<DoctorModel[]>> getDoctors();

    @PUT("update")
    public Single<ApiResult<Empty>> updateUser(@Body User user);

    @POST("account/login")
    public Single<ApiResult<LoginResponse>> login(@Body LoginModel model);

    @POST("account/register")
    public Single<ApiResult<String>> register(@Body RegistrationModel model);
}
