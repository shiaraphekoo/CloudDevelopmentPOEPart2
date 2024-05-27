namespace CloudDevelopmentPOE.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
