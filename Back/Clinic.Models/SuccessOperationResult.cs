namespace Clinic.Models
{
    public class SuccessOperationResult<T> : OperationResult
    {
        public SuccessOperationResult(T data)
            : base(true)
        {
            this.Data = data;
        }

        public T Data { get; set; }
    }
}
