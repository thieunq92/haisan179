<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingViewing.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.BookingViewing"
    MasterPageFile="MO.Master" %>

<%@ MasterType VirtualPath="MO.Master" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Enums.RestaurantBooking" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.BusinessLogic.Share" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Enums" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Chi tiết đặt chỗ</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:PlaceHolder ID="plhAdminContent" runat="server">
        <div ng-controller="globalVariableDeclareController"></div>
        <div class="page-header">
            <h3>Mã đoàn : <%= RestaurantBooking.Code %><img src="/images/new_blink.gif" width="40" <%= RestaurantBooking.HaveEmergencyUpdate ? "" : "style='display:none'"%>></h3>
        </div>
        <div class="row" id="saveController" ng-controller="saveController" ng-init="$root.restaurantBookingId = <%= RestaurantBooking.Id %>">
            <div class="col-xs-12">
                <asp:Button CssClass="btn btn-primary" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Style="display: none; margin-right: 0px" />
                <button id="btnSave" type="button" class="btn btn-primary" data-uniqueid="<%= btnSave.UniqueID %>" style="margin-right: 0px">Lưu</button>
                <asp:Button runat="server" ID="btnLock" OnClick="btnLock_Click" Text="Khóa" CssClass="btn btn-primary undisabled" Visible="false" Style="margin-right: 0px" />
                <asp:Button runat="server" ID="btnUnlock" OnClick="btnUnlock_Click" Text="Mở khóa" CssClass="btn btn-primary undisabled" Visible="false" Style="margin-right: 0px" />
                <button type="button" id="btnHistory" class="btn btn-primary undisabled" data-toggle="modal" data-target=".modal-history" style="margin-right: 0px">Xem lịch sử</button>
                <a href="TemplateEmail.aspx?NodeId=1&SectionId=15&BookingId=<%= RestaurantBooking.Id %>" class="btn btn-primary" id="sendemail">Mẫu email</a>

            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-8">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">Thực đơn</div>
                        <div class="col-xs-4" ng-controller="menuController" ng-init="menuId = '<%= RestaurantBooking.Menu != null? RestaurantBooking.Menu.Id : -1%>'">
                            <asp:DropDownList ID="ddlMenu" runat="server" CssClass="form-control" AppendDataBoundItems="true" ng-model="menuId" ng-change="menuGetById()">
                                <asp:ListItem Value="-1">-- Thực đơn --</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-1 nopadding-right">
                            Trạng thái
                        </div>
                        <div class="col-xs-3 nopadding-right">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="1">Xác nhận</asp:ListItem>
                                <asp:ListItem Value="2">Hủy</asp:ListItem>
                                <asp:ListItem Value="3">Chờ xác nhận</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-2 nopadding-right">
                            <div class="row">
                                <div class="col-xs-6 text-left nopadding-left">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <button id="btnReason" type="button" class="btn btn-primary hidden undisabled" style="height: 24px;" data-toggle="modal" data-target=".modal-reason">Lý do hủy</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-6 text-right">
                                    Yêu cầu  
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 nopadding-left">
                                    <input type="text" name="reason" style="visibility: hidden; height: 0; border: none; padding: 0;" class="form-control" id="hidReason" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            Ngày
                        </div>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtDate" CssClass="form-control" placeholder="Ngày (dd/mm/yyyy)" data-control="datetimepicker" />
                        </div>
                        <div class="col-xs-1 nopadding-right">
                            Thời gian
                        </div>
                        <div class="col-xs-3 nopadding-right">
                            <asp:DropDownList ID="ddlPartOfDay" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="1">Sáng</asp:ListItem>
                                <asp:ListItem Value="2">Trưa</asp:ListItem>
                                <asp:ListItem Value="3">Tối</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-2 nopadding-left nopadding-right">
                            <asp:TextBox runat="server" ID="txtTime" CssClass="form-control" placeholder="Giờ (hh:mm)" data-control="timepicker" />
                        </div>
                    </div>
                </div>
                <div class="form-group" ng-controller="bookerController" ng-init="agencyId=<%= RestaurantBooking.Agency != null ? RestaurantBooking.Agency.Id:-1 %>;bookerGetAllByAgencyId()">
                    <div class="row">
                        <div class="col-xs-2">
                            Đơn vị
                        </div>
                        <div class="col-xs-4">
                            <input type="text" name="txtAgency" id="ctl00_ctl00_AdminContentMain_AdminContent_agencySelectornameid" class="form-control"
                                readonly placeholder="Click to select agency" value="<%= RestaurantBooking.Agency != null ? RestaurantBooking.Agency.Name : "" %>" />
                            <input id="agencySelector" type="text" runat="server" ng-model="agencyId" ng-change="bookerGetAllByAgencyId()" data-id="hidAgencyId" style="display: none" />
                        </div>
                        <div class="col-xs-1 nopadding-right">
                            Người đặt
                        </div>
                        <div class="col-xs-3 nopadding-right">
                            <select ng-model="$root.bookerId" class="form-control" convert-to-number ng-init="$root.bookerId = <%= RestaurantBooking.Booker != null ? RestaurantBooking.Booker.Id : -1 %>" ng-change="fillBookerPhoneNumber()" id="ddlBooker">
                                <option value="-1">-- Người đặt --</option>
                                <option ng-repeat="item in listBooker" value="{{item.id}}" data-phonenumber="{{item.phoneNumber}}">{{item.name}}</option>
                            </select>
                        </div>
                        <div class="col-xs-2 nopadding-left nopadding-right">
                            <input type="text" class="form-control" placeholder="Số điện thoại" ng-model="bookerPhoneNumber">
                        </div>
                    </div>
                </div>
                <div class="form-group" ng-controller="numberOfPaxController" ng-init="$root.numberOfPax.Adult = <%= RestaurantBooking.NumberOfPaxAdult %>;$root.numberOfPax.Child = <%=RestaurantBooking.NumberOfPaxChild %>;$root.numberOfPax.Baby = <%=RestaurantBooking.NumberOfPaxBaby %>">
                    <div class="row">
                        <div class="col-xs-2">
                            Số suất ăn
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 28%">
                            <div class="input-group">
                                <asp:TextBox ID="txtNumberOfPaxAdult" runat="server" CssClass="form-control" placeholder="Suất ăn người lớn " Text="0" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="$root.numberOfPax.Adult" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon">Người lớn</span>
                            </div>
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 28%">
                            <div class="input-group">
                                <asp:TextBox ID="txtNumberOfPaxChild" runat="server" CssClass="form-control" placeholder="Suất ăn trẻ em " Text="0" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="$root.numberOfPax.Child" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon">Trẻ em</span>
                            </div>
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 27%">
                            <div class="input-group">
                                <asp:TextBox ID="txtNumberOfPaxBaby" runat="server" CssClass="form-control" placeholder="Suất ăn sơ sinh " Text="0" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="$root.numberOfPax.Baby" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon">Sơ sinh</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" ng-controller="priceController" ng-init="$root.costPerPerson.Adult='<%= RestaurantBooking.CostPerPersonAdult %>';$root.costPerPerson.Child='<%= RestaurantBooking.CostPerPersonChild %>';$root.costPerPerson.Baby='<%= RestaurantBooking.CostPerPersonBaby %>'">
                    <div class="row">
                        <div class="col-xs-2">
                            Đơn giá
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 28%">
                            <div class="input-group">
                                <asp:TextBox ID="txtCostPerPersonAdult" runat="server" CssClass="form-control" placeholder="Đơn giá người lớn " Text="0" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" ng-model="$root.costPerPerson.Adult" data-id="txtCostPerPersonAdult" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon" style="padding-left: 3px">₫/Người lớn</span>
                            </div>
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 28%">
                            <div class="input-group">
                                <asp:TextBox ID="txtCostPerPersonChild" runat="server" CssClass="form-control" placeholder="Đơn giá trẻ em " Text="0" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" ng-model="$root.costPerPerson.Child" data-id="txtCostPerPersonChild" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon" style="padding-left: 3px">₫/Trẻ em</span>
                            </div>
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 27%; display: none">
                            <div class="input-group">
                                <asp:TextBox ID="txtCostPerPersonBaby" runat="server" CssClass="form-control" placeholder="Đơn giá sơ sinh " Text="0" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" ng-model="$root.costPerPerson.Baby" data-id="txtCostPerPersonBaby" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon" style="padding-left: 3px">₫/Sơ sinh</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" ng-controller="numberOfDiscountedPaxController" ng-init="$root.numberOfDiscountedPax.Adult=<%=RestaurantBooking.NumberOfDiscountedPaxAdult %>;$root.numberOfDiscountedPax.Child=<%=RestaurantBooking.NumberOfDiscountedPaxChild %>;$root.numberOfDiscountedPax.Baby=<%=RestaurantBooking.NumberOfDiscountedPaxBaby %>">
                    <div class="row">
                        <div class="col-xs-2">
                            FOC
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 28%">
                            <div class="input-group">
                                <asp:TextBox ID="txtNumberOfDiscountedPaxAdult" runat="server" CssClass="form-control" placeholder="FOC người lơn " Text="0" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="$root.numberOfDiscountedPax.Adult" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon">Người lớn</span>
                            </div>
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 28%">
                            <div class="input-group">
                                <asp:TextBox ID="txtNumberOfDiscountedPaxChild" runat="server" CssClass="form-control" placeholder="FOC trẻ em " Text="0" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="$root.numberOfDiscountedPax.Child" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon">Trẻ em</span>
                            </div>
                        </div>
                        <div class="col-xs-3 nopadding-right" style="width: 27%; display: none">
                            <div class="input-group">
                                <asp:TextBox ID="txtNumberOfDiscountedPaxBaby" runat="server" CssClass="form-control" placeholder="FOC sơ sinh " Text="0" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="$root.numberOfDiscountedPax.Baby" ng-change="$root.calculatePriceOfSet()" />
                                <span class="input-group-addon">Sơ sinh</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-2">
                            Tiền ăn
                        </div>
                        <div class="col-xs-3" style="width: 29.6%" ng-controller="totalPriceOfSetController" ng-init="$root.totalPriceOfSet = <%= RestaurantBooking.TotalPriceOfSet %>">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtTotalPriceOfSet" CssClass="form-control" placeholder="Tiền ăn" Text="0" ng-model="$root.totalPriceOfSet" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" />
                                <span class="input-group-addon" style="padding: 5px 36.5px">₫</span>
                            </div>
                        </div>
                        <div class="col-xs-1 nopadding-left nopadding-right" ng-controller="vatController" ng-init="$root.bookingVAT = <%= RestaurantBooking.VAT.ToString().ToLower() %>">
                            <div class="checkbox" style="margin-top: 0px; margin-bottom: 0px">
                                <label>
                                    <asp:CheckBox ID="chkVAT" runat="server" Text="VAT" />
                                </label>
                            </div>
                        </div>
                        <div class="col-xs-1 nopadding-left nopadding-right">
                            Vị trí bàn ăn
                        </div>
                        <div class="col-xs-2 nopadding-left nopadding-right">
                            <asp:DropDownList ID="ddlTablePosition" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="4">Phòng 201</asp:ListItem>
                                <asp:ListItem Value="5">Phòng 202</asp:ListItem>
                                <asp:ListItem Value="2">Hội trường</asp:ListItem>
                                <asp:ListItem Value="3">Sảnh T1</asp:ListItem>   
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-1 ">
                            <div id="wrap-chkgala" class="checkbox" style="margin-top: 0px; margin-bottom: 0px">
                                <label>
                                    <asp:CheckBox ID="chkGala" runat="server" Text="Gala" />
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" ng-controller="totalPriceController" ng-init="$root.calculateTotalPrice()">
                    <div class="row">
                        <div class="col-xs-2">
                            Tổng giá
                        </div>
                        <div class="col-xs-3" style="width: 29.6%">
                            {{totalPrice +"₫"}}
                        </div>
                        <div class="col-xs-3 nopadding-left" style="width: 28.1%">
                            <div class="radio-inline">
                                <asp:RadioButton runat="server" ID="rbPayNow" GroupName="payment" Text="Thanh toán ngay" Checked="true"></asp:RadioButton>
                            </div>
                            <div class="radio-inline">
                                <asp:RadioButton runat="server" ID="rbDebt" GroupName="payment" Text="Công nợ"></asp:RadioButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" ng-controller="actuallyCollectedController" ng-init="$root.calculateActuallyCollected()">
                    <div class="row">
                        <div class="col-xs-2">
                            Thực thu
                        </div>
                        <div class="col-xs-2">
                            {{ $root.actuallyCollected + "₫"}}
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            Khởi tạo bởi <%= RestaurantBooking.CreatedBy!= null ? RestaurantBooking.CreatedBy.FullName : "" %> vào lúc <%= RestaurantBooking.CreatedDate.HasValue? RestaurantBooking.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") :"" %><br />
                            Chỉnh sửa lần cuối bởi <%= RestaurantBooking.LastEditedBy!= null ? RestaurantBooking.LastEditedBy.FullName : "" %> vào lúc <%= RestaurantBooking.LastEditedDate.HasValue? RestaurantBooking.LastEditedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") :"" %>
                        </div>
                    </div>
                </div>
                <div class="row" ng-controller="commissionController" ng-init="loadCommission()">
                    <div class="col-xs-12">
                        <h3>Trích ngoài
                <button type="button" class="btn btn-primary" ng-click="addCommission()">Thêm</button></h3>
                        <br />
                        <div class="form-group" ng-show="$root.listCommission.length > 0">
                            <div class="row">
                                <div class="col-xs-2">
                                    Số phiếu chi
                                </div>
                                <div class="col-xs-2">
                                    Trả cho
                                </div>
                                <div class="col-xs-2">
                                    Số tiền
                                </div>
                            </div>
                        </div>
                        <div class="form-group" ng-repeat="item in $root.listCommission">
                            <div class="row">
                                <input type="hidden" ng-model="item.id">
                                <div class="col-xs-2">
                                    <input type="text" class="form-control" placeholder="Số phiếu chi" data-inputmask="'alias': 'integer','allowMinus':false,'groupSeparator': ',', 'autoGroup': true,'rightAlign':false" ng-model="item.paymentVoucher">
                                </div>
                                <div class="col-xs-2 nopadding-right">
                                    <input type="text" class="form-control" placeholder="Trả cho" ng-model="item.payFor" />
                                </div>
                                <div class="col-xs-2 nopadding-right">
                                    <div class="input-group">
                                        <input type="text" class="form-control" placeholder="Số tiền" ng-model="item.amount" data-control="inputmask" ng-change="calculateTotalCommission() " />
                                        <span class="input-group-addon">₫</span>
                                    </div>
                                </div>
                                <div class="col-xs-2">
                                    <div class="checkbox" style="margin-top: 0px; margin-bottom: 0px">
                                        <label>
                                            <input type="checkbox" ng-model="item.transfer">Chuyển khoản
                                        </label>
                                    </div>
                                </div>
                                <div class="col-xs-1 nopadding-right">
                                    <button type="button" class="btn btn-primary" ng-click="removeCommission($index)">
                                        Xóa
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-offset-4 col-xs-8" ng-show="$root.listCommission.length > 0">
                                    Tổng : {{ calculateTotalCommission() }}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" ng-controller="serviceOutsideController" ng-init="loadServiceOutside()">
                    <div class="col-xs-12">
                        <h3>Dịch vụ ngoài 
                <button type="button" class="btn btn-primary" ng-click="addServiceOutside()">Thêm</button></h3>
                        <br />
                        <div class="form-group" ng-show="$root.listServiceOutside.length > 0">
                            <div class="row">
                                <div class="col-xs-2">
                                    Dịch vụ
                                </div>
                                <div class="col-xs-2">
                                    Đơn giá
                                </div>
                                <div class="col-xs-2" style="width: 12%">
                                    Số lượng
                                </div>
                                <div class="col-xs-2">
                                    Thành tiền
                                </div>
                            </div>
                        </div>
                        <div class="form-group" ng-repeat="item in $root.listServiceOutside">
                            <div class="row">
                                <input type="hidden" ng-model="item.id">
                                <div class="col-xs-2 nopadding-right">
                                    <input type="text" class="form-control" placeholder="Dịch vụ" ng-model="item.service" />
                                </div>
                                <div class="col-xs-2 nopadding-right">
                                    <div class="input-group">
                                        <input type="text" class="form-control" placeholder="Đơn giá" ng-model="item.unitPrice" data-control="inputmask" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false"
                                            ng-change="calculateServiceOutside($index, item.unitPrice, item.quantity)" />
                                        <span class="input-group-addon">₫</span>
                                    </div>
                                </div>
                                <div class="col-xs-2 nopadding-right" style="width: 12%">
                                    <input type="text" class="form-control" placeholder="Số lượng" ng-model="item.quantity" ng-change="calculateServiceOutside($index, item.unitPrice, item.quantity)" />
                                </div>
                                <div class="col-xs-2 nopadding-right">
                                    <div class="input-group">
                                        <input type="text" class="form-control" placeholder="Thành tiền" ng-model="item.totalPrice" data-control="inputmask" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" ng-change="calculateTotalServiceOutside()" />
                                        <span class="input-group-addon">₫</span>
                                    </div>
                                </div>
                                <div class="col-xs-1 nopadding-right">
                                    <div class="checkbox" style="margin-top: 0px; margin-bottom: 0px">
                                        <label>
                                            <input type="checkbox" ng-model="item.vat">VAT
                                        </label>
                                    </div>
                                </div>
                                <div class="col-xs-3 nopadding-right nopadding-left" style="width: 14%">
                                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target=".modal-serviceOutsideDetail{{item.id}}">Chi tiết</button>
                                    <button type="button" class="btn btn-primary" ng-click="removeServiceOutside($index)">
                                        Xóa
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" ng-show="$root.listServiceOutside.length > 0">
                            <div class="row">
                                <div class="col-xs-offset-7 col-xs-4" style="margin-left: 57.333%">
                                    Tổng : {{ calculateTotalServiceOutside() }}
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="row" ng-controller="guideController" ng-init="loadGuide()">
                    <div class="col-xs-12">
                        <h3>Hướng dẫn viên
                <button type="button" class="btn btn-primary" ng-click="addGuide()">Thêm</button></h3>
                        <br />
                        <div class="form-group" ng-show="$root.listGuide.length > 0">
                            <div class="row">
                                <div class="col-xs-2">
                                    Tên
                                </div>
                                <div class="col-xs-2">
                                    Số điện thoại
                                </div>
                            </div>
                        </div>
                        <div class="form-group" ng-repeat="item in $root.listGuide">
                            <div class="row">
                                <input type="hidden" ng-model="item.id">
                                <div class="col-xs-2 nopadding-right">
                                    <input type="text" class="form-control" placeholder="Tên" ng-model="item.name" />
                                </div>
                                <div class="col-xs-2 nopadding-right">
                                    <input type="text" class="form-control" placeholder="Số điện thoại" ng-model="item.phone" />
                                </div>
                                <div class="col-xs-1 nopadding-right">
                                    <button type="button" class="btn btn-primary" ng-click="removeGuide($index)">
                                        Xóa
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-4">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:TextBox runat="server" ID="txtSpecialRequest" CssClass="form-control" TextMode="MultiLine" placeholder="Yêu cầu" Rows="5" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:TextBox runat="server" ID="txtMenuDetail" data-id="txtMenuDetail" CssClass="form-control" TextMode="MultiLine" placeholder="Thực đơn" Rows="15" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div ng-controller="serviceOutsideDetailController">
            <div class="modal fade modal-serviceOutsideDetail{{serviceOutside.id}}" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="true" ng-init="loadServiceOutsideDetail(serviceOutside.id)" ng-repeat="serviceOutside in $root.listServiceOutside">
                <div class="modal-dialog" role="document" style="width: 1100px">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h3 class="modal-title">Chi tiết của dịch vụ {{serviceOutside.service}}
                            <button type="button" class="btn btn-primary" ng-click="addServiceOutsideDetail(serviceOutside.id)">Thêm</button></h3>
                        </div>
                        <div class="modal-body">
                            <div class="form-group" ng-show="serviceOutside.listServiceOutsideDetailDTO.length > 0">
                                <div class="row">
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        Tên
                                    </div>
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        Đơn giá
                                    </div>
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        Số lượng
                                    </div>
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        Thành tiền
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" ng-repeat="serviceOutsideDetail in serviceOutside.listServiceOutsideDetailDTO">
                                <div class="row">
                                    <input type="hidden" ng-model="item.id">
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        <input type="text" class="form-control" placeholder="Tên" ng-model="serviceOutsideDetail.name" />
                                    </div>
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        <div class="input-group">
                                            <input type="text" class="form-control" placeholder="Đơn giá" ng-model="serviceOutsideDetail.unitPrice" data-control="inputmask" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false"
                                                ng-change="calculateServiceOutsideDetail(serviceOutside, $index, serviceOutsideDetail.unitPrice, serviceOutsideDetail.quantity)" />
                                            <span class="input-group-addon">₫</span>
                                        </div>
                                    </div>
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        <input type="text" class="form-control" placeholder="Số lượng" ng-model="serviceOutsideDetail.quantity" ng-change="calculateServiceOutsideDetail(serviceOutside, $index, serviceOutsideDetail.unitPrice, serviceOutsideDetail.quantity)" />
                                    </div>
                                    <div class="col-xs-2 nopadding-right" style="width: 20%">
                                        <div class="input-group">
                                            <input type="text" class="form-control" placeholder="Thành tiền" ng-model="serviceOutsideDetail.totalPrice" data-control="inputmask" data-inputmask="'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'rightAlign':false" ng-change="calculateTotalServiceOutside()" />
                                            <span class="input-group-addon">₫</span>
                                        </div>
                                    </div>

                                    <div class="col-xs-2" style="width: 20%">
                                        <button type="button" class="btn btn-primary" ng-click="removeServiceOutsideDetail($index,serviceOutside.id)">
                                            Xóa
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" ng-show="serviceOutside.listServiceOutsideDetailDTO.length > 0">
                                <div class="row">
                                    <div class="col-xs-offset-7 col-xs-4" style="margin-left: 60%">
                                        Tổng : {{ calculateTotalServiceOutsideDetail(serviceOutside) }}
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade modal-history" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="true">
            <div class="modal-dialog" role="document" style="width: 1100px">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="modal-title">Lịch sử
                        </h3>
                    </div>
                    <div class="modal-body">
                        <asp:PlaceHolder ID="plhHistory" runat="server">
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
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="plhHistoryErrorMessage" Visible="false" runat="server">
                            <div class="alert alert-danger" role="alert">
                                <strong>Error!</strong>Bạn không có quyền xem lịch sử
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade modal-reason" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="true">
            <div class="modal-dialog" role="document">
                <div>
                    <div class="modal-body">
                        <div class="row">
                            <asp:TextBox runat="server" ID="txtReason" TextMode="MultiLine" CssClass="form-control" Rows="15" placeholder="Lý do hủy"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer" style="border-top: none; padding-top: 0; padding-right: 0">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="/modules/sails/admin/bookingviewingcontroller.js"></script>
    <script>
        $("#ctl00_ctl00_AdminContentMain_AdminContent_agencySelectornameid").click(function () {
            var width = 800;
            var height = 600;
            var popup = window.open('/Modules/Sails/Admin/AgencySelectorPage.aspx?NodeId=1&SectionId=15&clientid=<%=agencySelector.ClientID%>', 'Agencyselect', 'width=' + width + ',height=' + height + ',top=' + ((screen.height / 2) - (height / 2)) + ',left=' + ((screen.width / 2) - (width / 2)));
            var popupTick = setInterval(function () {
                if (popup.closed) {
                    clearInterval(popupTick);
                    var input = $('[data-id="hidAgencyId"]');
                    input.trigger('input');
                    input.trigger('change')
                }
            }, 500);
            return false;
        });
        $("#<%= chkVAT.ClientID%>").attr({ "ng-model": "$root.bookingVAT", "ng-change": "serviceOutsideChangeValueVAT()" })
    </script>
    <script>
        $("#sendemail").colorbox({
            iframe: true,
            width: 1200,
            height: 600,
        });
        $("#btnSave").click(function(){
            if($("#aspnetForm").valid()){
                angular.element('#saveController').scope().save();
                angular.element('#saveController').scope().$apply();
            }
        })
        $.validator.addMethod("valueNotEquals", function (value, element, arg) {
            return arg !== value;
        }, "");
        var startCheck = false;
        $.validator.addMethod("checkCancelledAndReasonRequired", function(value, element, args){
            var output = false;
            output = true;
            if($("#<%= ddlStatus.ClientID%>").val()==2){
                if(!value){
                    output=false;
                    startCheck=true;
                    $("#btnReason").addClass("btn-danger");
                }
            }
            return output
        },"")
        $("#aspnetForm").validate({
            rules: {
                <%=ddlMenu.UniqueID%>: { valueNotEquals: "-1" },
                reason: {checkCancelledAndReasonRequired:""}
            },
            messages: {
                <%=ddlMenu.UniqueID%>: { valueNotEquals: "Yêu cầu chọn thực đơn" },
                reason: {checkCancelledAndReasonRequired: "Yêu cầu điền lý do hủy"}
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
    <%if (RestaurantBooking.LockStatus == LockStatusEnum.Locked)
      {%>
    <script>
        $("button:not(.undisabled)").attr("disabled","true");
        $("input[type='text']").attr("disabled","true");
        $("input[type='checkbox']").attr("disabled","true");
        $("input[type='radio']").attr("disabled","true");
        $("select").attr("disabled","true");
        $("textArea").attr("disabled","true");
        $(".modal button").removeAttr("disabled");
    </script>
    <%}%>
    <%if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowLockBooking))
      {%>
    <script>
        $("#<%= btnLock.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền khóa đặt chỗ"});
    </script>
    <%}%>
    <%if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowUnlockBooking))
      {%>
    <script>
        $("#<%= btnUnlock.ClientID%>").attr({"disabled":"true","title":"Bạn không có quyền mở khóa đặt chỗ"});
    </script>
    <%}%>
    <%  if (RestaurantBooking.OnlyUnlockByRole != OnlyUnlockByRoleEnum.Null)
        {
            if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)OnlyUnlockByRoleEnum.Administrator))
            {%>
    <script>
        $("#<%= btnUnlock.ClientID%>").attr({"disabled":"true","title":"Booking này hiện chỉ có admin mới có quyền mở khóa"});
    </script>
    <%      }
        }%>
    <script>
        $(document).ready(function(){
            $(".modal-reason").css({"top":90})
            if($("#<%= ddlStatus.ClientID%>").val()==2){
                $("#btnReason").removeClass("hidden");
            }else{
                $("#btnReason").addClass("hidden");
            }
        })
        $("#<%= ddlStatus.ClientID%>").change(function(){
            if($(this).val()==2){
                $("#btnReason").removeClass("hidden");
                $(".modal-reason").modal("show");
            }else{
                $("#btnReason").addClass("hidden");
            }
        })
    </script>
    <script>
        $(document).ready(function(){
            $("#hidReason").val($("#<%=txtReason.ClientID%>").val());
        })
        $("#<%=txtReason.ClientID%>").change(function(){
            $("#hidReason").val($(this).val());
            $("#hidReason").trigger("change");
        })
    </script>
    <script>
        $("#hidReason,#<%= ddlStatus.ClientID%>").change(function(){
            if(startCheck){    
                if($("#<%= ddlStatus.ClientID%>").val()!=2){
                    $("#hidReason-error").addClass("hidden");
                    $("#btnReason").removeClass("btn-danger");
                }else{
                    $("#hidReason-error").removeClass("hidden");
                    $("#btnReason").addClass("btn-danger");
                    if($("#hidReason").val()){
                        $("#hidReason-error").addClass("hidden");
                        $("#btnReason").removeClass("btn-danger");
                    }
                }
            }
        })
    </script>
    <% if (!AllowEditBooking)
       { %>
    <script>
        $("#btnSave").attr({"disabled":"true","title":"Bạn không có quyền sửa đặt chỗ"});
    </script>
    <%} %>
    <% if (!AllowViewHistoryBooking)
       { %>
    <script>
        $("#btnHistory").attr({"disabled":"true","title":"Bạn không có quyền xem lịch sử"});
    </script>
    <%} %>
</asp:Content>
