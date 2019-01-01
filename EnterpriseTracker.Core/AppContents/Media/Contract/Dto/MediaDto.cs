using System;

namespace EnterpriseTracker.Core.AppContents.Media.Contract.Dto
{
    public class MediaDto
    {
        public Guid Id { get; set; }

        public byte[] Bytes { get; set; }

        public MediaType Type { get; set; }

        public string Url { get; set; }
        
        public string FileName
        {
            get
            {
                var extension = ".";
                if (Type == MediaType.Jpg)
                    extension += "jpg";
                return Id + extension;
            }
        }
    }

    public enum MediaType
    {
        Jpg
    }
}
