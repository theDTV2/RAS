using control.Manager.Interfaces;
using control.Models;
using Microsoft.IdentityModel.Tokens;

namespace control.Manager
{
    public class DataBaseManager  : IDataBaseManager
    { 
        //Dies hier ist notwendig, um an die Datenbank zu kommen
        private readonly control.Data.controlContext _context;
        public DataBaseManager(control.Data.controlContext context)
        {
            _context = context;
        }

        public async Task DeleteOldLoginLinksAsync()
        {
            while (true)
            {
                
                IList<LoginLink>? _loginLinks = _context.LoginLink.ToList();

                //CutoffTime is longer than actual login timeout, just to be sure
                DateTime _cutOffTime = DateTime.Now - TimeSpan.FromMinutes(1) - GeneralSettingsManager.GetUserLoginTimeout();
                if (!_loginLinks.IsNullOrEmpty())
                {
                    _loginLinks = _loginLinks.Where(l => l.GenerationTime > _cutOffTime).ToList();
                    _context.SaveChanges();
                }

                await Task.Delay(600000);

            }
        }

    }
}
