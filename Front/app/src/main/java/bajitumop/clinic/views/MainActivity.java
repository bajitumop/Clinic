package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.FloatingActionButton;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.view.MenuItem;

import bajitumop.clinic.R;
import bajitumop.clinic.models.ApiResult;
import bajitumop.clinic.models.DoctorShortModel;
import bajitumop.clinic.services.network.IClinicApi;
import bajitumop.clinic.services.network.NetworkService;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends BaseFlipperActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private DrawerLayout drawerLayout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        InitializeDrawerLayout();
        InitializeFab();
    }

    private void InitializeFab() {
        final FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                IClinicApi clinicApi = new NetworkService().getClinicApi();
                // Start
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
                // End
            }
        });

        //where ApiResult class: { boolean success, T data, string message, int statusCode }
        /*
            clinicApi.getDoctors(response => {
                if (response.success) {
                    // update view from response.data
                } else {
                    // check response.statusCode and show snackbar with message or view error page
                }
            })
        */
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
}
