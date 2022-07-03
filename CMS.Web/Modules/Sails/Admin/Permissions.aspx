<%@ Page Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true" CodeBehind="Permissions.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.Permissions" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Phân quyền chi tiết</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <h3 class="page-header">
        <asp:Literal runat="server" ID="litTitle"></asp:Literal>
    </h3>
    <div class="row">
        <div class="col-xs-12">
            <asp:Repeater ID="rptPermissions" runat="server" OnItemDataBound="rptPermissions_ItemDataBound">
                <ItemTemplate>
                    <div class="col-xs-12" id="divClear" runat="server" visible="false">
                    </div>
                    <div class="col-xs-3">
                        <asp:HiddenField ID="hiddenName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
                        <div class="checkbox">
                            <label>
                                <asp:CheckBox ID="chkPermission" runat="server" />
                            </label>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary"
                Text="Lưu"
                OnClick="btnSave_Click"></asp:Button>
        </div>
    </div>
</asp:Content>
