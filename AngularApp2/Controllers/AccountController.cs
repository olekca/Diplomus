using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AngularApp2.Models.Entity;
using AngularApp2.Models;
using Newtonsoft.Json;
namespace AngularApp2.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        public string Index()
        {
            return "This is my default action...";
        }
        [HttpGet("example")]
        public string Something()
        {
            return "yeh";
        }
        
        public AccountController(){}
        [HttpGet("login")]
        public string login(string email, string password)//not tested
        {
            LoginDTO res = new LoginDTO();
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.Email == email && u.Password == password).First();
                if (user != null)
                {
                    res.UserAuthorized(user);
                }
            }
                return JsonConvert.SerializeObject(res);
        }
        
    }
}
