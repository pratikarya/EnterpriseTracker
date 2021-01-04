using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.ViewModels.Orders;
using EnterpriseTracker.Droid.Controls;
using EnterpriseTracker.Droid.Utility;

namespace EnterpriseTracker.Droid.Fragments
{
    public class UpdateOrderDialogFragment : BaseDialogFragment<UpdateOrderDialogViewModel>
    {
        LinearLayout _llMain, _llContainer;
        Button _btnDone;
        OrderStatusControl _control;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.update_order_dialog, container, false);

            _llMain = view.FindViewById<LinearLayout>(Resource.Id.llMain);
            _llContainer = view.FindViewById<LinearLayout>(Resource.Id.llContainer);
            _btnDone = view.FindViewById<Button>(Resource.Id.btnDone);
            _btnDone.Click += BtnDone_Click;
            _control = new OrderStatusControl(Activity, ViewModel.CurrentOrder.Status, ViewModel.CurrentOrder.ToString());
            _control.StatusChanged = StatusChanged;
            _llContainer.AddView(_control);
            return view;
        }

        private void StatusChanged(object sender, EventArgs e)
        {
            ViewModel.CurrentOrder.Status = _control.SelectedStatus;
        }

        public override void OnStart()
        {
            base.OnStart();
            var metrics = Resources.DisplayMetrics;
            var widthInDp = AndroidHelper.ConvertPxToDp(metrics.WidthPixels) / 2;
            var heightInDp = AndroidHelper.ConvertPxToDp(metrics.HeightPixels) / 2;
            if (Dialog != null)
                Dialog.Window.SetLayout(metrics.WidthPixels - 20, metrics.HeightPixels/2 + 20);
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            ViewModel.UpdateCommand.Execute(null);
        }
    }
}