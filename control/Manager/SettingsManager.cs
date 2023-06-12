using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace control.Manager
{
    public static class SettingsManager
    {
        public static int UserLoginTimeOutMinutes { private get; set; } = 30;


        public static async Task CleanUpDataBase(HttpContext context, control.Data.controlContext dataContext)
        {
            DateTime _cuttOffTime = DateTime.Now - TimeSpan.FromMinutes(UserLoginTimeOutMinutes);

            foreach (var item in dataContext.LoginLink)
            {
                if (item.GenerationTime <  _cuttOffTime)
                {
                    dataContext.LoginLink.Remove(item);
                }

            }

            dataContext.SaveChanges();
            
        }

    }
}
