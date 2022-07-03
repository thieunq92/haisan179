<%@ Page Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true"
    CodeBehind="AgencyEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyEdit" %>
<%@ MasterType VirtualPath="MO.Master" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls.FileUpload"
    TagPrefix="svc" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title><%= Agency.Id == 0 ? "Thêm đối tác" : "Sửa đối tác" %></title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="container agency-panel">
            <h4><strong>Thông tin đối tác</strong></h4>
            <div class="row">
                <div class="col-xs-1"></div>
                <div class="col-xs-10">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Tên
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="textBoxName" runat="server" CssClass="form-control" placeholder="Tên"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Tên giao dịch
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="txtTradingName" runat="server" CssClass="form-control" placeholder="Tên giao dịch"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Số điện thoại
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Số điện thoại"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Email
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Địa chỉ
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Địa chỉ"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Thông tin xuất hóa đơn
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="txtInvoice" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Thông tin xuất hóa đơn"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Thanh toán
                            </div>
                            <div class="col-xs-3">
                                <asp:DropDownList ID="ddlPaymentType" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="" Text="-- Chọn thanh toán--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Thanh toán ngay"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Công nợ"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                VAT
                            </div>
                            <div class="col-xs-10">
                                <asp:CheckBox ID="chkVat" runat="server"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Thông tin khác
                            </div>
                            <div class="col-xs-10">
                                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Thông tin khác"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                            </div>
                            <div class="col-xs-10">
                                <asp:Literal ID="litCreated" runat="server"></asp:Literal>
                                <asp:Literal ID="litModified" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                            </div>
                            <div class="col-xs-10">
                                <asp:Button runat="server" ID="buttonSave" CssClass="btn btn-primary"
                                    Text="Lưu" OnClick="buttonSave_Click"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <h4><strong>Sales phụ trách</strong></h4>
            <div class="row">
                <div class="col-xs-1"></div>
                <div class="col-xs-10">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Sales phụ trách
                            </div>
                            <div class="col-xs-3">
                                <asp:DropDownList ID="ddlSales" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Text="--Sales phụ trách--" Value="-1"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                            <div class="col-xs-3">
                                <asp:TextBox ID="txtSaleStart" runat="server" CssClass="form-control" placeholder="Bắt đầu từ" data-control="datetimepicker"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                Lịch sử
                            </div>
                            <div class="col-xs-8">
                                <table style="width: 100%;">
                                    <asp:Repeater ID="rptHistory" runat="server" OnItemDataBound="rptHistory_ItemDataBound">
                                        <ItemTemplate>
                                            <tr id="trLine" runat="server">
                                                <td>
                                                    <asp:Literal ID="litSale" runat="server"></asp:Literal>
                                                    bắt đầu từ
                                                <asp:Literal ID="litSaleStart" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
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
        $("#aspnetForm").validate({
            rules: {
                <%=textBoxName.UniqueID%>: {required: true},
                <%=txtTradingName.UniqueID%>: {required: true}
            },
            messages: {
                <%=textBoxName.UniqueID%>: {required: "Yêu cầu điền tên đối tác"},
                <%=txtTradingName.UniqueID%>: {required: "Yêu cầu điền tên giao dịch"}
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
    <%if (Agency.Id == 0)
      {
          if (!AllowAddAgency)
          {%>
    <script>
        $("#<%= buttonSave.ClientID %>").attr({"disabled":"true","title":"Bạn không có quyền thêm đối tác"})
    </script>
    <%}
      }%>

    <%if (Agency.Id != 0)
      {
          if (!AllowEditAgency)
          {%>
    <script>
        $("#<%= buttonSave.ClientID %>").attr({"disabled":"true","title":"Bạn không có quyền sửa đối tác"})
    </script>
    <%}
      }%>
    <%if (!AllowChangeSalesInCharge)
      {%>
    <script>
        $("#<%= ddlSales.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền đổi sales phụ trách"})
        $("#<%= txtSaleStart.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền đổi sales phụ trách"})
    </script>
    <%}%>
</asp:Content>
