using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string Photo { get; set; }

        //JoinDate => CreatedAt from BaseEntity
        //public DateOnly JoinDate { get; set; }

        #region RelationShips
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<Membership> Membership { get; set; }
        public ICollection<Booking> MemberBooking { get; set; }
        #endregion

    }
}
