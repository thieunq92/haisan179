<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantBookingSelectedPayment.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.RestaurantBookingSelectedPayment"
    MasterPageFile="NewPopup.Master" %>

<asp:Content ID="AdminContent" runat="server" ContentPlaceHolderID="AdminContent">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <table class="table table-bordered table-common ">
            <tr>
                <th>Mã đoàn</th>
                <th>Tên đơn vị</th>
                <th>Ngày ăn</th>
                <th>Thực đơn</th>
                <th>Tổng thu</th>
                <th>Đã thanh toán</th>
                <th>Còn lại</th>
            </tr>
            <tr>
                <asp:Repeater runat="server" ID="rptRestaurantBooking">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Code")%></td>
                            <td style="text-align: left!important"><%# Eval("Agency.Name") %></td>
                            <td><%# ((DateTime?)Eval("Date")).HasValue ? ((DateTime?)Eval("Date")).Value.ToString("dd/MM/yyyy") :"" %></td>
                            <td><%# Eval("Menu.Name") %></td>
                            <td style="text-align: right!important"><%# ((Double)Eval("TotalPrice")).ToString("#,##0.##")+"₫"%></td>
                            <td style="text-align: right!important"><%# ((Double)Eval("TotalPaid")).ToString("#,##0.##")+"₫"%></td>
                            <td style="text-align: right!important"><%# ((Double)Eval("Receivable")).ToString("#,##0.##")+"₫"%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr style="display: <%= rptRestaurantBooking.Items.Count == 0 ? "" : "none"%>">
                            <td colspan="100%">Không tìm thấy bản ghi nào
                            </td>
                        </tr>
                        <tr style="font-weight: bold; display: <%= rptRestaurantBooking.Items.Count > 0 ? "" : "none"%>">
                            <td colspan="4">Tổng</td>
                            <td style="text-align: right!important"><%= ListRestaurantBooking.Select(x=>x.TotalPrice).Sum().ToString("#,##0.##")+"₫"%></td>
                            <td style="text-align: right!important"><%= ListRestaurantBooking.Select(x=>x.TotalPaid).Sum().ToString("#,##0.##")+"₫"%></td>
                            <td style="text-align: right!important"><%= ListRestaurantBooking.Select(x=>x.Receivable).Sum().ToString("#,##0.##")+"₫"%></td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </tr>
        </table>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Tổng thu
                </div>
                <div class="col-xs-1" style="text-align: right!important; width: 10%">
                    <%= ListRestaurantBooking.Select(x=>x.TotalPrice).Sum().ToString("#,##0.##")+"₫"%>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Đã thanh toán
                </div>
                <div class="col-xs-1" style="text-align: right!important; width: 10%">
                    <%= ListRestaurantBooking.Select(x=>x.TotalPaid).Sum().ToString("#,##0.##")+"₫"%>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Còn lại
                </div>
                <div class="col-xs-1" style="text-align: right!important; width: 10%">
                    <span id="lblTotalReceivable"><%= ListRestaurantBooking.Select(x=>x.Receivable).Sum().ToString("#,##0.##")+"₫"%></span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Số tiền thanh toán
                </div>
                <div class="col-xs-2" style="width: 11.4%">
                    <div class="input-group">
                        <asp:TextBox ID="txtPaid" runat="server" CssClass="form-control" placeholder="Số tiền thanh toán" Text="0" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':true" Style="padding: 0" />
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
                <div class="col-xs-3 nopadding-left">
                    <asp:DropDownList ID="ddlBankAccount" CssClass="form-control" Style="display: none" runat="server"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Số tiền thiếu sau thanh toán
                </div>
                <div class="col-xs-1" style="text-align: right!important; width: 10%">
                    <span id="lblDeficit"></span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    Số tiền còn lại nhỏ nhất 
                </div>
                <div class="col-xs-1" style="text-align: right!important; width: 10%">
                    <span id="lblLowwestReceivable"
                        data-bookingid='<%= ListRestaurantBooking.Count > 0 ? ListRestaurantBooking.OrderBy(x=>x.Receivable).FirstOrDefault().Id.ToString() : ""%>'
                        data-bookingcode='<%= ListRestaurantBooking.Count > 0 ? ListRestaurantBooking.OrderBy(x=>x.Receivable).FirstOrDefault().Code : ""%>'>
                        <%= ListRestaurantBooking.Count > 0 ? ListRestaurantBooking.OrderBy(x=>x.Receivable).FirstOrDefault().Receivable.ToString("#,##0.##") + "₫" : "0₫" %>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button runat="server" ID="btnPayment" CssClass="btn btn-primary" Text="Thanh toán" OnClick="btnPayment_Click"></asp:Button>
                </div>
            </div>
        </div>
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
    </script>
    <script>
        $(document).ready(function () {
            calculateDeficit();
        })
        $("#<%= txtPaid.ClientID %>").on("input", function () {
            calculateDeficit();
        })
        function calculateDeficit() {
            var totalReceivable = parseFloat($("#lblTotalReceivable").html().replace('₫', '').replace(/,/g, ''));
            var totalPaid = parseFloat($("#<%= txtPaid.ClientID %>").val().replace(/,/g, ''));
            var deficit = totalReceivable - totalPaid;
            $("#lblDeficit").html(deficit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫");
        }
    </script>
    <script>
        $.validator.addMethod("valueNotEquals", function (value, element, arg) {
            return arg !== value;
        }, "");
        $("#aspnetForm").validate({
            rules: {
                <%=ddlBankAccount.UniqueID%>: { valueNotEquals: "" },
            },
            messages: {
                <%=ddlBankAccount.UniqueID%>: { valueNotEquals: "Yêu cầu chọn tài khoản thanh toán" },
            },     
            errorElement: "em",
            errorPlacement: function (error, element) {
                error.addClass("help-block");

                if (element.prop("type") === "checkbox") {
                    error.insertAfter(element.parent("label"));
                } else {
                    error.insertAfter(element);
                }

                if (element.siblings("span").prop("class") === "input-group-addon") {
                    error.insertAfter(element.parent()).css({ color: "#a94442" });
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).closest("div").addClass("has-error").removeClass("has-success");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).closest("div").removeClass("has-error");
            } 
        });
    </script>
    <script>
        $("#<%=chkPayByBankAccount.ClientID%>").change(function(){
            if($("#<%= ddlBankAccount.ClientID%>").is(":hidden")){
                $("#<%= ddlBankAccount.ClientID%>").siblings().hide()
            }
        })
    </script>
    <script>
        $("#<%= btnPayment.ClientID %>").click(function(e){
            e.preventDefault();
            var lowwestReceivable = parseFloat($("#lblLowwestReceivable").html().replace('₫', '').replace(/,/g, ''));
            var totalReceivable = parseFloat($("#lblTotalReceivable").html().replace('₫', '').replace(/,/g, ''));
            var totalPaid = parseFloat($("#<%= txtPaid.ClientID %>").val().replace(/,/g, ''));
            var deficit = totalReceivable - totalPaid;
            var excessCash = totalPaid - (totalReceivable - lowwestReceivable);
            if(deficit < lowwestReceivable){
                $.confirm({
                    title:"",
                    content:"Số tiền thừa " + excessCash.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫ "+"sẽ được trả cho booking <a href='BookingViewing.aspx?NodeId=1&SectionId=15&bi="+$("#lblLowwestReceivable").attr("data-bookingId")+"'>" 
                    + $("#lblLowwestReceivable").attr("data-bookingCode")+"</a>. Hãy xác nhận",
                    buttons: {               
                        confirm: {
                            text: 'Xác nhận',
                            btnClass: 'btn-blue',
                            action: function(){
                                __doPostBack("<%= btnPayment.UniqueID %>", "OnClick");
                            },
                        },
                        cancel:{
                            text:"Hủy",
                        }
                    }
                });
            }
        })
    </script>
    <% if (!AllowPaymentBooking)
       {%>
    <script>
        $("#<%= btnPayment.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền thanh toán đặt chỗ"})
    </script>
    <%}%>
</asp:Content>

