using ShippingSystem.Enums;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class RequestInfoDto
    {
        public RequestTypeEnum? RequestType { get; set; }
        public RequestStatusEnum? RequestStatus { get; set; }
    }
}
