namespace AppouseProject.UI.Models.QuotaModels
{
    public class QuotaModel
    {
        public int UserId { get; set; }
        public int StorageSpaceByte { get; set; }
        public int UsedSpaceByte { get; set; }
        public int RemainingQuota { get; set; }
    }
}
