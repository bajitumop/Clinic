package bajitumop.clinic.services.network;

import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorModel;
import bajitumop.clinic.models.Empty;
import bajitumop.clinic.models.LoginModel;
import bajitumop.clinic.models.LoginResponse;
import bajitumop.clinic.models.RegistrationModel;
import bajitumop.clinic.models.ServiceModel;
import bajitumop.clinic.models.User;
import io.reactivex.Single;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;

public interface IClinicApi {

    @GET("doctors")
    Single<ApiResult<DoctorModel[]>> getDoctors();

    @GET("services")
    Single<ApiResult<ServiceModel[]>> getServices();

    @PUT("users/update")
    Single<ApiResult<Empty>> updateUser(@Body User user);

    @POST("account/login")
    Single<ApiResult<LoginResponse>> login(@Body LoginModel model);

    @POST("account/register")
    Single<ApiResult<String>> register(@Body RegistrationModel model);
}
