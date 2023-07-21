using control.Manager.Interfaces;
using control.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;

namespace control.Manager
{
    public class DataBaseManager : IDataBaseManager
    {
        //Dies hier ist notwendig, um an die Datenbank zu kommen
        private readonly control.Data.controlContext _context;
        public DataBaseManager(control.Data.controlContext context)
        {
            _context = context;
        }

        public async Task DeleteOldLoginLinksAsync(CancellationToken stopToken)
        {
            await Task.Delay(60000, stopToken);

            while (stopToken.IsCancellationRequested)
            {
                DbSet<LoginLink> _loginLinks = _context.LoginLink;

                //CutoffTime is a littlebit longer than actual login timeout, just to be sure
                DateTime _cutOffTime = DateTime.Now - TimeSpan.FromMinutes(2) - GeneralSettingsManager.GetUserLoginTimeout();
                if (!_loginLinks.IsNullOrEmpty())
                {
                    _loginLinks.RemoveRange(_context.LoginLink.Where(l => l.GenerationTime < _cutOffTime).ToList());

                    _context.SaveChanges();
                }

                await Task.Delay(600000, stopToken);
            }
        }

        public async Task DeleteOldAccountsAsync(CancellationToken stopToken)
        {
            //We need to wait to prevent the DB Access from being accessed during construction. 
            await Task.Delay(60000, stopToken);

            while (!stopToken.IsCancellationRequested)
            {
                DbSet<User> _userList = _context.User;

                //TODO: Add customizable deletion time
                DateTime _cutOffTime = DateTime.Now - TimeSpan.FromDays(365);

                if (!_userList.IsNullOrEmpty())
                {
                    _userList.RemoveRange(_context.User.Where(l => l.LastLogin < _cutOffTime
                    && l.AccessLevel < EAccessLevel.kAdmin).ToList());
                    _context.SaveChanges();
                }

                //This number is 7 days in milliseconds
                await Task.Delay(604800000, stopToken);

            }
        }

    }
}
