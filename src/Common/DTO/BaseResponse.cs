namespace Disk.Common.DTO
{
    public class BaseResponse
    {
        public string? Message { get; set; }

        public bool Succes { get; set; }

        public BaseResponse(string? message = null, bool succes = true)
        {
            this.Message = message;
            this.Succes = succes;
        }
    }
}
