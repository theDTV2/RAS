using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace control.Generator
{
    public static class SelectListGenerator
    {

        public static SelectList GenerateSelectListForDoor(control.Data.controlContext dataContext, HttpContext httpContext, User userToGenerateFor)
        {
            User _admin = AccountHelper.GetLoggedInUser(dataContext, httpContext);
            IList<Door> _doors = GetDoorListByUserAccess(dataContext, _admin);

            List<SelectListItem> _outputList = new();

            foreach (var item in _doors)
            {
                _outputList.Add(new SelectListItem
                {
                    Text = item.DisplayName,
                    Value = item.Id,
                    Selected = userToGenerateFor.AccessDoors.Contains(item)
                });

            }
            return new SelectList(_outputList, "Value", "Text", "Selected");
        }



        private static IList<Door> GetDoorListByUserAccess(control.Data.controlContext dataContext,User admin)
        {

            if (admin.AccessLevel >= EAccessLevel.kAdmin)
                return dataContext.Door.ToList();

            //Moderators are only allowed to administrate their own doors, 
            return dataContext.Door.Where(e => admin.AdminDoors.Contains(e)).ToList();

        }
    }
}
