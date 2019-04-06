package bajitumop.clinic.views;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.widget.ViewFlipper;

import bajitumop.clinic.R;

public abstract class BaseFlipperActivity extends BaseActivity {

    private final int CONTENT_VIEW = 0;
    private final int PROGRESS_VIEW = 1;
    private final int CONNECTION_ERROR_VIEW = 2;

    ViewFlipper viewFlipper;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_base);
        viewFlipper = findViewById(R.id.viewFlipper);
    }

    protected void setContentPage() {
        setView(CONTENT_VIEW);
    }

    protected void setProgressPage() {
        setView(PROGRESS_VIEW);
    }

    protected void setErrorView() {
        setView(CONNECTION_ERROR_VIEW);
    }

    private void setView(int child) {
        this.viewFlipper.setDisplayedChild(child);
    }
}
