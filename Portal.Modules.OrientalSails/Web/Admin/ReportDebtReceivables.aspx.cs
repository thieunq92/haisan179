using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemBox.Spreadsheet;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ReportDebtReceivables : SailsAdminBase
    {
        private double _totalReceivable = 0.0;
        private PermissionBLL permissionBLL;
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
        private UserBLL userBLL;
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                    userBLL = new UserBLL();
                return userBLL;
            }
        }

        public User CurrentUser
        {
            get
            {
                return UserBLL.UserGetCurrent();
            }
        }
        public bool AllowExportDebtReceivables
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportDebtReceivables);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Báo cáo nợ phải thu";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessReportDebtReceivablePage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                GetReportData();
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
        }
        private void GetReportData()
        {
            var lastDayOfMonth = DateTime.Now;
            var to = lastDayOfMonth;
            if (!string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                to = DateTime.ParseExact(Request.QueryString["t"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            txtTo.Text = to.ToString("dd/MM/yyyy");
            var agencyId = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["ai"]))
            {
                agencyId = Int32.Parse(Request.QueryString["ai"]);
                var agency = Module.AgencyGetById(Convert.ToInt32(agencyId));
                agencySelector.Value = agency.Id.ToString();
                agencySelectornameid.Text = agency.Name;
            }
            rptReport.DataSource = Module.GetReportDebtReceivables(to, agencyId);
            rptReport.DataBind();
        }
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportDebtReceivables.aspx" + QueryStringBuildByCriterion());
        }

        public string QueryStringBuildByCriterion()
        {
            NameValueCollection nvcQueryString = new NameValueCollection();
            nvcQueryString.Add("NodeId", "1");
            nvcQueryString.Add("SectionId", "15");

            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                nvcQueryString.Add("t", txtTo.Text);
            }
            if (!string.IsNullOrEmpty(agencySelector.Value))
            {
                nvcQueryString.Add("ai", agencySelector.Value);
            }
            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        protected void rptReport_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var debtReceivable = e.Item.DataItem as DebtReceivable;
            if (debtReceivable != null)
            {
                var hplAgency = (HyperLink)e.Item.FindControl("hplAgency");
                if (hplAgency != null)
                {
                    hplAgency.Text = debtReceivable.Agency != null ? debtReceivable.Agency.Name : "";
                    hplAgency.NavigateUrl = "javascript:;";
                    if (debtReceivable.Agency != null)
                        hplAgency.Attributes.Add("onclick",
                            string.Format("GotoReceivable('{0}','{1}','{2}')", Request["NodeId"], Request["SectionId"],
                                debtReceivable.Agency.Id));
                    //var url = "Receivables.aspx" + GetUrlAgency(debtReceivable.Agency);
                    //hplAgency.NavigateUrl = url;
                }
                Literal litTotalReceivable = (Literal)e.Item.FindControl("litTotalReceivable");
                if (litTotalReceivable != null)
                {
                    _totalReceivable += debtReceivable.TotalReceivable;
                    litTotalReceivable.Text = debtReceivable.TotalReceivable.ToString("#,##0.##");
                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal litTotal = (Literal)e.Item.FindControl("litTotal");
                if (litTotal != null)
                {
                    litTotal.Text = _totalReceivable.ToString("#,##0.##");
                }
            }
        }

        private object GetUrlAgency(Agency agency)
        {
            NameValueCollection nvcQueryString = new NameValueCollection();
            nvcQueryString.Add("NodeId", "1");
            nvcQueryString.Add("SectionId", "15");

            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                nvcQueryString.Add("t", txtTo.Text);
            }
            if (agency != null)
            {
                nvcQueryString.Add("ai", agency.Id.ToString());
            }
            nvcQueryString.Add("spay", "1");

            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            if (!AllowExportDebtReceivables) {
                ShowErrors("Bạn không có quyền xuất file báo cáo nợ phải thu");
                return;
            }
            var lastDayOfMonth = DateTime.Now;
            var to = lastDayOfMonth;
            if (!string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                to = DateTime.ParseExact(Request.QueryString["t"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            txtTo.Text = to.ToString("dd/MM/yyyy");
            var agencyId = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["ai"]))
            {
                agencyId = Int32.Parse(Request.QueryString["ai"]);
                var agency = Module.AgencyGetById(Convert.ToInt32(agencyId));
                agencySelector.Value = agency.Id.ToString();
                agencySelectornameid.Text = agency.Name;
            }
            var list = Module.GetReportDebtReceivables(to, agencyId);

            ExcelFile excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/bao_cao_no_phai_thu.xls"));
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            const int firstrow = 6;
            int crow = firstrow;

            sheet.Rows.InsertCopy(crow, list.Count, sheet.Rows[firstrow]);
            var index = 1;
            sheet.Cells["B4"].Value = to.ToString("dd/MM/yyyy");
            foreach (DebtReceivable debtReceivable in list)
            {
                sheet.Cells[crow, 0].Value = index;
                sheet.Cells[crow, 1].Value = debtReceivable.Agency != null ? debtReceivable.Agency.Name : "";
                _totalReceivable += debtReceivable.TotalReceivable;
                sheet.Cells[crow, 2].Value = debtReceivable.TotalReceivable.ToString("#,##0.##");
                crow++;
                index++;
            }

            sheet.Cells[crow, 1].Value = "Tổng";
            sheet.Cells[crow, 2].Value = _totalReceivable.ToString("#,##0.##");
            excelFile.Save(Response, string.Format("bao_cao_no_phai_thu_{0:dd_MM_yyyy}.xlsx", to));

        }

        protected void btnSaveStatusExport_OnClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptReport.Items)
            {
                if (item.ItemType == ListItemType.Item)
                {
                    CheckBox chkIsExportVat = (CheckBox)item.FindControl("chkIsExportVat");
                    if (chkIsExportVat != null)
                    {
                        if (chkIsExportVat.Checked && chkIsExportVat.Visible)
                        {
                            HiddenField hidId = (HiddenField)item.FindControl("hidId");
                            if (hidId != null)
                            {
                                var restaurantBooking = Module.GetObjectById(typeof(RestaurantBooking), Convert.ToInt32(hidId.Value)) as RestaurantBooking;
                                if (restaurantBooking != null)
                                {
                                    restaurantBooking.IsExportVat = true;
                                    Module.SaveOrUpdate(restaurantBooking);
                                }
                            }
                        }
                    }
                }
            }
            GetReportData();
        }
        public void ShowWarning(string warning)
        {
            Session["WarningMessage"] = "<strong>Warning!</strong> " + warning + "<br/>" + Session["WarningMessage"];
        }

        public void ShowErrors(string error)
        {
            Session["ErrorMessage"] = "<strong>Error!</strong> " + error + "<br/>" + Session["ErrorMessage"];
        }

        public void ShowSuccess(string success)
        {
            Session["SuccessMessage"] = "<strong>Success!</strong> " + success + "<br/>" + Session["SuccessMessage"];
        }
    }
}