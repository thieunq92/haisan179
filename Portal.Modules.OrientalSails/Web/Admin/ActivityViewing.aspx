<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityViewing.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ActivityViewing"
    MasterPageFile="MO-NoScriptManager.Master" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ MasterType VirtualPath="MO-NoScriptManager.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Theo dõi hoạt động</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <h3 class="page-header">
        <%= User.FullName %>
    </h3>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Từ ngày
            </div>
            <div class="col-xs-2">
                <asp:TextBox runat="server" ID="txtFrom" CssClass="form-control" placeholder="Từ ngày (dd/mm/yyyy)" data-control="datetimepicker" />
            </div>
            <div class="col-xs-1">
                Đến ngày
            </div>
            <div class="col-xs-2">
                <asp:TextBox runat="server" ID="txtTo" CssClass="form-control" placeholder="Đến ngày (dd/mm/yyyy)" data-control="datetimepicker" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button runat="server" ID="btnDisplay" Text="Hiển thị" CssClass="btn btn-primary" OnClick="btnDisplay_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <table class="table table-bordered table-hover table-common">
            <tr class="active">
                <th>STT</th>
                <th>Thời gian</th>
                <th>Chức năng</th>
                <th>Chi tiết</th>
                <th>Đối tượng</th>
            </tr>
            <asp:Repeater runat="server" ID="rptActivity">
                <ItemTemplate>
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# ((DateTime)Eval("CreatedTime")).ToString("dd/MM/yyyy HH:mm:ss")%></td>
                        <td><%# Eval("Function")%></td>
                        <td><%# Eval("Detail")%></td>
                        <td><%# GetLink((ActivityLogging)Container.DataItem)%>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr style="display: <%= rptActivity.Items.Count == 0 ? "" : "none"%>">
                        <td colspan="100%">Không tìm thấy bản ghi nào
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
</asp:Content>
