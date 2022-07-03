<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MO-NoScriptManager.Master"
    CodeBehind="ViewMeetings.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ViewMeetings" Title="View Meetings Page - Oriental Sails Management Office" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.20821, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="search-panel">
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    From   
                </div>
                <div class="col-xs-2">
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="From (dd/mm/yyyy)" data-toggle="tooltip" data-replacement="top" title="Date meeting"> 
                    </asp:TextBox>
                </div>
                <div class="col-xs-1">
                    To
                </div>
                <div class="col-xs-2">
                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="To (dd/mm/yyyy)" data-toggle="tooltip" data-replacement="top" title="Date meeting">
                    </asp:TextBox>
                </div>
                <asp:PlaceHolder runat="server" ID="plhSales">
                    <div class="col-xs-1">
                        Sales
                    </div>
                    <div class="col-xs-3">
                        <asp:DropDownList runat="server" ID="ddlSales" CssClass="form-control" />
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <asp:Button runat="server" ID="btnView" CssClass="btn btn-primary"
                        Text="Search" OnClick="btnView_OnClick"></asp:Button>
                </div>
                <div class="col-xs-2">
                    <asp:TextBox ID="txtPageSize" runat="server" CssClass="form-control" Style="display: inline-block; width: 20%"></asp:TextBox>
                    meetings/page
                </div>
            </div>
        </div>
    </div>
    <div class="meeting-panel">
        <table class="table table-bordered table-hover">
            <tbody>
                <asp:Repeater ID="rptMeetings" runat="server" OnItemDataBound="rptMeetings_OnItemDataBound">
                    <HeaderTemplate>
                        <tr class="active">
                            <th style="width: 7%">
                                <asp:LinkButton runat="server" ID="lbtUpdateTime" OnClick="lbtUpdateTime_OnClick"
                                    ToolTip="Click to sort descending or ascending">Update time</asp:LinkButton>
                                <asp:Image runat="server" ID="imgSortUtStatus" Width="8px" Visible="False" />
                            </th>
                            <th style="width: 7%">
                                <asp:LinkButton runat="server" ID="lbtDateMeeting" OnClick="lbtDateMeeting_OnClick"
                                    ToolTip="Click to sort descending or ascending">Date meeting</asp:LinkButton>
                                <asp:Image runat="server" ID="imgSortDmStatus" Width="8px" Visible="False" />
                            </th>
                            <th style="width: 10%" runat="server" id="thSales">Sales
                            </th>
                            <th>Meeting with
                            </th>
                            <th>Position
                            </th>
                            <th>Belong to agency
                            </th>
                            <th>Note
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="ltrUpdateTime" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrDateMeeting" />
                            </td>
                            <td runat="server" id="tdSales">
                                <asp:Literal runat="server" ID="ltrSale" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrName" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrPosition" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrAgency" />
                            </td>
                            <td>
                                <article>
                                    <p>
                                        <asp:Literal runat="server" ID="ltrNote" />
                                    </p>
                                </article>

                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-2">
                    <asp:Button runat="server"
                        Text="Export meetings"
                        OnClick="btnExportMeetings_OnClick" CssClass="btn btn-primary"></asp:Button>
                </div>
                <div class="col-xs-8">
                    <div class="pager">
                        <svc:Pager ID="pagerMeetings" runat="server" HideWhenOnePage="True" ShowTotalPages="True"
                            ControlToPage="rptMeetings" OnPageChanged="pagerMeetings_OnPageChanged" PageSize="20" />
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="row">
                <div class="col-xs-12">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $("#<%= txtFrom.ClientID %>").datetimepicker({
            timepicker: false,
            format: 'd/m/Y',
            scrollInput: false,
            scrollMonth: false,
        })

        $("#<%= txtTo.ClientID %>").datetimepicker({
            timepicker: false,
            format: 'd/m/Y',
            scrollInput: false,
            scrollMonth: false,
        })
    </script>
    <script>
        $(document).ready(function () {
            $('article').readmore({
                collapsedHeight: 50,
            });
        });
    </script>
</asp:Content>
