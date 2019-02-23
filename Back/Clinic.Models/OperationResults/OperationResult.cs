namespace Clinic.Models.OperationResults
{
    public class OperationResult
    {
        public OperationResult(bool success, string message = null)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
