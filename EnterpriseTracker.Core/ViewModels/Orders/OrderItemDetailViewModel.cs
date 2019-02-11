using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.ViewModels.Common;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class OrderItemDetailViewModel : BaseViewModel<OrderItemDto, OrderItemDto>
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

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

        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                Products = value.Products;
                SelectedProduct = Products.First();
                RaisePropertyChanged(() => SelectedCategory);
            }
        }

        private List<ProductDto> _products;
        public List<ProductDto> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        private ProductDto _selectedProduct;
        public ProductDto SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged(() => SelectedProduct);
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
                    CurrentOrderItem.Product = SelectedProduct;

                    //TODO : Proper order adding validations. For example id check, quantity check, product check, message check, etc.
                    if (ValidateOrderItem())
                    {
                        if (IsNewOrderItem)
                            CurrentOrderItem.CreatedDate = DateTime.Now;
                        CurrentOrderItem.ModifiedDate = DateTime.Now;
                        NavigationService.Close(this, CurrentOrderItem);
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
            string message = "";

            if (CurrentOrderItem.Units == 0.0f)
            {
                message += "Quantity must be specified.\n";
            }

            var isValid = string.IsNullOrEmpty(message);

            if(!isValid)
                UIService.ShowErrorDialog(message);

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
                    if (res?.IsValid == true && res.Result != null)
                    {
                        Categories = res.Result;
                        InitDefaults();
                    }
                }
                catch (Exception ex)
                {

                }
                UIService.ShowDialog(false);
            });
        }

        private void InitDefaults()
        {
            if(IsNewOrderItem)
            {
                SelectedCategory = Categories.First();                
                SelectedProduct = Products.First();
                CurrentOrderItem.Units = 0.5f;
                CurrentOrderItem.Time = DateTime.Now;
            }
            else
            {
                SelectedCategory = Categories.First(x => x.Products.Any(y => y.Id == CurrentOrderItem.Product.Id));
                SelectedProduct = Products.First(x => x.Id == CurrentOrderItem.Product.Id);

                var selectedCatIndex = Categories.IndexOf(SelectedCategory);
                var selectedProdIndex = Products.IndexOf(SelectedProduct);

                Categories.RemoveAt(selectedCatIndex);
                Categories.Insert(0, SelectedCategory);

                Products.RemoveAt(selectedProdIndex);
                Products.Insert(0, SelectedProduct);

                SelectedCategory = Categories.First();
                SelectedProduct = Products.First();
            }
            RaisePropertyChanged(() => CurrentOrderItem);
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
