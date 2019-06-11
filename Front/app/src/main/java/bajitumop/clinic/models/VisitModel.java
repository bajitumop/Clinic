package bajitumop.clinic.models;

import java.io.Serializable;
import java.util.Date;

public class VisitModel implements Serializable {
    private long id;

    private long doctorId;

    private long visitId;

    private Date dateTime;

    private String doctorFirstName;

    private String doctorSecondName;

    private String doctorThirdName;

    private String specialty;

    private String serviceDescription;

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public long getDoctorId() {
        return doctorId;
    }

    public void setDoctorId(long doctorId) {
        this.doctorId = doctorId;
    }

    public long getVisitId() {
        return visitId;
    }

    public void setVisitId(long visitId) {
        this.visitId = visitId;
    }

    public Date getDateTime() {
        return dateTime;
    }

    public void setDateTime(Date dateTime) {
        this.dateTime = dateTime;
    }

    public String getDoctorFirstName() {
        return doctorFirstName;
    }

    public void setDoctorFirstName(String doctorFirstName) {
        this.doctorFirstName = doctorFirstName;
    }

    public String getDoctorSecondName() {
        return doctorSecondName;
    }

    public void setDoctorSecondName(String doctorSecondName) {
        this.doctorSecondName = doctorSecondName;
    }

    public String getDoctorThirdName() {
        return doctorThirdName;
    }

    public void setDoctorThirdName(String doctorThirdName) {
        this.doctorThirdName = doctorThirdName;
    }

    public String getSpecialty() {
        return specialty;
    }

    public void setSpecialty(String specialty) {
        this.specialty = specialty;
    }

    public String getServiceDescription() {
        return serviceDescription;
    }

    public void setServiceDescription(String serviceDescription) {
        this.serviceDescription = serviceDescription;
    }

    public String getFullDoctorName() {
        return String.format("%s %s %s", doctorSecondName, doctorFirstName, doctorThirdName);
    }
}
