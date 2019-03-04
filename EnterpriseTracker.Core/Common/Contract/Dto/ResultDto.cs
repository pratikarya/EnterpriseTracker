namespace EnterpriseTracker.Core.Common.Contract.Dto
{
    public class ResultDto
    {
        public bool IsValid
        {
            get
            {
                return Status == ResultStatus.Ok;
            }
        }
        public string ErrorMessage { get; set; }
        public ResultStatus Status { get; set; }
    }

    public class ResultDto<T> : ResultDto
    {
        public T Result { get; set; }
    }

    public enum ResultStatus
    {
        Ok,
        ValidationError,
        ServerError
    }
}
