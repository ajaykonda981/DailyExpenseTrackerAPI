using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyExpensesTrackerAPI.Models
{
    public partial class Dailyexpenses
    {
        public Dailyexpenses()
        {
            Fileuploader = new HashSet<Fileuploader>();
        }

        public int Id { get; set; }
        public int? Category { get; set; }
        public double? Amount { get; set; }
        public DateTime? ExpensesDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Comments { get; set; }
        public int? PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ReasonForDeleting { get; set; }
        public string Attachments { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Paymentmode PaymentModeNavigation { get; set; }
        public virtual ICollection<Fileuploader> Fileuploader { get; set; }
    }
}
