using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyExpensesTrackerAPI.Models
{
    public partial class Fileuploader
    {
        public int Id { get; set; }
        public string ActualFileName { get; set; }
        public string GeneratedFileName { get; set; }
        public int? DailyExpensesId { get; set; }

        public virtual Dailyexpenses DailyExpenses { get; set; }
    }
}
