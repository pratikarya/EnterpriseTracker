using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.Order.Contract.Dto;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using System.Globalization;
using System.Linq;

namespace EnterpriseTracker.Droid.ViewHolders
{
    public class OrderViewHolder : MvxRecyclerViewHolder
    {
        public LinearLayout llMore;
        public TextView txtProductName, txtOrderTime, txtMessage, txtQuantity, txtMore, txtTotalValueMsg, txtTotalValueMore;
        public OrderViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            txtProductName = itemView.FindViewById<TextView>(Resource.Id.txtProductName);
            txtOrderTime = itemView.FindViewById<TextView>(Resource.Id.txtOrderTime);
            txtMessage = itemView.FindViewById<TextView>(Resource.Id.txtMessage);
            txtQuantity = itemView.FindViewById<TextView>(Resource.Id.txtQuantity);
            txtMore = itemView.FindViewById<TextView>(Resource.Id.txtMore);
            txtTotalValueMore = itemView.FindViewById<TextView>(Resource.Id.txtTotalValue);
            txtTotalValueMsg = itemView.FindViewById<TextView>(Resource.Id.txtTotalValueMsg);
            llMore = itemView.FindViewById<LinearLayout>(Resource.Id.llMore);
        }
        public void UpdateView(OrderDto order)
        {
            var firstItem = order.Items.First(); 
            if(order.Items.Count > 1)
            {
                llMore.Visibility = ViewStates.Visible;
                txtTotalValueMsg.Visibility = ViewStates.Gone;

                var remainingCount = order.Items.Count - 1;
                var moreText = remainingCount + " more ";
                if (remainingCount > 1)
                    moreText += "items.";
                else
                    moreText += "item.";

                txtMore.Text = moreText;
                txtTotalValueMore.Text = "Rs. " + order.TotalAmount;
            }
            else
            {
                llMore.Visibility = ViewStates.Gone;
                txtTotalValueMsg.Visibility = ViewStates.Visible;
                txtTotalValueMsg.Text = "Rs. " + order.TotalAmount.ToString();
            }

            txtProductName.Text = firstItem.Product.Name;
            txtOrderTime.Text = firstItem.Time.ToString("ddd d MMM - hh : mm tt", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(firstItem.Message))
            {
                txtMessage.Text = firstItem.Message;
                txtMessage.Visibility = ViewStates.Visible;
            }
            else
            {
                txtMessage.Text = "";
                txtMessage.Visibility = ViewStates.Gone;
            }
            txtQuantity.Text = firstItem.Units + firstItem.Product.Unit;
        }
    }
}