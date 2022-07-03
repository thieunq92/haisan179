<%@ Page Title="" Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true" CodeBehind="ReportDebtReceivables.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ReportDebtReceivables" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Báo Cáo Nợ Phải Thu</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-9">
                    <div class="form-group">
                        <div class="row">
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
                        <th>STT
                        </th>
                        <th>Tên đối tác
                        </th>
                        <th>Tổng nợ
                        </th>
                        <th>Pay all
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptReport" OnItemDataBound="rptReport_OnItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td><%#Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplAgency" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litTotalReceivable" runat="server"></asp:Literal>
                                </td>
                                <th></th>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                </td>
                                <th></th>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
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
        function GotoReceivable(nodeId, sectionId, agencyId) {
            var to = $("#<%=txtTo.ClientID%>").val();
            window.open('Receivables.aspx?NodeId=' + nodeId + "&SectionId=" + sectionId + "&from=none&ai=" + agencyId + "&spay=1" + "&t=" + to, '_blank');
        }
    </script>
    <% if (!AllowExportDebtReceivables)
       { %>
    <script>
        $("#<%=btnExport.ClientID%>").attr({ "disabled": "true", "title": "Bạn không có quyền xuất file báo cáo nợ phải thu" });
    </script>
    <% } %>
</asp:Content>
