using CMS.Core.Domain;
using CMS.Web.HttpModules;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class MO_Main : System.Web.UI.MasterPage
    {
        private SailsMasterBLL sailsMasterBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        private User currentUser;

        public SailsMasterBLL SailsMasterBLL
        {
            get
            {
                if (sailsMasterBLL == null)
                {
                    sailsMasterBLL = new SailsMasterBLL();
                }
                return sailsMasterBLL;
            }
        }
        public PermissionBLL PermissionBLL
        {
            get
            {
                if (permissionBLL == null)
                {
                    permissionBLL = new PermissionBLL();
                }
                return permissionBLL;
            }
        }
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                {
                    userBLL = new UserBLL();
                }
                return userBLL;
            }
        }
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = UserBLL.UserGetCurrent();
                }
                return currentUser;
            }
        }
        public string Title
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                lblTitle.Text = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            FillNavigateUrl();
            AgencyContactBirthdayBind();
            NavigateVisibleByPermission();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (sailsMasterBLL != null)
            {
                sailsMasterBLL.Dispose();
                sailsMasterBLL = null;
            }

            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }

            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }

            if (!Page.IsPostBack)
            {
                Session["Redirect"] = false;
            }

            if (!(bool)Session["Redirect"])
            {
                ClearMessage();
            }
        }

        protected void lbLogOut_Click(object sender, EventArgs e)
        {
            var am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
            am.Logout();

            Context.Response.Redirect(Context.Request.RawUrl);
        }

        public void FillNavigateUrl()
        {
            hlOrders.NavigateUrl = "OrderReport.aspx?NodeId=1&SectionId=15";
            hlAllPending.NavigateUrl = "OrderReport.aspx?NodeId=1&SectionId=15&mode=all";
            hlBookingPeriod.NavigateUrl = "BookingReportPeriodAll.aspx?NodeId=1&SectionId=15";
            hlIncomeReport.NavigateUrl = "IncomeReport.aspx?NodeId=1&SectionId=15";
            hlExpenseReport.NavigateUrl = "ExpenseReport.aspx?NodeId=1&SectionId=15";
            hlExpenseDebt.NavigateUrl = "PayableList.aspx?NodeId=1&SectionId=15";
            hlBalance.NavigateUrl = "BalanceReport.aspx?NodeId=1&SectionId=15";
            hlInspection.NavigateUrl = "InspectionReport.aspx?NodeId=1&SectionId=15";
            hlAgencyEdit.NavigateUrl = "AgencyEdit.aspx?NodeId=1&SectionId=15";
            hlAgencyList.NavigateUrl = "AgencyList.aspx?NodeId=1&SectionId=15";
            hlAgentList.NavigateUrl = "AgentList.aspx?NodeId=1s&SectionId=15";
            hlTripList.NavigateUrl = "SailsTripList.aspx?NodeId=1&SectionId=15";
            hlTripEdit.NavigateUrl = "SailsTripEdit.aspx?NodeId=1&SectionId=15";
            hlCruiseEdit.NavigateUrl = "CruisesEdit.aspx?NodeId=1&SectionId=15";
            hlCruiseList.NavigateUrl = "CruisesList.aspx?NodeId=1&SectionId=15";
            hlRoomList.NavigateUrl = "RoomList.aspx?NodeId=1&SectionId=15";
            hlRoomEdit.NavigateUrl = "RoomEdit.aspx?NodeId=1&SectionId=15";
            hlExtraOption.NavigateUrl = "ExtraOptionEdit.aspx?NodeId=1&SectionId=15";
            hlRoomClass.NavigateUrl = "RoomClassEdit.aspx?NodeId=1&SectionId=15";
            hlRoomType.NavigateUrl = "RoomTypexEdit.aspx?NodeId=1&SectionId=15";
            hlUSDRate.NavigateUrl = "ExchangeRate.aspx?NodeId=1&SectionId=15";
            hlCosting.NavigateUrl = "Costing.aspx?NodeId=1&SectionId=15";
            hlHaiPhong.NavigateUrl = "HaiPhongContracts.aspx?NodeId=1&SectionId=15";
            hlCostTypes.NavigateUrl = "CostTypes.aspx?NodeId=1&SectionId=15";
            hlHaiPhongExpenseReport.NavigateUrl = "HaiPhongExpenseReport.aspx?NodeId=1&SectionId=15";
            hlExpensePeriod.NavigateUrl = "ExpensePeriod.aspx?NodeId=1&SectionId=15";
            hlExpenseDate.NavigateUrl = hlBookingDate.NavigateUrl;
            hlPermissions.NavigateUrl = "SetPermission.aspx?NodeId=1&SectionId=15";
            hlReceiablePayable.NavigateUrl = "ReceivableTotal.aspx?NodeId=1&SectionId=s15";
            hlAddQuestion.NavigateUrl = "QuestionGroupEdit.aspx?NodeId=1&SectionId=15";
            hlQuestionList.NavigateUrl = "QuestionView.aspx?NodeId=1&SectionId=15";
            hlFeedbackReport.NavigateUrl = "FeedbackReport.aspx?NodeId=1&SectionId=15";
            hlFeedbackResponse.NavigateUrl = "FeedbackResponse.aspx?NodeId=1&SectionId=15";
            hlDocumentManage.NavigateUrl = "DocumentManage.aspx?NodeId=1&SectionId=15";
            hlViewDocument.NavigateUrl = "DocumentView.aspx?NodeId=1&SectionId=15";
            hlViewMeetings.NavigateUrl = "ViewMeetings.aspx?NodeId=1&SectionId=15";
            hlUserPanel.NavigateUrl = "User.aspx?NodeId=1&SectionId=15";
            hlVoucherEdit.NavigateUrl = "VoucherEdit.aspx?NodeId=1&SectionId=15";
            hlVoucherList.NavigateUrl = "VoucherList.aspx?NodeId=1&SectionId=15";
            hlAgencyLocation.NavigateUrl = "AgencyLocations.aspx?NodeId=1&SectionId=15";
            hlVoucherTemplates.NavigateUrl = "VoucherTemplates.aspx?NodeId=1&SectionId=15";
            hlAddSerialBookings.NavigateUrl = "AddSeriesBookings.aspx?NodeId=1&SectionId=15";
            hplSeriesManager.NavigateUrl = "SeriesManager.aspx?NodeId=1&SectionId=15";
            hlQuotationManagement.NavigateUrl = "QuotationManagement.aspx?NodeId=1&SectionId=15";
            hlContractManagement.NavigateUrl = "ContractManagement.aspx?NodeId=1&SectionId=15";
        }

        public void AgencyContactBirthdayBind()
        {
            rptBirthday.DataSource = SailsMasterBLL.AgencyContactGetAllByBirthday();
            rptBirthday.DataBind();
        }

        public void NavigateVisibleByPermission()
        {
            if (CurrentUser == null)
                return;

            if (PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Roles.Administrator))
            {
                return;
            }

            //pAddBooking.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ADDBOOKING);
            //pAddSeriesBookings.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ADDBOOKING);
            //pBookingList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGLIST);
            //pSerialManager.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGLIST);
            //pOrders.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ORDERREPORT);
            //pBookingDate.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGREPORT);
            //pBookingReport.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGREPORTPERIOD);
            //pIncomeReport.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_INCOMEREPORT);
            //pReceivable.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_PAYMENTREPORT);
            //pExpenseReport.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXPENSEREPORT);
            //pPayable.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_PAYABLELIST);
            //pBalance.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BALANCEREPORT);
            //pSummary.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_RECEIVABLETOTAL);
            //pAgencyEdit.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_AGENCYEDIT);
            //pAgencyList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_AGENCYLIST);
            //pAgencyPolicies.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_AGENTLIST);
            //pTripEdit.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_SAILSTRIPEDIT);
            //pTripList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_SAILSTRIPLIST);
            //pCruiseEdit.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_CRUISESEDIT);
            //pCruiseList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_CRUISESLIST);
            //pRoomClass.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ROOMCLASSEDIT);
            //pRoomType.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ROOMTYPEXEDIT);
            //pRoomEdit.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ROOMEDIT);
            //pRoomList.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_ROOMLIST);
            //pExtraService.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXTRAOPTIONEDIT);
            //pCostingConfig.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_COSTING);
            //pDailyManualCost.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_BOOKINGREPORT);
            //pHaiPhong.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_CRUISECONFIG);
            //pExpensePeriod.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXPENSEPERIOD);
            //pCostTypes.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_COSTTYPES);
            //pUSDRate.Visible = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.FORM_EXCHANGERATE);

            if (pAddBooking.Visible || pAddSeriesBookings.Visible || pBookingList.Visible || pSerialManager.Visible || pOrders.Visible || pBookingDate.Visible || pBookingReport.Visible)
            {
                tabBooking.Visible = true;
            }
            else
            {
                tabBooking.Visible = false;
            }

            if (pIncomeReport.Visible || pReceivable.Visible || pExpenseReport.Visible || pHaiPhongExpenseReport.Visible || pPayable.Visible
                || pBalance.Visible || pSummary.Visible || pInspection.Visible)
            {
                tabReports.Visible = true;
            }
            else
            {
                tabReports.Visible = false;
            }

            if (pAgencyEdit.Visible || pVoucherEdit.Visible || pVoucherList.Visible || pVoucherTemplates.Visible || pAgencyList.Visible
                || pAgencyPolicies.Visible || pAgencyViewMeetings.Visible || pAgencyLocation.Visible)
            {
                tabConfiguration.Visible = true;
            }
            else
            {
                tabConfiguration.Visible = false;
            }

            if (pTripEdit.Visible || pTripList.Visible || pCruiseEdit.Visible || pCruiseList.Visible)
            {
                tabTrips.Visible = true;
            }
            else
            {
                tabTrips.Visible = false;
            }

            if (pRoomClass.Visible || pRoomType.Visible || pRoomEdit.Visible || pRoomList.Visible)
            {
                tabRoom.Visible = true;
            }
            else
            {
                tabRoom.Visible = false;
            }

            if (pExtraService.Visible || pCostingConfig.Visible || pDailyManualCost.Visible || pExpensePeriod.Visible || pHaiPhong.Visible
                || pCostTypes.Visible || pUSDRate.Visible)
            {
                tabCost.Visible = true;
            }
            else
            {
                tabCost.Visible = false;
            }
        }

        public int AgencyContactBirthdayCount()
        {
            return SailsMasterBLL.AgencyContactBirthdayCount();
        }

        public int MyBookingPendingCount()
        {
            return SailsMasterBLL.MyBookingPendingCount();
        }

        public int MyTodayBookingPendingCount()
        {
            return MyBookingPendingCount();
        }

        public string UserCurrentGetName()
        {
            return UserBLL.UserCurrentGetName();
        }

        public string SystemBookingPendingMessaging()
        {
            if (CurrentUser == null)
            {
                return "";
            }

            if (PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Roles.Administrator))
            {
                return "System total: " + SailsMasterBLL.SystemBookingPendingCount();
            }

            return "";
        }

        public void ClearMessage()
        {
            Session["SuccessMessage"] = null;
            Session["InfoMessage"] = null;
            Session["WarningMessage"] = null;
            Session["ErrorMessage"] = null;
        }
    }
}