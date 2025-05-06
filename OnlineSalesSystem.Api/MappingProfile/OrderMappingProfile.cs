using AutoMapper;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Api.DTOs;

namespace OnlineSalesSystem.Api.MappingProfile
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // CreateMap<Fonte, Destino>();
            CreateMap<OrderCreateDTO, Order>()
                .ForMember(dest => dest.OrderDate, 
                         opt => opt.MapFrom(src => src.OrderDate.Kind == DateTimeKind.Unspecified 
                             ? DateTime.SpecifyKind(src.OrderDate, DateTimeKind.Utc) 
                             : src.OrderDate.ToUniversalTime()));

            CreateMap<OrderUpdateDTO, Order>()
                .ForMember(dest => dest.OrderDate,
                         opt => opt.MapFrom(src => src.OrderDate.Kind == DateTimeKind.Unspecified 
                             ? DateTime.SpecifyKind(src.OrderDate, DateTimeKind.Utc) 
                             : src.OrderDate.ToUniversalTime()));

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.CustomerName, 
                         opt => opt.MapFrom(src => src.Customer!.Name));
        }
    }
}