using AutoMapper;

namespace APIWizPedroKarut.Mapping
{
    public class ItemPedidoProfile : Profile
    {
        public ItemPedidoProfile()
        {
            CreateMap<Models.ItemPedido, DTOs.ItemPedidoDTO>().ReverseMap();
        }
    }
}
