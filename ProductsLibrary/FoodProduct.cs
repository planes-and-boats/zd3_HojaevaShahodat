using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsLibrary
{
    public class FoodProduct
    {
        // --- КОЛЛЕКЦИЯ 1 ---
        public static List<FoodProduct> BaseProductsList { get; private set; } = new List<FoodProduct>();

        // Используем автосвойства (поля с маленькой буквы больше не нужны, это убирает баги)
        public string Name { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }
        public string Category { get; set; }

        // Конструктор (теперь правильно заполняет свойства)
        public FoodProduct(string name, double proteins, double carbohydrates, double fats, string category)
        {
            Name = name;
            Proteins = proteins;
            Carbohydrates = carbohydrates;
            Fats = fats;
            Category = category;
        }

        // Расчет качества Q
        public virtual double GetQuality()
        {
            return Carbohydrates * 4 + Proteins * 4;
        }

        // Информация об объекте
        public virtual string GetInfo()
        {
            return $"[Базовый] {Name} | Б: {Proteins}г, У: {Carbohydrates}г | Q: {GetQuality():F2}";
        }

        // 1. Метод добавления (Основной - по готовому объекту)
        public static void AddProduct(FoodProduct product)
        {
            if (product != null) BaseProductsList.Add(product);
        }

        // 2. Метод добавления (Перегрузка 1 - через параметры полей)
        public static void AddProduct(string name, double proteins, double carbohydrates, double fats, string category)
        {
            var product = new FoodProduct(name, proteins, carbohydrates, fats, category);
            AddProduct(product);
        }

        // 3. Метод добавления (Перегрузка 2 - создание "пустого" продукта)
        public static void AddProduct(string name)
        {
            var product = new FoodProduct(name, 0, 0, 0, "Разное");
            AddProduct(product);
        }

        // 1. Метод удаления (Основной)
        public static void RemoveProduct(FoodProduct product)
        {
            if (product != null) BaseProductsList.Remove(product);
        }

        // 2. Метод удаления (Перегрузка 1 - по индексу)
        public static void RemoveProduct(int index)
        {
            if (index >= 0 && index < BaseProductsList.Count)
            {
                BaseProductsList.RemoveAt(index);
            }
        }

        // 3. Метод удаления (Перегрузка 2 - по названию продукта с LINQ)
        public static void RemoveProduct(string name)
        {
            var prod = BaseProductsList.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (prod != null) BaseProductsList.Remove(prod);
        }
    }
}
