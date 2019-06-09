package bajitumop.clinic.views;

import android.content.Context;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CalendarView;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Comparator;
import java.util.Date;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.VisitInfoStatusModel;
import bajitumop.clinic.services.network.IOnResponseCallback;

public class VisitDateTimePickerFragment extends BaseFragment {
    private OnDateTimeSelectListener onDateTimeSelectListener;
    private long doctorId;
    private CalendarView calendarView;
    private VisitInfoStatusModel[] visitInfoStatusModels;

    public void setDoctorId(long doctorId) {
        this.doctorId = doctorId;
        loadDoctorSchedule();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view =  inflater.inflate(R.layout.fragment_visit_date_time_picker, container, false);
        calendarView = view.findViewById(R.id.calendar);
        Date now = new Date();
        Calendar cal = Calendar.getInstance();
        cal.setTime(now);
        cal.add(Calendar.DATE, 14);
        calendarView.setMinDate(now.getTime());
        calendarView.setMaxDate(cal.getTimeInMillis());
        calendarView.setOnDateChangeListener(new CalendarView.OnDateChangeListener() {
            @Override
            public void onSelectedDayChange(CalendarView view, int year, int month, int dayOfMonth) {
                Date date = new Date(year, month, dayOfMonth);
                onCalendarDayChanged(date);
            }
        });
        return view;
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        if (context instanceof OnDateTimeSelectListener) {
            onDateTimeSelectListener = (OnDateTimeSelectListener) context;
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        reload();
    }

    private void loadDoctorSchedule() {
        sendRequest(clinicApi.getDoctorSchedule(doctorId), new IOnResponseCallback<VisitInfoStatusModel[]>() {
            @Override
            public void onResponse(ApiResult<VisitInfoStatusModel[]> result) {
                if(!result.isSuccess()){
                    snackbar(calendarView, result.getMessage());
                    return;
                }

                init(result.getData());
            }
        });
    }

    private void reload() {
        // ToDO: reset calendar;
        loadDoctorSchedule();
    }

    private void init(VisitInfoStatusModel[] visitInfoStatusModels) {
        this.visitInfoStatusModels = visitInfoStatusModels;
    }

    private void onCalendarDayChanged(Date date) {
        ArrayList<VisitInfoStatusModel> visitStatuses = new ArrayList<>();
        for (VisitInfoStatusModel model: visitInfoStatusModels) {
            if (model.getDate().getDate() == date.getDate()
                    && model.getDate().getMonth() == date.getDate()
                    && model.getDate().getYear() == date.getYear()) {
                visitStatuses.add(model);
            }
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        onDateTimeSelectListener = null;
    }

    public interface OnDateTimeSelectListener {
        void onDateTimeSelect(Date date);
        void onCancel();
    }
}
