package bajitumop.clinic.models;

import com.google.gson.annotations.SerializedName;

public class DoctorModel {
    @SerializedName("id")
    private long id;

    @SerializedName("firstName")
    private String firstName;

    @SerializedName("secondName")
    private String secondName;

    @SerializedName("thirdName")
    private String thirdName;

    @SerializedName("imageUrl")
    private String imageUrl;

    @SerializedName("info")
    private String info;

    @SerializedName("specialty")
    private String specialty;

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getSecondName() {
        return secondName;
    }

    public void setSecondName(String secondName) {
        this.secondName = secondName;
    }

    public String getThirdName() {
        return thirdName;
    }

    public void setThirdName(String thirdName) {
        this.thirdName = thirdName;
    }

    public String getImageUrl() {
        return imageUrl;
    }

    public void setImageUrl(String imageUrl) {
        this.imageUrl = imageUrl;
    }

    public String getInfo() {
        return info;
    }

    public void setInfo(String info) {
        this.info = info;
    }

    public String getSpecialty() {
        return specialty;
    }

    public void setSpecialty(String specialty) {
        this.specialty = specialty;
    }
}
