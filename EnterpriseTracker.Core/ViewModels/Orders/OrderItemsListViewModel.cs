using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.ViewModels.Common;
using MvvmCross.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class OrderItemsListViewModel : BaseViewModel<OrderDto, OrderDto>
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

        public OrderItemsListViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;
        }
               
        public override void PrepareImpl(OrderDto param)
        {
            IsNewOrder = param == null;
            if (!IsNewOrder)
            {
                CurrentOrder = param as OrderDto;
            }
        }

        public override Task Initialize()
        {
            ButtonText = IsNewOrder ? "Create" : "Update";
            return base.Initialize();
        }

        string errorMessage = "";
        public bool IsNewOrder { get; set; }

        private bool _isOrderUpdated;
        public bool IsOrderUpdated
        {
            get { return _isOrderUpdated; }
            set
            {
                _isOrderUpdated = value;
                RaisePropertyChanged(() => IsOrderUpdated);
            }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                RaisePropertyChanged(() => ButtonText);
            }
        }

        private OrderDto _currentOrder = new OrderDto();
        public OrderDto CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                _currentOrder = value;
                RaisePropertyChanged(() => CurrentOrder);
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
                orderItem = updatedOrderItem;
                RaisePropertyChanged(() => CurrentOrder);
                IsOrderUpdated = true;
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
                CurrentOrder.Items.Add(newOrderItem);
                RaisePropertyChanged(() => CurrentOrder);
                IsOrderUpdated = true;
            }
        }
        
        private MvxCommand _updateOrderCommand;
        public ICommand UpdateOrderCommand
        {
            get
            {
                _updateOrderCommand = _updateOrderCommand ?? new MvxCommand(DoUpdateOrder);
                return _updateOrderCommand;
            }
        }

        private void DoUpdateOrder()
        {
            Task.Run(() =>
            {
                UIService.ShowDialog(true);
                try
                {
                    //TODO : Proper order adding validations. For example id check, quantity check, product check, message check, etc.
                    if (ValidateOrder())
                    {
                        if (IsNewOrder)
                            CurrentOrder.CreatedDate = DateTime.Now;
                        CurrentOrder.ModifiedDate = DateTime.Now;

                        var res = RealmService.UpdateOrder(new SearchDto<OrderDto> { RequestDto = CurrentOrder });
                        if (res.IsValid)
                        {
                            CurrentOrder = res.Result;
                            NavigationService.Close(this, CurrentOrder);
                        }
                    }
                    else
                    {
                        UIService.ShowErrorDialog(errorMessage);
                    }
                }
                catch (Exception ex)
                {

                }
                UIService.ShowDialog(false);
            });
        }

        private bool ValidateOrder()
        {
            bool isValid = false;

            if (CurrentOrder.Items.Count == 0)
            {
                errorMessage += "*Product must be selected.\n";
            }

            isValid = string.IsNullOrEmpty(errorMessage);

            return isValid;
        }

        private MvxCommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                _backCommand = _backCommand ?? new MvxCommand(GoBack);
                return _backCommand;
            }
        }

        private void GoBack()
        {
            if (IsOrderUpdated)
                UIService.ShowConfirmationDialog(() => { UpdateOrderCommand.Execute(null); }, () => { NavigationService.Close(this, null); }, "Save changes ?");
            else
                NavigationService.Close(this, null);
        }
    }
}
