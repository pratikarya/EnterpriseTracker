using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.ViewModels.Orders;
using EnterpriseTracker.Droid.Adapters;
using EnterpriseTracker.Droid.Views.Common;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using System;

namespace EnterpriseTracker.Droid.Views.Orders
{
    [Activity(Label = "")]
    public class OrdersListView : BaseActivity<OrdersListViewModel>, IDialogInterfaceOnClickListener
    {
        LinearLayout _llNoData;
        private MvxRecyclerView _rvOrders;
        private OrdersAdapter _adapterOrders;

        private Android.Support.V7.Widget.Toolbar _toolbar;
        private TextView _toolbarTitle;
        private DatePickerDialog _datePickerDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.orders_list_view);

            GetReferences();

            SetSupportActionBar(_toolbar);
            _toolbarTitle.Text = "Orders";

            var layoutManager = new LinearLayoutManager(this);
            layoutManager.Orientation = LinearLayoutManager.Vertical;
            _rvOrders.SetLayoutManager(layoutManager);
            _adapterOrders = new OrdersAdapter((IMvxAndroidBindingContext)BindingContext);
            _rvOrders.Adapter = _adapterOrders;
            var mDividerDecoration = new DividerItemDecoration(_rvOrders.Context, layoutManager.Orientation);
            _rvOrders.AddItemDecoration(mDividerDecoration);

            SetDateDialog();
            
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                ViewModel.LoadOrdersCommand.Execute(null);
            }
        }

        public override void OnBackPressed()
        {
            //ViewModel.BackCommand.Execute(null);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuid_calendar:
                    _datePickerDialog.Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void SetDateDialog()
        {
            _datePickerDialog = new DatePickerDialog(this, (sender, args) =>
            {
                ViewModel.SelectedDate = args.Date;
                _datePickerDialog.Dismiss();                
            }, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
            _datePickerDialog.SetButton((int)DialogButtonType.Neutral, "Clear", this);
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            _datePickerDialog.Dismiss();
            ViewModel.SelectedDate = null;
        }

        private void GetReferences()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbarTitle = FindViewById<TextView>(Resource.Id.toolbarTitle);
            _rvOrders = FindViewById<MvxRecyclerView>(Resource.Id.rvOrders);
            _llNoData = FindViewById<LinearLayout>(Resource.Id.llNoContent);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Orders")
            {
                if(ViewModel.Orders?.Count > 0)
                {
                    _adapterOrders.NotifyDataSetChanged();
                    _llNoData.Visibility = ViewStates.Gone;
                    _rvOrders.Visibility = ViewStates.Visible;
                }
                else
                {
                    _llNoData.Visibility = ViewStates.Visible;
                    _rvOrders.Visibility = ViewStates.Gone;
                }
            }
        }
    }
}