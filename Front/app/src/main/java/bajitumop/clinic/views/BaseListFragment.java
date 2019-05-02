package bajitumop.clinic.views;

import android.content.Context;

public abstract class BaseListFragment<TListener> extends BaseFlipperFragment {
    protected TListener listener;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        listener = (TListener) context;
    }

    @Override
    public void onDetach() {
        super.onDetach();
        listener = null;
    }
}
