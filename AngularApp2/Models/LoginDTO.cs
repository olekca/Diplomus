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
            Role = "";
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

    public class AccountDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
        public DateOnly? DayOfBirth { get; set; }
        public string UserImg { get; set; }

    }

    public class ImgAccountDTO
    {
        public int UserId { get; set; }
        public string UserImg { get; set; }
    }

    public class NameAccountDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class EmailAccountDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
    }

    public class PasswordAccountDTO
    {
        public int UserId { get; set; }
        public string PrevPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class RoleAccountDTO
    {
        public int UserId { get; set; }

        public string Secret { get; set; }
    }


    public class ProductShort
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
    }

    public class NeedShort
    {
        public int NeedsId { get; set; }
        public string NeedsName { get; set; } = null!;
    }


    public class SearchRequest
    {
        public string SearchLine { get; set; }
        public ProductShort[] IncludedProducts { get; set; } = null!;
        public ProductShort[] ExcludedProducts { get; set; } = null!;
        public NeedShort[] Needs { get; set; } = null!;

        public int Page { get; set; }
        public int RecipesPerPage { get; set; }
    }



    public class RecipeNutrientInfo
    {
        public int NutrientId { get; set; }
        public string NutrientName { get; set; }
        public float NutrientAmount { get; set; }
        public double NutrientPercent { get; set; }
        public float NutrientDailyDose { get; set; }
    }


    public class RecipeDTO
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = null!;
        public string[]? Steps { get; set; }
        public string? RecipeImg { get; set; }
        public RecipeProds[] Products { get; set; } = null!;

    }

    public class RecipeProds
    {
        public int ProductId { get; set; }
        public long RecipeProductId { get; set; }
        public string ProductName { get; set; }
        public string? MeasureType { get; set; }
        public char? MeasureChar { get; set; }
        public float? MeasureNumber { get; set; }
    }

    public class DropDownValues
    {
        public char Type { get; set; }
        public string Name { get; set; }
        public DropDownValues(char type, string name)
        {
            Type = type;
            Name = name;
        }

    }

    public class StatNutrients
    {
        public short NutrientId { get; set; } 
        public string? NutrientName { get; set; }
        public float? DailyDose { get; set; }
        public string Color { get; set; }
        public double? NutrientPercent { get; set; }     
       
        public string? DoseMeasure { get; set; }
        public float NutrientAmount { get; set; }
    }

    public class NeedDTO
    {
        public int NeedsId { get; set; }
        public string NeedsName { get; set; } = null!;
        public string? Desc { get; set; }
    }

    public class UserNeedsUpdateDTO
    {
        public int UserId { get; set; }
        public NeedDTO[] Needs { get; set; }
    }

    public class RecipeStat
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = null!;
        public string? RecipeImg { get; set; }
        public short Amount { get; set; }
    }
    public class RecipeShort
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = null!;
        public string? RecipeImg { get; set; }
        public int Coef { get; set; }
    }

    public class SearchResult
    {
        public RecipeShort[] Recipes { get; set; }
        public int MaxPage { get; set; }
        public int CurPage { get; set; }
        public int RecipesNum { get; set; }
    }


}

     
     
     
     

