using System;
using System.Collections.Generic;
using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects;
using EnterpriseTracker.Core.ViewModels.Orders;
using MvvmCross;
using MvvmCross.ViewModels;

namespace EnterpriseTracker.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterCustomAppStart<AppStart>();
            InitAppContents();
            InitData();
            InitUsers();
        }

        private void InitData()
        {

        }

        private void InitUsers()
        {

        }

        private void InitAppContents()
        {
            bool AppContentsInit = false;
            var offlineService = Mvx.IoCProvider.Resolve<IOfflineService>();
            //offlineService.ClearOfflineDb();
            AppContentsInit = !string.IsNullOrEmpty(offlineService.GetValue("AppContentsInitKey"));
            if(!AppContentsInit)
            {
                var cakeCategory = new CategoryDto
                {
                    Id = Guid.Empty,
                    Name = "Cakes",
                    Desc = "Cakes desc",
                    Status = CategoryStatus.Active,
                    Products = new List<ProductDto>()
                    {
                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Chocolate Truffle",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 700,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Chocolate Caramel",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 700,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Chocolate Coffee",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 700,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Vanilla Caramel",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 700,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Pineapple",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 700,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Dutch Truffle",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Black Forest",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Fresh Fruit Cake",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Raspberry",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Blueberry",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "White Forest",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Butterscotch",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Chocolate Crunch",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Chocolate Chips",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },
                        
                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Bubblegum Crunch",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Strawberry (Seasonal)",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Mango (Seasonal)",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 800,
                            Unit = "kg"
                        },

                        new ProductDto
                        {
                            Id = Guid.Empty,
                            Name = "Fondant Cake",
                            Status = ProductStatus.Active,
                            IsVeg = true,
                            Price = 1100,
                            Unit = "kg"
                        }
                    }
                };
                var chocolateCategory = new CategoryDto
                {
                    Id = Guid.Empty,
                    Name = "Chocolates",
                    Status = CategoryStatus.Active,
                    Products = new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Id = Guid.Empty,
                            IsVeg = true,
                            Status = ProductStatus.Active,
                            Name = "Dry fruit chocolates",
                            Price = 1000,
                            Unit = "Kg"
                        }
                    },
                    Desc = "Almond and Cashew rocks."
                };

                offlineService.CreateCategory(new SearchDto<CategoryDto> { RequestDto = cakeCategory });
                offlineService.CreateCategory(new SearchDto<CategoryDto> { RequestDto = chocolateCategory });
                offlineService.SetValue("AppContentsInitKey", "true");
            }
        }
    }
}
