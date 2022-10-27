namespace BookShopDemo.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int bookId { get; set; }
        public int userId { get; set; }
        public string bookCode { get; set; }

    }
}
