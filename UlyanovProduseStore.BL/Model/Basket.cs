using System;
using System.Collections.Generic;
using System.Linq;

namespace UlyanovProduseStore.BL.Model
{
    public class Basket
    {
        public Basket(Client client)
        {
            if (client != null)
            {
                Client = client;
            }
        }

        public Basket(Client client, Product product)
        {
            if (client != null && product != null)
            {
                Client = client;
                Products.Add(product);
            }
        }

        public Client Client { get; set; }
        public List<Product> Products = new List<Product>();

        public decimal SumCostOfThisBasket()
        {
            return Products.Sum(prod => prod.Cost);
        }
    }
}
