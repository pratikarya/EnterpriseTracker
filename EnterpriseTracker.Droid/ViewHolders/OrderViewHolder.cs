using System.Globalization;

using Android.Views;
using Android.Widget;

using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;

using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace EnterpriseTracker.Droid.ViewHolders
{
    public class OrderViewHolder : MvxRecyclerViewHolder
    {
        public TextView txtProductName, txtOrderTime, txtMessage, txtTotalValue, txtStatus;
        public OrderViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            txtProductName = itemView.FindViewById<TextView>(Resource.Id.txtProductName);
            txtOrderTime = itemView.FindViewById<TextView>(Resource.Id.txtOrderTime);
            txtMessage = itemView.FindViewById<TextView>(Resource.Id.txtMessage);
            txtTotalValue = itemView.FindViewById<TextView>(Resource.Id.txtTotalValue);
            txtStatus = itemView.FindViewById<TextView>(Resource.Id.txtStatus);
        }
        public void UpdateView(OrderDto orderItem)
        {
            txtProductName.Text = orderItem.Product.Name + " " + orderItem.Units + orderItem.Product.Unit;
            //txtOrderTime.Text = orderItem.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
            txtOrderTime.Text = orderItem.Time.ToString("h : mm tt", CultureInfo.InvariantCulture);
            txtTotalValue.Text = "Rs. " + orderItem.TotalAmount;

            txtMessage.Visibility = ViewStates.Visible;
            if (!string.IsNullOrEmpty(orderItem.Message))
            {
                txtMessage.Text = orderItem.Message;
                //txtMessage.Visibility = ViewStates.Visible;
            }
            else
            {
                txtMessage.Text = "No Message"; 
                //txtMessage.Visibility = ViewStates.Gone;
            }

            if (orderItem.Status == OrderStatus.Confirmed)
                txtStatus.SetBackgroundResource(Resource.Color.confirmedColor);
            else if (orderItem.Status == OrderStatus.Preparing)
                txtStatus.SetBackgroundResource(Resource.Color.preparingColor);
            else if(orderItem.Status == OrderStatus.Ready)
                txtStatus.SetBackgroundResource(Resource.Color.readyColor);
            else if(orderItem.Status == OrderStatus.Completed)
                txtStatus.SetBackgroundResource(Resource.Color.completedColor);
            else if(orderItem.Status == OrderStatus.Cancelled)
                txtStatus.SetBackgroundResource(Resource.Color.cancelledColor);
        }
    }
}