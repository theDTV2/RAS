using control.Data;
using control.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace control.Helper
{
    public static class SearchHelper
    {

        public static IEnumerable<User> SearchUser(controlContext dataContext,  string searchTerm)
        {
            IList<User> _user = dataContext.User.Where(e =>
            e.UserName.ToLower().Contains(searchTerm.ToLower()) ||
            e.SecretCode.Contains(searchTerm) ||
            e.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
            e.LastName.ToLower().Contains(searchTerm.ToLower())).ToList();

            if (_user.IsNullOrEmpty() ) 
                return new List<User>();

            return _user;

        }
    }
}
