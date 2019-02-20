using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Droid.ViewHolders;
using MvvmCross.Droid.Support.V7.RecyclerView;
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
            return new OrderViewHolder(view, itemBindingContext)
            {
                Click = ItemClick,
                LongClick = ItemLongClick
            };
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