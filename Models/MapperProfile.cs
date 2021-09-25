using AutoMapper;
using MessingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<InventoryItem, FetchInventoryItemViewModel>()
                    .ReverseMap();

            CreateMap<InventoryItemType, FetchInventoryItemTypeViewModel>()
                    .ReverseMap();

            CreateMap<MessMember, MessMemberViewModel>()
                   .ReverseMap();

            //CreateMap<MemberMeal, Membermeal>()
            //       .ReverseMap();
        }
    }
}
