namespace Carpenter.Models
{
    public class AllProducts
    {
        public int Id { get; set; }  // Primary Key

        public string Name { get; set; }

        public decimal Height_inches { get; set; }

        public decimal Width_inches { get; set; }

        public decimal Length_inches { get; set; }

        public decimal Height_mm { get; set; }

        public decimal Width_mm { get; set; }

        public decimal Length_mm { get; set; }


        public string Category { get; set; }


    }

}
