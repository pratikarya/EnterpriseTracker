using System;
using System.Globalization;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.ViewModels.Orders;
using EnterpriseTracker.Droid.Controls;
using EnterpriseTracker.Droid.Utility;
using EnterpriseTracker.Droid.Views.Common;
using MvvmCross.Binding.BindingContext;

namespace EnterpriseTracker.Droid.Views.Orders
{
    [Activity(Label = "")]
    public class OrderItemDetailView : BaseActivity<OrderItemDetailViewModel>
    {
        Android.Support.V7.Widget.Toolbar _toolbar;
        TextView _toolbarTitle;
        LinearLayout _llContainer;

        EditText _etUnits, _etMessage, _etDateTime, _etDetails;
        Button _btnCreate;
        DatePickerDialog _datePickerDialog;
        TimePickerDialog _timePickerDialog;
        DropdownControl<CategoryDto> _dropdownCategory;
        DropdownControl<ProductDto> _dropdownProduct;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.order_item_detail_view);

            GetReferences();

            SetSupportActionBar(_toolbar);

            _toolbarTitle.Text = "OrderItem Detail";

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (IsFirstLoad)
            {
                ViewModel.LoadCommand.Execute(null);
                IsFirstLoad = false;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _dropdownCategory.Dispose();
            _dropdownProduct.Dispose();
        }

        public override void OnBackPressed()
        {
            ViewModel.BackCommand.Execute(null);
        }

        private void CreateUi()
        {
            _dropdownCategory = new DropdownControl<CategoryDto>(this, "Category");
            _dropdownCategory.SetData(ViewModel.Categories);
            _dropdownCategory.OnValueSelected += _dropdownCategory_OnValueSelected;

            _etUnits = new EditText(this);
            _etUnits.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal | Android.Text.InputTypes.NumberFlagSigned;
            _etUnits.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            _etUnits.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            {
                BottomMargin = AndroidHelper.ConvertDpToPx(8)
            };
            _etUnits.TextSize = AndroidHelper.CurrentTheme.TextSize;
            _etUnits.Hint = "Kg";

            _etDateTime = new EditText(this);
            _etDateTime.ShowSoftInputOnFocus = false;
            _etDateTime.FocusableInTouchMode = false;
            _etDateTime.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            {
                BottomMargin = AndroidHelper.ConvertDpToPx(8)
            };
            _etDateTime.TextSize = AndroidHelper.CurrentTheme.TextSize;
            _etDateTime.Hint = "Date and time";
            _datePickerDialog = new DatePickerDialog(this, (sender, args) =>
            {
                ViewModel.CurrentOrderItem.Time = args.Date;
                _datePickerDialog.Dismiss();
                _timePickerDialog.Show();
            }, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            _etDateTime.Click += (sender, args) =>
            {
                _datePickerDialog.Show();
            };
            _timePickerDialog = new TimePickerDialog(this, (sender, args) =>
            {
                ViewModel.CurrentOrderItem.Time = ViewModel.CurrentOrderItem.Time.AddHours(args.HourOfDay);
                ViewModel.CurrentOrderItem.Time = ViewModel.CurrentOrderItem.Time.AddMinutes(args.Minute);
                ViewModel.CurrentOrderItem.Time.AddHours(args.HourOfDay);
                ViewModel.CurrentOrderItem.Time.AddMinutes(args.Minute);
                _etDateTime.Text = ViewModel.CurrentOrderItem.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
                _timePickerDialog.Dismiss();
            }, DateTime.Now.Hour, DateTime.Now.Minute, Android.Text.Format.DateFormat.Is24HourFormat(this));

            //var img = Resources.GetDrawable(Resource.Drawable.ic_clock);
            //img.SetBounds(0, 0, 60, 60);
            //_etTime.SetCompoundDrawables(null, null, img, null);

            _etMessage = new EditText(this);
            _etMessage.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            {
                BottomMargin = AndroidHelper.ConvertDpToPx(8)
            };
            _etMessage.Hint = "Message";
            _etMessage.TextSize = AndroidHelper.CurrentTheme.TextSize;
            
            _etDetails = new EditText(this);
            _etDetails.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            {
                BottomMargin = AndroidHelper.ConvertDpToPx(8)
            };
            _etDetails.Hint = "Extra Details";
            _etDetails.TextSize = AndroidHelper.CurrentTheme.TextSize;

            _llContainer.AddView(_dropdownCategory);
            _llContainer.AddView(_etUnits);
            _llContainer.AddView(_etDateTime);
            _llContainer.AddView(_etMessage);
            _llContainer.AddView(_etDetails);
        }

        private void InitDefaults()
        {
            if(ViewModel.IsNewOrderItem)
            {
                _dropdownCategory.SelectedData = ViewModel.Categories.First();
                _dropdownProduct.SelectedData = _dropdownCategory.SelectedData.Products.First();
                ViewModel.CurrentOrderItem.Units = 0.5f;
                ViewModel.CurrentOrderItem.Time = DateTime.Now;
                _etDateTime.Text = ViewModel.CurrentOrderItem.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
            }
            else
            {
                _dropdownCategory.SelectedData = ViewModel.Categories.FirstOrDefault(x => x.Products.Any(y => y.Id == ViewModel.CurrentOrderItem.Product.Id));
                _dropdownProduct.SelectedData = ViewModel.CurrentOrderItem.Product;
                _etMessage.Text = ViewModel.CurrentOrderItem.Message;
                _etDateTime.Text = ViewModel.CurrentOrderItem.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
                _etDetails.Text = ViewModel.CurrentOrderItem.Details;
            }
        }

        private void CreateBinding()
        {
            var set = this.CreateBindingSet<OrderItemDetailView, OrderItemDetailViewModel>();
            set.Bind(_etMessage).To(vm => vm.CurrentOrderItem.Message);
            set.Bind(_etUnits).To(vm => vm.CurrentOrderItem.Units);
            set.Bind(_etDetails).To(vm => vm.CurrentOrderItem.Details);
            set.Apply();
        }

        private void _dropdownCategory_OnValueSelected(CategoryDto value)
        {
            if (value != null)
            {
                if (_dropdownProduct == null)
                {
                    _dropdownProduct = new DropdownControl<ProductDto>(this, "Product");
                    _dropdownProduct.OnValueSelected += _dropdownProduct_OnValueSelected;
                    var indexCategory = _llContainer.IndexOfChild(_dropdownCategory);
                    _llContainer.AddView(_dropdownProduct, indexCategory + 1);
                }
                _dropdownProduct.SetData(value.Products);
            }
        }

        private void _dropdownProduct_OnValueSelected(ProductDto value)
        {
            if (value != null)
                ViewModel.CurrentOrderItem.Product = value;
        }

        private void GetReferences()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbarTitle = FindViewById<TextView>(Resource.Id.toolbarTitle);
            _llContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);
            _btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Categories")
            {
                CreateUi();
                InitDefaults();
                CreateBinding();
            }
        }
    }
}