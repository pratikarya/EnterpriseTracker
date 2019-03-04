using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Media.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.UI;
using EnterpriseTracker.Core.Utility;
using EnterpriseTracker.Core.ViewModels.Common;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseTracker.Core.ViewModels.Orders
{
    public class OrderDetailViewModel : BaseViewModel<OrderDto, OrderDto>
    {
        public IRealmService RealmService { get; set; }
        public IUIService UIService { get; set; }

        public OrderDetailViewModel(IRealmService realmService, IUIService uiService)
        {
            RealmService = realmService;
            UIService = uiService;
        }

        public bool IsNewOrder { get; set; }

        public override void PrepareImpl(OrderDto param)
        {
            IsNewOrder = param == null;
            if (!IsNewOrder)
            {
                CurrentOrder = param;

                //CurrentOrder = new OrderDto
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
            ButtonText = IsNewOrder ? "Create" : "Update";
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

        private List<CategoryDto> _categories = new List<CategoryDto>();
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

        private List<OrderStatus> _statusList;
        public List<OrderStatus> StatusList
        {
            get { return _statusList; }
            set
            {
                _statusList = value;
                RaisePropertyChanged(() => StatusList);
            }
        }

        private List<MediaDto> _photos;
        public List<MediaDto> Photos
        {
            get { return _photos; }
            set
            {
                _photos = value;
                RaisePropertyChanged(() => Photos);
            }
        }

        private PrintDto _print;
        public PrintDto Print
        {
            get { return _print; }
            set
            {
                _print = value;
                RaisePropertyChanged(() => Print);
            }
        }        

        private OrderStatus _selectedStatus;
        public OrderStatus SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                RaisePropertyChanged(() => SelectedStatus);
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
                UIService.ShowLoadingDialog(true);
                try
                {
                    CurrentOrder.Product = SelectedProduct;
                    CurrentOrder.Status = SelectedStatus;
                    if (Print != null)
                        CurrentOrder.Print = Print;
                    if (Photos != null)
                        CurrentOrder.MediaList = Photos;

                    //TODO : Proper order adding validations. For example id check, quantity check, product check, message check, etc.
                    if (ValidateOrder())
                    {
                        if (IsNewOrder)
                            CurrentOrder.CreatedDate = DateTime.Now;
                        CurrentOrder.ModifiedDate = DateTime.Now;
                        var res = RealmService.UpdateOrder(new Core.Common.Contract.Dto.SearchDto<OrderDto>
                        {
                            RequestDto = CurrentOrder
                        });
                        if(res?.IsValid == true)
                        {
                            CurrentOrder = res.Result;
                            NavigationService.Close(this, CurrentOrder);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
                }
                UIService.ShowLoadingDialog(false);
            });
        }

        private bool ValidateOrder()
        {
            string message = "";

            if (CurrentOrder.Units == 0.0f)
            {
                message += "Quantity must be specified.\n";
            }

            var isValid = string.IsNullOrEmpty(message);

            if(!isValid)
                UIService.ShowErrorDialog(message);

            return isValid;
        }
        
        private MvxCommand<string> _fullScreenImageCommand;
        public ICommand FullScreenCommand
        {
            get
            {
                _fullScreenImageCommand = _fullScreenImageCommand ?? new MvxCommand<string>(DoFullScreenImageCommand);
                return _fullScreenImageCommand;
            }
        }

        private void DoFullScreenImageCommand(string imagePath)
        {
            DoFullScreenImage(imagePath);
        }

        private async void DoFullScreenImage(string imagePath)
        {
            if (imagePath == null)
                return;
            await NavigationService.Navigate<FullScreenImageViewModel, string>(imagePath);
        }

        private MvxCommand _addPrintCommand;
        public ICommand AddPrintCommand
        {
            get
            {
                _addPrintCommand = _addPrintCommand ?? new MvxCommand(DoAddPrintCommand);
                return _addPrintCommand;
            }
        }

        private void DoAddPrintCommand()
        {
            Task.Run(async() =>
            {
                UIService.ShowLoadingDialog(true);
                try
                {
                    if(CrossMedia.IsSupported && CrossMedia.Current.IsPickPhotoSupported)
                    {
                        if(!await Helper.HasPermission(Permission.Storage) || !await Helper.HasPermission(Permission.MediaLibrary))
                        {
                            //TODO Show permission denied alert
                            return;
                        }
                        var media = await CrossMedia.Current.PickPhotoAsync();
                        if(media != null)
                        {
                            var bytes = Helper.GetBytesFromStream(media.GetStream());
                            if(Print == null)
                            {
                                Print = new PrintDto
                                {
                                    Media = new MediaDto
                                    {
                                        Bytes = bytes,
                                        Url = media.Path
                                    }
                                };
                            }
                            else
                            {
                                Print.Media.Bytes = bytes;
                                Print.Media.Url = media.Path;
                                RaisePropertyChanged(() => Print);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
                }
                UIService.ShowLoadingDialog(false);
            });
        }

        private MvxCommand _addPhotoCommand;
        public ICommand AddPhotoCommand
        {
            get
            {
                _addPhotoCommand = _addPhotoCommand ?? new MvxCommand(DoAddPhotoCommand);
                return _addPhotoCommand;
            }
        }

        private void DoAddPhotoCommand()
        {
            Task.Run(async() =>
            {
                try
                {
                    if (CrossMedia.IsSupported && CrossMedia.Current.IsPickPhotoSupported)
                    {
                        if (!await Helper.HasPermission(Permission.Storage) || !await Helper.HasPermission(Permission.MediaLibrary))
                        {
                            //TODO Show permission denied alert
                            return;
                        }
                        var media = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { CompressionQuality = 90});
                        if (media != null)
                        {
                            var bytes = Helper.GetBytesFromStream(media.GetStream());
                            if (Photos == null)
                            {
                                Photos = new List<MediaDto>
                                {
                                    new MediaDto
                                    {
                                        Bytes = bytes,
                                        Url = media.Path
                                    }
                                };
                            }
                            else
                            {
                                Photos.Add(new MediaDto
                                {
                                    Bytes = bytes,
                                    Url = media.Path
                                });
                                RaisePropertyChanged(() => Photos);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
                }
            });
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
                UIService.ShowLoadingDialog(true);
                try
                {
                    var res = RealmService.GetCategories();
                    if (res?.IsValid == true && res.Result != null)
                    {
                        Categories = res.Result;
                        StatusList = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
                        InitDefaults();
                    }
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
                }
                UIService.ShowLoadingDialog(false);
            });
        }

        private void InitDefaults()
        {
            if(IsNewOrder)
            {
                SelectedCategory = Categories.First();                
                SelectedProduct = Products.First();
                CurrentOrder.Units = 0.5f;

                var date = DateTime.Now;
                var span = new TimeSpan(1, 0, 0);
                long ticks = (date.Ticks + span.Ticks - 1)/ span.Ticks;

                CurrentOrder.Time = new DateTime(ticks * span.Ticks);
                SelectedStatus = OrderStatus.Confirmed;
            }
            else
            {
                SelectedCategory = Categories.First(x => x.Products.Any(y => y.Id == CurrentOrder.Product.Id));
                SelectedProduct = Products.First(x => x.Id == CurrentOrder.Product.Id);
                SelectedStatus = CurrentOrder.Status;

                var selectedCatIndex = Categories.IndexOf(SelectedCategory);
                var selectedProdIndex = Products.IndexOf(SelectedProduct);
                var selectedStatusIndex = StatusList.IndexOf(SelectedStatus);

                Categories.RemoveAt(selectedCatIndex);
                Categories.Insert(0, SelectedCategory);

                Products.RemoveAt(selectedProdIndex);
                Products.Insert(0, SelectedProduct);

                StatusList.RemoveAt(selectedStatusIndex);
                StatusList.Insert(0, SelectedStatus);

                SelectedCategory = Categories.First();
                SelectedProduct = Products.First();
                SelectedStatus = StatusList.First();
                Print = CurrentOrder.Print;
                Photos = CurrentOrder.MediaList;
            }
            RaisePropertyChanged(() => CurrentOrder);
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
