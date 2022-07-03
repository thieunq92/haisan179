<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receivables.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.Receivables"
    MasterPageFile="MO.Master" %>
<%@ MasterType VirtualPath="MO.Master" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Báo Cáo Công Nợ</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-9">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-1">
                                Từ ngày
                            </div>
                            <div class="col-xs-2">
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="From (dd/MM/yyyy)" data-control="datetimepicker" autocomplete="off" />
                            </div>
                            <div class="col-xs-1">
                                Đến ngày
                            </div>
                            <div class="col-xs-2">
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="To (dd/MM/yyyy)" data-control="datetimepicker" autocomplete="off" />
                            </div>
                            <div class="col-xs-2">
                                Trạng thái thanh toán
                            </div>
                            <div class="col-xs-2">
                                <asp:DropDownList ID="ddlStatusPayment" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Còn lại = 0"></asp:ListItem>
                                    <asp:ListItem Value="1" Text=" Còn lại # 0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-1">
                                Mã đoàn
                            </div>
                            <div class="col-xs-2">
                                <asp:TextBox ID="txtCode" CssClass="form-control" placeholder="Mã đoàn" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-xs-1">
                                Công nợ
                            </div>
                            <div class="col-xs-2">
                                <asp:DropDownList ID="ddlPayment" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Thanh toán ngay"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Công nợ"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-xs-2">
                                Đối tác
                            </div>
                            <div class="col-xs-4">
                                <asp:TextBox ID="agencySelectornameid" CssClass="form-control" placeholder="Select agency" ReadOnly="True" runat="server"></asp:TextBox>
                                <input id="agencySelector" type="hidden" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-3">
                    <strong>Chú thích</strong>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-12">
                                <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px" class="custom-success"></div>
                                : Booking đã thu tiền
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-12">
                                <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px" class="custom-warning"></div>
                                : Booking thanh toán ngay chưa thu tiền 
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-12">
                                <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px"></div>
                                : Booking công nợ chưa thu tiền 
                            </div>
                        </div>
                    </div>
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
                <table class="table table-bordered table-common">
                    <tr class="active">
                        <th rowspan="2">STT</th>
                        <th rowspan="2">Mã đoàn
                        </th>
                        <th rowspan="2">Đối tác
                        </th>
                        <th rowspan="2">
                            <asp:Literal ID="litOrderDate" runat="server"></asp:Literal>
                        </th>
                        <th rowspan="2">Thời gian
                        </th>
                        <th rowspan="2" style="width: 8%">Menu
                        </th>
                        <th colspan="3">Số lượng khách
                        </th>
                        <th colspan="2">Đơn giá
                        </th>
                        <th colspan="2">FOC
                        </th>
                        <th rowspan="2">Tiền ăn
                        </th>
                        <th rowspan="2" style="word-wrap: break-word; width: 1%;">Tổng dịch vụ ngoài
                        </th>
                        <th rowspan="2">Tổng thu
                        </th>
                        <th rowspan="2" style="word-wrap: break-word; width: 1%;">Đã thanh toán
                        </th>
                        <th rowspan="2">Còn lại
                        </th>
                        <th rowspan="2" style="word-wrap: break-word; width: 1%;">Ngày thanh toán cuối cùng
                        </th>
                        <th rowspan="2"></th>
                        <th rowspan="2">Pay
                        <input type="checkbox" id="chkTemplate" />
                        </th>
                    </tr>
                    <tr class="active">
                        <th>Adult
                        </th>
                        <th>Child
                        </th>
                        <th>Baby
                        </th>
                        <th>Adult
                        </th>
                        <th>Child
                        </th>
                        <th>Adult
                        </th>
                        <th>Child
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptReceivablesTable" OnItemDataBound="rptReceivablesTable_OnItemDataBound">
                        <ItemTemplate>
                            <tr class="<%#GetClassBooking((RestaurantBooking)Container.DataItem)%>">
                                <td><%#Container.ItemIndex + 1%>
                                </td>
                                <td><a href="BookingViewing.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>"><%# Eval("Code") %></td>
                                <td>
                                    <asp:HyperLink ID="hplAgency" runat="server">HyperLink</asp:HyperLink>
                                </td>
                                <td><%# Eval("DateString")%></td>
                                <td><%# Eval("PartOfDayString")%></td>
                                <td><%# Eval("Menu.Name")%></td>
                                <td><%# Eval("NumberOfPaxAdult")%></td>
                                <td><%# Eval("NumberOfPaxChild")%></td>
                                <td><%# Eval("NumberOfPaxBaby")%></td>
                                <td><%# ((Double)Eval("CostPerPersonAdult")).ToString("#,##0.##") + "₫" %></td>
                                <td><%# ((Double)Eval("CostPerPersonChild")).ToString("#,##0.##") + "₫"%></td>
                                <td><%# Eval("NumberOfDiscountedPaxAdult")%></td>
                                <td><%# Eval("NumberOfDiscountedPaxChild")%></td>
                                <td style="text-align: right!important"><%# ((Double)Eval("TotalPriceOfSet")).ToString("#,##0.##") + "₫"%></td>
                                <td style="text-align: right!important">
                                    <span class="service_tooltip" data-tooltip-content="#tooltip_<%#Eval("Id")%>"><%# Eval("TotalServiceOutsidePrice","{0:#,##0.##}") + "₫"%></span>
                                    <div class="tooltip_templates" style="display: none">
                                        <div id="tooltip_<%#Eval("Id") %>">
                                            <asp:Literal ID="litServices" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </td>
                                <td style="text-align: right!important"><%# Eval("TotalPrice","{0:#,##0.##}") + "₫" %></td>
                                <td style="text-align: right!important"><%# Eval("TotalPaid","{0:#,##0.##}") + "₫" %></td>
                                <td style="text-align: right!important"><%# Eval("Receivable","{0:#,##0.##}") + "₫" %></td>
                                <td>
                                    <asp:Literal ID="litLastDatePayment" runat="server"></asp:Literal></td>
                                <td><a data-toggle="modal" data-target=".modal-bookingpayment" data-url="RestaurantBookingPayment.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>" data-id="aPayment">
                                    <i class="fa fa-money-bill fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Thanh toán"></i>
                                </a></td>
                                <td>
                                    <%# !((bool)Eval("IsPaid")) ? "<input type='checkbox' data-id='chkPay' data-restaurantbookingid='"+Eval("Id")+"' />":"" %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr style="display: <%= rptReceivablesTable.Items.Count == 0 ? "" : "none"%>">
                                <td colspan="100%">Không tìm thấy bản ghi nào
                                </td>
                            </tr>
                            <tr style="display: <%= rptReceivablesTable.Items.Count > 0 ? "" : "none"%>; font-weight: bold">
                                <td colspan="13">Tổng</td>
                                <td style="text-align: right!important">
                                    <asp:Literal ID="litTotalPriceOfSet" runat="server"></asp:Literal></td>
                                <td style="text-align: right!important">
                                    <asp:Literal ID="litTotalServiceOutsidePrice" runat="server"></asp:Literal></td>
                                <td style="text-align: right!important">
                                    <asp:Literal ID="litActuallyCollected" runat="server"></asp:Literal></td>
                                <td style="text-align: right!important">
                                    <asp:Literal ID="litTotalPaid" runat="server"></asp:Literal></td>
                                <td style="text-align: right!important">
                                    <asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
                                <td colspan="3">
                                    <a id="btnSelectedPayment" data-toggle="modal" data-target=".modal-bookingselectedpayment">
                                        <i class="fa fa-money-check-alt fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Thanh toán tất cả booking đã chọn"></i>
                                        Thanh toán nhiều
                                    </a>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
                <div class="modal fade modal-bookingpayment" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" role="document" style="width: 1230px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h3 class="modal-title">Thanh toán</h3>
                            </div>
                            <div class="modal-body">
                                <iframe frameborder="0" width="1200" scrolling="no" onload="resizeIframe(this)"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal fade modal-bookingselectedpayment" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" role="document" style="width: 1230px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h3 class="modal-title">Thanh toán</h3>
                            </div>
                            <div class="modal-body">
                                <iframe frameborder="0" width="1200" scrolling="no" onload="resizeIframe(this)"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $(document).ready(function () {
            $('.service_tooltip').tooltipster({
                animation: 'fade',
                delay: 0,
                theme: 'tooltipster-borderless'
            });
        });
    </script>
    <script>
        $('a[data-target = ".modal-bookingpayment"]').click(function () {
            $(".modal-bookingpayment iframe").attr('src', $(this).attr('data-url'))
        })
    </script>
    <script>
        $('#<%=agencySelectornameid.ClientID%>').click(function () {
            var width = 800;
            var height = 600;
            window.open('/Modules/Sails/Admin/AgencySelectorPage.aspx?NodeId=1&SectionId=15&clientid=<%=agencySelector.ClientID%>', 'Agencyselect', 'width=' + width + ',height=' + height + ',top=' + ((screen.height / 2) - (height / 2)) + ',left=' + ((screen.width / 2) - (width / 2)));
        });
    </script>
    <script>
        var listSelectedRestaurantBooking = [];
        $("#chkTemplate").change(function () {
            $("[data-id='chkPay']").prop("checked", $(this).prop("checked"));
        })
        $("#btnSelectedPayment").click(function () {
            listSelectedRestaurantBooking = [];
            $("[data-id='chkPay']").each(function (i, e) {
                var restaurantBookingId = $(this).attr("data-restaurantbookingid");
                if ($(this).is(":checked")) {
                    listSelectedRestaurantBooking.push(restaurantBookingId);
                }
            })
        })
    </script>
    <script>
        $('a[data-target = ".modal-bookingselectedpayment"]').click(function () {
            $(".modal-bookingselectedpayment iframe").attr('src', 'RestaurantBookingSelectedPayment.aspx?NodeId=1&SectionId=15&lbi=' + listSelectedRestaurantBooking);
        })
    </script>
    <% if (!AllowExportReceivable)
       {%>
    <script>
        $("#<%= btnExport.ClientID%>").attr({ "disabled": "true", "title": "Bạn không có quyền trích xuất công nợ" })
    </script>
    <%}%>
    <% if (!AllowAccessPayment)
       {%>
    <script>
        $("[data-id='aPayment']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền truy cập thanh toán đặt chỗ" });
        $("[data-id='aPayment']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <script>
        $("#btnSelectedPayment").addClass("fa-disabled").attr({ "title": "Bạn không có quyền truy cập thanh toán đặt chỗ" })
            .find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền truy cập thanh toán đặt chỗ" });
        $("#btnSelectedPayment").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <%}%>
</asp:Content>
