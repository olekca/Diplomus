using AngularApp2.Models.Entity;

namespace AngularApp2.Models

{
    public class LoginDTO
    {
        public bool IsLoggedIn;
        public int UserId;
        public string Role;
        public LoginDTO()
        {
            IsLoggedIn = false;
            UserId = -1;
            Role = "";
        }
        public void UserAuthorized(Users user)
        {
            IsLoggedIn = true;
            UserId = user.UserId;
            Role = user.Role;
            
        }
    }

    public class RegisterDTO
    {
        public bool UserExists;
        public bool IsSuccessful;
        public int UserId;
        public string Role;
        public RegisterDTO()
        {
            UserId = -1;
            Role= "";
        }
        public void UserExisting()
        {
            UserExists = true;
            IsSuccessful = false;
        }
        public void IsSuccesful(Users user)
        {
            IsSuccessful = true;
            UserExists = false;
            UserId = user.UserId;
            Role = "user";
        }
    }
}
