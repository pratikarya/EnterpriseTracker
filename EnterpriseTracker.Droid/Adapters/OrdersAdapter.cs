using Android.Views;

using AndroidX.RecyclerView.Widget;

using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Droid.ViewHolders;

using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace EnterpriseTracker.Droid.Adapters
{
    public class OrdersAdapter : MvxRecyclerAdapter
    {
        public OrdersAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, viewType, itemBindingContext);
            var vh = new OrderViewHolder(view, itemBindingContext)
            {
            };
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

            var order = (OrderDto)GetItem(position);
            var h = (OrderViewHolder)holder;
            h.UpdateView(order);
        }
    }
}