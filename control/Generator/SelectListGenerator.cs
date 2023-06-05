using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace control.Generator
{
    public static class SelectListGenerator
    {

        public static MultiSelectList GenerateSelectListForDoor(control.Data.controlContext dataContext, HttpContext httpContext, User userToGenerateFor)
        {       
            //TODO: Move this into another function
             var _username = AccountHelper.GetUserName(httpContext);

            User _admin = dataContext.User.Where(u => u.UserName == _username).Include(u => u.AdminDoors).FirstOrDefault()!;

            IList<Door> _doors = GetDoorListByUserAccess(dataContext, _admin);

            List<SelectListItem> _outputList = new();
            List<string> _selectedList = new();

            //TODO: find a better way for this workaround
            _outputList.Add(new SelectListItem
            {
                Text = "None",
                Value = "",
                Selected = true
            });

            foreach (var item in _doors)
            {
                _outputList.Add(new SelectListItem
                {
                    Text = item.DisplayName,
                    Value = item.Id,
                });

                if (userToGenerateFor.AccessDoors.Contains(item))
                    _selectedList.Add(item.Id);
            }
            return new MultiSelectList(_outputList, "Value", "Text", _selectedList);
        }
        public static MultiSelectList GenerateSelectListForUser(control.Data.controlContext dataContext, HttpContext httpContext, Door doorToGenerateFor)
        {
            //TODO: Move this into another function
            IList<User> _users = dataContext.User.ToList();

            List<SelectListItem> _outputList = new();
            List<string> _selectedList = new();

            //TODO: find a better way for this workaround
            _outputList.Add(new SelectListItem
            {
                Text = "None",
                Value = "",
                Selected = true
            });

            foreach (var item in _users)
            {
                _outputList.Add(new SelectListItem
                {
                    Text = item.FirstName + " " + item.LastName,
                    Value = item.UserName,
                });

                if (doorToGenerateFor.AdminUsers.Contains(item))
                    _selectedList.Add(item.UserName);
            }
            return new MultiSelectList(_outputList, "Value", "Text", _selectedList);
        }


        private static IList<Door> GetDoorListByUserAccess(control.Data.controlContext dataContext, User admin)
        {

            if (admin.AccessLevel >= EAccessLevel.kAdmin)
                return dataContext.Door.ToList();

            //Moderators are only allowed to administrate their own doors, 
            return dataContext.Door.Where(e => admin.AdminDoors.Contains(e)).ToList();

        }


        public static SelectList GetAccessListByUserAccess(control.Data.controlContext context, HttpContext httpContext)
        {
            EAccessLevel _accessLevel = AccountHelper.GetEAccessLevel(httpContext);

            List<SelectListItem> _outputList = new(){
                 new SelectListItem{Text = "None",Value = "0" },
                  new SelectListItem{Text="User",Value = "1"},
                  new SelectListItem{Text="Moderator",Value = "2"}
                  };

            if (_accessLevel == EAccessLevel.kAdmin)
                _outputList.Add(new SelectListItem { Text = "User", Value = "3" });

            return new SelectList(_outputList, "Value", "Text");

        }
    }
}
