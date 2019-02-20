using EnterpriseTracker.Core.AppContents.Media.Contract.Dto;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Media.Contract.Dto
{
    public class MediaRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public byte[] Bytes { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string FileName
        {
            get
            {
                var extension = ".";
                if (Type == (int)MediaType.Jpg)
                    extension += "jpg";
                return Id + extension;
            }
        }
    }
}
