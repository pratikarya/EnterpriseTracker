using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
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
    public class OrderItemDetailViewModel : BaseViewModel<OrderItemDto, OrderItemDto>
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }
        string message = "";

        public OrderItemDetailViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;
        }

        public bool IsNewOrderItem { get; set; }

        public override void PrepareImpl(OrderItemDto param)
        {
            IsNewOrderItem = param == null;
            if (!IsNewOrderItem)
            {
                CurrentOrderItem = param;

                //CurrentOrderItem = new OrderItemDto
                //{
                //    Id = param.Id,
                //    AdditionalCharge = param.AdditionalCharge,
                //    CreatedDate = param.CreatedDate,
                //    Customer = param.Customer,
                //    DeliveryCharge = param.DeliveryCharge,
                //    DesignCharge = param.DesignCharge,
                //    Details = param.Details,
                //    Images = param.Images,
                //    Message = param.Message,
                //    ModifiedDate = param.ModifiedDate,
                //    Owner = param.Owner,
                //    PrintCharge = param.PrintCharge,
                //    Product = param.Product,
                //    Status = param.Status,
                //    Time = param.Time,
                //    Units = param.Units
                //};
            }
        }

        public override Task Initialize()
        {
            ButtonText = IsNewOrderItem ? "Create" : "Update";
            return base.Initialize();
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

        private OrderItemDto _currentOrderItem = new OrderItemDto();
        public OrderItemDto CurrentOrderItem
        {
            get { return _currentOrderItem; }
            set
            {
                _currentOrderItem = value;
                RaisePropertyChanged(() => CurrentOrderItem);
            }
        }

        private List<CategoryDto> _categories;
        public List<CategoryDto> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged(() => Categories);
            }
        }

        private MvxCommand _createCommand;
        public ICommand CreateCommand
        {
            get
            {
                _createCommand = _createCommand ?? new MvxCommand(DoCreateCommand);
                return _createCommand;
            }
        }

        private void DoCreateCommand()
        {
            Task.Run(() =>
            {
                UIService.ShowDialog(true);
                try
                {
                    //TODO : Proper order adding validations. For example id check, quantity check, product check, message check, etc.
                    if (ValidateOrderItem())
                    {
                        if (IsNewOrderItem)
                            CurrentOrderItem.CreatedDate = DateTime.Now;
                        CurrentOrderItem.ModifiedDate = DateTime.Now;
                        NavigationService.Close(this, CurrentOrderItem);
                    }
                    else
                    {
                        UIService.ShowErrorDialog(message);
                    }
                }
                catch (Exception ex)
                {

                }
                UIService.ShowDialog(false);
            });
        }

        private bool ValidateOrderItem()
        {
            bool isValid = false;

            if (CurrentOrderItem.Units == 0.0f)
            {
                message += "Quantity must be specified.\n";
            }

            isValid = string.IsNullOrEmpty(message);

            return isValid;
        }

        private MvxCommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                _loadCommand = _loadCommand ?? new MvxCommand(DoLoadCommand);
                return _loadCommand;
            }
        }

        private void DoLoadCommand()
        {
            Task.Run(() =>
            {
                UIService.ShowDialog(true);
                try
                {
                    var res = RealmService.GetCategories();
                    if (res.IsValid)
                    {
                        Categories = res.Result;
                    }
                }
                catch (Exception ex)
                {

                }
                UIService.ShowDialog(false);
            });
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
            NavigationService.Close(this, null);
        }
    }
}
