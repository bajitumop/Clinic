package bajitumop.clinic.services.network;

import bajitumop.clinic.models.ApiResult;

public interface IOnResponseCallback<T> {
    void onResponse(ApiResult<T> result);
}
