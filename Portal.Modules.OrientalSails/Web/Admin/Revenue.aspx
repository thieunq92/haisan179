<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MO.Master" CodeBehind="Revenue.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.Revenue" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Báo Cáo Doanh Thu</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    From
                </div>
                <div class="col-xs-2">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="From (dd/MM/yyyy)" data-control="datetimepicker" autocomplete="off" />
                </div>
                <div class="col-xs-1">
                    To
                </div>
                <div class="col-xs-2">
                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="To (dd/MM/yyyy)" data-control="datetimepicker" autocomplete="off" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button ID="btnDisplay" runat="server" CssClass="btn btn-primary" Text="Hiển thị" OnClick="btnDisplay_Click" />
                    <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Trích xuất" OnClick="btnExport_OnClick" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-hover table-common">
                    <tr class="active">
                        <th rowspan="2">Ngày
                        </th>
                        <th colspan="3">Số lượng khách
                        </th>
                        <th rowspan="2">Tổng Giá
                        </th>
                        <th rowspan="2">Tổng trích ngoài
                        </th>
                        <th rowspan="2">Tổng dịch vụ ngoài
                        </th>
                        <th rowspan="2">Thực thu
                        </th>
                        <th rowspan="2">Đã Thanh toán
                        </th>
                        <th rowspan="2">Còn lại
                        </th>
                    </tr>
                    <tr class="active">
                        <th>Sáng 
                        </th>
                        <th>Trưa 
                        </th>
                        <th>Tối
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptRevenue" OnItemDataBound="rptRevenue_OnItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                <td>
                                    <asp:Literal ID="litSang" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litTrua" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litToi" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litTongGia" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litTrichNgoai" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litDichVuNgoai" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litThucThu" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litDaThanhToan" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litConLai" runat="server"></asp:Literal></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <td>Tổng</td>
                            <td>
                                <asp:Literal ID="litTotalSang" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalTrua" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalToi" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litTotalTongGia" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litTotalTrichNgoai" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalDichVuNgoai" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalThucThu" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalDaThanhToan" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalConLai" runat="server"></asp:Literal></td>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
                <div class="modal fade modal-bookingpayment" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" role="document" style="width: 1230px; height: 580px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h3 class="modal-title">Booking payment</h3>
                            </div>
                            <div class="modal-body">
                                <iframe frameborder="0" width="1200" height="570"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <% if (!AllowExportRevenue)
       {%>
    <script>
        $("#<%= btnExport.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền trích xuất doanh thu"})
    </script>
    <%}%>
</asp:Content>
