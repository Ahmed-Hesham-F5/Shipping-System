using AutoMapper;
using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.EmployeeDTOs;
using ShippingSystem.DTOs.HubDTOs;
using ShippingSystem.DTOs.RequestDTOs;
using ShippingSystem.DTOs.ShipmentDTOs;
using ShippingSystem.Models;

namespace ShippingSystem.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateShipmentDto, Shipment>();

            CreateMap<Shipment, ShipmentDetailsDto>()
                .ForMember(dest => dest.ShipmentStatuses,
                    opt => opt.MapFrom(src => src.ShipmentStatuses
                        .OrderByDescending(ss => ss.Timestamp)));

            CreateMap<Shipment, ShipmentListDto>()
                .ForMember(dest => dest.LatestShipmentStatus,
                opt => opt.MapFrom(src => src.ShipmentStatuses
                .OrderByDescending(ss => ss.Timestamp)
                .FirstOrDefault()));

            CreateMap<ShipmentStatus, LatestShipmentStatusDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            CreateMap<ShipmentStatus, ShipmentStatusHistoryDto>();

            CreateMap<UserAddress, AddressDto>().ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<UpdateShipmentDto, Shipment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Shipment, ToPickupShipmentListDto>()
                .ForMember(dest => dest.LatestShipmentStatus,
                opt => opt.MapFrom(src => src.ShipmentStatuses
                .OrderByDescending(ss => ss.Timestamp)
                .FirstOrDefault()));

            CreateMap<Shipment, ToReturnShipmentListDto>()
                .ForMember(dest => dest.LatestShipmentStatus,
                opt => opt.MapFrom(src => src.ShipmentStatuses
                .OrderByDescending(ss => ss.Timestamp)
                .FirstOrDefault()));

            CreateMap<CreatePickupRequestDto, PickupRequest>()
                .ForMember(dest => dest.PickupRequestShipments,
                opt => opt.MapFrom(src =>
                (src.ShipmentIds ?? new List<int>())
                .Select(id => new PickupRequestShipment { ShipmentId = id })
                ));

            CreateMap<CreateReturnRequestDto, ReturnRequest>()
                .ForMember(dest => dest.ReturnRequestShipments,
                opt => opt.MapFrom(src =>
                (src.ShipmentIds ?? new List<int>())
                .Select(id => new ReturnRequestShipment { ShipmentId = id })
                ));

            CreateMap<CreateCancellationRequestDto, CancellationRequest>()
                .ForMember(dest => dest.CancellationRequestShipments,
                opt => opt.MapFrom(src =>
                (src.ShipmentIds ?? new List<int>())
                .Select(id => new CancellationRequestShipment { ShipmentId = id })
                ));

            CreateMap<CreateRescheduleRequestDto, RescheduleRequest>();

            CreateMap<RequestBase, RequestListDto>();

            CreateMap<PickupRequest, PickupRequestDetailsDto>()
                .ForMember(dest => dest.Shipments, opt => opt.MapFrom(src => src.PickupRequestShipments.Select(p => p.Shipment)))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName));


            CreateMap<ReturnRequest, ReturnRequestDetailsDto>()
                .ForMember(dest => dest.Shipments, opt => opt.MapFrom(src => src.ReturnRequestShipments.Select(r => r.Shipment)))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<CancellationRequest, CancellationRequestDetailsDto>()
                .ForMember(dest => dest.Shipments, opt => opt.MapFrom(src => src.CancellationRequestShipments.Select(c => c.Shipment)))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<RescheduleRequest, RescheduleRequestDetailsDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<PickupRequestShipment, ToCancelShipmentListDto>()
                .ForMember(dest => dest.ShipmentId, opt => opt.MapFrom(src => src.Shipment.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Shipment.CustomerName))
                .ForMember(dest => dest.ShipmentDescription, opt => opt.MapFrom(src => src.Shipment.ShipmentDescription))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Shipment.Quantity))
                .ForMember(dest => dest.ShipmentWeight, opt => opt.MapFrom(src => src.Shipment.ShipmentWeight))
                .ForMember(dest => dest.CollectionAmount, opt => opt.MapFrom(src => src.Shipment.CollectionAmount))
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.PickupRequestId))
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.PickupRequest.RequestType));

            CreateMap<ReturnRequestShipment, ToCancelShipmentListDto>()
                .ForMember(dest => dest.ShipmentId, opt => opt.MapFrom(src => src.Shipment.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Shipment.CustomerName))
                .ForMember(dest => dest.ShipmentDescription, opt => opt.MapFrom(src => src.Shipment.ShipmentDescription))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Shipment.Quantity))
                .ForMember(dest => dest.ShipmentWeight, opt => opt.MapFrom(src => src.Shipment.ShipmentWeight))
                .ForMember(dest => dest.CollectionAmount, opt => opt.MapFrom(src => src.Shipment.CollectionAmount))
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.ReturnRequest.Id))
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.ReturnRequest.RequestType));

            CreateMap<PickupRequest, ToRescheduleRequestListDto>()
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.PickupDate));

            CreateMap<ReturnRequest, ToRescheduleRequestListDto>()
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.PickupDate));

            CreateMap<CreateHubDto, Hub>()
                .ForMember(dest => dest.Type, opt => opt.Ignore());

            CreateMap<Hub, HubListDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager!.User.FirstName + " " + src.Manager.User.LastName))
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees!.Count));

            CreateMap<Hub, HubSelectDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<Employee, EmployeeListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.Phones!.FirstOrDefault()!.PhoneNumber))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.Role.ToString()))
                .ForMember(dest => dest.HubName, opt => opt.MapFrom(src => src.Hub!.Name));
        }
    }
}
