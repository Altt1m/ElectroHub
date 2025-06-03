namespace ElectroHub.Helpers
{
    public class AnnouncementQueryObject
    {
        public string? Title { get; set; } = null;
        public bool WasUsed { get; set; } = false;
        public bool ForExchange { get; set; } = false;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
