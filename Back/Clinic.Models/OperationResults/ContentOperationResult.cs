namespace Clinic.Models.OperationResults
{
    public class ContentOperationResult<T> : OperationResult
    {
        public ContentOperationResult(bool success, T data, string message = null)
            : base(success, message)
        {
            this.Data = data;
        }

        public T Data { get; }
    }
}
