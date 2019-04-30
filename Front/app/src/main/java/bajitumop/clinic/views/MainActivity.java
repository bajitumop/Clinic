package bajitumop.clinic.views;

import android.content.Intent;
import android.os.Bundle;
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
import android.widget.ViewFlipper;

import bajitumop.clinic.R;
import bajitumop.clinic.models.User;
import bajitumop.clinic.views.MainFragments.ContactsFragment;

public class MainActivity extends BaseActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private final int CONTENT_VIEW = 0;
    private final int PROGRESS_VIEW = 1;
    private final int CONNECTION_ERROR_VIEW = 2;

    Toolbar toolbar;
    ViewFlipper viewFlipper;
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
        viewFlipper = findViewById(R.id.viewFlipper);

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
        TextView navigationUserTextView = header.findViewById(R.id.nav_header_username);
        navigationUserTextView.setText(String.format("%s %s", user.getFirstName(), user.getSecondName()));
        ImageView logoutImageView = header.findViewById(R.id.logoutImageView);
        logoutImageView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                updateUser(null);
                finish();
            }
        });
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
                Fragment fragment = new ContactsFragment();
                setFragment(fragment);
                break;
            default:
                break;
        }

        drawerLayout.closeDrawer(GravityCompat.START);
        return true;
    }

    private void goToSettings() {
        startActivity(new Intent(this, SettingsActivity.class));
    }

    private void setFragment(Fragment fragment) {
        getSupportFragmentManager().beginTransaction().replace(R.id.fragmentContainer, fragment).commit();
    }

    private void setView(int child) {
        this.viewFlipper.setDisplayedChild(child);
    }
}
