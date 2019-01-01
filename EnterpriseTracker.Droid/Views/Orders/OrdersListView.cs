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
    public class OrdersListView : BaseActivity<OrdersListViewModel>
    {
        private MvxRecyclerView _rvOrders;
        private OrdersAdapter _adapterOrders;

        private FloatingActionButton _fabCreateOrder;
        private Android.Support.V7.Widget.Toolbar _toolbar;
        private TextView _toolbarTitle;

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
            var mDividerItemDecoration = new DividerItemDecoration(_rvOrders.Context, layoutManager.Orientation);
            _rvOrders.AddItemDecoration(mDividerItemDecoration);

            _fabCreateOrder.Click += (sender, e) =>
            {
                ViewModel.CreateOrderCommand.Execute(null);
            };

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override void OnResume()
        {
            base.OnResume();
            //if(IsFirstLoad)
            //{
            //    ViewModel.LoadOrdersCommand.Execute(null);
            //    IsFirstLoad = false;
            //}
            ViewModel.LoadOrdersCommand.Execute(null);
        }

        private void GetReferences()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbarTitle = FindViewById<TextView>(Resource.Id.toolbarTitle);
            _rvOrders = FindViewById<MvxRecyclerView>(Resource.Id.rvOrders);
            _fabCreateOrder = FindViewById<FloatingActionButton>(Resource.Id.fabCreateOrder);
        }
        
        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Orders")
            {
                _adapterOrders.NotifyDataSetChanged();
            }
        }
    }
}