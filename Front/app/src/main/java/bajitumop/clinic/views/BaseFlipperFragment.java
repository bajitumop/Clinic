package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.view.View;
import android.widget.ViewFlipper;

import bajitumop.clinic.R;

public abstract class BaseFlipperFragment extends BaseFragment {
    private final int CONTENT_VIEW = 0;
    private final int PROGRESS_VIEW = 1;
    private final int CONNECTION_ERROR_VIEW = 2;
    private final int EMPTY_LIST_VIEW = 3;

    private ViewFlipper viewFlipper;

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        viewFlipper = view.findViewById(R.id.viewFlipper);
    }

    protected void setContent() {
        setView(CONTENT_VIEW);
    }

    protected void setProgress() {
        setView(PROGRESS_VIEW);
    }

    protected void setConnectionError() {
        setView(CONNECTION_ERROR_VIEW);
    }

    protected void setEmpty() {
        setView(EMPTY_LIST_VIEW);
    }

    private void setView(int child) {
        this.viewFlipper.setDisplayedChild(child);
    }
}
