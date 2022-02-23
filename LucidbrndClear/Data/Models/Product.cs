using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear.Data.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int AmountOfSizeXS { get; set; }
        public int AmountOfSizeS { get; set; }
        public int AmountOfSizeM { get; set; }
        public int AmountOfSizeL { get; set; }
        public int AmountOfSizeXL { get; set; }
        public string Description { get; set; }
        public string Composition { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public string Img1 { get; set; }
        public string Img2 { get; set; }
        public string Img3 { get; set; }
        public int Amount
        {
            get
            {
                return AmountOfSizeS + AmountOfSizeXS + AmountOfSizeM + AmountOfSizeL + AmountOfSizeXL;
            }
        }
    }
}
