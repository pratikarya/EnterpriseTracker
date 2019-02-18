using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.ViewModels.Orders;
using EnterpriseTracker.Droid.Fragments;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace EnterpriseTracker.Droid
{
    public class PhonePresenter : MvxAndroidViewPresenter
    {
        public Fragment CurrentFragment { get; set; }

        public PhonePresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            if(request.ViewModelType == typeof(UpdateOrderDialogViewModel))
            {
                var frag = new UpdateOrderDialogFragment();
                var vm = (request as MvxViewModelInstanceRequest).ViewModelInstance;
                frag.ViewModel = (UpdateOrderDialogViewModel)vm;
                frag.Show(CurrentFragmentManager, "UpdateOrderDialog");
                CurrentFragment = frag;
                return Task.FromResult(true);
            }
            else
            {
                return base.Show(request);
            }
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            if(viewModel.GetType() == typeof(UpdateOrderDialogViewModel))
            {
                if (CurrentFragment != null)
                    (CurrentFragment as UpdateOrderDialogFragment).Dismiss();
            }
            return base.Close(viewModel);
        }
    }
}