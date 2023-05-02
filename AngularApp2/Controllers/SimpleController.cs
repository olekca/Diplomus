using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using AngularApp2.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace AngularApp2.Controllers
{
    [Controller]
    [Route("please")]
    public class SimpleController : ControllerBase
    {

        DiplomusContext context;

        public SimpleController(DiplomusContext context)
        {
            this.context = context;
        }

        public string Index()
        {
            
            using (
                DiplomusContext db = new DiplomusContext())
            {
                bool t = db.Database.CanConnect();
                string res = ""+db.Users.First().Email;
                
                return res;
            }
                
            return "{\r\n    \"Fucc\": \"Same shit?\" }";
        }
    }
}
