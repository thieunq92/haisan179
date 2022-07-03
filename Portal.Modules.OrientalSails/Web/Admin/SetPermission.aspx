<%@ Page Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true"
    CodeBehind="SetPermission.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SetPermission"%>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Phân quyền</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <h4><strong>Vai trò</strong></h4>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-hover table-common">
                <asp:Repeater ID="rptRoles" runat="server" OnItemDataBound="rptRoles_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="litName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:HyperLink ID="hplSetPermission" runat="server">Phân quyền</asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="pager">
                <svc:Pager ID="pagerRoles" runat="server" ControlToPage="rptRoles" OnPageChanged="pagerRoles_PageChanged" />
            </div>
        </div>
    </div>
    <h4><strong>Người dùng</strong></h4>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-2">
                Tên đăng nhập
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Tên đăng nhập"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-2">
                Đăng nhập lần cuối từ ngày
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="Từ ngày (dd/mm/yyyy)" data-control="datetimepicker"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                Đến ngày
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="Đến ngày (dd/mm/yyyy)" data-control="datetimepicker"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button ID="btnFind" runat="server" Text="Tìm kiếm" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12">
            <div id="tabs">
                <ul class="nav nav-tabs">
                    <li><a href="#tabs-1" data-toggle="tab">Hoạt động</a></li>
                    <li><a href="#tabs-2" data-toggle="tab">Không hoạt động</a></li>
                </ul>
                <div class="tab-content">
                    <div id="tabs-1" class="tab-pane fade">
                        <table class="table table-bordered table-hover table-common">
                            <asp:Repeater ID="rptUsersActive" runat="server" OnItemDataBound="rptUsersActive_ItemDataBound">
                                <HeaderTemplate>
                                    <tr class="active">
                                        <th>Tên đăng nhập
                                        </th>
                                        <th>Họ
                                        </th>
                                        <th>Tên
                                        </th>
                                        <th>Email
                                        </th>
                                        <th>Website
                                        </th>
                                        <th>Đăng nhập lần cuối
                                        </th>
                                        <th>IP lần cuối
                                        </th>
                                        <th>Vai trò</th>
                                        <th></th>
                                        <th></th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <tr>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "FirstName") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "LastName") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Email") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Website") %>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLastLogin" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "LastIp") %>
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrRoles" /></td>
                                            <td>
                                                <asp:HyperLink ID="hplEdit" runat="server">Phân vai trò</asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="hplSetPermission" runat="server">Phân quyền</asp:HyperLink>
                                            </td>

                                            <td>
                                                <a href="ActivityViewing.aspx?NodeId=1&SectionId=15&ui=<%# Eval("Id")%>">Hoạt động</a>
                                            </td>
                                        </tr>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <div class="pager">
                            <svc:Pager ID="pagerUserActive" runat="server" ControlToPage="rptUsersActive" OnPageChanged="pagerUserActive_PageChanged" />
                        </div>
                    </div>
                    <div id="tabs-2" class="tab-pane fade">
                        <table class="table table-bordered table-hover table-common">
                            <asp:Repeater ID="rptUsersInActive" runat="server" OnItemDataBound="rptUsersInActive_ItemDataBound">
                                <HeaderTemplate>
                                    <tr class="active">
                                        <th>Tên đăng nhập
                                        </th>
                                        <th>Họ
                                        </th>
                                        <th>Tên
                                        </th>
                                        <th>Email
                                        </th>
                                        <th>Website
                                        </th>
                                        <th>Đăng nhập lần cuối
                                        </th>
                                        <th>IP lần cuối
                                        </th>
                                        <th>Vai trò</th>
                                        <th></th>
                                        <th></th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <tr>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "FirstName") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "LastName") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Email") %>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "Website") %>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLastLogin" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "LastIp") %>
                                            </td>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrRoles"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="hplEdit" runat="server">Phân vai trò</asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="hplSetPermission" runat="server">Phân quyền</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <div class="pager">
                            <svc:Pager ID="pagerUserInActive" runat="server" ControlToPage="rptUsersInActive"
                                OnPageChanged="pagerUserInActive_PageChanged" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="tabs-1" />
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
     <script>
         $("ul.nav-tabs > li > a").on("shown.bs.tab", function (e) {
             var id = $(e.target).attr("href").substr(1);
             $("#<%=hdnSelectedTab.ClientID%>").val(id);
         });

         var hash = $("#<%=hdnSelectedTab.ClientID%>").val();
         $("#tabs a[href='#"+ hash + "']").tab('show');
    </script>
</asp:Content>
