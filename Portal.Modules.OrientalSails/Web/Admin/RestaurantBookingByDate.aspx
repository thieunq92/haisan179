<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantBookingByDate.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.RestaurantBookingByDate"
    MasterPageFile="MO.Master" %>

<%@ MasterType VirtualPath="MO.Master" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Enums.RestaurantBooking" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Quản lý đặt chỗ theo ngày</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="row">
            <div class="col-xs-9">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-1 nopadding-right">
                            Ngày 
                        </div>
                        <div class="col-xs-2 nopadding-left nopadding-right">
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" data-control="datetimepicker" autocomplete="off" placeholder="Date (dd/mm/yyyy)"></asp:TextBox>
                        </div>
                        <div class="col-xs-2">
                            <asp:Button ID="btnDisplay" runat="server" Text="Hiển thị" OnClick="btnDisplay_Click"
                                CssClass="btn btn-primary" />
                        </div>
                        <div class="col-xs-2" style="display: none">
                            <asp:TextBox runat="server" ID="txtBookingCode" placeholder="Booking code" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <br />
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <a href="<%= GetLinkPartOfDayFilter(0) %>" class="btn btn-default <%= String.IsNullOrEmpty(Request.QueryString["pod"]) ? "active":""%>">Tất cả</a>
                            <a href="<%= GetLinkPartOfDayFilter(2) %>" class="btn btn-default <%= Request.QueryString["pod"] == "2" ? "active":""%> <%= GetTotalPaxByPartOfDay(2) > 800 ? "custom-danger":"" %>">Trưa ( <%= GetTotalBookingAndPaxByTime(2) %> )</a>
                            <a href="<%= GetLinkPartOfDayFilter(3) %>" class="btn btn-default <%= Request.QueryString["pod"] == "3" ? "active":""%> <%= GetTotalPaxByPartOfDay(3) > 800 ? "custom-danger":"" %>">Tối ( <%= GetTotalBookingAndPaxByTime(3) %> )</a>
                            <a href="<%= GetLinkPartOfDayFilter(1) %>" class="btn btn-default <%= Request.QueryString["pod"] == "1" ? "active":""%> <%= GetTotalPaxByPartOfDay(1) > 800 ? "custom-danger":"" %>">Sáng ( <%= GetTotalBookingAndPaxByTime(1) %> )</a>
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
                            : Đặt chỗ đã thu tiền
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px" class="custom-warning"></div>
                            : Đặt chỗ thanh toán ngay chưa thu tiền 
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px"></div>
                            : Đặt chỗ công nợ chưa thu tiền 
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-common">
                    <thead>
                        <tr class="active">
                            <th rowspan="2">STT
                            </th>
                            <th rowspan="2">Mã đoàn
                            </th>
                            <th rowspan="2">Tên đơn vị
                            </th>
                            <th rowspan="2">Hướng dẫn viên
                            </th>
                            <th rowspan="2" colspan="2" id="time-title-cell">Giờ ăn
                            </th>
                            <th rowspan="2">Vị trí bàn ăn
                            </th>
                            <th colspan="3">Số suất ăn
                            </th>
                            <th colspan="2">Đơn giá</th>
                            <th rowspan="2">Dịch vụ
                            </th>
                            <th rowspan="2">Yêu cầu
                            </th>
                            <th rowspan="2">VAT</th>
                            <th rowspan="2">Tổng giá
                            </th>
                            <th rowspan="2">Thực thu</th>
                            <th rowspan="2" style="width: 4.55%"></th>
                        </tr>
                        <tr class="active">
                            <th>Người lớn
                            </th>
                            <th>Trẻ em
                            </th>
                            <th>Sơ sinh
                            </th>
                            <th>Người lớn
                            </th>
                            <th>Trẻ em
                            </th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="rptBooking" runat="server" OnItemDataBound="rptBooking_ItemDataBound" OnItemCommand="rptBooking_OnItemCommand">
                        <ItemTemplate>
                            <tbody>
                                <tr class="<%# ((bool)Eval("IsPaid")) ? "custom-success" : ((int)Eval("Payment")) == 1 ? "custom-warning":"" %>">
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Container.ItemIndex + 1%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                        <a href="BookingViewing.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>"><%# Eval("Code")%>
                                            <%# ((bool)Eval("HaveEmergencyUpdate") ? "<br/>" : "")%>
                                            <img src="/images/new_blink.gif" width="40" <%# ((bool)Eval("HaveEmergencyUpdate") ? "" : "style='display:none'")%>>
                                        </a>
                                        <br />
                                        <asp:LinkButton runat="server" ID="btnLock" Text="Khóa" Visible='<%# ((LockStatusEnum)Eval("LockStatus")) == LockStatusEnum.Unlocked||((LockStatusEnum)Eval("LockStatus")) == LockStatusEnum.NotLock ? true:false %>' CommandName="Lock" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Xác nhận khóa booking này')" data-toggle="tooltip" title="Khóa">
                                         <i class="fa fa-lock-open fa-lg text-success" aria-hidden="true"></i>        
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="btnUnlock" Text="Mở khóa" Visible='<%# ((LockStatusEnum)Eval("LockStatus")) == LockStatusEnum.Locked ? true:false %>' CommandName="Unlock" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Xác nhận mở khóa booking này')" data-toggle="tooltip" title="Mở khóa">
                                       <i class="fa fa-lock fa-lg text-danger" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%> ">
                                        <a href="AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=<%# Eval("Agency") != null ? Eval("Agency.Id") : ""%>"><%# Eval("Agency") != null ? Eval("Agency.TradingName"):""%>
                                    </td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" style="text-align: left!important"><%# GetBookerAndGuides((RestaurantBooking)Container.DataItem)%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((int)Eval("PartOfDay")) == 1 ? "Sáng" : ((int)Eval("PartOfDay")) == 2 ? "Trưa" : ((int)Eval("PartOfDay")) == 3 ? "Tối" : "" %></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" class="time-cell"><%# Eval("Time")%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                        <%# ((int)Eval("VITRIBANAN")) == 0 ? "" : ((int)Eval("VITRIBANAN")) == 1 ? "Phòng ăn riêng" : ((int)Eval("VITRIBANAN")) == 2 ? "Hội trường" : ((int)Eval("VITRIBANAN")) == 3 ? "Sảnh T1" : ((int)Eval("VITRIBANAN")) == 4 ? "Phòng 201" : ((int)Eval("VITRIBANAN")) == 5 ? "Phòng 202" : ""%>
                                        <%# ((bool)Eval("GALA")) == true ? " - <span style='color:red'>Gala</span>" : ""%>
                                    </td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Eval("NumberOfPaxAdult")%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Eval("NumberOfPaxChild")%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Eval("NumberOfPaxBaby")%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((Double)Eval("CostPerPersonAdult")).ToString("#,##0.##") + "₫"%></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((Double)Eval("CostPerPersonChild")).ToString("#,##0.##") + "₫"%></td>
                                    <td>
                                        <a href="MenuEditing.aspx?NodeId=1&SectionId=15&mi=<%# Eval("Menu") != null ? Eval("Menu.Id"):""%>"><%# Eval("Menu") != null ? Eval("Menu.Name"):""%>
                                    </td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" style="text-align: left!important">
                                        <%# !String.IsNullOrEmpty(((string)Eval("SpecialRequest"))) ? ((string)Eval("SpecialRequest")) .Replace("\n","<br/>") + "<br/><br/>" : ""%>
                                        <%# !String.IsNullOrEmpty(((string)Eval("MenuDetail"))) ? ((string)Eval("MenuDetail")).Replace("\n","<br/>") : ""%>
                                    </td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                        <%# ((bool)Eval("VAT")) == true ? "Yes" : ""%>
                                    </td>
                                    <td style="text-align: right!important" rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                        <%# ((Double)Eval("TotalPrice")).ToString("#,##0.##") + "₫"%>
                                    </td>
                                    <td style="text-align: right!important" rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((Double)Eval("ActuallyCollected")).ToString("#,##0.##") + "₫" %></td>
                                    <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                        <a href="javascript:void(0)" data-toggle="modal" data-target=".modal-bookingpayment" data-url="RestaurantBookingPayment.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>" data-id="aPayment">
                                            <i class="fa fa-money-bill fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Thanh toán"></i>
                                        </a>
                                        <asp:LinkButton ID="lbtExportByAgency" CommandArgument='<%#Eval("Id") %>' CommandName="ExportByAgency" runat="server" data-id="aExportForKitchen"><i class="fa fa-file-excel fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Xuất lệnh bếp theo đoàn"></i></asp:LinkButton>
                                        <a href='PrintMenu.aspx?Id=<%#Eval("Id") %>' target="_blank" data-id="aExportMenu">
                                            <i class="fa fa-calendar-minus fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Xuất thực đơn"></i>
                                        </a>
                                    </td>
                                </tr>
                                <asp:Repeater runat="server" ID="rptServiceOutside">
                                    <ItemTemplate>
                                        <tr class="<%# ((RestaurantBooking)((RepeaterItem)Container.Parent.Parent).DataItem).IsPaid ? "custom-success" : ((RestaurantBooking)((RepeaterItem)Container.Parent.Parent).DataItem).Payment == 1 ? "custom-warning":"" %>">
                                            <td><a href="javascript:void(0)" data-toggle="tooltip" title="<%# GetServiceOutsideDetail((ServiceOutside)Container.DataItem) %>"><%# Eval("Service")%>|SL:<%# Eval("Quantity")%></a></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr style="display: <%= rptBooking.Items.Count == 0 ? "" : "none"%>">
                                <td colspan="100%">Không tìm thấy bản ghi nào
                                </td>
                            </tr>
                            <tr style="display: <%= rptBooking.Items.Count > 0 ? "" : "none"%>; font-weight: bold">
                                <td colspan="6" id="total-title-cell"><strong>Tổng hợp</strong></td>
                                <td>
                                    <asp:Label runat="server" ID="lblTotalAdult"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" ID="lblTotalChild"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" ID="lblTotalBaby"></asp:Label></td>
                                <td colspan="5"></td>
                                <td style="text-align: right!important">
                                    <asp:Label runat="server" ID="lblTotalOfTotalPrice"></asp:Label></td>
                                <td style="text-align: right!important">
                                    <asp:Label runat="server" ID="lblTotalActuallyCollected"></asp:Label></td>
                                <td></td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="100%" class="custom-danger" style="font-weight: bold; text-align: left">Các bookings bị Huỷ hoặc đổi ngày cần lưu ý (Huỷ hoặc đổi ngày trong vòng 24h)
                        </td>
                    </tr>
                    <asp:Repeater ID="rptCancelledAndChangeDateBooking" runat="server" OnItemDataBound="rptCancelledAndChangeDateBooking_ItemDataBound" OnItemCommand="rptCancelledAndChangeDateBooking_ItemCommand">
                        <ItemTemplate>
                            <tr class="custom-danger">
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Container.ItemIndex + 1%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                    <a href="BookingViewing.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>"><%# Eval("Code")%></a>
                                    <%# ((bool)Eval("HaveEmergencyUpdate") ? "<br/>" : "")%>
                                    <img src="/images/new_blink.gif" width="40" <%# ((bool)Eval("HaveEmergencyUpdate") ? "" : "style='display:none'")%> />
                                    <br />
                                    <asp:LinkButton runat="server" ID="btnLock" Text="Khóa" Visible='<%# ((LockStatusEnum)Eval("LockStatus")) == LockStatusEnum.NotLock||((LockStatusEnum)Eval("LockStatus")) == LockStatusEnum.Unlocked ? true:false %>' CommandName="Lock" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Xác nhận khóa booking này')" data-toggle="tooltip" title="Khóa">
                                    <i class="fa fa-lock-open fa-lg text-success" aria-hidden="true"></i> 
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnUnlock" Text="Mở khóa" Visible='<%# ((LockStatusEnum)Eval("LockStatus")) == LockStatusEnum.Locked ? true:false %>' CommandName="Unlock" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Xác nhận mở khóa booking này')" data-toggle="tooltip" title="Mở khóa">
                                    <i class="fa fa-lock fa-lg text-danger" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%> ">
                                    <a href="AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=<%# Eval("Agency") != null ? Eval("Agency.Id") : ""%>"><%# Eval("Agency") != null ? Eval("Agency.TradingName"):""%>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" style="text-align: left!important"><%# GetBookerAndGuides((RestaurantBooking)Container.DataItem)%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((int)Eval("PartOfDay")) == 1 ? "Sáng" : ((int)Eval("PartOfDay")) == 2 ? "Trưa" : ((int)Eval("PartOfDay")) == 3 ? "Tối" : "" %></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" class="time-cell"><%# Eval("Time")%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Eval("NumberOfPaxAdult")%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Eval("NumberOfPaxChild")%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# Eval("NumberOfPaxBaby")%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((Double)Eval("CostPerPersonAdult")).ToString("#,##0.##") + "₫"%></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((Double)Eval("CostPerPersonChild")).ToString("#,##0.##") + "₫"%></td>
                                <td>
                                    <a href="MenuEditing.aspx?NodeId=1&SectionId=15&mi=<%# Eval("Menu") != null ? Eval("Menu.Id"):""%>"><%# Eval("Menu") != null ? Eval("Menu.Name"):""%>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" style="text-align: left!important">
                                    <%# !String.IsNullOrEmpty(((string)Eval("SpecialRequest"))) ? ((string)Eval("SpecialRequest")) .Replace("\n","<br/>") + "<br/><br/>" : ""%>
                                    <%# !String.IsNullOrEmpty(((string)Eval("MenuDetail"))) ? ((string)Eval("MenuDetail")).Replace("\n","<br/>") : ""%>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>" style="text-align: left!important">
                                    <%# GetCancelledOrChangeDateInformation((RestaurantBooking)Container.DataItem) %>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                    <%# ((int)Eval("VITRIBANAN")) == 0 ? "" : ((int)Eval("VITRIBANAN")) == 1 ? "Phòng ăn riêng" : ((int)Eval("VITRIBANAN")) == 2 ? "Hội trường" : ((int)Eval("VITRIBANAN")) == 3 ? "Sảnh T1" : ((int)Eval("VITRIBANAN")) == 4 ? "Phòng 201" : ((int)Eval("VITRIBANAN")) == 5 ? "Phòng 202" : ""%>
                                    <%# ((bool)Eval("GALA")) == true ? " - <span style='color:red'>Gala</span>" : ""%>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                    <%# ((bool)Eval("VAT")) == true ? "Có" : ""%>
                                </td>
                                <td style="text-align: right!important" rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>">
                                    <%# ((Double)Eval("TotalPrice")).ToString("#,##0.##") + "₫"%>
                                </td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"><%# ((Double)Eval("ActuallyCollected")).ToString("#,##0.##") + "₫" %></td>
                                <td rowspan="<%# ((IList<ServiceOutside>)Eval("ListServiceOutside")).Count + 1%>"></td>
                            </tr>
                            <asp:Repeater runat="server" ID="rptServiceOutside">
                                <ItemTemplate>
                                    <tr class="custom-danger">
                                        <td><a href="javascript:void(0)" data-toggle="tooltip" title="<%# GetServiceOutsideDetail((ServiceOutside)Container.DataItem) %>"><%# Eval("Service")%> ; SL: <%# Eval("Quantity")%></a></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <div class="row">
                    <div class="col-xs-12">
                        <asp:Button runat="server" ID="btnSalesReportExport" OnClick="btnSalesReportExport_Click" Text="Trích xuất BCDT" CssClass="btn btn-primary" />
                        <asp:Button runat="server" ID="btnExportForKitchen" OnClick="btnExportForKitchen_OnClick" Text="Xuất lệnh bếp" CssClass="btn btn-primary" />
                        <asp:Button runat="server" ID="btnLockDate" OnClick="btnLockDate_Click" Text="Khóa booking ngày" OnClientClick="return confirm('Xác nhận khóa tất cả booking trong ngày này')" CssClass="btn btn-primary" />
                    </div>
                </div>
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
    <script>
        $('a[data-target = ".modal-bookingpayment"]').click(function () {
            $(".modal iframe").attr('src', $(this).attr('data-url'))
        })
    </script>
    <script>
        //xóa cột time nếu không có dữ liệu
        $(document).ready(function () {
            var hasData = false;
            $(".time-cell").each(function () {
                if ($(this).html() !== "") {
                    hasData = true;
                }
            })
            if (!hasData) {
                $(".time-cell").remove();
                $("#time-title-cell").attr("colspan", "1");
                $("#total-title-cell").attr("colspan", "5");
            }
        })
    </script>
    <script>
        <%
        if (GetTotalPaxByPartOfDay(2) > 800)
        {%>
        $.notify("Lượng khách buổi trưa đang vượt quá công suất nhà hàng", {
            autoHide: false,
            clickToHide: true,
        });
        <%}%>
        <%
        if (GetTotalPaxByPartOfDay(1) > 800)
        {%>
        $.notify("Lượng khách buổi sáng đang vượt quá công suất nhà hàng", {
            autoHide: false,
            clickToHide: true,
        });
        <%}%>
        <%
        if (GetTotalPaxByPartOfDay(3) > 800)
        {%>
        $.notify("Lượng khách buổi tối đang vượt quá công suất nhà hàng", {
            autoHide: false,
            clickToHide: true,
        });
        <%}%>
    </script>
    <% if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Portal.Modules.OrientalSails.Enums.Roles.Administrator))
       { %>
    <script>
        $("#<%= btnLockDate.ClientID%>").prop("disabled", "true");
    </script>
    <% }%>
    <% if (!AllowExportSalesReport)
       {%>
    <script>
        $("#<%= btnSalesReportExport.ClientID%>").attr({ "disabled": "true", "title": "Bạn không có quyền trích xuất báo cáo doanh thu" });
    </script>
    <% }%>
    <% if (!AllowExportForKitchen)
       {%>
    <script>
        $("#<%= btnExportForKitchen.ClientID%>").attr({ "disabled": "true", "title": "Bạn không có quyền xuất lệnh bếp" });
        $("[data-id='aExportForKitchen']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền xuất lệnh bếp" });
        $("[data-id='aExportForKitchen']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <% }%>
    <% if (!AllowAccessPayment)
       {%>
    <script>
        $("[data-id='aPayment']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền thanh toán đặt chỗ" });
        $("[data-id='aPayment']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <% }%>
    <% if (!AllowExportMenu)
       {%>
    <script>
        $("[data-id='aExportMenu']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền xuất thực đơn" });
        $("[data-id='aExportMenu']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <% }%>
</asp:Content>
