<%@ Page Language="C#" MasterPageFile="MO-NoScriptManager.Master" AutoEventWireup="true"
    CodeBehind="AgencyList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyList" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Trang quản lý đối tác</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="search-panel">
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-1">
                        Tên
                    </div>
                    <div class="col-xs-3">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Tên"></asp:TextBox>
                    </div>
                    <div class="col-xs-1 nopadding-right">
                        Sales phụ trách
                    </div>
                    <div class="col-xs-3">
                        <asp:DropDownList ID="ddlSales" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-1 nopadding-right">
                        Tình trạng hợp đồng
                    </div>
                    <div class="col-xs-3">
                        <asp:DropDownList runat="server" ID="ddlContracts" CssClass="form-control">
                            <asp:ListItem Value="-1">Tất cả tình trạng hợp đồng</asp:ListItem>
                            <asp:ListItem Value="0">Không có hợp đồng</asp:ListItem>
                            <asp:ListItem Value="4">Hợp đồng đã gửi</asp:ListItem>
                            <asp:ListItem Value="2">Hợp đồng còn hiệu lực</asp:ListItem>
                            <asp:ListItem Value="3">Hợp đồng hết hạn trong 30 ngày</asp:ListItem>
                            <asp:ListItem Value="1">Hợp đồng đã hết hạn</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-12">
                        <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary"
                            OnClick="btnSearch_Click" Text="Hiển thị"></asp:Button>
                        <asp:Button runat="server" ID="btnReboundSale" CssClass="btn btn-primary"
                            OnClick="btnReboundSale_Click" Text="Rebound sales"></asp:Button>
                        <asp:Button runat="server" ID="btnRecheck" CssClass="btn btn-primary"
                            OnClick="btnRecheck_Click" Text="Recheck sales"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
        <div class="agency-panel">
            <div class="row">
                <div class="col-xs-12">
                    <table class="table table-bordered table-hover table-agency">
                        <tr class="active">
                            <th>STT
                            </th>
                            <th>Tên
                            </th>
                            <th>Số điện thoại
                            </th>
                            <th>Email
                            </th>
                            <th>Tình trạng hợp đồng
                            </th>
                            <th>Thanh toán
                            </th>
                            <th>Sales phụ trách
                            </th>
                            <th>Last booking
                            </th>
                            <th>VAT</th>
                        </tr>
                        <asp:Repeater ID="rptAgencies" runat="server" OnItemDataBound="rptAgencies_ItemDataBound">
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="item">
                                    <td>
                                        <%# Container.ItemIndex + 1%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hplName" runat="server"></asp:HyperLink>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem,"Phone") %>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem,"Email") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litContract" runat="server"></asp:Literal>
                                        <asp:HyperLink ID="hplContract" runat="server"></asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litPayment"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litSale" runat="server"></asp:Literal>
                                    </td>
                                    <td id="tdLastBooking" runat="server"></td>
                                    <td>
                                        <asp:CheckBox ID="chkVat" runat="server" Enabled="False" /></td>
                                    <td>
                                        <asp:HyperLink ID="hplEdit" runat="server">
                                        <i class="fa fa-pencil-square-o fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i>
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%if (rptAgencies.Items.Count == 0)
                          {%>
                        <tr>
                            <td colspan="100%">Không tìm thấy bản ghi nào</td>
                        </tr>
                        <%}%>
                    </table>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Button runat="server" ID="btnExportAgency" CssClass="btn btn-primary"
                                    Text="Export agency" OnClick="btnExport_Click"></asp:Button>
                            </div>
                            <div class="col-xs-12">
                                <div class="pager">
                                    <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptAgencies"
                                        OnPageChanged="pagerBookings_PageChanged" PageSize="20" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
