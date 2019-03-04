using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.ViewModels.Common;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class UpdateOrderDialogViewModel : BaseViewModel<OrderDto, OrderDto>
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

        public UpdateOrderDialogViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;
        }

        public override void PrepareImpl(OrderDto param)
        {
            CurrentOrder = param;
        }

        private OrderDto _currentOrder;
        public OrderDto CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                _currentOrder = value;
                RaisePropertyChanged(() => CurrentOrder);
            }
        }

        private MvxCommand _updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                _updateCommand = _updateCommand ?? new MvxCommand(DoUpdateCommand);
                return _updateCommand;
            }
        }

        private void DoUpdateCommand()
        {
            Task.Run(() => DoUpdate());
        }

        private async Task DoUpdate()
        {
            try
            {
                var res = RealmService.UpdateOrder(new SearchDto<OrderDto>
                {
                    RequestDto = CurrentOrder
                });
                if(res?.IsValid == true)
                {
                    await NavigationService.Close(this, CurrentOrder);
                }
            }
            catch(Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
            }
        }
    }
}
