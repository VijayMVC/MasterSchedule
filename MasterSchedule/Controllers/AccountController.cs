using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class AccountController
    {
        public static AccountModel Select(string userName, string password)
        {
            var @UserName = new SqlParameter("@UserName", userName);
            var @Password = new SqlParameter("@Password", password);
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AccountModel>("EXEC spm_SelectAccountByUserNamePassword @UserName, @Password", @UserName, @Password).FirstOrDefault();
        }
        
        public static List<AccountModel> SelectAccount()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<AccountModel>("EXEC spm_SelectAccount").ToList();
        }
    }
}
