<%@ Page Title="" Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true" CodeBehind="ReportVAT.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ReportVAT" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Báo Cáo Quản Lý VAT</title>
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
                            <div class="col-xs-1">
                                Đối tác
                            </div>
                            <div class="col-xs-2">
                                <asp:TextBox ID="agencySelectornameid" CssClass="form-control" Width="250px" placeholder="Select agency" ReadOnly="True" runat="server"></asp:TextBox>
                                <input id="agencySelector" type="hidden" runat="server" />
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
                                Ngày
                            </div>
                            <div class="col-xs-2">
                                <asp:TextBox ID="txtDate" CssClass="form-control" placeholder="Ngày" data-control="datetimepicker" runat="server" autocomplete="off"></asp:TextBox>
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
                                : Booking đã xuất hóa đơn VAT
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
                    <asp:Button ID="btnSaveStatusExport" runat="server" CssClass="btn btn-primary" Text="Lưu hóa đơn đã xuất VAT" OnClick="btnSaveStatusExport_OnClick" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-hover table-common">
                    <tr class="active">
                        <th>STT
                        </th>
                        <th>Mã đoàn
                        </th>
                        <th>
                            <asp:Literal ID="litOrderDate" runat="server">Ngày</asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="litOrderAgency" runat="server">Đối tác</asp:Literal>

                        </th>
                        <th>Số tiền
                        </th>
                        <th>Đã xuất</th>
                        <th style="width: 6%">Thông tin xuất hóa đơn</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptReport" OnItemDataBound="rptReport_OnItemDataBound">
                        <ItemTemplate>
                            <tr class="<%# ((bool)Eval("IsExportVat")) ? "custom-success":"" %>">
                                <td><%#Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplCode" runat="server">HyperLink</asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litAgency" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litPrice" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsExportVat" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("Id") %>' runat="server" />
                                </td>
                                <td><a href="javascript:void(0)" data-toggle="modal" data-target=".modal-bookingpayment" data-url="AgencyInvoiceInfo.aspx?NodeId=1&SectionId=15&id=<%# Eval("Id")%>">
                                    <i class="fa fa-info-circle fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Thông tin xuất hóa đơn"></i>
                                </a></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Literal ID="litTotalPrice" runat="server"></asp:Literal>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="modal fade modal-bookingpayment" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog" role="document" style="width: 800px; height: 500px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="modal-title">Booking payment</h3>
                    </div>
                    <div class="modal-body">
                        <iframe frameborder="0" width="750" height="450" scrolling="no"></iframe>
                    </div>
                </div>
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
        $('a[data-target = ".modal-bookingpayment"]').click(function () {
            $(".modal iframe").attr('src', $(this).attr('data-url'))
        })
    </script>
    <%if (!AllowSaveVAT)
      { %>
    <script>
        $("#<%= btnSaveStatusExport.ClientID %>").attr({ "disabled": "true", "title": "Bạn không có quyền lưu hóa đơn đã xuất VAT" })
    </script>
    <%}%>
    <%if (!AllowExportVAT)
      { %>
    <script>
        $("#<%= btnExport.ClientID %>").attr({ "disabled": "true", "title": "Bạn không xuất file báo cáo VAT" })
    </script>
    <%}%>
</asp:Content>
