namespace Clinic.Models
{
    public abstract class OperationResult
    {
        protected OperationResult(bool success)
        {
            this.Success = true;
        }

        public bool Success { get; set; }
    }
}
