<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingManagement.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingManagement"
    MasterPageFile="MO.Master" %>

<%@ MasterType VirtualPath="MO.Master" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Enums.RestaurantBooking" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Quản lý đặt chỗ</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="row">
            <div class="col-xs-9">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            Mã đoàn   
                        </div>
                        <div class="col-xs-3">
                            <asp:TextBox runat="server" ID="txtCode" CssClass="form-control" placeholder="Mã đoàn (HLXXXXX)" />
                        </div>
                        <div class="col-xs-2">
                            Ngày
                        </div>
                        <div class="col-xs-3">
                            <asp:TextBox runat="server" ID="txtDate" CssClass="form-control" placeholder="Ngày (dd/MM/yyyy)" data-control="datetimepicker" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            Tên đơn vị
                        </div>
                        <div class="col-xs-3">
                            <asp:TextBox runat="server" ID="txtAgency" CssClass="form-control" placeholder="Tên đơn vị" />
                        </div>
                        <div class="col-xs-2">
                            Giờ ăn
                        </div>
                        <div class="col-xs-3">
                            <asp:DropDownList runat="server" ID="ddlPartOfDay" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="-1" Text="-- Giờ ăn --" />
                                <asp:ListItem Value="1" Text="Sáng" />
                                <asp:ListItem Value="2" Text="Trưa" />
                                <asp:ListItem Value="3" Text="Tối" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Button runat="server" ID="btnDisplay" Text="Hiển thị" OnClick="btnDisplay_Click" CssClass="btn btn-primary" />
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
                            : Đặt chỗ đã chạy
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px" class="custom-danger"></div>
                            : Đặt chỗ bị hủy
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <div style="width: 20px; height: 10px; display: inline-block; border: solid 1px"></div>
                            : Đặt chỗ đang chờ xác nhận 
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-common">
                    <tr class="active">
                        <th rowspan="2">Ngày
                        </th>
                        <th rowspan="2">Mã đoàn
                        </th>
                        <th rowspan="2">Tên đơn vị
                        </th>
                        <th rowspan="2" colspan="2" id="time-title-cell">Giờ ăn
                        </th>
                        <th colspan="3">Số suất ăn
                        </th>
                        <th rowspan="2">Thực đơn
                        </th>
                        <th rowspan="2">Yêu cầu
                        </th>
                        <th rowspan="2">Tổng giá
                        </th>
                        <th rowspan="2">Trạng thái
                        </th>
                        <th rowspan="2">Thanh toán
                        </th>
                        <th rowspan="2"></th>
                    </tr>
                    <tr class="active">
                        <th>Người lớn
                        </th>
                        <th>Trẻ em
                        </th>
                        <th>Sơ sinh
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptBooking">
                        <ItemTemplate>
                            <tr class="<%# GetColor((StatusEnum)Eval("Status"),(DateTime?)Eval("Date"))%>">
                                <td><%# Eval("Date","{0:dd/MM/yyyy}")%></td>
                                <td><a href="BookingViewing.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>"><%# Eval("Code")%></td>
                                <td><a href="AgencyView.aspx?NodeId=1&SectionId=15&AgencyId=<%# Eval("Agency")!= null ? Eval("Agency.Id"):""%>"><%# Eval("Agency")!= null ? Eval("Agency.TradingName"):""%></a></td>
                                <td><%# ((int)Eval("PartOfDay")) == 1 ? "Sáng" : ((int)Eval("PartOfDay")) == 2 ? "Trưa" : ((int)Eval("PartOfDay")) == 3 ? "Tối" : "" %></td>
                                <td class="time-cell"><%# Eval("Time")%></td>
                                <td><%# Eval("NumberOfPaxAdult")%></td>
                                <td><%# Eval("NumberOfPaxChild")%></td>
                                <td><%# Eval("NumberOfPaxBaby")%></td>
                                <td><a href="MenuEditing.aspx?NodeId=1&SectionId=15&mi=<%# Eval("Menu") != null ? Eval("Menu.Id") : ""%>"><%# Eval("Menu") != null ? Eval("Menu.Name") : ""%></td>
                                <td style="text-align: left!important">
                                    <%# !String.IsNullOrEmpty(((string)Eval("SpecialRequest"))) ? ((string)Eval("SpecialRequest")) .Replace("\n","<br/>") + "<br/><br/>" : ""%>
                                    <%# !String.IsNullOrEmpty(((string)Eval("MenuDetail"))) ? ((string)Eval("MenuDetail")).Replace("\n","<br/>") : ""%>
                                </td>
                                <td style="text-align: right!important"><%# Eval("TotalPrice","{0:#,##0.##}") + "₫"%></td>
                                <td><%# GetStatus((StatusEnum)Eval("Status")) %></td>
                                <td><%# ((int)Eval("Payment")) == 0 ? "" : ((int)Eval("Payment")) == 1 ? "Thanh toán ngay" : "Công nợ" %></td>
                                <td>
                                    <a href="javascript:void(0)" data-toggle="modal" data-target=".modal-bookinghistory" data-url="RestaurantBookingHistory.aspx?NodeId=1&SectionId=15&bi=<%# Eval("Id")%>" data-id="aHistory">
                                        <i class="fa fa-history fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Xem lịch sử"></i>
                                    </a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <nav arial-label="...">
                    <div class="pager">
                        <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptBooking"
                            PagerLinkMode="HyperLinkQueryString" PageSize="50" />
                    </div>
                </nav>
                <div class="modal fade modal-bookinghistory" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" role="document" style="width: 1230px">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h3 class="modal-title">Lịch sử</h3>
                            </div>
                            <div class="modal-body">
                                <iframe frameborder="0" width="1200" onload="resizeIframe(this)"></iframe>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
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
        $('a[data-target = ".modal-bookinghistory"]').click(function () {
            $(".modal iframe").attr('src', $(this).attr('data-url'))
        })
    </script>
    <% if (!AllowViewHistoryBooking)
       { %>
    <script>
        $("[data-id='aHistory']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền xem lịch sử" });
        $("[data-id='aHistory']").click(function (e) {
            e.stopPropagation();
        });
    </script>
    <% } %>
</asp:Content>
