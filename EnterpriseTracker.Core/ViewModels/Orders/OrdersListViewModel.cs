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
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class OrdersListViewModel : BaseViewModel
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

        public OrdersListViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;
        }

        private List<OrderDto> _orders = new List<OrderDto>();
        public List<OrderDto> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                RaisePropertyChanged(() => Orders);
            }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                LoadOrdersCommand.Execute(null);
                RaisePropertyChanged(() => SelectedDate);
            }
        }

        private MvxCommand<OrderDto> _selectOrderCommand;
        public ICommand SelectOrderCommand
        {
            get
            {
                _selectOrderCommand = _selectOrderCommand ?? new MvxCommand<OrderDto>(DoSelectOrder);
                return _selectOrderCommand;
            }
        }

        private async void DoSelectOrder(OrderDto order)
        {
            var updatedOrder = await NavigationService.Navigate<OrderDetailViewModel, OrderDto, OrderDto>(order);
            if(updatedOrder != null)
            {
                LoadOrdersCommand.Execute(null);
            }
        }

        private MvxCommand _addOrderCommand;
        public ICommand AddOrderCommand
        {
            get
            {
                _addOrderCommand = _addOrderCommand ?? new MvxCommand(DoAddOrder);
                return _addOrderCommand;
            }
        }

        private async void DoAddOrder()
        {
            var newOrder = await NavigationService.Navigate<OrderDetailViewModel, OrderDto, OrderDto>(null);
            if(newOrder != null)
            {
                LoadOrdersCommand.Execute(null);
            }
        }
        
        private MvxCommand<OrderDto> _longClickCommand;
        public ICommand LongClickCommand
        {
            get
            {
                _longClickCommand = _longClickCommand ?? new MvxCommand<OrderDto>(DoLongClick);
                return _longClickCommand;
            }
        }

        private void DoLongClick(OrderDto obj)
        {
            Task.Run(() => LongClick(obj));
        }

        private async Task LongClick(OrderDto order)
        {
            var updated = await NavigationService.Navigate<UpdateOrderDialogViewModel, OrderDto, OrderDto>(order);
            if(updated != null)
            {
                LoadOrdersCommand.Execute(null);
            }
        }

        private MvxCommand _loadOrdersCommand;
        public ICommand LoadOrdersCommand
        {
            get
            {
                _loadOrdersCommand = _loadOrdersCommand ?? new MvxCommand(DoLoadOrders);
                return _loadOrdersCommand;
            }
        }

        private void DoLoadOrders()
        {
            Task.Run(() =>
            {
                UIService.ShowLoadingDialog(true);
                try
                {
                    var searchDto = new OrdersSearchDto
                    {
                        Date = SelectedDate
                    };
                    var res = RealmService.GetOrders(searchDto);
                    if (res.IsValid && res.Result != null)
                    {
                        Orders = res.Result;
                    }
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
                }
                UIService.ShowLoadingDialog(false);
            });
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(() => IsRefreshing);
            }
        }

        public ICommand ReloadCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsRefreshing = true;
                    Task.Run(() => DoLoadOrders());
                    IsRefreshing = false;
                });
            }
        }
    }
}
