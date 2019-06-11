package bajitumop.clinic.services;

import java.text.SimpleDateFormat;
import java.util.Date;

public class DateTime {
    public final static String ISO = "yyyy-MM-dd'T'HH:mm:ss";
    private static final SimpleDateFormat utcFormat = new SimpleDateFormat(ISO);

    public static Date getNow() {
        return new Date();
    }

    public static String formatTime(Date date){
        return String.format(
                "%02d:%02d",
                date.getHours() + 3,
                date.getMinutes());
    }

    public static String formatFullDate(Date date){
        return String.format(
                "%02d.%02d.%d в %02d:%02d",
                date.getDate(),
                date.getMonth() + 1,
                date.getYear() + 1900,
                date.getHours() + 3,
                date.getMinutes());
    }

    public static String formatUtc(Date date){
        return utcFormat.format(date);
    }
}
