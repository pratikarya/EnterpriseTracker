using System;
using System.Threading.Tasks;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.Speech;
using EnterpriseTracker.Core.TextProcess;
using EnterpriseTracker.Core.UI;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace EnterpriseTracker.Core.ViewModels.Common
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        public BaseViewModel()
        {

        }

        public IMvxNavigationService NavigationService { get { return Mvx.IoCProvider.Resolve<IMvxNavigationService>(); } }
        public IMvxMessenger Messenger { get { return Mvx.IoCProvider.Resolve<IMvxMessenger>(); } }
        public IOfflineService RealmService { get { return Mvx.IoCProvider.Resolve<IOfflineService>(); } }
        public IUIService UIService { get { return Mvx.IoCProvider.Resolve<IUIService>(); } }
        public ISpeechService SpeechService { get { return Mvx.IoCProvider.Resolve<ISpeechService>(); } }
        public ITextProcessor TextProcessor { get { return Mvx.IoCProvider.Resolve<ITextProcessor>(); } }

        private string _title;
        public string Title
        {
            get { return _title; }
            set 
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }


        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        public virtual void DisposeImpl()
        {

        }

        public void Dispose()
        {
            DisposeImpl();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseViewModel()
        {
            Dispose(false);
        }
    }

    public abstract class BaseViewModel<TParam> : BaseViewModel, IMvxViewModel<TParam>
    {
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public void Prepare(TParam parameter)
        {
            PrepareImpl(parameter);
        }

        public abstract void PrepareImpl(TParam param);
    }

    public abstract class BaseViewModel<TParam, TRes> : BaseViewModel, IMvxViewModel<TParam, TRes>
    {
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public void Prepare(TParam parameter)
        {
            PrepareImpl(parameter);
        }

        public abstract void PrepareImpl(TParam param);
    }
}
