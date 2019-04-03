package bajitumop.clinic.models;

import com.google.gson.annotations.SerializedName;

public class ApiResult<Data> extends BaseApiResult {
    @SerializedName("data")
    private Data data;

    public Data getData() {
        return data;
    }
}
