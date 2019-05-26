package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.NavigationView;
import android.support.v4.app.Fragment;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.widget.Toolbar;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import bajitumop.clinic.R;
import bajitumop.clinic.models.DoctorModel;
import bajitumop.clinic.models.ServiceModel;
import bajitumop.clinic.models.User;
import bajitumop.clinic.views.MainFragments.ContactsFragment;
import bajitumop.clinic.views.MainFragments.DoctorsFragment;
import bajitumop.clinic.views.MainFragments.ServicesFragment;
import bajitumop.clinic.views.MainFragments.SettingsFragment;

public class MainActivity extends BaseActivity implements
        NavigationView.OnNavigationItemSelectedListener,
        DoctorsFragment.IDoctorsListInteractionListener,
        ServicesFragment.IServicesListInteractionListener,
        SettingsFragment.IUserStorageProvider {

    Toolbar toolbar;
    TextView navigationUserTextView;
    private DrawerLayout drawerLayout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        User user = getUser();
        if (user == null) {
            finish();
            return;
        }

        setContentView(R.layout.activity_main);

        toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        initDrawerLayout(user);
        initFab();
    }

    private void initFab() {
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
            }
        });
    }

    private void initDrawerLayout(User user) {
        drawerLayout = findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawerLayout, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawerLayout.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        View header = navigationView.getHeaderView(0);
        navigationUserTextView = header.findViewById(R.id.nav_header_username);
        ImageView logoutImageView = header.findViewById(R.id.logoutImageView);
        logoutImageView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                updateUser(null);
                finish();
            }
        });
        updateUserInfo(user);
    }

    @Override
    public void onBackPressed() {
        if (drawerLayout.isDrawerOpen(GravityCompat.START)) {
            drawerLayout.closeDrawer(GravityCompat.START);
        } else {
            android.os.Process.killProcess(android.os.Process.myPid());
            System.exit(0);
        }
    }

    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem item) {
        switch (item.getItemId()) {
            case R.id.nav_visits:
                break;
            case R.id.nav_history_visits:
                break;
            case R.id.nav_doctors:
                setFragment(new DoctorsFragment());
                break;
            case R.id.nav_services:
                setFragment(new ServicesFragment());
                break;
            case R.id.nav_settings:
                setFragment(new SettingsFragment());
                break;
            case R.id.nav_contacts:
                setFragment(new ContactsFragment());
                break;
            default:
                break;
        }

        drawerLayout.closeDrawer(GravityCompat.START);
        return true;
    }

    private void setFragment(Fragment fragment) {
        getSupportFragmentManager().beginTransaction().replace(R.id.fragmentContainer, fragment).commit();
    }

    @Override
    public void onDoctorClick(DoctorModel doctor) {
        int x = 7;
    }

    @Override
    public void onServiceClick(ServiceModel service) {
        int x = 7;
    }

    @Override
    public User readUser() {
        return getUser();
    }

    @Override
    public void writeUser(User user) {
        updateUser(user);
        updateUserInfo(user);
    }

    private void updateUserInfo(User user) {
        navigationUserTextView.setText(String.format("%s %s", user.getFirstName(), user.getSecondName()));
    }

}
