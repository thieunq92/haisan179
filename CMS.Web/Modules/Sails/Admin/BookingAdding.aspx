<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingAdding.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingAdding"
    MasterPageFile="MO.Master" %>
<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Thêm đặt chỗ
    </title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder runat="server" ID="plhAdminContent">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    Đối tác
                </div>
                <div class="col-xs-4">
                    <input type="text" name="txtAgency" id="ctl00_ctl00_AdminContentMain_AdminContent_agencySelectornameid" class="form-control"
                        readonly placeholder="Ấn để chọn đối tác" />
                    <input id="agencySelector" type="hidden" runat="server" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    Ngày ăn
                </div>
                <div class="col-xs-4">
                    <asp:TextBox runat="server" ID="txtDate" CssClass="form-control" autocomplete="off" data-control="datetimepicker" placeholder="Date (dd/mm/yyyy)" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button runat="server" ID="btnAdd" CssClass="btn btn-primary" Text="Thêm" OnClick="btnAdd_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $("#ctl00_ctl00_AdminContentMain_AdminContent_agencySelectornameid").click(function () {
            var width = 800;
            var height = 600;
            window.open('/Modules/Sails/Admin/AgencySelectorPage.aspx?NodeId=1&SectionId=15&clientid=<%= agencySelector.ClientID%>', 'Agencyselect', 'width=' + width + ',height=' + height + ',top=' + ((screen.height / 2) - (height / 2)) + ',left=' + ((screen.width / 2) - (width / 2)));
        });
    </script>
    <script>
        $("#aspnetForm").validate({
            rules: {
                <%=txtDate.UniqueID%>: { required:true },
                txtAgency : {required:true},
            },
            messages: {
                <%=txtDate.UniqueID%>: { required: "Yêu cầu chọn ngày ăn" },
                txtAgency : {required:"Yêu cầu chọn đối tác"}
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
    <% if (!AllowAddBooking)
       {
    %>
    <script>
        $("#<%= btnAdd.ClientID %>").attr({"disabled":"true","title":"Bạn không có quyền thêm đặt chỗ"});
    </script>
    <% }%>
</asp:Content>
