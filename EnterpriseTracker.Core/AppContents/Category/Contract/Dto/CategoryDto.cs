using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using System;
using System.Collections.Generic;

namespace EnterpriseTracker.Core.AppContents.Category.Contract.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<ProductDto> Products { get; set; }
        public CategoryStatus Status { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public enum CategoryStatus
    {
        Inactive,
        Active
    }
}
