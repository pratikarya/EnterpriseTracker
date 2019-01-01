using Android.Graphics;
using Android.Support.V4.Content;

namespace EnterpriseTracker.Droid.Utility
{
    public interface ITheme
    {
        float TextSize { get; }
        Color ColorPlaceHolder { get; }
        Color TextColor { get; }
    }

    public class DefaultTheme : ITheme
    {
        public float TextSize
        {
            get
            {
                return 16f;
            }
        }

        public Color ColorPlaceHolder
        {
            get
            {
                return new Color(ContextCompat.GetColor(AndroidHelper.CurrentActivity, Resource.Color.colorPlaceholder));
            }
        }

        public Color TextColor
        {
            get
            {
                return new Color(ContextCompat.GetColor(AndroidHelper.CurrentActivity, Resource.Color.textColor));
            }
        }
    }
}