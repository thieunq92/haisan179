<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DashBoard"
    MasterPageFile="MO-NoScriptManager.Master" %>

<%@ MasterType VirtualPath="MO-NoScriptManager.Master" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<asp:Content ID="Head" runat="server" ContentPlaceHolderID="Head">
    <title>Bảng tổng hợp</title>
</asp:Content>
<asp:Content ID="AdminContent" runat="server" ContentPlaceHolderID="AdminContent">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <h2>Xin chào <%= CurrentUser.FullName %>, chúc bạn một ngày làm việc đầy năng lượng</h2>
        <div class="row">
            <div class="col-xs-8 nopadding-right">
                <div class="row">
                    <div class="col-xs-8">
                        <h4><strong>Tất cả đặt chỗ hôm nay- <%= DateTime.Now.ToString("dd/MM/yyyy") %> </strong></h4>
                    </div>
                    <div class="col-xs-4">
                        <h4><strong>Đặt chỗ 7 ngày tới </strong></h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-7">
                        <table class="table table-bordered table-common">
                            <tr>
                                <th>Buổi</th>
                                <th>Mã đoàn</th>
                                <th>Tên đơn vị</th>
                                <th>SLK</th>
                                <th>Tổng thu</th>
                            </tr>
                            <asp:Repeater runat="server" ID="rptBookingByPartOfDay" OnItemDataBound="rptBookingByPartOfDay_ItemDataBound">
                                <ItemTemplate>
                                    <asp:PlaceHolder runat="server" ID="plhTableRow"></asp:PlaceHolder>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr style="font-weight: bold">
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; border-bottom: 1px solid white;"></td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; border-bottom: 1px solid white;"></td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; text-align: left!important">Total</td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white;">
                                            <asp:Label runat="server" ID="lblTotalOfTotalSet"></asp:Label></td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; text-align: right!important">
                                            <asp:Label runat="server" ID="lblTotalOfTotalPrice"></asp:Label></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                    <div class="col-xs-offset-1 col-xs-4">
                        <table class="table table-bordered table-common">
                            <tr>
                                <th>Ngày</th>
                                <th>SLK</th>
                                <th>Tổng thu</th>
                            </tr>
                            <asp:Repeater runat="server" ID="rptBookingNext7Day" OnItemDataBound="rptBookingNext7Day_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><a href="RestaurantBookingByDate.aspx?NodeId=1&SectionId=15&d=<%# ((DateTime)Eval("Key")).ToString("dd/MM/yyyy") %>"><%# ((DateTime)Eval("Key")).ToString("dd/MM/yyyy") %></a></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblTotalOfTotalSet" /></td>
                                        <td style="text-align: right!important">
                                            <asp:Label runat="server" ID="lblTotalOfTotalPrice" /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <h4><strong>Booking mới ngày hôm nay</strong><img src="/images/new_blink.gif" width="40"></h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <table class="table table-bordered table-common">
                            <tr>
                                <th>Mã đoàn</th>
                                <th>SLK</th>
                                <th>Ngày ăn</th>
                                <th>Tổng thu</th>
                                <th>Tạo bởi</th>
                            </tr>
                            <asp:Repeater runat="server" ID="rptNewRestaurantBooking">
                                <ItemTemplate>
                                    <tr>
                                        <td><a href="BookingViewing.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>"><%# Eval("Code")%></a></td>
                                        <td><%# ((int)Eval("TotalSet")).ToString("#,##0.##")%></td>
                                        <td><a href="RestaurantBookingByDate.aspx?NodeId=1&SectionId=15&d=<%# ((DateTime)Eval("Date")).ToString("dd/MM/yyyy")%>"><%# ((DateTime)Eval("Date")).ToString("dd/MM/yyyy")%></a></td>
                                        <td><%# ((double)Eval("TotalPrice")).ToString("#,##0.##")%></td>
                                        <td><%# Eval("CreatedBy.FullName")%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr style="display: <%= rptNewRestaurantBooking.Items.Count == 0 ? "" : "none"%>">
                                        <td colspan="100%">Không tìm thấy bản ghi nào
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <h4><strong>Đối tác mới ngày hôm nay</strong><img src="/images/new_blink.gif" width="40"></h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <ul style="list-style-type: none; padding-left: 0">
                            <asp:Repeater runat="server" ID="rptNewAgency">
                                <ItemTemplate>
                                    <li><a href="AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=<%# Eval("Id")%>"><%# Eval("Name")%></a></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <li style="display: <%= rptNewAgency.Items.Count == 0 ? "" : "none"%>">Không tìm thấy bản ghi nào
                                    </li>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-xs-offset-1 col-xs-3 nopadding-left">
                <div class="row">
                    <div class="col-xs-12">
                        <h4><strong>Báo cáo tháng </strong></h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6 nopadding-right">
                        <ul style="list-style-type: none; padding-left: 0">
                            <li>Doanh thu:
                            </li>
                            <li>
                                <ul style="list-style-type: none; padding-left: 30px">
                                    <li>Đã thanh toán:</li>
                                    <li>Còn lại:</li>
                                </ul>
                            </li>
                            <li>Công nợ tồn:</li>
                            <li>DT có VAT Tháng:</li>
                        </ul>
                    </div>
                    <div class="col-xs-6 nopadding-left" style="text-align: right">
                        <ul style="list-style-type: none; padding-left: 0">
                            <li>
                                <asp:Label runat="server" ID="lblTotalOfTotalPrice" /></li>
                            <li>
                                <asp:Label runat="server" ID="lblTotalPaid" /></li>
                            <li>
                                <asp:Label runat="server" ID="lblReceivable" /></li>
                            <li>
                                <asp:Label runat="server" ID="lblOutstandingDebt" /></li>
                            <li>
                                <asp:Label runat="server" ID="lblMonthlyVAT" /></li>
                        </ul>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <h4><strong>Top 10 đối tác tháng</strong></h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <table class="table table-bordered table-common">
                            <tr>
                                <th>No</th>
                                <th>Tên</th>
                                <th>SLK</th>
                                <th>Tổng thu</th>
                            </tr>
                            <asp:Repeater runat="server" ID="rptTop10Agency" OnItemDataBound="rptTop10Agency_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Container.ItemIndex + 1 %></td>
                                        <td style="text-align: left!important"><a href="Receivables.aspx?NodeId=1&SectionId=15&f=<%=new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).ToString("dd/MM/yyyy")%>&t=<%= new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy") %>&ai=<%#Eval("Agency.Id")%>">
                                            <%# Eval("Agency.Name")%></td>
                                        <td><%# ((int)Eval("TotalOfTotalSet")).ToString("#,##0.##")%></td>
                                        <td style="text-align: right!important"><%# ((double)Eval("TotalOfTotalPrice")).ToString("#,##0.##") + "₫"%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr style="display: <%= rptTop10Agency.Items.Count == 0 ? "" : "none"%>">
                                        <td colspan="100%">Không tìm thấy bản ghi nào
                                        </td>
                                    </tr>
                                    <tr style="font-weight: bold">
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; border-bottom: 1px solid white;"></td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; text-align: left!important">Total</td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white;">
                                            <asp:Label runat="server" ID="lblTotal_TotalOfTotalSet" /></td>
                                        <td style="border-left: 1px solid white; border-right: 1px solid white; text-align: right!important">
                                            <asp:Label runat="server" ID="lblTotal_TotalOfTotalPrice" /></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
