using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Common.Contract.Dto
{
    public class GenericRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
