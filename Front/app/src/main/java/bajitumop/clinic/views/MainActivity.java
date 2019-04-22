package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;
import android.widget.ViewFlipper;

import bajitumop.clinic.R;

public class MainActivity extends BaseActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private final int CONTENT_VIEW = 0;
    private final int PROGRESS_VIEW = 1;
    private final int CONNECTION_ERROR_VIEW = 2;
    private final int NO_AUTHORIZED_ERROR_VIEW = 3;

    ViewFlipper viewFlipper;
    private DrawerLayout drawerLayout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        viewFlipper = findViewById(R.id.viewFlipper);
        InitializeGoToLoginListener();
        InitializeDrawerLayout();
        InitializeFab();
    }

    private void InitializeGoToLoginListener() {
        TextView goToLoginTV = findViewById(R.id.goToLoginTV);
        goToLoginTV.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                goToAuthorization();
            }
        });
    }

    private void InitializeFab() {
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                goToCreatingRecord();
            }
        });

        /*IClinicApi clinicApi = new NetworkService().getClinicApi();
        clinicApi.getDoctors()
                .enqueue(new Callback<ApiResult<DoctorShortModel[]>>() {
                    @Override
                    public void onResponse(@NonNull Call<ApiResult<DoctorShortModel[]>> call, @NonNull Response<ApiResult<DoctorShortModel[]>> response) {
                        ApiResult<DoctorShortModel[]> body = response.body();
                        // more logic
                    }

                    @Override
                    public void onFailure(@NonNull Call<ApiResult<DoctorShortModel[]>> call, @NonNull Throwable t) {
                        // more logic
                    }
                });


            clinicApi.getDoctors(response => {
                if (response.success) {
                    // update view from response.data
                } else {
                    // check response.statusCode and show snackbar with message or view error page
                }
            })*/
    }

    private void InitializeDrawerLayout() {
        drawerLayout = findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawerLayout, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawerLayout.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
    }

    @Override
    public void onBackPressed() {
        if (drawerLayout.isDrawerOpen(GravityCompat.START)) {
            drawerLayout.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.nav_visits:
                break;
            case R.id.nav_history_visits:
                break;
            case R.id.nav_doctors:
                break;
            case R.id.nav_services:
                break;
            case R.id.nav_contacts:
                break;
            default:
                break;
        }

        drawerLayout.closeDrawer(GravityCompat.START);
        return true;
    }

    private void goToAuthorization() {

    }

    private void goToCreatingRecord() {
        
    }

    private void setFragment() {

    }

    private void setContentPage() {
        setView(CONTENT_VIEW);
    }

    private void setProgressPage() {
        setView(PROGRESS_VIEW);
    }

    private void setErrorView() {
        setView(CONNECTION_ERROR_VIEW);
    }

    private void setNoAuthorizedView() {
        setView(NO_AUTHORIZED_ERROR_VIEW);
    }

    private void setView(int child) {
        this.viewFlipper.setDisplayedChild(child);
    }
}
