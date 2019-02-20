using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Media.Contract.Dto
{
    public class PrintRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public MediaRealmDto Media { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
