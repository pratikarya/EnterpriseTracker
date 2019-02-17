using EnterpriseTracker.Core.AppContents.Category.Contract;
using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using System;

namespace EnterpriseTracker.Core.AppContents.Product.Contract.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Price { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsVeg { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }

    public enum ProductStatus
    {
        Inactive,
        Active
    }
}
