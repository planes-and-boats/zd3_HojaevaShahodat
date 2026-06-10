using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using ProductsLibrary;

namespace Tests
{
    [TestClass]
    public class ProductTests
    {
        //1. ТЕСТЫ НА КАЧЕСТВО В БАЗОВОМ КЛАССЕ
        [TestMethod]
        public void BaseProduct_GetQuality_CalculatesCorrectly()
        {
            // Формула: Углеводы * 4 + Белки * 4
            // Углеводы = 10, Белки = 5. Ожидаем: (10 * 4) + (5 * 4) = 60
            var product = new FoodProduct("Тест Базовый", 5, 10, 2, "Тест");

            double quality = product.GetQuality();

            Assert.AreEqual(60.0, quality);
        }

        //2. ТЕСТЫ НА КАЧЕСТВО В ПОТОМКЕ 
        [TestMethod]
        public void DetailedProduct_GetQuality_CalculatesCorrectly()
        {
            // Базовое Q = 10 * 4 + 5 * 4 = 60
            // Формула Qp = Q * 1.2 + Калории * 7 = (60 * 1.2) + (100 * 7) = 72 + 700 = 772
            var detailed = new DetailedFoodProduct("Тест Потомок", 5, 10, 2, "Тест", 100, "Фабрика");

            double quality = detailed.GetQuality();

            Assert.AreEqual(772.0, quality);
        }

        // 3. ТЕСТЫ НА ИНФО В БАЗОВОМ КЛАССЕ (List)
        [TestMethod]
        public void BaseProduct_GetInfo_ReturnsString()
        {
            var product = new FoodProduct("Молоко", 3, 5, 3, "Молочные");
            string info = product.GetInfo();

            Assert.IsFalse(string.IsNullOrEmpty(info));
            Assert.IsTrue(info.Contains("Молоко"));
        }


        // 4. ТЕСТЫ НА УДАЛЕНИЯ В БАЗОВОМ КЛАССЕ (List)

        [TestMethod]
        public void BaseProduct_RemoveProduct_ByObject()
        {
            var list = new List<FoodProduct>();
            var p1 = new FoodProduct("Хлеб", 8, 45, 1, "Выпечка");
            list.Add(p1);

            list.Remove(p1); // Удаление по ссылке на объект

            Assert.AreEqual(0, list.Count);
        }


        // 5. ТЕСТЫ НА УДАЛЕНИЯ В ПОТОМКЕ (Queue)

        [TestMethod]
        public void DetailedProduct_RemoveProduct_ClassicDequeue()
        {
            var queue = new Queue<DetailedFoodProduct>();
            queue.Enqueue(new DetailedFoodProduct("Печенье", 5, 60, 15, "Сладости", 400, "Бренд"));

            queue.Dequeue(); // Извлечение из головы очереди

            Assert.AreEqual(0, queue.Count);
        }

    }
}
