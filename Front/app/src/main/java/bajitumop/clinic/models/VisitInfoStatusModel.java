package bajitumop.clinic.models;

import java.util.Date;

public class VisitInfoStatusModel {
    private Date dateTime;

    private VisitStatus status;

    public Date getDate() {
        return dateTime;
    }

    public void setDate(Date dateTime) {
        this.dateTime = dateTime;
    }

    public VisitStatus getStatus() {
        return status;
    }

    public void setStatus(VisitStatus status) {
        this.status = status;
    }
}
