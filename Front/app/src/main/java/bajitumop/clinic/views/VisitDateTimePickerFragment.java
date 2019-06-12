package bajitumop.clinic.views;

import android.content.Context;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CalendarView;
import android.widget.ScrollView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.VisitInfoStatusModel;
import bajitumop.clinic.services.DateTime;
import bajitumop.clinic.services.network.IOnResponseCallback;

public class VisitDateTimePickerFragment extends BaseFragment implements OnTimeClickListener {
    private long doctorId;
    private TimeListAdapter adapter;
    private OnDateTimeSelectListener onDateTimeSelectListener;
    private VisitInfoStatusModel[] visitInfoStatusModels;
    private ArrayList<VisitInfoStatusModel> visitStatusesForList = new ArrayList<>();
    private Date currentSelectedDateTime;
    private ScrollView scrollView;

    private Button confirm;
    private TextView result;
    private CalendarView calendarView;

    public void setDoctorId(long doctorId) {
        this.doctorId = doctorId;
        loadDoctorSchedule();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view =  inflater.inflate(R.layout.fragment_visit_date_time_picker, container, false);
        result = view.findViewById(R.id.resultDate);
        scrollView = view.findViewById(R.id.scrollViewFragment);
        calendarView = view.findViewById(R.id.calendar);
        calendarView.setOnDateChangeListener(new CalendarView.OnDateChangeListener() {
            @Override
            public void onSelectedDayChange(CalendarView view, int year, int month, int dayOfMonth) {
                onChangeCalendarDate(new Date(year, month, dayOfMonth));
            }
        });

        view.findViewById(R.id.cancel).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onDateTimeSelectListener.onDateTimeSelect(null);
            }
        });

        confirm = view.findViewById(R.id.confirm);
        confirm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onDateTimeSelectListener.onDateTimeSelect(currentSelectedDateTime);
            }
        });


        Context context = view.getContext();
        RecyclerView recyclerView = view.findViewById(R.id.timeList);
        recyclerView.setLayoutManager(new LinearLayoutManager(context));
        adapter = new TimeListAdapter(visitStatusesForList, this, context);
        recyclerView.setAdapter(adapter);

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
        loadDoctorSchedule();
    }

    private void loadDoctorSchedule() {
        sendRequest(clinicApi.getDoctorSchedule(doctorId), new IOnResponseCallback<VisitInfoStatusModel[]>() {
            @Override
            public void onResponse(ApiResult<VisitInfoStatusModel[]> result) {
                if(!result.isSuccess()){
                    snackbar(calendarView, result.getMessage());
                    return;
                }

                visitInfoStatusModels = result.getData();
                resetCalendar();
            }
        });
    }

    private void resetCalendar() {
        Date utcNow = DateTime.getNow();
        Calendar cal = Calendar.getInstance();
        cal.setTime(utcNow);
        cal.add(Calendar.DATE, 14);
        calendarView.setMinDate(utcNow.getTime());
        calendarView.setMaxDate(cal.getTimeInMillis());
        calendarView.setDate(utcNow.getTime());
        onChangeCalendarDate(new Date(calendarView.getMinDate()));
    }

    private void onChangeCalendarDate(Date date) {
        result.setText("");
        currentSelectedDateTime = null;
        visitStatusesForList.clear();
        for (VisitInfoStatusModel model: visitInfoStatusModels) {
            if (model.getDate().getDate() == date.getDate()
                    && model.getDate().getMonth() == date.getMonth()
                    && model.getDate().getYear() + 1900 == date.getYear()) {
                visitStatusesForList.add(model);
            }
        }
        adapter.notifyDataSetChanged();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        onDateTimeSelectListener = null;
    }

    @Override
    public void onTimeClick(VisitInfoStatusModel visitStatusModel) {
        this.currentSelectedDateTime = visitStatusModel.getDate();
        result.setText(DateTime.formatFullDate(this.currentSelectedDateTime));
        confirm.getParent().requestChildFocus(confirm, confirm);
    }

    public void reload() {
        loadDoctorSchedule();
    }

    public interface OnDateTimeSelectListener {
        void onDateTimeSelect(Date date);
    }
}

