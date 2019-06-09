package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
import android.text.InputType;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.ViewFlipper;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorModel;
import bajitumop.clinic.models.ServiceModel;
import bajitumop.clinic.services.network.IOnResponseCallback;

public class RecordActivity extends BaseActivity implements VisitDateTimePickerFragment.OnDateTimeSelectListener {

    private final int SPINNERS_VIEW = 0;
    private final int CALENDAR_VIEW = 1;

    private final int DEFAULT_ID = -1;
    private static final SimpleDateFormat dateTimeFormat = new SimpleDateFormat("dd.MM.yyyy в HH:mm", Locale.getDefault());

    private DoctorModel[] doctors;
    private ServiceModel[] services;

    private int initDoctorId;
    private int initServiceId;
    private String initSpecialty;

    private Spinner specialtySpinner;
    private Spinner serviceSpinner;
    private Spinner doctorSpinner;

    ArrayAdapter<String> specialtyAdapter;
    ArrayAdapter<ServiceModel> serviceAdapter;
    ArrayAdapter<DoctorModel> doctorAdapter;

    VisitDateTimePickerFragment visitPickerFragment;
    ViewFlipper viewFlipper;
    EditText dateTime;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_record);

        visitPickerFragment = new VisitDateTimePickerFragment();
        getSupportFragmentManager().beginTransaction().replace(R.id.visitPickerFragment, visitPickerFragment).commit();

        Intent intent = getIntent();
        initSpecialty = intent.getStringExtra("specialty");
        initServiceId = intent.getIntExtra("serviceId", DEFAULT_ID);
        initDoctorId = intent.getIntExtra("doctorId", DEFAULT_ID);
        visitPickerFragment.setDoctorId(initDoctorId);

        specialtySpinner = findViewById(R.id.specialtySpinner);
        serviceSpinner = findViewById(R.id.serviceSpinner);
        doctorSpinner = findViewById(R.id.doctorSpinner);
        viewFlipper = findViewById(R.id.viewFlipper);
        dateTime = findViewById(R.id.dateTime);
        dateTime.setRawInputType(InputType.TYPE_NULL);
        dateTime.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showCalendar();
            }
        });


        specialtySpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String currentSpecialty = parent.getItemAtPosition(position).toString();
                filterDoctors(currentSpecialty);
                filterServices(currentSpecialty);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

        serviceSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                onServiceReselected((ServiceModel)parent.getItemAtPosition(position));
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        doctorSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                onDoctorReselected((DoctorModel)parent.getItemAtPosition(position));
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        load();
    }

    @Override
    public void onBackPressed() {
        if (viewFlipper.getDisplayedChild() == CALENDAR_VIEW){
            onCancel();
        } else {
            super.onBackPressed();
        }
    }

    private void onDoctorReselected(DoctorModel doctor) {
        //snackbar(specialtySpinner, String.format("onDoctorReselected: %s", doctor.getId()));
        visitPickerFragment.setDoctorId(doctor.getId());
    }

    private void onServiceReselected(ServiceModel service) {
        //snackbar(specialtySpinner, String.format("onServiceReselected: %s", service.getId()));
    }

    private void filterServices(final String specialty) {
        List<ServiceModel> filteredList = new ArrayList<>();
        for (ServiceModel service : services) {
            if (service.getSpecialty().equals(specialty)){
                filteredList.add(service);
            }
        }

        ServiceModel[] filtered = new ServiceModel[filteredList.size()];
        for (int i = 0; i < filtered.length; i++) {
            filtered[i] = filteredList.get(i);
        }

        serviceAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, filtered);
        serviceSpinner.setAdapter(serviceAdapter);
        int position = 0;
        for (int i = 0; i < filtered.length; i++) {
            if (services[i].getId() == initServiceId) {
                position = i;
                break;
            }
        }

        serviceSpinner.setSelection(position);
    }

    private void filterDoctors(final String specialty) {
        List<DoctorModel> filteredList = new ArrayList<>();
        for (DoctorModel doctor : doctors) {
            if (doctor.getSpecialty().equals(specialty)){
                filteredList.add(doctor);
            }
        }

        DoctorModel[] filtered = new DoctorModel[filteredList.size()];
        for (int i = 0; i < filtered.length; i++) {
            filtered[i] = filteredList.get(i);
        }

        doctorAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, filtered);
        doctorSpinner.setAdapter(doctorAdapter);
        int position = 0;
        for (int i = 0; i < filtered.length; i++) {
            if (doctors[i].getId() == initDoctorId) {
                position = i;
                break;
            }
        }

        doctorSpinner.setSelection(position);
    }

    private void initSpinners() {
        ArrayList<String> specialtiesList = new ArrayList<>();
        for (DoctorModel doctor : doctors) {
            if(!specialtiesList.contains(doctor.getSpecialty())){
                specialtiesList.add(doctor.getSpecialty());
            }
        }

        String[] specialties = new String[specialtiesList.size()];
        for (int i = 0; i < specialties.length; i++) {
            specialties[i] = specialtiesList.get(i);
        }

        specialtyAdapter = new ArrayAdapter<>(RecordActivity.this, android.R.layout.simple_list_item_1, specialties);
        specialtySpinner.setAdapter(specialtyAdapter);
        int position = initSpecialty != null
                ? specialtyAdapter.getPosition(initSpecialty)
                : 0;

        specialtySpinner.setSelection(position);
        String currentSpecialty = specialtyAdapter.getItem(position);

        filterDoctors(currentSpecialty);
        filterServices(currentSpecialty);
    }

    private void load() {
        sendRequest(clinicApi.getServices(), new IOnResponseCallback<ServiceModel[]>() {
            @Override
            public void onResponse(ApiResult<ServiceModel[]> servicesResult) {
                if (!servicesResult.isSuccess()){
                    snackbar(serviceSpinner, servicesResult.getMessage());
                    return;
                }

                services = servicesResult.getData();
                sendRequest(clinicApi.getDoctors(), new IOnResponseCallback<DoctorModel[]>() {
                    @Override
                    public void onResponse(ApiResult<DoctorModel[]> doctorsResult) {
                        if(!doctorsResult.isSuccess()) {
                            snackbar(doctorSpinner, doctorsResult.getMessage());
                            return;
                        }

                        doctors = doctorsResult.getData();
                        initSpinners();
                    }
                });
            }
        });
    }

    private void showSpinners(){
        this.viewFlipper.setDisplayedChild(SPINNERS_VIEW);
    }

    private void showCalendar(){
        this.viewFlipper.setDisplayedChild(CALENDAR_VIEW);
    }

    @Override
    public void onDateTimeSelect(Date date) {
        showSpinners();
        String value = dateTimeFormat.format(date);
        dateTime.setText(value);
    }

    @Override
    public void onCancel() {
        showSpinners();
    }
}
