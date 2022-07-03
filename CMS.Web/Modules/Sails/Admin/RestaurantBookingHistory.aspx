<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantBookingHistory.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.RestaurantBookingHistory"
    MasterPageFile="NewPopup.Master" %>

<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <h4 class="page-header">Trạng thái</h4>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    <strong>Sửa bởi</strong>
                </div>
                <div class="col-xs-2">
                    <strong>Thời gian</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Từ</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Chuyển sang</strong>
                </div>
            </div>
        </div>
        <asp:Repeater runat="server" ID="rptHistoryStatus">
            <ItemTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            <%# Eval("CreatedBy.FullName") %>
                        </div>
                        <div class="col-xs-2">
                            <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                        </div>
                        <div class="col-xs-4">
                            <%# GetStatus(Eval("OriginValue").ToString())%>
                        </div>
                        <div class="col-xs-4">
                            <%# GetStatus(Eval("NewValue").ToString())%>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <h4 class="page-header">Ngày</h4>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    <strong>Sửa bởi</strong>
                </div>
                <div class="col-xs-2">
                    <strong>Thời gian</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Từ</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Chuyển sang</strong>
                </div>
            </div>
        </div>
        <asp:Repeater runat="server" ID="rptHistoryDate">
            <ItemTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            <%# Eval("CreatedBy.FullName") %>
                        </div>
                        <div class="col-xs-2">
                            <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                        </div>
                        <div class="col-xs-4">
                            <%# GetDate(Eval("OriginValue").ToString())%>
                        </div>
                        <div class="col-xs-4">
                            <%# GetDate(Eval("NewValue").ToString())%>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <h4 class="page-header">Số suất ăn</h4>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    <strong>Sửa bởi</strong>
                </div>
                <div class="col-xs-2">
                    <strong>Thời gian</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Từ</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Chuyển sang</strong>
                </div>
            </div>
        </div>
        <asp:Repeater runat="server" ID="rptHistoryNumberOfSet">
            <ItemTemplate>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            <%# Eval("CreatedBy.FullName") %>
                        </div>
                        <div class="col-xs-2">
                            <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                        </div>
                        <div class="col-xs-4">
                            <%# Eval("OriginValue").ToString()%>
                        </div>
                        <div class="col-xs-4">
                            <%# Eval("NewValue").ToString()%>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <h4 class="page-header">Đơn giá</h4>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    <strong>Sửa bởi</strong>
                </div>
                <div class="col-xs-2">
                    <strong>Thời gian</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Từ</strong>
                </div>
                <div class="col-xs-4">
                    <strong>Chuyển sang</strong>
                </div>
            </div>
            <asp:Repeater runat="server" ID="rptHistoryPricePerPerson">
                <ItemTemplate>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <%# Eval("CreatedBy.FullName") %>
                            </div>
                            <div class="col-xs-2">
                                <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                            </div>
                            <div class="col-xs-4">
                                <%# Eval("OriginValue").ToString()%>
                            </div>
                            <div class="col-xs-4">
                                <%# Eval("NewValue").ToString()%>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
    </asp:PlaceHolder>
</asp:Content>
