using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.RealmObjects.Order;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.Utility;
using EnterpriseTracker.Droid.UI;
using EnterpriseTracker.Droid.Utility;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace EnterpriseTracker.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup()
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IMvxMessenger, MvxMessengerHub>();

            Mvx.IoCProvider.RegisterSingleton<IMvxLog>(new CustomLogger());
            Mvx.IoCProvider.RegisterSingleton<IRealmService>(new RealmService());
            Mvx.IoCProvider.RegisterSingleton<IUIService>(new UIService());
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new PhonePresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
        }
    }
}