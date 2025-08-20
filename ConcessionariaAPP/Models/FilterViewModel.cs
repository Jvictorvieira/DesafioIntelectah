namespace ConcessionariaAPP.Models
{
    public class FilterViewModel
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalPage { get; set; }

        
    }
}