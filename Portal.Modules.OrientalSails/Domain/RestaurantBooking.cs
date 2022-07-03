using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Domain
{
    public class RestaurantBooking
    {
        private StatusEnum status;
        private IList<TrackingChangeBooking> listPendingTrackingChangeBooking;
        private Nullable<DateTime> date;
        private string trackingChangeNumberOfSet = "";
        private string trackingChangeCostPerPerson = "";
        private LockStatusEnum lockStatus;
        public virtual int Id { get; set; }
        public virtual StatusEnum Status
        {
            get
            {
                return (StatusEnum)Enum.Parse(typeof(StatusEnum), status.ToString());
            }
            set
            {
                SetPropertyEnum<StatusEnum>("Status", ref status, (StatusEnum)value);
            }
        }
        public virtual string Time { get; set; }
        public virtual int NumberOfPaxAdult { get; set; }
        public virtual int NumberOfPaxChild { get; set; }
        public virtual int NumberOfPaxBaby { get; set; }
        public virtual double CostPerPersonAdult { get; set; }
        public virtual double CostPerPersonChild { get; set; }
        public virtual double CostPerPersonBaby { get; set; }
        public virtual int NumberOfDiscountedPaxAdult { get; set; }
        public virtual int NumberOfDiscountedPaxChild { get; set; }
        public virtual int NumberOfDiscountedPaxBaby { get; set; }
        public virtual string SpecialRequest { get; set; }
        public virtual int Payment { get; set; }
        public virtual double TotalPriceOfSet { get; set; }
        public virtual double TotalPrice
        {
            get
            {
                return TotalPriceOfSet + TotalServiceOutsidePrice;
            }
        }
        public virtual Nullable<DateTime> Date
        {
            get
            {
                return date;
            }
            set
            {
                SetProperty<DateTime>("Date", ref date, value);
            }
        }

        public virtual string DateString
        {
            get
            {
                if (Date != null) return Convert.ToDateTime(Date).ToString("dd/MM/yyyy");
                return "";
            }
        }
        public virtual Agency Agency { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual double TotalPaid { get; set; }
        public virtual double Receivable { get; set; }
        public virtual bool MarkIsPaid { get; set; }
        public virtual bool VAT { get; set; }
        public virtual bool IsExportVat { get; set; }
        public virtual int PartOfDay { get; set; }
        public virtual string MenuDetail { get; set; }
        public virtual AgencyContact Booker { get; set; }
        public virtual IList<PaymentHistory> ListPaymentHistory { get; set; }
        public virtual IList<Guide> ListGuide { get; set; }
        public virtual IList<Commission> ListCommission { get; set; }
        public virtual IList<ServiceOutside> ListServiceOutside { get; set; }
        public virtual IList<TrackingChangeBooking> ListTrackingChangeBooking { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual User LastEditedBy { get; set; }
        public virtual DateTime? LastEditedDate { get; set; }
        public virtual bool GALA { get; set; }
        public virtual int VITRIBANAN { get; set; }
        public virtual String Code
        {
            get
            {
                return String.Format("HL{0:D5}", Id);
            }
        }
        public virtual bool IsPaid
        {
            get
            {
                if (TotalPrice > 0 && Receivable <= 0 || MarkIsPaid)
                {
                    return true;
                }
                return false;
            }
        }
        public virtual double ActuallyCollected
        {
            get
            {
                var totalCommission = 0.0;
                foreach (var commission in ListCommission)
                {
                    totalCommission += commission.Amount;
                }
                return TotalPrice - totalCommission;
            }
        }
        public virtual string PartOfDayString
        {
            get
            {
                switch (PartOfDay)
                {
                    case 1:
                        return "Sáng";
                    case 2:
                        return "Trưa";
                    case 3:
                        return "Tối";
                    default:
                        return "";
                }
            }
        }
        public virtual int TotalSet
        {
            get
            {
                return NumberOfPaxAdult + NumberOfPaxChild + NumberOfPaxBaby;
            }
        }
        public virtual int TotalFOC
        {
            get
            {
                return NumberOfDiscountedPaxAdult + NumberOfDiscountedPaxChild + NumberOfDiscountedPaxBaby;
            }
        }
        public virtual double TotalCommissionAmount
        {
            get
            {
                var totalCommissionAmount = 0.0;
                foreach (var commission in ListCommission)
                {
                    totalCommissionAmount += commission.Amount;
                }
                return totalCommissionAmount;
            }
        }
        public virtual double TotalServiceOutsidePrice
        {
            get
            {
                var totalServiceOutsidePrice = 0.0;
                foreach (var serviceOutside in ListServiceOutside)
                {
                    totalServiceOutsidePrice += serviceOutside.TotalPrice;
                }
                return totalServiceOutsidePrice;
            }
        }
        public virtual IList<TrackingChangeBooking> ListPendingTrackingChangeBooking
        {
            get
            {
                if (listPendingTrackingChangeBooking == null)
                {
                    listPendingTrackingChangeBooking = new List<TrackingChangeBooking>();
                }
                return listPendingTrackingChangeBooking;
            }
            set
            {
                listPendingTrackingChangeBooking = value;
            }
        }
        public virtual string TrackingChangeNumberOfSet
        {
            get
            {
                return trackingChangeNumberOfSet;
            }
            set
            {
                SetProperty<string>("NumberOfSet", ref trackingChangeNumberOfSet, value);
            }
        }
        public virtual string TrackingChangeCostPerPerson
        {
            get
            {
                return trackingChangeCostPerPerson;
            }
            set
            {
                SetProperty<string>("CostPerPerson", ref trackingChangeCostPerPerson, value);
            }
        }
        public virtual User CurrentUser { get; set; }
        protected void SetProperty<T>(string name, ref T originValue, T newValue) where T : System.IEquatable<T>
        {
            if (originValue == null || !originValue.Equals(newValue))
            {
                var trackingChangeBooking = new TrackingChangeBooking()
                {
                    ColumnName = name,
                    OriginValue = originValue.ToString(),
                    NewValue = newValue.ToString(),
                    CreatedDate = DateTime.Now,
                    RestaurantBooking = this,
                    CreatedBy = CurrentUser,
                };
                ListPendingTrackingChangeBooking.Add(trackingChangeBooking);
                originValue = newValue;
            }
        }
        protected void SetProperty<T>(string name, ref Nullable<T> originValue, Nullable<T> newValue) where T : struct, System.IEquatable<T>
        {
            if (originValue.HasValue != newValue.HasValue || (newValue.HasValue && !originValue.Value.Equals(newValue.Value)))
            {
                var trackingChangeBooking = new TrackingChangeBooking()
                {
                    ColumnName = name,
                    OriginValue = originValue.ToString(),
                    NewValue = newValue.ToString(),
                    CreatedDate = DateTime.Now,
                    RestaurantBooking = this,
                    CreatedBy = CurrentUser,
                };
                ListPendingTrackingChangeBooking.Add(trackingChangeBooking);
                originValue = newValue;
            }
        }
        protected void SetPropertyEnum<T>(string name, ref T originValue, T newValue) where T : System.IComparable
        {
            if (originValue == null || originValue.CompareTo(newValue) != 0)
            {
                var trackingChangeBooking = new TrackingChangeBooking()
                {
                    ColumnName = name,
                    OriginValue = originValue.ToString(),
                    NewValue = newValue.ToString(),
                    CreatedDate = DateTime.Now,
                    RestaurantBooking = this,
                    CreatedBy = CurrentUser,
                };
                ListPendingTrackingChangeBooking.Add(trackingChangeBooking);
                originValue = newValue;
            }
        }
        public virtual void InitializeTrackingChangeValue()
        {
            trackingChangeNumberOfSet = String.Format("Tổng số suất ăn: {0} <br/>Người lớn: {1}<br/>Trẻ em: {2}<br/>Sơ sinh: {3}", TotalSet, NumberOfPaxAdult, NumberOfPaxChild, NumberOfPaxBaby);
            trackingChangeCostPerPerson = String.Format("Người lớn: {0}₫<br/>Trẻ em : {1}₫<br/>Baby: {2}₫", CostPerPersonAdult.ToString("#,##0.##"), CostPerPersonChild.ToString("#,##0.##"), CostPerPersonBaby.ToString("#,##0.##"));
        }
        public virtual bool HaveEmergencyUpdate
        {
            get
            {
                if (!Date.HasValue) return false;
                var startWatchingTime = Date.Value.AddDays(-1).Add(new TimeSpan(18, 0, 0));
                var noonEndTime = Date.Value.Add(new TimeSpan(14, 0, 0));
                var nightEndTime = Date.Value.Add(new TimeSpan(21, 0, 0));
                if (PartOfDay == 2)
                {
                    if ((startWatchingTime <= LastEditedDate && LastEditedDate <= noonEndTime) && (DateTime.Now <= noonEndTime))
                    {
                        return true;
                    }
                }
                else if (PartOfDay == 3)
                {
                    if (startWatchingTime <= LastEditedDate && LastEditedDate <= nightEndTime && (DateTime.Now <= nightEndTime))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public virtual string CancelledOrChangeDate(DateTime dateToCompare)
        {
            var output = "";
            var lastTrackingChangeStatus = ListTrackingChangeBooking.Where(x => x.ColumnName == "Status").LastOrDefault();
            if (lastTrackingChangeStatus != null)
            {
                if (dateToCompare <= lastTrackingChangeStatus.CreatedDate
                    && lastTrackingChangeStatus.NewValue == "Cancel")
                {
                    output += "Cancel";
                }
            }
            var lastTrackingChangeDate = ListTrackingChangeBooking.Where(x => x.ColumnName == "Date").LastOrDefault();
            if (lastTrackingChangeDate != null)
            {
                if (lastTrackingChangeDate.NewValue == dateToCompare.Date.ToString("M/d/yyyy hh:mm:ss tt"))
                {
                    return output;
                }
            }
            var listTrackingChangeBookingChangedDate = ListTrackingChangeBooking.Where(x => x.ColumnName == "Date")
                .Where(x => x.CreatedDate >= dateToCompare.Date && x.CreatedDate <= dateToCompare.Date.Add(new TimeSpan(23, 59, 59)))
                .Where(x => x.OriginValue == dateToCompare.Date.ToString("M/d/yyyy hh:mm:ss tt"))
                .ToList();
            if (listTrackingChangeBookingChangedDate.Count > 0)
            {
                output += "ChangeDate";
            }
            return output;
        }

        public virtual string StatusOfPayment
        {
            get
            {
                if (Payment == 1)
                {
                    if (IsPaid)
                    {
                        if (Receivable <= 0)
                            return "Đã TT";
                        else
                            return "TT Thiếu";
                    }
                    else
                    {
                        return "Chưa TT";
                    }
                }
                else if (Payment == 2)
                {
                    if (IsPaid)
                        return "Đã TT";
                    else
                        return "CN";
                }
                return "";
            }
        }
        public virtual LockStatusEnum LockStatus
        {
            get
            {
                if (!Date.HasValue)
                    lockStatus = LockStatusEnum.NotLock;
                DateTime timeToLock = new DateTime();
                if (lockStatus == LockStatusEnum.NotLock)
                {
                    if (IsPaid)
                        lockStatus = LockStatusEnum.Locked;
                    timeToLock = Date.Value.AddDays(2);
                    if (PartOfDay == 2)
                    {
                        timeToLock = timeToLock.Add(new TimeSpan(12, 0, 0));
                    }
                    else if (PartOfDay == 3)
                    {
                        timeToLock = timeToLock.Add(new TimeSpan(18, 0, 0));
                    }
                    if (DateTime.Now > timeToLock)
                    {
                        lockStatus = LockStatusEnum.Locked;
                    }
                }
                if (lockStatus == LockStatusEnum.Unlocked)
                {
                    if (!LastUnlockedTime.HasValue) return lockStatus = LockStatusEnum.NotLock;
                    timeToLock = LastUnlockedTime.Value.Add(new TimeSpan(24, 0, 0));
                    if (DateTime.Now > timeToLock)
                    {
                        lockStatus = LockStatusEnum.Locked;
                    }
                }
                return lockStatus;
            }
            set
            {
                lockStatus = value;
            }
        }
        public virtual DateTime? LastUnlockedTime { get; set; }
        public virtual OnlyUnlockByRoleEnum OnlyUnlockByRole { get; set; }
        public virtual string ReceiptVoucher { get; set; }
        public virtual string Reason { get; set; }

    }
}