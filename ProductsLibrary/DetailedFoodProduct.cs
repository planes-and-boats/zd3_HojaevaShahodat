using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsLibrary
{
    public class DetailedFoodProduct : FoodProduct
    {
        // --- КОЛЛЕКЦИЯ 2 ---
        public static Queue<DetailedFoodProduct> DetailedProductsQueue { get; private set; } = new Queue<DetailedFoodProduct>();

        public double Calories { get; set; } // Дополнительное поле P
        public string Manufacturer { get; set; } // Доп. свойство 1

        // Конструктор (теперь правильно заполняет автосвойства)
        public DetailedFoodProduct(string name, double proteins, double carbohydrates, double fats, string category, double calories, string manufacturer)
            : base(name, proteins, carbohydrates, fats, category)
        {
            Calories = calories;
            Manufacturer = manufacturer;
        }

        // 1. Метод добавления (Основной - по готовому объекту в конец очереди)
        public static void AddDetailedProduct(DetailedFoodProduct product)
        {
            if (product != null) DetailedProductsQueue.Enqueue(product);
        }

        // 2. Метод добавления (Перегрузка 1 - через полный набор параметров)
        public static void AddDetailedProduct(string name, double proteins, double carbohydrates, double fats, string category, double calories, string manufacturer)
        {
            var product = new DetailedFoodProduct(name, proteins, carbohydrates, fats, category, calories, manufacturer);
            AddDetailedProduct(product);
        }

        // 3. Метод добавления (Перегрузка 2 - быстрое добавление только с калорийностью)
        public static void AddDetailedProduct(string name, double calories)
        {
            var product = new DetailedFoodProduct(name, 0, 0, 0, "Полуфабрикат", calories, "Неизвестен");
            AddDetailedProduct(product);
        }

        // 1. Метод удаления (Основной - извлечение первого элемента из головы очереди)
        public static void RemoveDetailedProduct()
        {
            if (DetailedProductsQueue.Count > 0)
            {
                DetailedProductsQueue.Dequeue();
            }
        }

        // 2. Метод удаления (Перегрузка 1 - удаление конкретного объекта из очереди через пересборку с LINQ)
        public static void RemoveDetailedProduct(DetailedFoodProduct product)
        {
            if (product == null) return;
            // Фильтруем очередь, исключая удаляемый объект
            var updatedList = DetailedProductsQueue.Where(p => p != product).ToList();
            DetailedProductsQueue.Clear();
            foreach (var item in updatedList) DetailedProductsQueue.Enqueue(item);
        }

        // 3. Метод удаления (Перегрузка 2 - удаление по названию продукта с LINQ)
        public static void RemoveDetailedProduct(string name)
        {
            var updatedList = DetailedProductsQueue.Where(p => !p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            DetailedProductsQueue.Clear();
            foreach (var item in updatedList) DetailedProductsQueue.Enqueue(item);
        }

        // Переопределение качества Qp
        public override double GetQuality()
        {
            double baseQ = base.GetQuality();
            return baseQ * 1.2 + Calories * 7;
        }

        // Переопределение вывода информации
        public override string GetInfo()
        {
            return $"[Потомок] {Name} | Калории (P): {Calories} | Производитель: {Manufacturer} | Qp: {GetQuality():F2}";
        }
    }
}
