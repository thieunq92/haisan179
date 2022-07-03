<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantBookingPayment.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.RestaurantBookingPayment"
    MasterPageFile="NewPopup.Master" %>

<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Mã đoàn
                </div>
                <div class="col-xs-10">
                    <%= RestaurantBooking.Code %>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Tên đơn vị
                </div>
                <div class="col-xs-10">
                    <%= RestaurantBooking.Agency != null ? RestaurantBooking.Agency.Name:"" %>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Ngày ăn
                </div>
                <div class="col-xs-10">
                    <%= ((DateTime)RestaurantBooking.Date).ToString("dd/MM/yyyy") %>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Thực đơn
                </div>
                <div class="col-xs-10">
                    <%= RestaurantBooking.Menu != null ? RestaurantBooking.Menu.Name : ""%>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Tổng giá
                </div>
                <div class="col-xs-10">
                    <%= RestaurantBooking.TotalPrice.ToString("#,##0.##") + "₫"%>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Đã thanh toán
                </div>
                <div class="col-xs-2">
                    <%= RestaurantBooking.TotalPaid.ToString("#,##0.##") + "₫"%>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Số tiền thanh toán
                </div>
                <div class="col-xs-2" style="width: 12%">
                    <div class="input-group">
                        <asp:TextBox ID="txtPaid" runat="server" CssClass="form-control" placeholder="Số tiền thanh toán" Text="0" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" />
                        <span class="input-group-addon" style="padding-left: 3px">₫</span>
                    </div>
                </div>
                <div class="col-xs-2 nopadding-left">
                    <div class="input-group">
                        <span class="input-group-addon" style="padding-left: 3px">Số phiếu thu</span>
                        <asp:TextBox ID="txtReceiptVoucher" runat="server" CssClass="form-control" placeholder="Số phiếu thu" />
                    </div>
                </div>
                <div class="col-xs-2 nopadding-left">
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox runat="server" ID="chkPayByBankAccount" Text="Thanh Toán Chuyển Khoản" />
                        </label>
                    </div>
                </div>
                <div class="col-xs-3">
                    <asp:DropDownList ID="ddlBankAccount" CssClass="form-control" Style="display: none" runat="server"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Còn lại
                </div>
                <div class="col-xs-10">
                    <%= RestaurantBooking.Receivable.ToString("#,##0.##") + "₫" %>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                </div>
                <div class="col-xs-10">
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox runat="server" ID="chkPaid" Text="Đánh dấu đã thanh toán" />
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button runat="server" ID="btnPayment" CssClass="btn btn-primary" Text="Thanh toán" OnClick="btnPayment_Click" OnClientClick="return checkPaymentByBankAccount()"></asp:Button>
                </div>
            </div>
        </div>
        <h3>Lịch sử thanh toán</h3>
        <table class="table table-bordered table-hover table-common">
            <tr class="active">
                <th>Thời gian
                </th>
                <th>Trả bởi</th>
                <th>Số tiền thanh toán</th>
                <th>Tạo bởi</th>
                <asp:Repeater runat="server" ID="rptPaymentHistory">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Time","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                            <td><%# Eval("Payby.Name")%></td>
                            <td><%# ((double)Eval("Amount")).ToString("#,##0.##")%></td>
                            <td><%# Eval("Createdby.FullName")%></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $("#<%=chkPayByBankAccount.ClientID%>").on("click", function () {
            let checked = $(this).is(":checked");
            if (checked) {
                $("#<%=ddlBankAccount.ClientID%>").css("display", "block");
            }
            else $("#<%=ddlBankAccount.ClientID%>").css("display", "none");
        });

        function checkPaymentByBankAccount() {
            let chkPaid = $("#<%=chkPaid.ClientID%>").is(":checked");
            let paid = $("#<%=txtPaid.ClientID%>").val();
            if (!chkPaid && paid === "0") {
                alert("Số tiền thanh toán đang bằng 0");
                return false;
            } else if (chkPaid && paid === "0") {
                alert("Số tiền thanh toán đang bằng 0");
            }

            let checked = $("#<%=chkPayByBankAccount.ClientID%>").is(":checked");
            if (checked) {
                let bankAccount = $("#<%=ddlBankAccount.ClientID%>").val();
                if (bankAccount.length <= 0) {
                    alert("Phải chọn tài khoản nếu chọn thanh toán chuyển khoản");
                    return false;
                }
                else return true;
            } else return true;
            return false;
        }
    </script>
    <% if (!AllowPaymentBooking)
       {%>
    <script>
        $("#<%= btnPayment.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền thanh toán đặt chỗ"})
    </script>
    <%}%>
</asp:Content>

