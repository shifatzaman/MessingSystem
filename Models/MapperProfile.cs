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

            CreateMap<ExtraMessing, AddExtraMessingViewModel>()
                   .ReverseMap();

            CreateMap<CafeterialBill, AddCafeteriaBillViewModel>()
                   .ReverseMap();

            CreateMap<UtilityBill, AddUtilityBillViewModel>()
                   .ReverseMap();

            CreateMap<Room, RoomViewModel>()
                   .ReverseMap();

            CreateMap<DailyMessingTemplate, DailyMessingTemplateViewModel>()
                   .ReverseMap();

            CreateMap<DailyMessingTemplateItem, DailyMessingTemplateItemViewModel>()
                   .ReverseMap();

            //CreateMap<MemberMeal, Membermeal>()
            //       .ReverseMap();
        }
    }
}
