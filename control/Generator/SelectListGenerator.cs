using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                    Text = item.DisplayText,
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
                //Do not display locked users, except when they already are selected
                if (item.AccessLevel != EAccessLevel.kNone || doorToGenerateFor.AdminUsers.Contains(item))
                {
                    _outputList.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName + " (" + item.UserName + ")",
                        Value = item.UserName,
                    });

                }
                if (doorToGenerateFor.AdminUsers.Contains(item))
                    _selectedList.Add(item.UserName);
            }
            return new MultiSelectList(_outputList, "Value", "Text", _selectedList);
        }

        public static SelectList GenerateSelectListForLanguage(string currentLanguage)
        {
            List<SelectListItem> _outputList = new(){
                new SelectListItem{Text = "English",Value = "en" },
                new SelectListItem{Text = "German",Value = "de" }
                  };

            return new SelectList(_outputList, "Value", "Text", currentLanguage);
        }

        private static IList<Door> GetDoorListByUserAccess(control.Data.controlContext dataContext, User admin)
        {

            if (admin.AccessLevel >= EAccessLevel.kAdmin)
                return dataContext.Door.ToList();

            //Moderators are only allowed to administrate their own doors, 
            return dataContext.Door.Where(e => admin.AdminDoors.Contains(e)).ToList();

        }


        public static SelectList GetAccessListByUserAccess(control.Data.controlContext context, HttpContext httpContext, User userToGenerateFor)
        {
            EAccessLevel _accessLevel = AccountHelper.GetEAccessLevel(httpContext);

            List<SelectListItem> _outputList = new(){
                new SelectListItem{Text = "Locked",Value = "0" }
                  };

            if (userToGenerateFor.AdminDoors.IsNullOrEmpty())
            {
                _outputList.Add(new SelectListItem { Text = "User", Value = "1" });

                if (_accessLevel >= EAccessLevel.kSuperAdmin)
                {
                    _outputList.Add(new SelectListItem { Text = "Admin", Value = "3" });
                    _outputList.Add(new SelectListItem { Text = "Super Admin", Value = "4" });
                }
                return new SelectList(_outputList, "Value", "Text");
            }

            //If we end up here, the user is an moderator
            _outputList.Add(new SelectListItem { Text = "Moderator", Value = "2" });

            return new SelectList(_outputList, "Value", "Text");
        }


    }
}
