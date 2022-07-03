<%@ Page Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true"
    CodeBehind="DocumentManage.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DocumentManage"
    Title="Document Management" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="Portal.Modules.OrientalSails" Namespace="Portal.Modules.OrientalSails.Web.Controls"
    TagPrefix="orc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="page-header">
        <h3>Document management</h3>
    </div>
    <div class="row">
        <div class="col-xs-4">
            <ul style="list-style-type: none">
                <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID='hplEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'
                                CommandName="edit">
                            </asp:HyperLink>
                            <ul style="list-style-type: none">
                                <asp:Repeater ID="rptDocuments" runat="server" OnItemDataBound="rptDocuments_ItemDataBound">
                                    <ItemTemplate>
                                        <li>
                                            <asp:HyperLink ID='hplEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'
                                                CommandName="edit">
                                            </asp:HyperLink></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="col-xs-8">
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-1">
                        Name
                    </div>
                    <div class="col-xs-11">
                        <asp:TextBox ID="txtServiceName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-1">
                        Category
                    </div>
                    <div class="col-xs-11">
                        <asp:DropDownList ID="ddlSuppliers" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-1">
                        Document file
                    </div>
                    <div class="col-xs-11">
                        <asp:HyperLink runat="server" ID="hplCurrentFile" Visible="False"></asp:HyperLink><asp:FileUpload runat="server" ID="fileUpload"></asp:FileUpload>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="service"
                            CssClass="btn btn-primary" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                            CssClass="btn btn-primary" OnClientClick="return confirm('Are you sure ?')"/>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
