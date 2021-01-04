using Android.Content;
using Android.Views;
using Android.Widget;

using EnterpriseTracker.Droid.Utility;

namespace EnterpriseTracker.Droid.Controls
{
    public class SeperatorControl : View
    {
        Context _context;
        public SeperatorControl(Context context, int marginTop = 0, int marginBottom = 0) : base(context)
        {
            _context = context;
            LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, AndroidHelper.ConvertDpToPx(1)) { TopMargin = AndroidHelper.ConvertDpToPx(marginTop), BottomMargin = AndroidHelper.ConvertDpToPx(marginBottom) };
            SetBackgroundColor(AndroidHelper.CurrentTheme.ColorPlaceHolder);
        }
    }
}