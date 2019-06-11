package bajitumop.clinic.models;

import java.io.Serializable;

public class DoctorModel implements Serializable {
    private long id;

    private String firstName;

    private String secondName;

    private String thirdName;

    private String imageUrl;

    private String info;

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

    @Override
    public String toString() {
        return getShortName();
    }

    public String getFullName(){
        return String.format("%s %s %s", secondName, firstName, thirdName);
    }

    public String getShortName(){
        return String.format("%s %s. %s.", secondName, firstName.charAt(0), thirdName.charAt(0));
    }
}
