namespace Clinic.Models
{
    public class ErrorOperationResult : OperationResult
    {
        public ErrorOperationResult(string message)
            : base(false)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
