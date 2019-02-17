using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using EnterpriseTracker.Core.ViewModels.Orders;
using EnterpriseTracker.Droid.Adapters;
using EnterpriseTracker.Droid.Views.Common;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace EnterpriseTracker.Droid.Views.Orders
{
    [Activity(Label = "")]
    public class OrderItemsListView : BaseActivity<OrderItemsListViewModel>
    {
        //LinearLayout _llContactNumber;
        //EditText _etContactNumber;
        private MvxRecyclerView _rvOrderItems;
        private OrderItemsAdapter _adapterOrderItems;

        private Android.Support.V7.Widget.Toolbar _toolbar;
        private TextView _toolbarTitle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.order_items_list_view);

            GetReferences();

            SetSupportActionBar(_toolbar);
            _toolbarTitle.Text = "Order Items";

            var layoutManager = new LinearLayoutManager(this);
            layoutManager.Orientation = LinearLayoutManager.Vertical;
            _rvOrderItems.SetLayoutManager(layoutManager);
            _adapterOrderItems = new OrderItemsAdapter((IMvxAndroidBindingContext)BindingContext);
            _rvOrderItems.Adapter = _adapterOrderItems;
            var mDividerItemDecoration = new DividerItemDecoration(_rvOrderItems.Context, layoutManager.Orientation);
            _rvOrderItems.AddItemDecoration(mDividerItemDecoration);
            
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            //if (string.IsNullOrEmpty(ViewModel.CurrentOrder.ContactNumber))
            //{
            //    _llContactNumber.Visibility = Android.Views.ViewStates.Gone;
            //    _etContactNumber.Visibility = Android.Views.ViewStates.Visible;
            //}
            //else
            //{
            //    _llContactNumber.Visibility = Android.Views.ViewStates.Visible;
            //    _etContactNumber.Visibility = Android.Views.ViewStates.Gone;
            //}
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
                ViewModel.LoadOrderItemsCommand.Execute(null);
            }
        }

        public override void OnBackPressed()
        {
            //ViewModel.BackCommand.Execute(null);
        }

        private void GetReferences()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbarTitle = FindViewById<TextView>(Resource.Id.toolbarTitle);
            _rvOrderItems = FindViewById<MvxRecyclerView>(Resource.Id.rvOrderItems);
            //_llContactNumber = FindViewById<LinearLayout>(Resource.Id.llContactNumber);
            //_etContactNumber = FindViewById<EditText>(Resource.Id.etContactNumber);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentOrder")
            {
                _adapterOrderItems.NotifyDataSetChanged();
                //if(string.IsNullOrEmpty( ViewModel.CurrentOrder.ContactNumber))
                //{
                //    _llContactNumber.Visibility = Android.Views.ViewStates.Gone;
                //    _etContactNumber.Visibility = Android.Views.ViewStates.Visible;
                //}
                //else
                //{
                //    _llContactNumber.Visibility = Android.Views.ViewStates.Visible;
                //    _etContactNumber.Visibility = Android.Views.ViewStates.Gone;
                //}
            }
        }
    }
}