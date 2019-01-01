using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.Message;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.Utility;
using EnterpriseTracker.Core.ViewModels.Common;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class OrdersListViewModel : BaseViewModel
    {
        MvxSubscriptionToken _ordersListUpdatedToken;

        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

        public OrdersListViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;

            _ordersListUpdatedToken = Messenger.Subscribe<OrdersListUpdatedMessage>((message) =>
            {
                LoadOrdersCommand.Execute(null);
            });
        }

        private List<OrderDto> _orders = new List<OrderDto>();
        public List<OrderDto> Orders
        {
            get { return _orders; }
            set {
                _orders = value;
                RaisePropertyChanged(() => Orders);
            }
        }

        private MvxCommand<OrderDto> _selectOrderCommand;
        public ICommand SelectedOrderCommand
        {
            get
            {
                _selectOrderCommand = _selectOrderCommand ?? new MvxCommand<OrderDto>(DoSelectOrder);
                return _selectOrderCommand;
            }
        }

        private async void DoSelectOrder(OrderDto order)
        {
            var updatedOrder = await NavigationService.Navigate<OrderItemsListViewModel, OrderDto, OrderDto>(order);
            if (updatedOrder != null)
            {
                //If selected order was updated, reload.
                order = updatedOrder;
                RaisePropertyChanged(() => Orders);
            }
        }

        private MvxCommand _createOrderCommand;
        public ICommand CreateOrderCommand
        {
            get
            {
                _createOrderCommand = _createOrderCommand ?? new MvxCommand(DoCreateOrder);
                return _createOrderCommand;
            }
        }

        private async void DoCreateOrder()
        {
            var newOrder = await NavigationService.Navigate<OrderItemsListViewModel, OrderDto, OrderDto>(null);
            if (newOrder != null)
            {
                Orders.Add(newOrder);
                RaisePropertyChanged(() => Orders);
            }
        }

        private MvxCommand _loadOrdersCommand;
        public ICommand  LoadOrdersCommand
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
                UIService.ShowDialog(true);
                try
                {
                    var res = RealmService.GetOrders();
                    if (res.IsValid && res.Result.Count > 0)
                    {
                        Orders = res.Result.OrderBy(x => x.Time.Value).ToList();
                    }
                }
                catch (Exception ex)
                {

                }
                UIService.ShowDialog(false);
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

        public override void DisposeImpl()
        {
            base.DisposeImpl();
            if(_ordersListUpdatedToken != null)
            {
                _ordersListUpdatedToken.Dispose();
            }
        }
    }
}
