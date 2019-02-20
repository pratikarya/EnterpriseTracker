using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.Common.Contract.Dto
{
    public class OrdersSearchDto : SearchDto
    {
        public DateTime? Date { get; set; }
    }
}
