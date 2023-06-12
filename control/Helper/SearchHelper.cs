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
            e.UserName.Contains(searchTerm) ||
            e.SecretCode.Contains(searchTerm) ||
            e.FirstName.Contains(searchTerm) ||
            e.LastName.Contains(searchTerm)).ToList();

            if (_user.IsNullOrEmpty() ) 
                return new List<User>();

            return _user;

        }
    }
}
