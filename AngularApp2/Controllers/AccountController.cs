using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AngularApp2.Models.Entity;
using AngularApp2.Models;
using Newtonsoft.Json;
using System.Xml.Linq;
using System;
using System.Configuration;

namespace AngularApp2.Controllers
{
    
    [ApiController]
    [Route("[controller]")]


    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public string Index()
        {
            return "This is my default action...";
        }
        [HttpGet("example")]
        public string Something()
        {
            return "yeh";
        }
        
        public AccountController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpGet("login")]
        public string Login(string email, string password)
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
        public string Register(Users user)
        {
            RegisterDTO res = new RegisterDTO();
            using (DiplomusContext db = new DiplomusContext())
            {
                Users newUser = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                if (newUser == null)
                {
                    user.Role = "user";
                    user.UserImg = "https://drive.google.com/file/d/1rMIHlrwFMzj6NGkZCcgruwxafC_fNhZI/view?usp=sharing";
                    user.Needs = new int[] {};
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
        public IActionResult ChangeEmail(EmailAccountDTO accountDTO)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == accountDTO.UserId).FirstOrDefault();
                if (user != null)
                {
                    Users otherUser = db.Users.Where(u => u.Email == accountDTO.Email).FirstOrDefault();
                    if (otherUser == null)
                    {
                        user.Email = accountDTO.Email;
                        db.SaveChanges();
                        return StatusCode(200);                        
                    }
                    else
                    {
                        return StatusCode(406);
                    }                    
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
        public IActionResult ChangeName(NameAccountDTO nameAccount)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == nameAccount.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.UserName = nameAccount.UserName;
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }

        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(PasswordAccountDTO accountDTO)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == accountDTO.UserId && u.Password==accountDTO.PrevPassword).FirstOrDefault();
                if (user != null)
                {
                    user.Password = accountDTO.NewPassword;
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }

        }


        [HttpPost("ChangePic")]
        public IActionResult ChangePic(ImgAccountDTO imgDTO)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == imgDTO.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.UserImg = imgDTO.UserImg;
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }
        }

        [HttpPost("MakeAdmin")]
        public IActionResult MakeAdmin(RoleAccountDTO accountDTO)
        {
            Console.WriteLine();
            if (accountDTO.Secret != _configuration.GetValue<String>("secret"))
            {
                return StatusCode(403);
            }
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == accountDTO.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Role = "admin";
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }
        }

        [HttpPost("MakeUser")]
        public IActionResult MakeUser(RoleAccountDTO accountDTO)
        {
            Console.WriteLine();
            if (accountDTO.Secret != _configuration.GetValue<String>("secret"))
            {
                return StatusCode(403);
            }
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == accountDTO.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Role = "user";
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }
        }

        [HttpGet("DeleteUser")]
        public IActionResult DeleteUser(int user_id, string secret)
        {
            if (secret != _configuration.GetValue<String>("secret"))
            {
                return StatusCode(403);
            }
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u => u.UserId == user_id).FirstOrDefault();
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);
                }
            }
        }

        [HttpGet("user")]
        public string getUser(int id)
        {
            AccountDTO res;
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Where(u=>u.UserId == id).FirstOrDefault();
                if (user != null) {
                    res = _mapper.Map<AccountDTO>(user);
                    return JsonConvert.SerializeObject(res);
                }
                else
                {
                    return "User not found";
                }

            }          
        }


        [HttpGet("AllUsers")]

        public string getUsers(int page, int usersPerPage)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                List<Users> nextPage = db.Users
                .OrderBy(u => u.UserId)
                .Skip((page-1)*usersPerPage)
                .Take(usersPerPage)
                .ToList();
                List<AccountDTO> res = _mapper.Map<List<AccountDTO>>(nextPage);
                return JsonConvert.SerializeObject(res);
            }            
        }

        [HttpGet("usersPageCount")]
        public int userPagesCount(int usersPerPage)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                return (int)Math.Ceiling(db.Users.Count()/(double)usersPerPage);
            }
        }

        [HttpGet("userNeeds")]
        public string getUserNeeds(int userId)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Find(userId);
                if (user != null)
                {
                    int[] needsAr = user.Needs;
                    List<NeedDTO> res = new List<NeedDTO>();
                    for (int i = 0; i < needsAr.Length; ++i)
                    {
                        NeedDTO temp = _mapper.Map<NeedDTO>(db.Needs.Find(needsAr[i]));
                        res.Add(temp);
                    }
                    return JsonConvert.SerializeObject(res);
                }else
                {
                    return "";
                }
            }
            
        }

        [HttpPost("updateNeeds")]
        public string setUserNeeds(UserNeedsUpdateDTO dto)
        {
            using (DiplomusContext db = new DiplomusContext())
            {
                Users user = db.Users.Find(dto.UserId);
                if (user != null)
                {
                    user.Needs = excludeId(dto.Needs);
                    db.SaveChanges();
                }
                
            }
            return getUserNeeds(dto.UserId);
        }

        public int[] excludeId(NeedDTO[] ar)
        {
            int[] res = new int[ar.Length];
            for (int i = 0; i<ar.Length; ++i)
            {
                res[i] = ar[i].NeedsId;
            }
            return res;
        }

    }

}
