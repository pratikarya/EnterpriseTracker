using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.AppContents.Media.Contract.Dto
{
    public class PrintDto
    {
        public Guid Id { get; set; }
        public MediaDto Media { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PrintShape PrintShape { get; set; }
    }

    public enum PrintShape
    {
        Rectangle,
        Square,
        Circle,
        Other
    }
}
