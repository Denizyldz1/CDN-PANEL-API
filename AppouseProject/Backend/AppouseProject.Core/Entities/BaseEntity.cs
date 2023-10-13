namespace AppouseProject.Core.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.CreatedDate = DateTime.Now;
            this.IsDeleted = false;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
