using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.RealmObjects.Order;
using EnterpriseTracker.Core.Speech;
using EnterpriseTracker.Core.TextProcess;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.Utility;
using EnterpriseTracker.Droid.Speech;
using EnterpriseTracker.Droid.UI;

using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
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
            Mvx.IoCProvider.RegisterSingleton<IOfflineService>(new RealmService());
            Mvx.IoCProvider.RegisterSingleton<IUIService>(new UIService());
            Mvx.IoCProvider.RegisterSingleton<ISpeechService>(new SpeechService());
            Mvx.IoCProvider.RegisterSingleton<ITextProcessor>(new TextProcessor());
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new PhonePresenter(AndroidViewAssemblies);
        }
    }
}