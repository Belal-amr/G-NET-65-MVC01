namespace GymManagement.DAL.Data.Models
{
    public class Plan : GymUser
    {
        
        public string Description { get; set; } = default!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        #region RelationShips
        public ICollection<Membership> PlanMembers { get; set; } = default!;
        #endregion

    }
}
