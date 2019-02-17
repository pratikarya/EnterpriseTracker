using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.ViewModels.Common;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class OrderItemsListViewModel : BaseViewModel
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

        public OrderItemsListViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;
        }

        //private OrderDto _currentOrder = new OrderDto();
        //public OrderDto CurrentOrder
        //{
        //    get { return _currentOrder; }
        //    set
        //    {
        //        _currentOrder = value;
        //        RaisePropertyChanged(() => CurrentOrder);
        //    }
        //}
               
        private List<OrderItemDto> _orderItems = new List<OrderItemDto>();
        public List<OrderItemDto> OrderItems
        {
            get { return _orderItems; }
            set
            {
                _orderItems = value;
                RaisePropertyChanged(() => OrderItems);
            }
        }

        private MvxCommand<OrderItemDto> _selectOrderItemCommand;
        public ICommand SelectOrderItemCommand
        {
            get
            {
                _selectOrderItemCommand = _selectOrderItemCommand ?? new MvxCommand<OrderItemDto>(DoSelectOrderItem);
                return _selectOrderItemCommand;
            }
        }

        private async void DoSelectOrderItem(OrderItemDto orderItem)
        {
            var updatedOrderItem = await NavigationService.Navigate<OrderItemDetailViewModel, OrderItemDto, OrderItemDto>(orderItem);
            if(updatedOrderItem != null)
            {
                LoadOrderItemsCommand.Execute(null);
            }
        }

        private MvxCommand _addOrderItemCommand;
        public ICommand AddOrderItemCommand
        {
            get
            {
                _addOrderItemCommand = _addOrderItemCommand ?? new MvxCommand(DoAddOrderItem);
                return _addOrderItemCommand;
            }
        }

        private async void DoAddOrderItem()
        {
            var newOrderItem = await NavigationService.Navigate<OrderItemDetailViewModel, OrderItemDto, OrderItemDto>(null);
            if(newOrderItem != null)
            {
                LoadOrderItemsCommand.Execute(null);
            }
        }

        private MvxCommand _loadOrderItemsCommand;
        public ICommand LoadOrderItemsCommand
        {
            get
            {
                _loadOrderItemsCommand = _loadOrderItemsCommand ?? new MvxCommand(DoLoadOrderItems);
                return _loadOrderItemsCommand;
            }
        }

        private void DoLoadOrderItems()
        {
            Task.Run(() =>
            {
                UIService.ShowDialog(true);
                try
                {
                    var res = RealmService.GetOrderItems();
                    if (res.IsValid && res.Result.Count > 0)
                    {
                        OrderItems = res.Result;
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
                    Task.Run(() => DoLoadOrderItems());
                    IsRefreshing = false;
                });
            }
        }
    }
}
