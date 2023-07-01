using AutoMapper;
using System;
using AngularApp2.Models.Entity;
namespace AngularApp2.Models
{
    public class AppMappingProfile:Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Users, AccountDTO>();
            CreateMap<Recipes, RecipeDTO>();
            CreateMap<Products, ProductShort>();
            CreateMap<Needs, NeedShort>();
            CreateMap<Recipes, RecipeShort>();
            CreateMap<Needs, NeedDTO>();

        }
    }
}
