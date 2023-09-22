namespace FlightPlanner.Models
{
    public class PageResult
    {
        public int Page => Items.Length;
        public int TotalItems => Items.Length;
        public Flight[] Items {get; set; }

        public PageResult(Flight[] items)
        {
            Items = items;
        }
    }
}
