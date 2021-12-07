using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyExpensesTrackerAPI.Models
{
    public partial class User
    {
        public User()
        {
            Dailyexpenses = new HashSet<Dailyexpenses>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Dailyexpenses> Dailyexpenses { get; set; }
    }
}
