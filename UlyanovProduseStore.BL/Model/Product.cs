namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс, содержащий информацию о продукте, такую как его стоимость, название и т.п .
    /// </summary>
    public class Product
    {
        public Product() { }

        /// <summary>
        /// Создаёт новый экземпляр Product.
        /// </summary>
        /// <param name="name"> Имя нового экземпляра Product. </param>
        /// <param name="cost"> Стоимость нового экземпляра Product. </param>
        public Product(string name, decimal cost)
        {
            if (string.IsNullOrWhiteSpace(name) is false &&
                name.Contains(".") is false &&
                cost > 0)
            {
                Name = name;
                Cost = cost;
            }
        }

        #region params

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Значение Name экземпляра Product, если оно не пусто. Иначе - null. </returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name) is false)
            {
                return Name;
            }
            return null;
        }

        /// <summary>
        /// Сравнивает два объекта Product.
        /// </summary>
        /// <param name="otherProduct"> Другой объект Product. </param>
        /// <returns> True - если сравниваемые объекты равны по определённым в методе параметрам, иначе - false. </returns>
        public bool Equals(Product otherProduct)
        {
            if (otherProduct.Name == Name &&
                otherProduct.Cost == Cost)
            {
                return true;
            }
            return false;   
        }

        #endregion
    }
}
