using System;
using System.Text;
using System.Web.Services.Discovery;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Aspose.Words;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Portal.Modules.OrientalSails.BusinessLogic;
using Aspose.Words.Rendering;
using Microsoft.Office.Interop.Word;
using System.Linq;
using Portal.Modules.OrientalSails.Enums.Shared;
using C = Portal.Modules.OrientalSails.Enums.Contract;
using Q = Portal.Modules.OrientalSails.Enums.Quotation;
using System.Web;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyView : SailsAdminBasePage
    {
        private bool _editPermission;
        private bool _viewBookingPermission;
        private bool _contactsPermission;
        private bool _recentActivitiesPermission;
        private bool _contractsPermission;
        private AgencyViewBLL agencyViewBLL;
        public AgencyViewBLL AgencyViewBLL
        {
            get
            {
                if (agencyViewBLL == null)
                    agencyViewBLL = new AgencyViewBLL();
                return agencyViewBLL;
            }
        }
        public Agency Agency
        {
            get
            {
                Agency agency = null;
                try
                {
                    if (Request.QueryString["AgencyId"] != null)
                        agency = AgencyViewBLL.AgencyGetById(Convert.ToInt32(Request.QueryString["AgencyId"]));
                }
                catch (Exception) { }
                return agency;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            _editPermission = Module.PermissionCheck(Permission.ACTION_EDITAGENCY, UserIdentity);
            _viewBookingPermission = Module.PermissionCheck("VIEWBOOKINGBYAGENCY", UserIdentity);
            _contactsPermission = Module.PermissionCheck("CONTACTS", UserIdentity);
            _recentActivitiesPermission = Module.PermissionCheck("RECENTACTIVITIES", UserIdentity);
            _contractsPermission = Module.PermissionCheck("CONTRACTS", UserIdentity);

            if (!IsPostBack)
            {
                if (Request.QueryString["agencyid"] != null)
                {
                    var agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["agencyid"]));

                    if (agency.Sale == UserIdentity)
                    {
                        _editPermission = true;
                    }
                    litName1.Text = agency.Name;
                    litName.Text = agency.Name;
                    litTradingName.Text = agency.TradingName;
                    //litRepresentative.Text = agency.Representative;
                    //litRepresentativePosition.Text = agency.RepresentativePosition;
                    //litContact.Text = agency.Contact;
                    //litContactAddress.Text = agency.ContactAddress;
                    //litContactEmail.Text = agency.ContactEmail;
                    //litContactPosition.Text = agency.ContactPosition;
                    //litWebsite.Text = agency.Website;
                    //if (agency.Role != null)
                    //    litRole.Text = agency.Role.Name;
                    //else
                    //{
                    //    litRole.Text = "Customize Role";
                    //}
                    if (agency.Sale != null)
                    {
                        litSale.Text = agency.Sale.AllName;
                        litSalePhone.Text = agency.Sale.Website;
                    }
                    else
                        litSale.Text = @"Unbound";
                    //litTax.Text = agency.TaxCode;
                    //if (agency.Location != null)
                    //    litLocation.Text = agency.Location.Name;
                    litAddress.Text = agency.Address;

                    if (!string.IsNullOrEmpty(agency.Email))
                    {
                        hplEmail.NavigateUrl = string.Format("mailto:{0}", agency.Email);
                        hplEmail.Text = agency.Email;
                    }
                    litPhone.Text = agency.Phone;
                    //litFax.Text = agency.Fax;

                    //litAccountant.Text = agency.Accountant;
                    if (agency.PaymentType == 1) litPayment.Text = "Thanh toán ngay";
                    if (agency.PaymentType == 2) litPayment.Text = "Công nợ";
                    litIvoice.Text = agency.Invoice;
                    chkVat.Checked = agency.IsVat;
                    //litNote.Text = agency.Description;

                    var agencyId = Agency.Id;
                    rptContracts.DataSource = AgencyViewBLL.AgencyContractGetAllByAgency(agencyId);
                    rptContracts.DataBind();

                    rptContacts.DataSource = Module.ContactGetByAgency(agency, !_editPermission); // nếu không có quyền edit thì ko có quyền view
                    rptContacts.DataBind();

                    hplEditAgency.Visible = _editPermission;
                    hplEditAgency.NavigateUrl = string.Format("AgencyEdit.aspx?NodeId={0}&SectionId={1}&agencyid={2}", Node.Id,
                                                        Section.Id, agency.Id);

                    hplAddContact.Visible = _editPermission;
                    hplAddContact.NavigateUrl = "javascript:";
                    string url = string.Format("AgencyContactEdit.aspx?NodeId={0}&SectionId={1}&agencyid={2}",
                                                    Node.Id, Section.Id, agency.Id);
                    hplAddContact.Attributes.Add("onclick", CMS.ServerControls.Popup.OpenPopupScript(url, "Contact", 300, 400));

                    url = string.Format("AgencyContractEdit.aspx?NodeId={0}&SectionId={1}&agencyid={2}",
                                                    Node.Id, Section.Id, agency.Id);

                    hplBookingList.NavigateUrl = string.Format(
                        "BookingManagement.aspx?NodeId={0}&SectionId={1}&ai={2}", Node.Id, Section.Id, agency.Id);
                    hplReceivable.NavigateUrl =
                        string.Format("Receivables.aspx?NodeId={0}&SectionId={1}&ai={2}&f={3}&t={4}",
                                      Node.Id, Section.Id, agency.Id, HttpUtility.UrlEncode(DateTime.Today.AddMonths(-3).ToString("dd/MM/yyyy")), HttpUtility.UrlEncode(DateTime.Today.ToString("dd/MM/yyyy")));

                    rptActivities.DataSource = Module.GetObject<Activity>(Expression.And(Expression.Eq("ObjectType", "MEETING"), Expression.Eq("Params", Convert.ToString(agency.Id))), 0, 0,
                                                                          Order.Desc("DateMeeting"));
                    rptActivities.DataBind();
                }
            }
            LoadContracts();
            RenderViewBookingByThisAgency();
            RenderContacts();
            RenderRecentActivities();
            RenderContracts();
            ddlContractTemplate.DataSource = AgencyViewBLL.ContractGetAll();
            ddlContractTemplate.DataTextField = "Name";
            ddlContractTemplate.DataValueField = "Id";
            ddlContractTemplate.DataBind();
            ddlQuotationTemplate.DataSource = AgencyViewBLL.QuotationGetAll();
            ddlQuotationTemplate.DataTextField = "Name";
            ddlQuotationTemplate.DataValueField = "Id";
            ddlQuotationTemplate.DataBind();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (agencyViewBLL != null)
            {
                agencyViewBLL.Dispose();
                agencyViewBLL = null;
            }
        }
        public void RenderViewBookingByThisAgency()
        {
            if (!_viewBookingPermission)
            {
                hplBookingList.CssClass = hplBookingList.CssClass + " " + "disable";
                hplBookingList.Attributes["href"] = "javascript:";
                var script = @"<script type = 'text/javascript'>";
                script = script +
                         @"$('#" + hplBookingList.ClientID + "').click(function(){$('#disableInform').dialog({resiable:false,modal:true,draggable:false})})";
                script = script + "</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "disableInform", script);
            }
        }
        public void RenderContacts()
        {
            if (!_contactsPermission)
            {
                plhContacts.Visible = false;
                lblContacts.Visible = true;
            }
        }
        public void RenderRecentActivities()
        {
            if (!_recentActivitiesPermission)
            {
                plhActivities.Visible = false;
                lblActivities.Visible = true;
            }
        }
        public void RenderContracts()
        {
            if (!_contractsPermission)
            {
                plhContracts.Visible = false;
                lblContracts.Visible = true;
            }
        }
        public void LoadContracts()
        {
            var agencyId = Agency.Id;
            rptContracts.DataSource = AgencyViewBLL.AgencyContractGetAllByAgency(agencyId);
            rptContracts.DataBind();
        }
        protected void rptContracts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AgencyContract)
            {
                var contract = (AgencyContract)e.Item.DataItem;

                ValueBinder.BindLiteral(e.Item, "litName", contract.ContractName);
                var expiredDate = new DateTime();
                try
                {
                    expiredDate = contract.ExpiredDate.Value;
                }
                catch { }

                ValueBinder.BindLiteral(e.Item, "litExpired", expiredDate == DateTime.MinValue ? "" : expiredDate.ToString("dd/MM/yyyy"));
                if (contract.CreateDate != null)
                    ValueBinder.BindLiteral(e.Item, "litCreatedDate", contract.CreateDate.Value.ToString("dd/MM/yyyy"));

                if (contract.Received == true)
                {
                    ValueBinder.BindLiteral(e.Item, "litReceived", "Yes");
                }
                else
                {
                    ValueBinder.BindLiteral(e.Item, "litReceived", "No");
                }

                var hplDownload = (HyperLink)e.Item.FindControl("hplDownload");
                hplDownload.NavigateUrl = contract.FilePath;
                //hplDownload.NavigateUrl = contract.FileName;
                hplDownload.Text = contract.FileName;

                var linkEdit = (HyperLink)e.Item.FindControl("hplEdit");

                var url = string.Format("AgencyContractEdit.aspx?NodeId={0}&SectionId={1}&contractid={2}",
                                Node.Id, Section.Id, contract.Id);
                linkEdit.NavigateUrl = "javascript:";
                linkEdit.Attributes.Add("onclick", CMS.ServerControls.Popup.OpenPopupScript(url, "Contract", 300, 400));
                linkEdit.Visible = _editPermission;
                ///////////////////////////////////////////////////////////////////////////////////////////////////////

                var plhContractQuotation = (PlaceHolder)e.Item.FindControl("plhContractQuotation");
                var contracts = contract.Contract;
                var contractName = "";
                try
                {
                    contractName = contracts.Name;
                }
                catch { }
                var quotation = contract.Quotation;
                var quotationName = "";
                try
                {
                    quotationName = quotation.Name;
                }
                catch { }
                var lbtContractTemplate = new LinkButton()
                {
                    Text = contractName,
                    CommandArgument = contract.Id.ToString(),
                };
                lbtContractTemplate.Click += new EventHandler(lbtContractTemplate_Click);
                var lbtQuotationTemplate = new LinkButton()
                {
                    Text = quotationName,
                    CommandArgument = contract.Id.ToString(),
                };
                lbtQuotationTemplate.Click += new EventHandler(lbtQuotationTemplate_Click);
                var separator = new Literal()
                {
                    Text = @" \ ",
                };
                plhContractQuotation.Controls.Add(lbtContractTemplate);
                if (contractName != "" && quotationName != "")
                    plhContractQuotation.Controls.Add(separator);
                plhContractQuotation.Controls.Add(lbtQuotationTemplate);

                var litStatus = (Literal)e.Item.FindControl("litStatus");
                var status = contract.Status;
                var statusName = "";
                switch (status)
                {
                    case 1:
                        statusName = "Contract sent";
                        break;
                    case 2:
                        statusName = "Contract in valid";
                        break;
                }
                litStatus.Text = statusName;
            }
        }
        public void lbtQuotationTemplate_Click(object sender, EventArgs e)
        {
            var agencyContractId = -1;
            try
            {
                agencyContractId = Int32.Parse(((LinkButton)sender).CommandArgument);
            }
            catch { }
            var agencyContract = AgencyViewBLL.AgencyContractGetById(agencyContractId);
            ExportQuotationToWord(agencyContract);
        }
        public void lbtContractTemplate_Click(object sender, EventArgs e)
        {
            var agencyContractId = -1;
            try
            {
                agencyContractId = Int32.Parse(((LinkButton)sender).CommandArgument);
            }
            catch { }
            var agencyContract = AgencyViewBLL.AgencyContractGetById(agencyContractId);
            ExportContractToWord(agencyContract);
        }
        protected void rptContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AgencyContact)
            {
                var contact = (AgencyContact)e.Item.DataItem;

                if (!contact.Enabled)
                {
                    e.Item.Visible = false;
                    return;
                }

                var ltrName = (Literal)e.Item.FindControl("ltrName");
                ltrName.Text = contact.Name;

                var hplName = (HyperLink)e.Item.FindControl("hplName");
                hplName.NavigateUrl = "javascript:";

                if (_editPermission)
                {
                    string url = string.Format("AgencyContactEdit.aspx?NodeId={0}&SectionId={1}&contactid={2}",
                                               Node.Id, Section.Id, contact.Id);
                    hplName.Attributes.Add("onclick", CMS.ServerControls.Popup.OpenPopupScript(url, "Contact", 300, 400));
                }

                var linkEmail = (HyperLink)e.Item.FindControl("hplEmail");
                linkEmail.Text = contact.Email;
                linkEmail.NavigateUrl = string.Format("mailto:{0}", contact.Email);

                ValueBinder.BindLiteral(e.Item, "litPosition", contact.Position);
                ValueBinder.BindLiteral(e.Item, "litPhone", contact.Phone);

                if (contact.IsBooker)
                {
                    ValueBinder.BindLiteral(e.Item, "litBooker", "Booker");
                }

                var lbtDelete = (LinkButton)e.Item.FindControl("lbtDelete");
                lbtDelete.Visible = _editPermission;
                lbtDelete.CommandArgument = contact.Id.ToString();

                {
                    var hplCreateMeeting = (HyperLink)e.Item.FindControl("hplCreateMeeting");
                    hplCreateMeeting.NavigateUrl = "javascript:";
                    string url = string.Format("EditMeeting.aspx?NodeId={0}&SectionId={1}&contact={2}",
                                               Node.Id, Section.Id, contact.Id);
                    hplCreateMeeting.Attributes.Add("onclick",
                                                 CMS.ServerControls.Popup.OpenPopupScript(url, "Contract", 300, 400));
                }
            }
        }
        protected void lbtDelete_Click(object sender, EventArgs e)
        {
            var btn = (IButtonControl)sender;
            var contact = Module.ContactGetById(Convert.ToInt32(btn.CommandArgument));

            contact.Enabled = false;
            Module.SaveOrUpdate(contact);

            PageRedirect(Request.RawUrl);
        }
        protected void rptActivities_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var activity = e.Item.DataItem as Activity;
            var name = e.Item.FindControl("ltrName") as Literal;
            var position = e.Item.FindControl("ltrPosition") as Literal;
            var dateMeeting = e.Item.FindControl("ltrDateMeeting") as Literal;
            if (activity != null)
            {
                if (dateMeeting != null) dateMeeting.Text = activity.DateMeeting.ToString("dd/MM/yyyy");
                if (name != null) name.Text = Module.GetObject<AgencyContact>(activity.ObjectId).Name;
                if (position != null) position.Text = Module.GetObject<AgencyContact>(activity.ObjectId).Position;

                var note = e.Item.FindControl("ltrNote") as Literal;
                var strBuilder = new StringBuilder();
                string[] noteWord = activity.Note.Split(new char[] { ' ' });
                bool isLessWords = false;
                for (int i = 0; i <= 50; i++)
                {
                    try
                    {
                        strBuilder.Append(noteWord[i] + " ");
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        isLessWords = true;
                        break;
                    }
                }
                if (!isLessWords) strBuilder.Append("...");
                if (note != null) note.Text = strBuilder.ToString();

                var lbtEditActivity = (LinkButton)e.Item.FindControl("lbtEditActivity");

                var ltrSale = (Literal)e.Item.FindControl("ltrSale");
                ltrSale.Text = activity.User.FullName;

                lbtEditActivity.PostBackUrl = "javascript:";
                string url = string.Format("EditMeeting.aspx?NodeId={0}&SectionId={1}&activity={2}",
                                           Node.Id, Section.Id, activity.Id);
                lbtEditActivity.Attributes.Add("onclick",
                                             CMS.ServerControls.Popup.OpenPopupScript(url, "Contract", 300, 400));
            }
        }
        protected void lbtDeleteActivity_Click(object sender, EventArgs e)
        {
            var btn = (IButtonControl)sender;
            Activity activity = Module.GetObject<Activity>(Convert.ToInt32(btn.CommandArgument));
            Module.Delete(activity);
            PageRedirect(Request.RawUrl);
        }
        protected void btnExportContractPreviewWord_Click(object sender, EventArgs e)
        {
            ExportContractToWord();
        }
        protected void btnExportContractPreviewPdf_Click(object sender, EventArgs e)
        {
            ExportContractToPdf();
        }
        protected void btnExportQuotationPreviewWord_Click(object sender, EventArgs e)
        {
            ExportQuotationToWord();
        }
        protected void btnExportQuotationPreviewPdf_Click(object sender, EventArgs e)
        {
            ExportQuotationToPdf();
        }
        public void ExportContractToWord()
        {
            var doc = GetGeneratedContract();
            var m = new MemoryStream();
            doc.Save(m, SaveFormat.Doc);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("{0}.doc", "contract"));
            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            m.Close();
            Response.End();
        }
        public void ExportContractToWord(AgencyContract agencyContract)
        {
            var doc = GetGeneratedContract(agencyContract);
            var m = new MemoryStream();
            doc.Save(m, SaveFormat.Doc);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("{0}.doc", "contract"));
            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            m.Close();
            Response.End();
        }
        public void ExportQuotationToWord()
        {
            var doc = GetGeneratedQuotation();
            var m = new MemoryStream();
            doc.Save(m, SaveFormat.Doc);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("{0}.doc", "quotation"));
            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            m.Close();
            Response.End();
        }
        public void ExportQuotationToWord(AgencyContract agencyContract)
        {
            var doc = GetGeneratedQuotation(agencyContract);
            var m = new MemoryStream();
            doc.Save(m, SaveFormat.Doc);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("{0}.doc", "quotation"));
            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            m.Close();
            Response.End();
        }
        public void ExportContractToPdf()
        {
            var doc = GetGeneratedContract();
            var m = new MemoryStream();
            doc.Save(m, SaveFormat.Doc);
            byte[] wordContent = m.GetBuffer();
            var tmpFile = Path.GetTempFileName();
            var tmpFileStream = File.OpenWrite(tmpFile);
            tmpFileStream.Write(wordContent, 0, wordContent.Length);
            tmpFileStream.Close();

            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            var wordDocument = appWord.Documents.Open(tmpFile);
            var tmpFileExport = Path.GetTempFileName();
            wordDocument.ExportAsFixedFormat(tmpFileExport, WdExportFormat.wdExportFormatPDF);
            var fileStream = new FileStream(tmpFileExport, FileMode.Open, FileAccess.ReadWrite);
            var mtest = new MemoryStream();
            fileStream.CopyTo(mtest);
            fileStream.Close();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("{0}.pdf", "contract"));
            Response.OutputStream.Write(mtest.GetBuffer(), 0, mtest.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            mtest.Close();
            m.Close();
            Response.End();
        }
        public void ExportQuotationToPdf()
        {
            var doc = GetGeneratedQuotation();
            var m = new MemoryStream();
            doc.Save(m, SaveFormat.Doc);
            byte[] wordContent = m.GetBuffer();
            var tmpFile = Path.GetTempFileName();
            var tmpFileStream = File.OpenWrite(tmpFile);
            tmpFileStream.Write(wordContent, 0, wordContent.Length);
            tmpFileStream.Close();

            Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
            var wordDocument = appWord.Documents.Open(tmpFile);
            var tmpFileExport = Path.GetTempFileName();
            wordDocument.ExportAsFixedFormat(tmpFileExport, WdExportFormat.wdExportFormatPDF);
            var fileStream = new FileStream(tmpFileExport, FileMode.Open, FileAccess.ReadWrite);
            var mtest = new MemoryStream();
            fileStream.CopyTo(mtest);
            fileStream.Close();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("{0}.pdf", "contract"));
            Response.OutputStream.Write(mtest.GetBuffer(), 0, mtest.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            mtest.Close();
            m.Close();
            Response.End();
        }
        public Aspose.Words.Document GetGeneratedContract(AgencyContract agencyContract)
        {
            Contracts contract = null;
            if (agencyContract != null)
            {
                contract = agencyContract.Contract;
            }
            else
            {
                var selectedContract = -1;
                try
                {
                    selectedContract = Int32.Parse(ddlContractTemplate.SelectedValue);
                }
                catch { }
                contract = AgencyViewBLL.ContractGetById(selectedContract);
            }
            var listContractValid = contract.ListContractValid;
            var templatePath = "";
            if (0 < listContractValid.Count && listContractValid.Count < 2)
            {
                templatePath = "ExportTemplates/Contract1ValidTime.doc";
            }
            if (listContractValid.Count >= 2)
            {
                templatePath = "ExportTemplates/Contract2ValidTime.doc";
            }

            DateTime? validFromDate = null;

            try
            {
                validFromDate = DateTime.ParseExact(txtContractValidFromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception) { }

            var validFromDay = validFromDate != null ? validFromDate.Value.Day.ToString("#00") : "";
            var validFrommonth = validFromDate != null ? (validFromDate.Value.Month + 1).ToString("#00") : "";
            var validFromYear = validFromDate != null ? validFromDate.Value.Year.ToString() : "";

            var doc = new Aspose.Words.Document(Server.MapPath(templatePath));
            var agencyName = "";
            var agencyTradingName = "";
            var agencyRepresentative = "";
            var agencyRepresentativePosition = "";
            var agencyContact = "";
            var agencyContactEmail = "";
            var agencyContactAddress = "";
            var agencyContactPosition = "";
            var agencyAddress = "";
            var agencyPhone = "";
            var agencyFax = "";
            var agencyTaxCode = "";
            var agencyWebsite = "";

            try
            {
                agencyName = !String.IsNullOrEmpty(Agency.Name) ? Agency.Name : "";
            }
            catch { }

            try
            {
                agencyTradingName = !String.IsNullOrEmpty(Agency.TradingName) ? Agency.TradingName : "";
            }
            catch { }

            //try
            //{
            //    agencyRepresentative = !String.IsNullOrEmpty(Agency.Representative) ? Agency.Representative : "";
            //}
            //catch { }

            //try
            //{
            //    agencyRepresentativePosition = !String.IsNullOrEmpty(Agency.RepresentativePosition) ? Agency.RepresentativePosition : "";
            //}
            //catch { }

            //try
            //{
            //    agencyContact = !String.IsNullOrEmpty(Agency.Contact) ? Agency.Contact : "";
            //}
            //catch { }

            //try
            //{
            //    agencyContactAddress = !String.IsNullOrEmpty(Agency.ContactAddress) ? Agency.ContactAddress : "";
            //}
            //catch { }

            //try
            //{
            //    agencyContactPosition = !String.IsNullOrEmpty(Agency.ContactPosition) ? Agency.ContactPosition : "";
            //}
            //catch { }

            //try
            //{
            //    agencyContactEmail = !String.IsNullOrEmpty(Agency.ContactEmail) ? Agency.ContactEmail : "";
            //}
            //catch { }

            try
            {
                agencyAddress = !String.IsNullOrEmpty(Agency.Address) ? Agency.Address : "";
            }
            catch { }

            try
            {
                agencyPhone = !String.IsNullOrEmpty(Agency.Phone) ? Agency.Phone : "";
            }
            catch { }

            try
            {
                agencyFax = !String.IsNullOrEmpty(Agency.Fax) ? Agency.Fax : "";
            }
            catch { }

            //try
            //{
            //    agencyTaxCode = !String.IsNullOrEmpty(Agency.TaxCode) ? Agency.TaxCode : "";
            //}
            //catch { }

            DateTime? validToDate = null;
            try
            {
                validToDate = DateTime.ParseExact(txtContractValidToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }

            var textValidToDate = "";
            try
            {
                textValidToDate = validToDate.Value.ToString("dd/MM/yyyy");
            }
            catch { }

            //try
            //{
            //    agencyWebsite = !String.IsNullOrEmpty(Agency.Website) ? Agency.Website : "";
            //}
            //catch { }

            doc.Range.Replace(new Regex("(\\[ValidFromDay\\])"), validFromDay);
            doc.Range.Replace(new Regex("(\\[ValidFromMonth\\])"), validFrommonth);
            doc.Range.Replace(new Regex("(\\[ValidFromYear\\])"), validFromYear);
            doc.Range.Replace(new Regex("(\\[AgencyName\\])"), agencyName);
            doc.Range.Replace(new Regex("(\\[TradingName\\])"), agencyTradingName);
            doc.Range.Replace(new Regex("(\\[Representative\\])"), agencyRepresentative);
            doc.Range.Replace(new Regex("(\\[RepresentativePosition\\])"), agencyRepresentativePosition);
            doc.Range.Replace(new Regex("(\\[Contact\\])"), agencyContact);
            doc.Range.Replace(new Regex("(\\[ContactPosition\\])"), agencyContactPosition);
            doc.Range.Replace(new Regex("(\\[ContactAddress\\])"), agencyContactAddress);
            doc.Range.Replace(new Regex("(\\[ContactEmail\\])"), agencyContactEmail);
            doc.Range.Replace(new Regex("(\\[AgencyWebsite\\])"), agencyWebsite);
            doc.Range.Replace(new Regex("(\\[AgencyAddress\\])"), agencyAddress);
            doc.Range.Replace(new Regex("(\\[AgencyPhone\\])"), agencyPhone);
            doc.Range.Replace(new Regex("(\\[AgencyFax\\])"), agencyFax);
            doc.Range.Replace(new Regex("(\\[AgencyTaxCode\\])"), agencyTaxCode);
            doc.Range.Replace(new Regex("(\\[ValidToDate\\])"), textValidToDate);
            doc = FillContractPrice(doc, contract);
            return doc;

        }
        public Aspose.Words.Document GetGeneratedContract()
        {
            return GetGeneratedContract(null);
        }

        public Aspose.Words.Document FillContractPrice(Aspose.Words.Document doc, Contracts contract)
        {
            var listContractValid = contract.ListContractValid;
            var index = 1;
            foreach (var contractValid in listContractValid)
            {
                var txtOs2d1nDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs2d1nSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs2d1nChildren6to11 = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Children6to11, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs12d1nCharter = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs22d1nCharter1to4passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._1to4passenger);
                var txtOs22d1nCharter5to8passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._5to8passenger);
                var txtOs22d1nCharter9to12passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._9to12passenger);
                var txtOs22d1nCharter13to16passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._13to16passenger);
                var txtOs3d2nDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs3d2nSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs3d2nChildren6to11 = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Children6to11, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs13d2nCharter = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails1, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum.Unknow);
                var txtOs23d2nCharter1to4passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._1to4passenger);
                var txtOs23d2nCharter5to8passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._5to8passenger);
                var txtOs23d2nCharter9to12passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._9to12passenger);
                var txtOs23d2nCharter13to16passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.OrientalSails2, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._13to16passenger);
                var txtCls2d1nDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls2d1nSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls2d1nChildren6to11 = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Children6to11, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls2d1nCharter = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls3d2nDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls3d2nSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls3d2nChildren6to11 = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Children6to11, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtCls3d2nCharter = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Calypso, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nDeluxeDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nDeluxeSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nDeluxeExtrabed = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Extrabed, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nExecutiveDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Executive, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nExecutiveSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Executive, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nExecutiveExtrabed = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Executive, (int)C.RoomTypeEnum.Extrabed, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nSuiteDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Suite, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nSuiteSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Suite, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nSuiteExtrabed = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Suite, (int)C.RoomTypeEnum.Extrabed, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl2d1nCharter1to30passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._1to30passenger);
                var txtStl2d1nCharter31to40passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._31to40passenger);
                var txtStl2d1nCharter41to50passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._41to50passenger);
                var txtStl2d1nCharter51to64passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._2Day1Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._51to64passenger);
                var txtStl3d2nDeluxeDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nDeluxeSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nDeluxeExtrabed = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Deluxe, (int)C.RoomTypeEnum.Extrabed, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nExecutiveDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Executive, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nExecutiveSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Executive, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nExecutiveExtrabed = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Executive, (int)C.RoomTypeEnum.Extrabed, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nSuiteDouble = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Suite, (int)C.RoomTypeEnum.Double, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nSuiteSingle = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Suite, (int)C.RoomTypeEnum.Single, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nSuiteExtrabed = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Suite, (int)C.RoomTypeEnum.Extrabed, false, (int)C.NumberOfPassengerEnum.Unknow);
                var txtStl3d2nCharter1to30passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._1to30passenger);
                var txtStl3d2nCharter31to40passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._31to40passenger);
                var txtStl3d2nCharter41to50passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._41to50passenger);
                var txtStl3d2nCharter51to64passenger = GetCurrency(contract) + GetPriceFormatted(contractValid, (int)C.CruiseEnum.Starlight, (int)C.TripEnum._3Day2Night, (int)C.RoomClassEnum.Unknow, (int)C.RoomTypeEnum.Unknow, true, (int)C.NumberOfPassengerEnum._51to64passenger);

                var textValidFromDate = "";
                try
                {
                    textValidFromDate = contractValid.ValidFrom.Value.ToString("dd/MM/yyyy");
                }
                catch { }
                var textValidToDate = "";
                try
                {
                    textValidToDate = contractValid.ValidTo.Value.ToString("dd/MM/yyyy");
                }
                catch { }
                doc.Range.Replace(new Regex("(\\[" + index + "ValidFromDate\\])"), textValidFromDate);
                doc.Range.Replace(new Regex("(\\[" + index + "ValidToDate\\])"), textValidToDate);
                doc.Range.Replace(new Regex("(\\[" + index + "Os2d1nDouble\\])"), txtOs2d1nDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Os2d1nSingle\\])"), txtOs2d1nSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Os2d1nChildren6to11\\])"), txtOs2d1nChildren6to11);
                doc.Range.Replace(new Regex("(\\[" + index + "Os2d1nCharter\\])"), txtOs12d1nCharter);
                doc.Range.Replace(new Regex("(\\[" + index + "Os22d1nc14p\\])"), txtOs22d1nCharter1to4passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os22d1nc58p\\])"), txtOs22d1nCharter5to8passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os22d1nc912p\\])"), txtOs22d1nCharter9to12passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os22d1nc1316p\\])"), txtOs22d1nCharter13to16passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os3d2nDouble\\])"), txtOs3d2nDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Os3d2nSingle\\])"), txtOs3d2nSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Os3d2nChildren6to11\\])"), txtOs3d2nChildren6to11);
                doc.Range.Replace(new Regex("(\\[" + index + "Os3d2nCharter\\])"), txtOs13d2nCharter);
                doc.Range.Replace(new Regex("(\\[" + index + "Os23d2nc14p\\])"), txtOs23d2nCharter1to4passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os23d2nc58p\\])"), txtOs23d2nCharter5to8passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os23d2nc912p\\])"), txtOs23d2nCharter9to12passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Os23d2nc1316p\\])"), txtOs23d2nCharter13to16passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls2d1nDouble\\])"), txtCls2d1nDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls2d1nSingle\\])"), txtCls2d1nSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls2d1nChildren6to11\\])"), txtCls2d1nChildren6to11);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls2d1nCharter\\])"), txtCls2d1nCharter);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls3d2nDouble\\])"), txtCls3d2nDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls3d2nSingle\\])"), txtCls3d2nSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls3d2nChildren6to11\\])"), txtCls3d2nChildren6to11);
                doc.Range.Replace(new Regex("(\\[" + index + "Cls3d2nCharter\\])"), txtCls3d2nCharter);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nDeluxeDouble\\])"), txtStl2d1nDeluxeDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nDeluxeSingle\\])"), txtStl2d1nDeluxeSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nDeluxeExtrabed\\])"), txtStl2d1nDeluxeExtrabed);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nExecutiveDouble\\])"), txtStl2d1nExecutiveDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nExecutiveSingle\\])"), txtStl2d1nExecutiveSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nExecutiveExtrabed\\])"), txtStl2d1nExecutiveExtrabed);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nSuiteDouble\\])"), txtStl2d1nSuiteDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nSuiteSingle\\])"), txtStl2d1nSuiteSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nSuiteExtrabed\\])"), txtStl2d1nSuiteExtrabed);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nc130p\\])"), txtStl2d1nCharter1to30passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nc3140p\\])"), txtStl2d1nCharter31to40passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nc4150p\\])"), txtStl2d1nCharter41to50passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl2d1nc5164p\\])"), txtStl2d1nCharter51to64passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nDeluxeDouble\\])"), txtStl3d2nDeluxeDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nDeluxeSingle\\])"), txtStl3d2nDeluxeSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nDeluxeExtrabed\\])"), txtStl3d2nDeluxeExtrabed);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nExecutiveDouble\\])"), txtStl3d2nExecutiveDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nExecutiveSingle\\])"), txtStl3d2nExecutiveSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nExecutiveExtrabed\\])"), txtStl3d2nExecutiveExtrabed);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nSuiteDouble\\])"), txtStl3d2nSuiteDouble);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nSuiteSingle\\])"), txtStl3d2nSuiteSingle);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nSuiteExtrabed\\])"), txtStl3d2nSuiteExtrabed);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nc130p\\])"), txtStl3d2nCharter1to30passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nc3140p\\])"), txtStl3d2nCharter31to40passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nc4150p\\])"), txtStl3d2nCharter41to50passenger);
                doc.Range.Replace(new Regex("(\\[" + index + "Stl3d2nc5164p\\])"), txtStl3d2nCharter51to64passenger);
                index++;
            }
            return doc;
        }
        public string GetCurrency(Contracts contract)
        {
            switch (contract.Currency)
            {
                case (int)CurrencyEnum.USD:
                    return "$";
                case (int)CurrencyEnum.VND:
                    return "₫";
                default:
                    return "";
            }
        }
        public double GetPrice(ContractValid contractValid, int cruiseId, int tripId, int roomClassId, int roomTypeId, bool isCharter, int numberOfPassenger)
        {
            var contractPrice = contractValid.ListContractPrice.Where(x => x.CruiseId == cruiseId
            && x.TripId == tripId
            && x.RoomClassId == roomClassId
            && x.RoomTypeId == roomTypeId
            && x.IsCharter == isCharter
            && x.NumberOfPassenger == numberOfPassenger).FirstOrDefault();

            if (contractPrice != null)
                return contractPrice.Price;
            else
                return 0.0;
        }
        public string GetPriceFormatted(ContractValid contractValid, int cruiseId, int tripId, int roomClassId, int roomTypeId, bool isCharter, int numberOfPassenger)
        {
            var price = GetPrice(contractValid, cruiseId, tripId, roomClassId, roomTypeId, isCharter, numberOfPassenger);
            return String.Format("{0:#,##0.##}", price);
        }

        public Aspose.Words.Document GetGeneratedQuotation(AgencyContract agencyContract)
        {
            Quotation quotation = null;
            if (agencyContract != null)
            {
                quotation = agencyContract.Quotation;
            }
            else
            {
                var selectedQuotation = -1;
                try
                {
                    selectedQuotation = Int32.Parse(ddlQuotationTemplate.SelectedValue);
                }
                catch { }
                quotation = AgencyViewBLL.QuotationGetById(selectedQuotation);
            }

            var doc = new Aspose.Words.Document(Server.MapPath("ExportTemplates/Quotation.doc"));

            var textValidFromDate = "";
            try
            {
                textValidFromDate = quotation.ValidFrom.Value.ToString("dd/MM/yyyy");
            }
            catch { }
            var textValidToDate = "";
            try
            {
                textValidToDate = quotation.ValidTo.Value.ToString("dd/MM/yyyy");
            }
            catch { }

            var txtOs2d1nDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs2d1nSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs2d1nChildren6to11 = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Children6to11, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs12d1nCharter = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs22d1nCharter1to4passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._1to4passenger);
            var txtOs22d1nCharter5to8passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._5to8passenger);
            var txtOs22d1nCharter9to12passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._9to12passenger);
            var txtOs22d1nCharter13to17passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._13to17passenger);
            var txtOs3d2nDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs3d2nSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs3d2nChildren6to11 = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Children6to11, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs13d2nCharter = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails1, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtOs23d2nCharter1to4passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._1to4passenger);
            var txtOs23d2nCharter5to8passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._5to8passenger);
            var txtOs23d2nCharter9to12passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._9to12passenger);
            var txtOs23d2nCharter13to17passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.OrientalSails2, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._13to17passenger);
            var txtCls2d1nDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls2d1nSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls2d1nChildren6to11 = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Children6to11, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls2d1nCharter = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls3d2nDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls3d2nSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls3d2nChildren6to11 = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Children6to11, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtCls3d2nCharter = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Calypso, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nDeluxeDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nDeluxeSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nDeluxeExtrabed = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Extrabed, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nExecutiveDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Executive, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nExecutiveSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Executive, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nExecutiveExtrabed = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Executive, (int)Q.RoomTypeEnum.Extrabed, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nSuiteDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Suite, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nSuiteSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Suite, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nSuiteExtrabed = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Suite, (int)Q.RoomTypeEnum.Extrabed, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl2d1nCharter1to40passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._1to40passenger);
            var txtStl2d1nCharter41to50passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._41to50passenger);
            var txtStl2d1nCharter51to63passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._2Day1Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._51to63passenger);
            var txtStl3d2nDeluxeDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nDeluxeSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nDeluxeExtrabed = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Deluxe, (int)Q.RoomTypeEnum.Extrabed, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nExecutiveDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Executive, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nExecutiveSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Executive, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nExecutiveExtrabed = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Executive, (int)Q.RoomTypeEnum.Extrabed, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nSuiteDouble = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Suite, (int)Q.RoomTypeEnum.Double, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nSuiteSingle = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Suite, (int)Q.RoomTypeEnum.Single, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nSuiteExtrabed = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Suite, (int)Q.RoomTypeEnum.Extrabed, false, (int)Q.NumberOfPassengerEnum.Unknow);
            var txtStl3d2nCharter1to40passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._1to40passenger);
            var txtStl3d2nCharter41to50passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._41to50passenger);
            var txtStl3d2nCharter51to63passenger = GetCurrency(quotation) + GetPriceFormatted(quotation, (int)Q.CruiseEnum.Starlight, (int)Q.TripEnum._3Day2Night, (int)Q.RoomClassEnum.Unknow, (int)Q.RoomTypeEnum.Unknow, true, (int)Q.NumberOfPassengerEnum._51to63passenger);

            doc.Range.Replace(new Regex("(\\[ValidFromDate\\])"), textValidFromDate);
            doc.Range.Replace(new Regex("(\\[ValidToDate\\])"), textValidToDate);
            doc.Range.Replace(new Regex("(\\[Os2d1nDouble\\])"), txtOs2d1nDouble);
            doc.Range.Replace(new Regex("(\\[Os2d1nSingle\\])"), txtOs2d1nSingle);
            doc.Range.Replace(new Regex("(\\[Os2d1nChildren6to11\\])"), txtOs2d1nChildren6to11);
            doc.Range.Replace(new Regex("(\\[Os2d1nCharter\\])"), txtOs12d1nCharter);
            doc.Range.Replace(new Regex("(\\[Os22d1nc14p\\])"), txtOs22d1nCharter1to4passenger);
            doc.Range.Replace(new Regex("(\\[Os22d1nc58p\\])"), txtOs22d1nCharter5to8passenger);
            doc.Range.Replace(new Regex("(\\[Os22d1nc912p\\])"), txtOs22d1nCharter9to12passenger);
            doc.Range.Replace(new Regex("(\\[Os22d1nc1317p\\])"), txtOs22d1nCharter13to17passenger);
            doc.Range.Replace(new Regex("(\\[Os3d2nDouble\\])"), txtOs3d2nDouble);
            doc.Range.Replace(new Regex("(\\[Os3d2nSingle\\])"), txtOs3d2nSingle);
            doc.Range.Replace(new Regex("(\\[Os3d2nChildren6to11\\])"), txtOs3d2nChildren6to11);
            doc.Range.Replace(new Regex("(\\[Os3d2nCharter\\])"), txtOs13d2nCharter);
            doc.Range.Replace(new Regex("(\\[Os23d2nc14p\\])"), txtOs23d2nCharter1to4passenger);
            doc.Range.Replace(new Regex("(\\[Os23d2nc58p\\])"), txtOs23d2nCharter5to8passenger);
            doc.Range.Replace(new Regex("(\\[Os23d2nc912p\\])"), txtOs23d2nCharter9to12passenger);
            doc.Range.Replace(new Regex("(\\[Os23d2nc1317p\\])"), txtOs23d2nCharter13to17passenger);
            doc.Range.Replace(new Regex("(\\[Cls2d1nDouble\\])"), txtCls2d1nDouble);
            doc.Range.Replace(new Regex("(\\[Cls2d1nSingle\\])"), txtCls2d1nSingle);
            doc.Range.Replace(new Regex("(\\[Cls2d1nChildren6to11\\])"), txtCls2d1nChildren6to11);
            doc.Range.Replace(new Regex("(\\[Cls2d1nCharter\\])"), txtCls2d1nCharter);
            doc.Range.Replace(new Regex("(\\[Cls3d2nDouble\\])"), txtCls3d2nDouble);
            doc.Range.Replace(new Regex("(\\[Cls3d2nSingle\\])"), txtCls3d2nSingle);
            doc.Range.Replace(new Regex("(\\[Cls3d2nChildren6to11\\])"), txtCls3d2nChildren6to11);
            doc.Range.Replace(new Regex("(\\[Cls3d2nCharter\\])"), txtCls3d2nCharter);
            doc.Range.Replace(new Regex("(\\[Stl2d1nDeluxeDouble\\])"), txtStl2d1nDeluxeDouble);
            doc.Range.Replace(new Regex("(\\[Stl2d1nDeluxeSingle\\])"), txtStl2d1nDeluxeSingle);
            doc.Range.Replace(new Regex("(\\[Stl2d1nDeluxeExtrabed\\])"), txtStl2d1nDeluxeExtrabed);
            doc.Range.Replace(new Regex("(\\[Stl2d1nExecutiveDouble\\])"), txtStl2d1nExecutiveDouble);
            doc.Range.Replace(new Regex("(\\[Stl2d1nExecutiveSingle\\])"), txtStl2d1nExecutiveSingle);
            doc.Range.Replace(new Regex("(\\[Stl2d1nExecutiveExtrabed\\])"), txtStl2d1nExecutiveExtrabed);
            doc.Range.Replace(new Regex("(\\[Stl2d1nSuiteDouble\\])"), txtStl2d1nSuiteDouble);
            doc.Range.Replace(new Regex("(\\[Stl2d1nSuiteSingle\\])"), txtStl2d1nSuiteSingle);
            doc.Range.Replace(new Regex("(\\[Stl2d1nSuiteExtrabed\\])"), txtStl2d1nSuiteExtrabed);
            doc.Range.Replace(new Regex("(\\[Stl2d1nc140p\\])"), txtStl2d1nCharter1to40passenger);
            doc.Range.Replace(new Regex("(\\[Stl2d1nc4150p\\])"), txtStl2d1nCharter41to50passenger);
            doc.Range.Replace(new Regex("(\\[Stl2d1nc5163p\\])"), txtStl2d1nCharter51to63passenger);
            doc.Range.Replace(new Regex("(\\[Stl3d2nDeluxeDouble\\])"), txtStl3d2nDeluxeDouble);
            doc.Range.Replace(new Regex("(\\[Stl3d2nDeluxeSingle\\])"), txtStl3d2nDeluxeSingle);
            doc.Range.Replace(new Regex("(\\[Stl3d2nDeluxeExtrabed\\])"), txtStl3d2nDeluxeExtrabed);
            doc.Range.Replace(new Regex("(\\[Stl3d2nExecutiveDouble\\])"), txtStl3d2nExecutiveDouble);
            doc.Range.Replace(new Regex("(\\[Stl3d2nExecutiveSingle\\])"), txtStl3d2nExecutiveSingle);
            doc.Range.Replace(new Regex("(\\[Stl3d2nExecutiveExtrabed\\])"), txtStl3d2nExecutiveExtrabed);
            doc.Range.Replace(new Regex("(\\[Stl3d2nSuiteDouble\\])"), txtStl3d2nSuiteDouble);
            doc.Range.Replace(new Regex("(\\[Stl3d2nSuiteSingle\\])"), txtStl3d2nSuiteSingle);
            doc.Range.Replace(new Regex("(\\[Stl3d2nSuiteExtrabed\\])"), txtStl3d2nSuiteExtrabed);
            doc.Range.Replace(new Regex("(\\[Stl3d2nc140p\\])"), txtStl3d2nCharter1to40passenger);
            doc.Range.Replace(new Regex("(\\[Stl3d2nc4150p\\])"), txtStl3d2nCharter41to50passenger);
            doc.Range.Replace(new Regex("(\\[Stl3d2nc5163p\\])"), txtStl3d2nCharter51to63passenger);
            return doc;
        }
        public Aspose.Words.Document GetGeneratedQuotation()
        {
            return GetGeneratedQuotation(null);
        }
        public string GetCurrency(Quotation quotation)
        {
            switch (quotation.Currency)
            {
                case (int)CurrencyEnum.USD:
                    return "$";
                case (int)CurrencyEnum.VND:
                    return "₫";
                default:
                    return "";
            }
        }
        public double GetPrice(Quotation quotation, int cruiseId, int tripId, int roomClassId, int roomTypeId, bool isCharter, int numberOfPassenger)
        {
            var quotationPrice = quotation.ListQuotationPrice.Where(x => x.CruiseId == cruiseId
            && x.TripId == tripId
            && x.RoomClassId == roomClassId
            && x.RoomTypeId == roomTypeId
            && x.IsCharter == isCharter
            && x.NumberOfPassenger == numberOfPassenger).FirstOrDefault();

            if (quotationPrice != null)
                return quotationPrice.Price;
            else
                return 0.0;
        }
        public string GetPriceFormatted(Quotation quotation, int cruiseId, int tripId, int roomClassId, int roomTypeId, bool isCharter, int numberOfPassenger)
        {
            var price = GetPrice(quotation, cruiseId, tripId, roomClassId, roomTypeId, isCharter, numberOfPassenger);
            return String.Format("{0:#,##0.##}", price);
        }
        protected void btnIssueQuotation_Click(object sender, EventArgs e)
        {
            var agencyId = Agency.Id;
            var existedAgencyContract = AgencyViewBLL.AgencyContractGetAllByAgency(agencyId);
            var agencyContract = new AgencyContract();
            if (existedAgencyContract.Count > 0)
            {
                agencyContract = existedAgencyContract.FirstOrDefault();
            }
            agencyContract.Agency = Agency;
            var selectedQuotation = -1;
            try
            {
                selectedQuotation = Int32.Parse(ddlQuotationTemplate.SelectedValue);
            }
            catch { }
            agencyContract.Quotation = AgencyViewBLL.QuotationGetById(selectedQuotation);
            AgencyViewBLL.AgencyContractSaveOrUpdate(agencyContract);
            LoadContracts();
        }
        protected void btnIssueContract_Click(object sender, EventArgs e)
        {
            var agencyId = Agency.Id;
            var existedAgencyContract = AgencyViewBLL.AgencyContractGetAllByAgency(agencyId);
            var agencyContract = new AgencyContract();
            if (existedAgencyContract.Count > 0)
            {
                agencyContract = existedAgencyContract.FirstOrDefault();
            }

            agencyContract.Agency = Agency;

            var selectedContract = -1;
            try
            {
                selectedContract = Int32.Parse(ddlContractTemplate.SelectedValue);
            }
            catch { }
            var contract = AgencyViewBLL.ContractGetById(selectedContract);
            agencyContract.Contract = contract;

            var contractValidFromDate = new DateTime();
            try
            {
                contractValidFromDate = DateTime.ParseExact(txtContractValidFromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            if (contractValidFromDate == DateTime.MinValue)
                agencyContract.ContractValidFromDate = null;
            else
                agencyContract.ContractValidFromDate = contractValidFromDate;

            var contractValidToDate = new DateTime();
            try
            {
                contractValidToDate = DateTime.ParseExact(txtContractValidToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }

            if (contractValidToDate == DateTime.MinValue)
                agencyContract.ContractValidToDate = null;
            else
                agencyContract.ContractValidToDate = contractValidToDate;

            var selectedStatus = -1;
            try
            {
                selectedStatus = Int32.Parse(ddlStatus.SelectedValue);
            }
            catch { }
            agencyContract.Status = selectedStatus;
            AgencyViewBLL.AgencyContractSaveOrUpdate(agencyContract);
            LoadContracts();
        }
    }
}