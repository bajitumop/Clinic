package bajitumop.clinic.views.MainFragments;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import bajitumop.clinic.R;
import bajitumop.clinic.views.BaseFragment;
import bajitumop.clinic.views.MainActivity;

public class ContactsFragment extends BaseFragment {
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        ((MainActivity)getActivity()).getSupportActionBar().setTitle(R.string.contacts_title);
        return inflater.inflate(R.layout.fragment_contacts, container, false);
    }
}
