<%@ Page Language="C#" MasterPageFile="MO-NoScriptManager.Master" AutoEventWireup="true" CodeBehind="DocumentView.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DocumentView" Title="Document view" %>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <ul class="nav nav-tabs" ng-controller="tabController">
        <li class="active"><a data-toggle="tab" href="#all">All</a></li>
        <li ng-repeat="item in listDocumentCategory">
            <a data-toggle="tab" href="#{{item.Id}}">{{item.Name}}</a>
        </li>
    </ul>
    <div class="tab-content" ng-controller="tabPaneController">
        <div id="all" class="tab-pane fade in active" ng-controller="allPanelController">
            <table class="document-table table table-bordered table-hover">
                <tbody>
                    <tr class="active">
                        <th>Name</th>
                        <th>Category</th>
                    </tr>
                    <tr ng-repeat="item in listDocument">
                        <td><a href="{{item.Url}}">{{item.Name}}</a></td>
                        <td>{{item.CategoryName}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div ng-repeat="documentCategory in listDocumentCategory" id="{{documentCategory.Id}}" class="tab-pane fade">
            <table class="document-table table table-bordered table-hover">
                <tbody>
                    <tr class="active">
                        <th>Name</th>
                        <th>Category</th>
                    </tr>
                    <tr ng-repeat="document in documentCategory.ListDocumentDTO">
                        <td><a href="{{document.Url}}">{{document.Name}}</a></td>
                        <td>{{document.CategoryName}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="/modules/sails/admin/documentviewcontroller.js"></script>
</asp:Content>
