using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using AutoMapper;


namespace AngularApp2
{
    public class User 
    {
        public int userId;
        public string email;
        public role Role;
        public enum role { Admin, User };
    }
    public class UserForRegistrationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /* CreateMap<Company, CompanyDto>()
                 .ForMember(c => c.FullAddress,
                     opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));*/
          //  CreateMap<UserForRegistrationDto, User>()
            //    .ForMember(u => u., opt => opt.MapFrom(x => x.Email));
        }
    }
    
}
