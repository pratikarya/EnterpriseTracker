using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Category.Contract.Dto
{
    public class CategoryRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Status { get; set; }
        public IList<ProductRealmDto> Products { get; }
        public override string ToString()
        {
            return Name;
        }
    }
}
