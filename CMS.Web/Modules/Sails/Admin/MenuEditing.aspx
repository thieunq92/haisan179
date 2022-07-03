<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuEditing.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.MenuEditing"
    MasterPageFile="MO.Master" %>

<%@ MasterType VirtualPath="MO.Master" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Sửa thực đơn</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
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
                        <asp:TextBox runat="server" ID="txtCostOfAdult" class="form-control" placeholder="Người lớn" aria-describedby="basic-addon2" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'placeholder': '0', 'rightAlign':false" />
                        <span class="input-group-addon" id="basic-addon2">₫/Người lớn</span>
                    </div>
                </div>
                <div class="col-xs-2">
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtCostOfChild" class="form-control" placeholder="Trẻ em" aria-describedby="basic-addon2" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'placeholder': '0', 'rightAlign':false" />
                        <span class="input-group-addon" id="basic-addon2">₫/Trẻ em</span>
                    </div>
                </div>
                <div class="col-xs-2">
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtCostOfBaby" class="form-control" placeholder="Sơ sinh" aria-describedby="basic-addon2" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'placeholder': '0', 'rightAlign':false" />
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
                    <asp:Button runat="server" ID="btnEdit" OnClick="btnEdit_Click" CssClass="btn btn-primary" Text="Sửa" />
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
    <%if (!AllowEditMenu)
      {%>
    <script>
        $("#<%=btnEdit.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền sửa thực đơn"})
    </script>
    <%}%>
</asp:Content>
