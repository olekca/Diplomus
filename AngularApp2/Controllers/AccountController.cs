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
        public string Login(string email, string password)//tested a little
        {
            LoginDTO res = new LoginDTO();
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                if (user != null)
                {
                    res.UserAuthorized(user);
                }
            }
                return JsonConvert.SerializeObject(res);
        }
        [HttpPost("Register")]
        public string Register(Users user)//tested a little, dont forget to add not sended data to user (like role). Do it when go to angular
        {
            RegisterDTO res = new RegisterDTO();
            using (DiplomusContext db = new DiplomusContext())
            {
                Users newUser = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                if (newUser == null)
                {
                    db.Users.Add(user);                   
                    db.SaveChanges();
                    res.IsSuccesful(user);
                }
                else
                {
                    res.UserExisting();
                }
            }
            return JsonConvert.SerializeObject(res);
            
        }

        [HttpPost("ChangeEmail")]
        public IActionResult ChangeEmail(int user_id, string email)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == user_id).FirstOrDefault();
                if (user != null)
                {
                    user.Email = email;
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }
            
        }

        [HttpPost("ChangeDob")]
        public IActionResult ChangeDob(int user_id, string date)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == user_id).FirstOrDefault();
                if (user != null)
                {
                    user.DayOfBirth = DateOnly.Parse(date);
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }

        }

        [HttpPost("ChangeName")]
        public IActionResult ChangeName(int user_id, string name)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == user_id).FirstOrDefault();
                if (user != null)
                {
                    user.UserName = name;
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }

        }
    }
}
