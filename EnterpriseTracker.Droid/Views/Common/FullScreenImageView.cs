using Android.App;
using Android.OS;

using EnterpriseTracker.Core.ViewModels.Common;

using ImageViews.Photo;

namespace EnterpriseTracker.Droid.Views.Common
{
    [Activity(Label = "")]
    public class FullScreenImageView : BaseActivity<FullScreenImageViewModel>
    {
        private PhotoView _ivFullScreen;
        private PhotoViewAttacher _attacher;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.full_screen_image_view);

            GetReferences();

            var imageUri = Android.Net.Uri.FromFile(new Java.IO.File(ViewModel.ImagePath));
            _ivFullScreen.SetImageURI(imageUri);
            _attacher = new PhotoViewAttacher(_ivFullScreen);
            _attacher.Update();
        }

        private void GetReferences()
        {
            _ivFullScreen = FindViewById<PhotoView>(Resource.Id.ivFullScreen);
        }
    }
}