using AutoMapper;
using gRPC_Server;
using gRPC_Server.Models.Filters;


namespace Infastructure.AutoMapper;


public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Create_Product_Request,Product>().ReverseMap();
        CreateMap<Get_Product_Response,Product>().ReverseMap();
        CreateMap<Update_Product_Request, Product>().ReverseMap();
        CreateMap<Get_All_Products_Request, ProductFilter>().ReverseMap();
    }
}