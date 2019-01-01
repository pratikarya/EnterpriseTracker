using System;

namespace EnterpriseTracker.Core.Common.Contract.Dto
{
    public class SearchDto
    {
        public Guid Id { get; set; }
    }

    public class SearchDto<T> : SearchDto
    {
        public T RequestDto { get; set; }
    }
}
