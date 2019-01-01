using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto
{
    public class ProductRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
        public bool IsVeg { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
