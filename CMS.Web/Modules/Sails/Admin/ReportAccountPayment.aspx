<%@ Page Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true" CodeBehind="ReportAccountPayment.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ReportAccountPayment" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Báo cáo tài khoản thanh toán</title>
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
                                Tài khoản thanh toán
                            </div>
                            <div class="col-xs-3">
                                <asp:DropDownList ID="ddlBankAccount" CssClass="form-control" runat="server"></asp:DropDownList>
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
                                Đối tác
                            </div>
                            <div class="col-xs-2">
                                <asp:TextBox ID="agencySelectornameid" CssClass="form-control" Width="250px" placeholder="Select agency" ReadOnly="True" runat="server"></asp:TextBox>
                                <input id="agencySelector" type="hidden" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-3">
                    <asp:Button ID="btnDisplay" runat="server" CssClass="btn btn-primary" Text="Hiển thị" OnClick="btnDisplay_Click" />
                    <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" Text="Xuất file" OnClick="btnExport_OnClick" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-hover table-common">
                    <tr class="active">
                        <th>STT</th>
                        <th>Thời gian</th>
                        <th>Tên Tài Khoản</th>
                        <th>Số tiền</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptReport">
                        <ItemTemplate>
                            <itemtemplate>
                            <tr>
                                <td><%#Container.ItemIndex + 1%></td>
                                <td><%# Eval("Time","{0:dd/MM/yyyy HH:mm:ss}")%></td>
                                <td><%# Eval("BankAccount.AccName") %></td>
                                <td><%# ((double)Eval("Amount")).ToString("#,##0.##")%></td>
                            </tr>
                        </itemtemplate>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr style="font-weight: bold">
                        <td colspan="3">Tổng</td>
                        <td>
                            <asp:Literal ID="litTotalPrice" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $('#<%=agencySelectornameid.ClientID%>').click(function () {
            var width = 800;
            var height = 600;
            window.open('/Modules/Sails/Admin/AgencySelectorPage.aspx?NodeId=1&SectionId=15&clientid=<%=agencySelector.ClientID%>', 'Agencyselect', 'width=' + width + ',height=' + height + ',top=' + ((screen.height / 2) - (height / 2)) + ',left=' + ((screen.width / 2) - (width / 2)));
        });
    </script>
    <%if (!AllowExportAccountPayment)
      {%>
    <script>
        $("#<%=btnExport.ClientID%>").attr({ "disabled": "true", "title": "Bạn không có quyền xuất file tài khoản thanh toán" });
    </script>
    <%}%>
</asp:Content>
