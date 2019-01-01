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
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;

namespace EnterpriseTracker.Droid
{
    public class PhonePresenter : MvxAndroidViewPresenter
    {
        public PhonePresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return base.Show(request);
        }
    }
}