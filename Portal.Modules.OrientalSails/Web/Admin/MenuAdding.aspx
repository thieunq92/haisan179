<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuAdding.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.MenuAdding"
    MasterPageFile="MO.Master" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Thêm thực đơn</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder runat="server" ID="plhAdminContent">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    Tên
                </div>
                <div class="col-xs-6">
                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" placeholder="Tên" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    Giá
                </div>
                <div class="col-xs-2">
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtCostOfAdult" class="form-control" placeholder="Adult" aria-describedby="basic-addon2" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" Text="0" />
                        <span class="input-group-addon" id="basic-addon2">₫/Người lớn</span>
                    </div>
                </div>
                <div class="col-xs-2">
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtCostOfChild" class="form-control" placeholder="Child" aria-describedby="basic-addon2" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" Text="0" />
                        <span class="input-group-addon" id="basic-addon2">₫/Trẻ em</span>
                    </div>
                </div>
                <div class="col-xs-2">
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtCostOfBaby" class="form-control" placeholder="Baby" aria-describedby="basic-addon2" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" Text="0" />
                        <span class="input-group-addon" id="basic-addon2">₫/Sơ sinh</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    Chi tiết
                </div>
                <div class="col-xs-6">
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDetails" placeholder="Chi tiết" TextMode="MultiLine" Rows="15" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" CssClass="btn btn-primary" Text="Thêm" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $("#aspnetForm").validate({
            rules: {
                <%=txtName.UniqueID%>: {required:true},
            },
            messages: {
                <%=txtName.UniqueID%>: { required: "Yêu cầu điền tên thực đơn" },
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
    <%if (!AllowAddMenu)
      {%>
    <script>
        $("#<%=btnAdd.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền thêm thực đơn"})
    </script>
    <%}%>
</asp:Content>
