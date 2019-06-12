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
        calendarView = view.findViewById(R.id.calendar);
        Date utcNow = DateTime.getNow();
        Calendar cal = Calendar.getInstance();
        cal.setTime(utcNow);
        cal.add(Calendar.DATE, 14);
        calendarView.setMinDate(utcNow.getTime());
        calendarView.setMaxDate(cal.getTimeInMillis());
        calendarView.setOnDateChangeListener(new CalendarView.OnDateChangeListener() {
            @Override
            public void onSelectedDayChange(CalendarView view, int year, int month, int dayOfMonth) {
                Date date = new Date(year, month, dayOfMonth);
                onCalendarDayChanged(date);
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

        result = view.findViewById(R.id.resultDate);

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
        result.setText(""); // may be it's need to move in calendarOnDayChanged
        currentSelectedDateTime = null; // may be it's need to move in calendarOnDayChanged
        calendarView.setDate(calendarView.getMinDate());
        visitStatusesForList.clear(); // maybe it's not needed
        adapter.notifyDataSetChanged(); // maybe it's not needed
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
            }
        });
    }

    private void onCalendarDayChanged(Date date) {
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
        confirm.requestFocus(); // ToDo: not working
    }

    public interface OnDateTimeSelectListener {
        void onDateTimeSelect(Date date);
    }
}

