<%@ Page Title="" Language="C#" MasterPageFile="NewPopup.Master" AutoEventWireup="true" CodeBehind="BankAccountEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BankAccountEdit" %>

<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-3">
                Tên Tài Khoản
            </div>
            <div class="col-xs-6">
                <asp:TextBox runat="server" ID="txtAccName" CssClass="form-control" placeholder="Tên Tài Khoản" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-3">
                Số Tài Khoản
            </div>
            <div class="col-xs-6">
                <asp:TextBox runat="server" ID="txtAccNo" CssClass="form-control" placeholder="Số Tài Khoản" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-3">
                Chi Tiết Tài Khoản
            </div>
            <div class="col-xs-6">
                <asp:TextBox runat="server" CssClass="form-control" ID="txtAccDetail" placeholder="Chi Tiết Tài Khoản" TextMode="MultiLine" Rows="15" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button runat="server" ID="btnSaveInfo" OnClick="btnSave_OnClick" CssClass="btn btn-primary" Text="Lưu" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $("#aspnetForm").validate({
            rules: {
                <%=txtAccNo.UniqueID%>: {required: true},
                <%=txtAccName.UniqueID%>: {required: true}
            },
            messages: {
                 <%=txtAccNo.UniqueID%>: { required: "Yêu cầu điền số tài khoản" },
                 <%=txtAccName.UniqueID%>: {required: "Yêu cầu điền tên"}
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
</asp:Content>
