using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.SfPicker;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.Utility;
using EnterpriseTracker.Core.ViewModels.Orders;
using EnterpriseTracker.Droid.Utility;
using EnterpriseTracker.Droid.Views.Common;
using FFImageLoading;
using FFImageLoading.Views;
using MvvmCross.Platforms.Android.Binding.Views;

namespace EnterpriseTracker.Droid.Views.Orders
{
    [Activity(Label = "")]
    public class OrderDetailView : BaseActivity<OrderDetailViewModel>
    {
        Android.Support.V7.Widget.Toolbar _toolbar;
        TextView _toolbarTitle;

        LinearLayout _llContainer;

        LinearLayout _llCategory, _llProduct, _llDateTime, _llMessgae, _llUnits, _llPrintList, _llPhotosList;
        TextView _txtCategory, _txtProduct, _txtDateTime, _txtMessage, _txtUnits, _txtTotalValue;
        EditText _etDateTime, _etMessage, _etUnits;
        MvxSpinner _spnCategory, _spnProduct;
        SfPicker _sfpCategories;
        Button _btnCreate;

        DatePickerDialog _datePickerDialog;
        TimePickerDialog _timePickerDialog;

        bool _isFirstLoad = true, _spnCatFirstLoad = true, _spnProdFirstLoad = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.order_detail_view);

            GetReferences();

            SetSupportActionBar(_toolbar);

            _toolbarTitle.Text = "Details";

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SetDateTimeDialog();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_isFirstLoad)
            {
                ViewModel.LoadCommand.Execute(null);
                _isFirstLoad = false;
            }
        }

        private void _sfpCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void _sfpCategories_OnPickerLoaded(object sender, PickerViewEventsArgs e)
        {

        }

        private void SetDateTimeDialog()
        {
            _datePickerDialog = new DatePickerDialog(this, (sender, args) =>
            {
                ViewModel.CurrentOrder.Time = args.Date;
                _datePickerDialog.Dismiss();
                _timePickerDialog.Show();
            }, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            _etDateTime.Click += (sender, args) =>
            {
                _datePickerDialog.Show();
            };
            _timePickerDialog = new TimePickerDialog(this, (sender, args) =>
            {
                ViewModel.CurrentOrder.Time = ViewModel.CurrentOrder.Time.AddHours(args.HourOfDay);
                ViewModel.CurrentOrder.Time = ViewModel.CurrentOrder.Time.AddMinutes(args.Minute);
                ViewModel.CurrentOrder.Time.AddHours(args.HourOfDay);
                ViewModel.CurrentOrder.Time.AddMinutes(args.Minute);
                _etDateTime.Text = ViewModel.CurrentOrder.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
                _timePickerDialog.Dismiss();
            }, DateTime.Now.Hour, DateTime.Now.Minute, Android.Text.Format.DateFormat.Is24HourFormat(this));
            _etDateTime.ShowSoftInputOnFocus = false;
            _etDateTime.FocusableInTouchMode = false;
        }

        public override void OnBackPressed()
        {
            ViewModel.BackCommand.Execute(null);
        }

        private void GetReferences()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbarTitle = FindViewById<TextView>(Resource.Id.toolbarTitle);
            _llContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);
            _btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            _txtCategory = FindViewById<TextView>(Resource.Id.txtCategory);
            _txtProduct = FindViewById<TextView>(Resource.Id.txtProduct);
            _txtDateTime = FindViewById<TextView>(Resource.Id.txtDateTime);
            _txtMessage = FindViewById<TextView>(Resource.Id.txtMessage);
            _txtUnits = FindViewById<TextView>(Resource.Id.txtUnits);
            _etDateTime = FindViewById<EditText>(Resource.Id.etDateTime);
            _etMessage = FindViewById<EditText>(Resource.Id.etMessage);
            _etUnits = FindViewById<EditText>(Resource.Id.etUnits);
            _spnCategory = FindViewById<MvxSpinner>(Resource.Id.spnCategory);
            _spnProduct = FindViewById<MvxSpinner>(Resource.Id.spnProduct);
            _txtTotalValue = FindViewById<TextView>(Resource.Id.txtTotalValue);

            _llPhotosList = FindViewById<LinearLayout>(Resource.Id.llPhotosList);
            _llPrintList = FindViewById<LinearLayout>(Resource.Id.llPrintList);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Categories")
            {
                if (ViewModel.Categories != null)
                {
                }
            }
            if (e.PropertyName == "CurrentOrder")
            {
                _etDateTime.Text = ViewModel.CurrentOrder.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
                _txtTotalValue.Text = ViewModel.CurrentOrder.TotalAmount.ToString();
            }
            if (e.PropertyName == "Print")
            {
                if (ViewModel.Print != null)
                {
                    var bmp = BitmapFactory.DecodeFile(ViewModel.Print.Media.Url);
                    var dimens = AndroidHelper.GetScreenDimens(this, false);
                    var resizedBmp = AndroidHelper.GetResizedBitmap(bmp, dimens[0], 0);
                    var ivPrint = new ImageView(this)
                    {
                        LayoutParameters = new LinearLayout.LayoutParams(resizedBmp.Width, resizedBmp.Height)
                        {
                            Gravity = GravityFlags.Center
                        }
                    };
                    ivPrint.Click += IvPrint_Click;
                    ivPrint.SetImageBitmap(resizedBmp);
                    _llPrintList.RemoveAllViews();
                    _llPrintList.AddView(ivPrint);
                }
            }
            if (e.PropertyName == "Photos")
            {
                if (ViewModel.Photos?.Count > 0)
                {
                    _llPhotosList.RemoveAllViews();
                    foreach (var photo in ViewModel.Photos)
                    {
                        var bmp = BitmapFactory.DecodeFile(photo.Url);
                        var dimens = AndroidHelper.GetScreenDimens(this, false);
                        var resizedBmp = AndroidHelper.GetResizedBitmap(bmp, dimens[0], 0);
                        var ivParams = new LinearLayout.LayoutParams(resizedBmp.Width, resizedBmp.Height)
                        {
                            Gravity = GravityFlags.Center,
                        };
                        ivParams.SetMargins(0, 0, 0, AndroidHelper.ConvertDpToPx(5));
                        var ivMedia = new ImageView(this)
                        {
                            LayoutParameters = ivParams,
                            Tag = photo.Url
                        };
                        ivMedia.Click += IvMedia_Click;
                        ivMedia.SetImageBitmap(resizedBmp);
                        _llPhotosList.AddView(ivMedia);
                    }
                }
            }
        }

        private void IvMedia_Click(object sender, EventArgs e)
        {
            var iv = sender as ImageView;
            if(iv != null)
            {
                var path = iv.Tag.ToString();
                if(!string.IsNullOrEmpty(path))
                    ViewModel.FullScreenCommand.Execute(path);
            }
        }

        private void IvPrint_Click(object sender, EventArgs e)
        {
            ViewModel.FullScreenCommand.Execute(ViewModel.Print.Media.Url);
        }
    }
}