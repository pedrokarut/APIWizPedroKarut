using AutoMapper;

namespace APIWizPedroKarut.Mapping
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<Models.Pedido, DTOs.PedidoDTO>().ReverseMap();
        }
    }
}
