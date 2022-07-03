<%@ Page Title="" Language="C#" MasterPageFile="MO.Master" AutoEventWireup="true" CodeBehind="BankAccountList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BankAccountList" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Danh Sách Tài Khoản</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-3">
                    <a data-toggle="modal" data-target=".modal-popup-action" data-url="BankAccountEdit.aspx?NodeId=1&SectionId=15" data-id="aAddBankAccount">
                        <i class="fa fa-plus-circle fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Thêm Tài Khoản"></i>Thêm Tài Khoản
                    </a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-hover table-common">
                    <tr class="active">
                        <th>Tên
                        </th>
                        <th>Số Tài Khoản
                        </th>
                        <th>Chi Tiết
                        </th>
                        <th></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptBankAccount">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("AccName") %>
                                </td>
                                <td>
                                    <%# Eval("AccNo") %>
                                </td>
                                <td>
                                    <%# Eval("AccDetail") %>
                                </td>
                                <td><a data-toggle="modal" data-target=".modal-popup-action" data-url="BankAccountEdit.aspx?NodeId=1&SectionId=15&id=<%# Eval("Id")%>" data-id="aEditBankAccount">
                                    <i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Tài Khoản"></i>
                                </a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="modal fade modal-popup-action" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog" role="document" style="width: 820px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="modal-title">Tài khoản</h3>
                    </div>
                    <div class="modal-body">
                        <iframe frameborder="0" width="800" scrolling="no" onload="resizeIframe(this)"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $('a[data-target = ".modal-popup-action"]').click(function () {
            $(".modal iframe").attr('src', $(this).attr('data-url'));
        })
    </script>
    <%if (!AllowAddBankAccount)
      {%>
    <script>
        $("[data-id='aAddBankAccount']").addClass("fa-disabled").attr({ "title": "Bạn không có quyền thêm tài khoản ngân hàng" })
        .find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền thêm tài khoản ngân hàng" });
        $("[data-id='aAddBankAccount']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <%}%>
    <%if (!AllowEditBankAccount)
      {%>
    <script>
        $("[data-id='aEditBankAccount']").find("i").addClass("fa-disabled").attr({ "title": "Bạn không có quyền sửa tài khoản ngân hàng" })
        $("[data-id='aEditBankAccount']").click(function (e) {
            e.stopPropagation();
        })
    </script>
    <%}%>
</asp:Content>
