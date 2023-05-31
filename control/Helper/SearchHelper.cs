using control.Data;
using control.Models;

namespace control.Helper
{
    public static class SearchHelper
    {

        public static IEnumerable<User> SearchUser(controlContext context,  string searchTerm)
        {
            var _user = context.User.Where(e =>
            e.UserName.Contains(searchTerm) ||
            e.AccessCode.Contains(searchTerm) ||
            e.FirstName.Contains(searchTerm) ||
            e.LastName.Contains(searchTerm)).ToList();

            return _user;

        }
    }
}
