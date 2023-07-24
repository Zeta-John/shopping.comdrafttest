namespace shoppingcomdraft5.Models
{
    public class CartItem
    {
        public int ListingId { get; set; }
        public string ListingName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Quantity * Price; } }

        public CartItem() { }

        public CartItem(Listing listing) 
        {
            ListingId = listing.ListingID;
            ListingName = listing.ListingName;
            Price = listing.Price;
            Quantity = 1;
        }
    }
}
