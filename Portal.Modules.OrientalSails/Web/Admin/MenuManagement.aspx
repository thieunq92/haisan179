<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuManagement.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.MenuManagement"
    MasterPageFile="MO.Master" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Quản lý thực đơn</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-hover table-common">
                    <tr class="active">
                        <th rowspan="2">Name
                        </th>
                        <th colspan="3">Cost
                        </th>
                        <th rowspan="2">Details
                        </th>
                        <th rowspan="2"></th>
                    </tr>
                    <tr class="active">
                        <td>Adult
                        </td>
                        <td>Child</td>
                        <td>Baby</td>
                    </tr>
                    <asp:Repeater runat="server" ID="rptMenuTable">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Name") %>
                                </td>
                                <td>
                                    <%# ((double)Eval("CostOfAdult")).ToString("#,##0.##")  + "₫" %>
                                </td>
                                <td>
                                    <%# ((double)Eval("CostOfChild")).ToString("#,##0.##")  + "₫" %>
                                </td>
                                <td>
                                    <%# ((double)Eval("CostOfBaby")).ToString("#,##0.##")  + "₫" %>
                                </td>
                                <td style="text-align: left!important">
                                    <%# ((String)Eval("Details")).Replace("\r\n","<br/>")%>
                                </td>
                                <td><a href="MenuEditing.aspx?NodeId=1&SectionId=15&mi=<%# Eval("Id") %>" data-id="aEditMenu">
                                    <i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i>
                                </a></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr style="display: <%= rptMenuTable.Items.Count == 0 ? "" : "none"%>">
                                <td colspan="100%">No records found
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <%if (!AllowEditMenu)
      {%>
    <script>
        $("[data-id='aEditMenu']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền sửa thực đơn" });
        $("[data-id='aEditMenu']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <%}%>
</asp:Content>
