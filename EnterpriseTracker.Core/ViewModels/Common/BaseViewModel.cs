using System;
using System.Threading.Tasks;
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
