using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Web.Server
{
    public partial class BackupSql : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Microsoft.SqlServer.Management.Smo.Server myServer = new Microsoft.SqlServer.Management.Smo.Server(@"112.78.2.112,1433");
            myServer.ConnectionContext.LoginSecure = false;
            myServer.ConnectionContext.Login = "mon43574_mon65140";
            myServer.ConnectionContext.Password = "HongLong123@";
            myServer.ConnectionContext.Connect();

            Backup bkpDBFull = new Backup();
            bkpDBFull.Action = BackupActionType.Database;
            bkpDBFull.Database = "mon43574_nhhl2";
            bkpDBFull.Devices.AddDevice(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "morestaurant.bak"), DeviceType.File);
            bkpDBFull.BackupSetName = "morestaurant";
            bkpDBFull.BackupSetDescription = "morestaurant database - Full Backup";
            bkpDBFull.ExpirationDate = DateTime.Today.AddDays(360);
            bkpDBFull.Initialize = false;
            bkpDBFull.SqlBackup(myServer);
        }
    }
}