using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace EnterpriseTracker.Droid.ViewHolders
{
    public class OrderItemViewHolder : MvxRecyclerViewHolder
    {
        public TextView txtProductName, txtOrderTime, txtMessage, txtQuantity, txtTotalValue;
        public OrderItemViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            txtProductName = itemView.FindViewById<TextView>(Resource.Id.txtProductName);
            txtOrderTime = itemView.FindViewById<TextView>(Resource.Id.txtOrderTime);
            txtMessage = itemView.FindViewById<TextView>(Resource.Id.txtMessage);
            txtQuantity = itemView.FindViewById<TextView>(Resource.Id.txtQuantity);
            txtTotalValue = itemView.FindViewById<TextView>(Resource.Id.txtTotalValue);
        }
        public void UpdateView(OrderItemDto orderItem)
        {
            txtProductName.Text = orderItem.Product.Name;
            txtOrderTime.Text = orderItem.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
            txtTotalValue.Text = "Rs. " + orderItem.TotalAmount;

            if (!string.IsNullOrEmpty(orderItem.Message))
            {
                txtMessage.Text = orderItem.Message;
                txtMessage.Visibility = ViewStates.Visible;
            }
            else
            {
                txtMessage.Text = ""; 
                txtMessage.Visibility = ViewStates.Gone;
            }
            txtQuantity.Text = orderItem.Units + orderItem.Product.Unit;
        }
    }
}