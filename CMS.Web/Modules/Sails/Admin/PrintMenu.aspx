<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintMenu.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.PrintMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In menu</title>
    <style>
        html, body {
            height: 297mm;
            width: 210mm;
        }

        table {
            width: 99%;
            border-collapse: collapse;
        }

            table td {
                border: 1px solid #cdcdcd;
                height: 140mm;
                padding: 10px;
                vertical-align: text-top;
                text-align: center;
                line-height: 2;
                font-size: 18px;
            }
    </style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
        <asp:PlaceHolder ID="plhAdminContent" runat="server">
            <div>
                <table>
                    <tr>
                        <td><b>THỰC ĐƠN</b>
                            <br />
                            <asp:Literal ID="litMenu1" runat="server"></asp:Literal></td>
                        <td><b>THỰC ĐƠN</b><br />
                            <asp:Literal ID="litMenu2" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td><b>THỰC ĐƠN</b><br />
                            <asp:Literal ID="litMenu3" runat="server"></asp:Literal></td>
                        <td><b>THỰC ĐƠN</b><br />
                            <asp:Literal ID="litMenu4" runat="server"></asp:Literal></td>
                    </tr>
                </table>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plhErrorMessage" runat="server" Visible="false">
            <div class="alert alert-danger"><strong>Error!</strong>Bạn không có quyền xuất thực đơn</div>
        </asp:PlaceHolder>
    </form>
</body>
</html>
