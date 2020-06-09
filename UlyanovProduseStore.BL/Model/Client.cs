using System;
using System.Collections.Generic;

namespace UlyanovProduseStore.BL.Model
{
    class Client
    {
        public Client(string name)
        {
            Name = name;
            products = new List<Product>();
        }

        public string Name { get; private set; }
        public List<Product> products { get; set; }
        public byte DiscountForPurchasedProducts { get; set; } //TODO: Допилить механизм скидки






    }
}
