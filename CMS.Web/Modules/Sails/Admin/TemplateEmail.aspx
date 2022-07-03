<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Popup.Master" CodeBehind="TemplateEmail.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.TemplateEmail" %>

<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2, Version=2.5.2912.21007, Culture=neutral, PublicKeyToken=4f86767c9b519a06" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .btn {
            display: inline-block;
            margin-bottom: 0;
            font-weight: normal;
            text-align: center;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            background-image: none;
            border: 1px solid transparent;
            white-space: nowrap;
            padding: 6px 12px;
            font-size: 12px;
            line-height: 1.42857143;
            border-radius: 4px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

        .btn-primary {
            color: #fff;
            background-color: #337ab7;
            border-color: #2e6da4;
        }

        .btn {
            padding: 3px 12px;
            margin-right: 3px;
        }
    </style>
    <div class="basicinfo">
        <table>
            <%--<tr>
                <td>
                    <button type="button" id="btnCopy" onclick="fnCopy();" class="btn btn-primary undisabled" data-toggle="modal" data-target=".modal-history" style="margin-right: 0px">Copy</button></td>
            </tr>--%>
            <tr>
                <td>
                    <FCKeditorV2:FCKeditor ID="fckContent" runat="server" Width="100%" Height="600"
                        BasePath="~/support/fckeditor/" ToolbarSet="EmailTemplate">
                    </FCKeditorV2:FCKeditor>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: none">
                        <textarea id="inputCopy"></textarea>
                    </div>
                </td>
            </tr>

        </table>
    </div>
    <script>
        function fnCopy() {
            debugger;
            document.getElementById("inputCopy").value = document.getElementById("ctl00_AdminContent_fckContent").value;
            var copyText = document.getElementById("inputCopy");
            copyText.select();
            document.execCommand("copy");
            console.log(copyText.value);
            alert("Đã copy nội dung");
        }
    </script>
</asp:Content>
