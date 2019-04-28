package bajitumop.clinic.models;

import com.google.gson.annotations.SerializedName;

public class ApiResult<Data> {
    @SerializedName("success")
    private boolean success;

    @SerializedName("message")
    private String message;

    @SerializedName("data")
    private Data data;

    public boolean isSuccess() {
        return success;
    }

    public String getMessage() {
        return message;
    }

    public void setSuccess(boolean success) {
        this.success = success;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Data getData() {
        if (data instanceof Empty) {
            throw new IllegalArgumentException("No such field for empty api result");
        }

        return data;
    }

    public void setData(Data data) {
        if (data instanceof Empty) {
            throw new IllegalArgumentException("No such field for empty api result");
        }

        this.data = data;
    }
}
