package bajitumop.clinic.models;

public class RegistrationModel extends LoginModel {

    private String firstName;

    private String secondName;

    private String thirdName;

    public RegistrationModel() {

    }

    public RegistrationModel(String username, String passwordHash, String firstName, String secondName, String thirdName) {
        super(username, passwordHash);
        this.firstName = firstName;
        this.secondName = secondName;
        this.thirdName = thirdName;
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
}
