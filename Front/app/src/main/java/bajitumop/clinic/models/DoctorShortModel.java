package bajitumop.clinic.models;

import com.google.gson.annotations.SerializedName;

public class DoctorShortModel {
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

    @SerializedName("specialties")
    private String[] specialties;

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

    public String[] getSpecialties() {
        return specialties;
    }

    public void setSpecialties(String[] specialties) {
        this.specialties = specialties;
    }
}
