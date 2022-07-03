<%@ Page Language="c#" CodeBehind="UserEdit.aspx.cs" AutoEventWireup="false" Inherits="CMS.Web.Admin.UserEdit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>UserEdit</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</head>
<body ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <script type="text/javascript"> <!--
    function confirmDeleteUser(userId) {
        if (confirm("Bạn có chắc chắn muốn xóa người dùng này?"))
            document.location.href = "UserEdit.aspx?UserId=" + userId + "&Action=Delete";
    }
    // -->
        </script>
        <div class="group">
            <h4>Cấu hình cơ bản</h4>
            <table>
                <tr>
                    <td style="width: 200px">Tên đăng nhập</td>
                    <td>
                        <asp:TextBox ID="txtUsername" runat="server" Width="200px"></asp:TextBox><asp:Label ID="lblUsername" runat="server" Visible="False"></asp:Label><asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Phải có tên đăng nhập" CssClass="validator"
                            Display="Dynamic" EnableClientScript="False" ControlToValidate="txtUsername"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td>Họ</td>
                    <td>
                        <asp:TextBox ID="txtFirstname" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Tên</td>
                    <td>
                        <asp:TextBox ID="txtLastname" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" EnableClientScript="False"
                            Display="Dynamic" CssClass="validator" ErrorMessage="Phải có email"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" EnableClientScript="False"
                                Display="Dynamic" CssClass="validator" ErrorMessage="Email không hợp lệ" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td>Website</td>
                    <td>
                        <asp:TextBox ID="txtWebsite" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Hoạt động</td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server"></asp:CheckBox></td>
                </tr>
                <tr>
                    <td>Múi giờ</td>
                    <td>
                        <asp:DropDownList ID="ddlTimeZone" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Mật khẩu</td>
                    <td>
                        <asp:TextBox ID="txtPassword1" runat="server" Width="200px" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Nhập lại mật khẩu</td>
                    <td>
                        <asp:TextBox ID="txtPassword2" runat="server" Width="200px" TextMode="Password"></asp:TextBox><asp:CompareValidator ID="covPassword" runat="server" ControlToValidate="txtPassword1" EnableClientScript="False"
                            Display="Dynamic" CssClass="validator" ErrorMessage="Hai ô mật khẩu không trùng nhau" ControlToCompare="txtPassword2"></asp:CompareValidator></td>
                </tr>
                <tr>
                    <td>Thời gian hết hạn cần login lại (phút)</td>
                    <td>
                        <asp:TextBox ID="txtTimeExpireLogin" Text="30" runat="server"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div class="group">
            <h4>Vai trò</h4>
            <table class="tbl">
                <asp:Repeater ID="rptRoles" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Vai trò</th>
                            <th></th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                            <td style="text-align: center">
                                <asp:CheckBox ID="chkRole" runat="server"></asp:CheckBox></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div>
            <asp:Button ID="btnSave" runat="server" Text="Lưu"></asp:Button><asp:Button ID="btnCancel" runat="server" Text="Hoàn tác" CausesValidation="False"></asp:Button><asp:Button ID="btnDelete" runat="server" Text="Xóa"></asp:Button>
        </div>
    </form>
</body>
</html>
