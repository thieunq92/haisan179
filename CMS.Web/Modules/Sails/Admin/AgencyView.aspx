<%@ Page Language="C#" MasterPageFile="MO-NoScriptManager.Master" AutoEventWireup="true"
    CodeBehind="AgencyView.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyView"
    Title="Agency View" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" type="text/css" href="/css/jqueryfileupload/v9.21.0/jquery.fileupload.css" />
    <link rel="stylesheet" type="text/css" href="/css/jqueryfileupload/v9.21.0/jquery.fileupload-ui.css" />
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <svc:Popup ID="popupManager" runat="server">
    </svc:Popup>
    <h2 class="page-header">
        <asp:Literal runat="server" ID="litName1"></asp:Literal></h2>
    <div class="panel-agency">
        <div class="row">
            <div class="col-xs-2">
                <label>Tên</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litName"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2">
                <label>Tên giao dịch</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litTradingName"></asp:Literal>
            </div>
        </div>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Representative</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litRepresentative"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-1">
                <label>Representative Position</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litRepresentativePosition"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-1">
                <label>Contact</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litContact"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-1">
                <label>Contact Address</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litContactAddress"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-1">
                <label>Contact Position</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litContactPosition"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-1">
                <label>Contact Email</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litContactEmail"></asp:Literal>
            </div>
        </div>--%>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Role</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litRole"></asp:Literal>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-xs-2">
                <label>Địa chỉ</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litAddress"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2">
                <label>Phone</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litPhone"></asp:Literal>
            </div>
        </div>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Fax</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litFax"></asp:Literal>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-xs-2">
                <label>Email</label>
            </div>
            <div class="col-xs-10">
                <asp:HyperLink runat="server" ID="hplEmail"></asp:HyperLink>
            </div>
        </div>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Website</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litWebsite"></asp:Literal>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-xs-5">
                <label>Sale in charge</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litSale"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2">
                <label>Sale phone</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litSalePhone"></asp:Literal>
            </div>
        </div>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Tax code</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litTax"></asp:Literal>
            </div>
        </div>--%>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Location</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litLocation"></asp:Literal>
            </div>
        </div>--%>
        <%--<div class="row">
            <div class="col-xs-1">
                <label>Accountant</label>
            </div>
            <div class="col-xs-11">
                <asp:Literal runat="server" ID="litAccountant"></asp:Literal>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-xs-2">
                <label>Thanh toán</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litPayment"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2">
                <label>Thông tin hóa đơn</label>
            </div>
            <div class="col-xs-9">
                <asp:Literal runat="server" ID="litIvoice"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2">
                <label>VAT</label>
            </div>
            <div class="col-xs-10">
                <asp:CheckBox runat="server" ID="chkVat" Enabled="False"></asp:CheckBox>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-2">
                <label>Thông tin khác</label>
            </div>
            <div class="col-xs-10">
                <asp:Literal runat="server" ID="litNote"></asp:Literal>
            </div>
        </div>
        <div class="btn-toolbar">
            <asp:HyperLink runat="server" ID="hplEditAgency" CssClass="btn btn-primary">Edit agency</asp:HyperLink>
            <asp:HyperLink runat="server" ID="hplBookingList" CssClass="btn btn-primary">Booking by agency</asp:HyperLink>
            <div id="disableInform" style="display: none">
                You don't have permission to use this function. If you want to use this function please contact administrator
            </div>
            <asp:HyperLink runat="server" ID="hplReceivable"
                CssClass="btn btn-primary">Receivables (last 3 months)</asp:HyperLink>
            <a href="SeriesManager.aspx?NodeId=1&SectionId=15&ai=<%= Request.QueryString["AgencyId"] %>" class="btn btn-primary">Series Booking</a>
        </div>
    </div>
    <div class="panel-contacts">
        <h4>CONTACTS</h4>
        <asp:PlaceHolder runat="server" ID="plhContacts">
            <table class="table table-bordered table-hover table-common">
                <tr class="active">
                    <th>Name
                    </th>
                    <th>Position
                    </th>
                    <th>Booker
                    </th>
                    <th>Phone
                    </th>
                    <th>Email
                    </th>
                    <th>Birthday
                    </th>
                    <th>Note
                    </th>
                    <th></th>
                    <th></th>
                    <th></th>
                </tr>
                <asp:Repeater runat="server" ID="rptContacts" OnItemDataBound="rptContacts_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="ltrName"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litPosition"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litBooker"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litPhone"></asp:Literal>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplEmail"></asp:HyperLink>
                            </td>
                            <td>
                                <%# ((DateTime?)Eval("Birthday"))==null? "" : ((DateTime?)Eval("Birthday")).Value.ToString("dd/MM/yyyy")%>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Note") %>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplCreateMeeting"><i class="fa fa-users fa-lg text-success" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Add meeting"></i></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplName"><i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i></asp:HyperLink>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbtDelete" OnClick="lbtDelete_Click" CommandArgument='<%#Eval("Id")%>'
                                    OnClientClick="return confirm('Are you sure?')"><i class="fa fa-trash-alt fa-lg text-danger" aria-hidden="true" title="" data-toggle="tooltip" data-placement="top" data-original-title="Delete"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="btn-toolbar">
                <asp:HyperLink runat="server" ID="hplAddContact" CssClass="btn btn-primary">Add contact</asp:HyperLink>
            </div>
        </asp:PlaceHolder>
        <asp:Label runat="server" ID="lblContacts" Text="You don't have permission to use this function. If you want to use this function please contact administrator"
            Visible="False" />
    </div>
    <div class="panel-recentactivities">
        <h4>RECENT ACTIVITIES
        </h4>
        <asp:PlaceHolder runat="server" ID="plhActivities">
            <table class="table table-bordered table-hover table-common">
                <tr class="active">
                    <th>Date meeting
                    </th>
                    <th>Sale
                    </th>
                    <th>Meeting with
                    </th>
                    <th>Position
                    </th>
                    <th>Note
                    </th>
                    <th></th>
                    <th></th>
                </tr>
                <asp:Repeater runat="server" ID="rptActivities" OnItemDataBound="rptActivities_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="ltrDateMeeting" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrSale" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrName" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrPosition" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrNote" />
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbtEditActivity"><i class="fa fa-pencil-square-o fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbtDeleteActivity" OnClick="lbtDeleteActivity_Click"
                                    CommandArgument='<%#Eval("Id")%>' OnClientClick="return confirm('Are you sure?')"><i class="fa fa-close fa-lg text-danger" aria-hidden="true" title="" data-toggle="tooltip" data-placement="top" data-original-title="Delete"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:PlaceHolder>
        <asp:Label runat="server" ID="lblActivities" Text="You don't have permission to use this function. If you want to use this function please contact administrator"
            Visible="False" />
    </div>
    <div class="panel-contracts">
        <h4>CONTRACTS</h4>
        <asp:PlaceHolder runat="server" ID="plhContracts">
            <table class="table table-bordered table-hover table-common">
                <tr class="active">
                    <th>Valid From</th>
                    <th>Valid To</th>
                    <th>Contract \ Quotation</th>
                    <th>Status</th>
                    <th></th>
                </tr>
                <asp:Repeater runat="server" ID="rptContracts" OnItemDataBound="rptContracts_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="hide">
                                <asp:Literal runat="server" ID="litCreatedDate" />
                            </td>
                            <td class="hide">
                                <asp:Literal runat="server" ID="litName"></asp:Literal>
                            </td>
                            <td class="hide">
                                <asp:Literal runat="server" ID="litExpired"></asp:Literal>
                            </td>
                            <td class="hide">
                                <asp:Literal runat="server" ID="litReceived"></asp:Literal>
                            </td>
                            <td>
                                <%# ((DateTime?)Eval("ContractValidFromDate")) == null ? "" : ((DateTime?)Eval("ContractValidFromDate")).Value.ToString("dd/MM/yyyy") %>
                            </td>
                            <td>
                                <%# ((DateTime?)Eval("ContractValidToDate")) == null ? "" : ((DateTime?)Eval("ContractValidToDate")).Value.ToString("dd/MM/yyyy") %>
                            </td>
                            <td>
                                <asp:PlaceHolder runat="server" ID="plhContractQuotation"></asp:PlaceHolder>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litStatus"></asp:Literal>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplDownload" ToolTip="Download"></asp:HyperLink>
                            </td>
                            <td class="hide">
                                <asp:HyperLink runat="server" ID="hplEdit"><i class="fa fa-pencil-square-o fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i></asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="btn-toolbar">
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target=".modal-issuecontract">Issue contract</button>
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target=".modal-issuequotation">Issue quotation</button>
            </div>
        </asp:PlaceHolder>
        <asp:Label runat="server" ID="lblContracts" Text="You don't have permission to use this function. If you want to use this function please contact administrator"
            Visible="False" />
    </div>
    <div class="modal fade modal-issuecontract" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title">Issue Contract</h3>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Status</label>
                            </div>
                            <div class="col-xs-10">
                                <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Status --" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Contract sent" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Contract in valid" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Valid from</label>
                            </div>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" ID="txtContractValidFromDate" CssClass="form-control" placeholder="Valid from (dd/mm/yyyy)" data-type="datetimepicker"></asp:TextBox>
                            </div>
                            <div class="col-xs-2">
                                <label>Valid to</label>
                            </div>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" ID="txtContractValidToDate" CssClass="form-control" placeholder="Valid to (dd/mm/yyyy)" data-type="datetimepicker"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Select contract</label>
                            </div>
                            <div class="col-xs-10">
                                <asp:DropDownList runat="server" ID="ddlContractTemplate" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-- Select contract --" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Upload</label>
                            </div>
                            <div class="col-xs-10">
                                <div class="row">
                                    <div class="col-xs-12" style="margin-bottom: 10px">
                                        <span class="btn btn-success fileinput-button">
                                            <i class="glyphicon glyphicon-plus"></i>
                                            <span>Add file</span>
                                            <input id="btnFileUploadContract" name="file" multiple="" type="file">
                                            <asp:HiddenField runat="server" ID="hifContractTemplatePath" />
                                            <asp:HiddenField runat="server" ID="hifContractTemplateName" />
                                        </span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="progress">
                                            <div class="progress-bar progress-bar-success progress-bar-striped" role="progressbar" aria-valuenow="" aria-valuemin="0" aria-valuemax="100">
                                                <span class="sr-only"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Preview</label>
                            </div>
                            <div class="col-xs-10">
                                <div class="btn-toolbar">
                                    <button type="button" class="btn btn-primary">Send email</button>
                                    <asp:Button runat="server" ID="btnExportContractPreviewWord" class="btn btn-primary" Text="Export to Word" OnClick="btnExportContractPreviewWord_Click"></asp:Button>
                                    <asp:Button runat="server" ID="btnExportContractPreviewPdf" class="btn btn-primary" Text="Export to Pdf" OnClick="btnExportContractPreviewPdf_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="btnIssueContract" CssClass="btn btn-primary" Text="Issue" OnClick="btnIssueContract_Click"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade modal-issuequotation" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title">Issue Quotation</h3>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Valid from</label>
                            </div>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" ID="txtQuotationValidFromDate" CssClass="form-control" placeholder="Valid from (dd/mm/yyyy)" data-type="datetimepicker"></asp:TextBox>
                            </div>
                            <div class="col-xs-2">
                                <label>Valid to</label>
                            </div>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" ID="txtQuotationValidToDate" CssClass="form-control" placeholder="Valid to (dd/mm/yyyy)" data-type="datetimepicker"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Select quotation</label>
                            </div>
                            <div class="col-xs-10">
                                <asp:DropDownList ID="ddlQuotationTemplate" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Text="-- Select quotation --" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Upload</label>
                            </div>
                            <div class="col-xs-10">
                                <div class="row">
                                    <div class="col-xs-12" style="margin-bottom: 10px">
                                        <span class="btn btn-success fileinput-button">
                                            <i class="glyphicon glyphicon-plus"></i>
                                            <span>Add file</span>
                                            <input id="btnFileUploadQuotation" name="file" multiple="" type="file">
                                            <asp:HiddenField runat="server" ID="hifQuotationTemplatePath" />
                                            <asp:HiddenField runat="server" ID="hifQuotationTemplateName" />
                                        </span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="progress">
                                            <div class="progress-bar progress-bar-success progress-bar-striped" role="progressbar" aria-valuenow="" aria-valuemin="0" aria-valuemax="100">
                                                <span class="sr-only"></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Preview</label>
                            </div>
                            <div class="col-xs-10">
                                <div class="btn-toolbar">
                                    <button type="button" class="btn btn-primary">Send email</button>
                                    <asp:Button runat="server" ID="btnExportQuotationPreviewWord" class="btn btn-primary" Text="Export to Word" OnClick="btnExportQuotationPreviewWord_Click"></asp:Button>
                                    <asp:Button runat="server" ID="btnExportQuotationPreviewPdf" class="btn btn-primary" Text="Export to Pdf" OnClick="btnExportQuotationPreviewPdf_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="btnIssueQuotation" CssClass="btn btn-primary" Text="Issue" OnClick="btnIssueQuotation_Click"></asp:Button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="/scripts/jqueryfileupload/v9.21.0/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/scripts/jqueryfileupload/v9.21.0/jquery.fileupload-ui.js"></script>
    <script type="text/javascript" src="/scripts/jqueryfileupload/v9.21.0/jquery.iframe-transport.js"></script>
    <script type="text/javascript">
        $("[data-type='datetimepicker']").datetimepicker({
            timepicker: false,
            format: 'd/m/Y',
            scrollInput: false,
            scrollMonth: false
        });
    </script>
    <script type="text/javascript">
        $('#btnFileUploadContract').fileupload({
            url: 'Handler/FileUploadHandler.ashx?upload=start',
            add: function (e, data) {
                $('.progress-bar').css('width', 0 + '%');
                data.submit();
            },
            progress: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('.progress-bar').css('width', progress + '%');
            },
            error: function (error) {
            }
        }).bind('fileuploaddone', function (e, data) {
            $("#<%= hifContractTemplatePath.ClientID %>").val(data.result.path)
            $("#<%= hifContractTemplateName.ClientID %>").val(data.result.name)
        })

        $('#btnFileUploadQuotation').fileupload({
            url: 'Handler/FileUploadHandler.ashx?upload=start',
            add: function (e, data) {
                $('.progress-bar').css('width', 0 + '%');
                data.submit();
            },
            progress: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('.progress-bar').css('width', progress + '%');
            },
            error: function (error) {
            }
        }).bind('fileuploaddone', function (e, data) {
            $("#<%= hifQuotationTemplatePath.ClientID %>").val(data.result.path)
            $("#<%= hifQuotationTemplateName.ClientID %>").val(data.result.name)
        })
    </script>
</asp:Content>
