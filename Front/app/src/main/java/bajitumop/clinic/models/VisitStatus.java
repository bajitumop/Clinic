package bajitumop.clinic.models;

import com.google.gson.annotations.SerializedName;

public enum VisitStatus {
    @SerializedName("0")
    Opened,
    @SerializedName("1")
    Reserved,
    @SerializedName("2")
    Closed
}
