using AutoMapper;
using FBru.Repository.Entities;
using FBru.WebAPI.Models;

namespace FBru.WebAPI
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Advertisement, AdvertisementModel>();
            CreateMap<Restaurant, RestaurantModel>();
            CreateMap<Dish, DishViewModel>();
        }
    }
}