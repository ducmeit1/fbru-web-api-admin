using AutoMapper;
using FBru.DTO;
using FBru.Repository.Entities;
using FBru.WebAdmin.Models;

namespace FBru.WebAdmin
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            /*New*/
            CreateMap<DishDto, DishModel>();
            CreateMap<DishModel, Dish>();
        }
    }
}