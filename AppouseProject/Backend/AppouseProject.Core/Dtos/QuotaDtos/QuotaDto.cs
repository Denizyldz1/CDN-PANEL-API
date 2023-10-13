namespace AppouseProject.Core.Dtos.QuatoDtos
{
    public class QuotaDto
    {
        public int UserId { get; set; }
        public int StorageSpaceByte { get; set; }
        public int UsedSpaceByte { get; set; }
        public int RemainingQuota
        {
            get { return StorageSpaceByte - UsedSpaceByte; }
        }
    }
}
