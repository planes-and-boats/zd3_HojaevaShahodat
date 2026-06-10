using ProductsLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Демонстрация: Наполнение при старте через разные варианты
            FoodProduct.AddProduct("Яйцо", 12, 1, 10, "Общее");
            DetailedFoodProduct.AddDetailedProduct("Мармелад", 320);

            UpdateList();
        }
        // Вспомогательный метод обновления списка на экране
        private void UpdateList()
        {
            var items = FoodProduct.BaseProductsList.Select(p => p.GetInfo())
                .Concat(DetailedFoodProduct.DetailedProductsQueue.Select(p => p.GetInfo()));
            LstProducts.ItemsSource = items.ToList();
        }

        // --- ДЕЙСТВИЯ ПАНЕЛИ БАЗОВОГО КЛАССА ---

        // [П1] Добавление базового продукта по полному набору введенных параметров
        private void BtnAddBaseFull_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на заполненность текстового поля имени
            if (string.IsNullOrWhiteSpace(TxtBaseName.Text))
            {
                MessageBox.Show("Введите название базового продукта!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Попытка конвертации строк в числа и создания объекта
                var prod = new FoodProduct(
                    TxtBaseName.Text,
                    Convert.ToDouble(TxtBaseProteins.Text),
                    Convert.ToDouble(TxtBaseCarbs.Text),
                    Convert.ToDouble(TxtBaseFats.Text),
                    TxtBaseCategory.Text
                );

                FoodProduct.AddProduct(prod);
                UpdateList();
                MessageBox.Show($"Продукт '{TxtBaseName.Text}' успешно добавлен в базовый список!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибки в числовых полях (Б/Ж/У)! Убедитесь, что ввели корректные числа.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // [П2] Перегрузка добавления: только по имени (остальные поля по нулям)
        private void BtnAddBaseName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBaseName.Text))
            {
                MessageBox.Show("Поле 'Название продукта' пустое! Нечего добавлять.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FoodProduct.AddProduct(TxtBaseName.Text);
            UpdateList();
            MessageBox.Show($"Продукт '{TxtBaseName.Text}' добавлен в упрощенном режиме.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // [П3] Перегрузка добавления: создание объекта со значениями по умолчанию
        private void BtnAddBaseDefault_Click(object sender, RoutedEventArgs e)
        {
            FoodProduct.AddProduct("Новый базовый продукт");
            UpdateList();
            MessageBox.Show("Добавлен стандартный базовый шаблон.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // [П1] Удаление базового продукта: по ссылке на объект из списка
        private void BtnDeleteBaseObj_Click(object sender, RoutedEventArgs e)
        {
            if (LstProducts.SelectedIndex >= 0 && LstProducts.SelectedIndex < FoodProduct.BaseProductsList.Count)
            {
                var target = FoodProduct.BaseProductsList[LstProducts.SelectedIndex];
                FoodProduct.RemoveProduct(target); // Вызов перегрузки удаления по объекту
                UpdateList();
                MessageBox.Show("Объект успешно удален из базовой коллекции.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите конкретный БАЗОВЫЙ продукт в списке для удаления!", "Ошибка выбора", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // [П2] Перегрузка удаления: по индексу позиции в коллекции
        private void BtnDeleteBaseIndex_Click(object sender, RoutedEventArgs e)
        {
            if (LstProducts.SelectedIndex >= 0 && LstProducts.SelectedIndex < FoodProduct.BaseProductsList.Count)
            {
                FoodProduct.RemoveProduct(LstProducts.SelectedIndex); // Вызов перегрузки по индексу
                UpdateList();
                MessageBox.Show("Продукт удален по его порядковому индексу.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось удалить по индексу. Выделите базовую строку в списке.", "Ошибка выбора", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // [П3] Перегрузка удаления: поиск в коллекции по тексту имени
        private void BtnDeleteBaseName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBaseName.Text))
            {
                MessageBox.Show("Введите имя продукта для поиска и удаления!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Дополнительная проверка через LINQ: есть ли вообще такой элемент
            bool exists = FoodProduct.BaseProductsList.Any(p => p.Name.Equals(TxtBaseName.Text, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                FoodProduct.RemoveProduct(TxtBaseName.Text); // Вызов перегрузки по строке
                UpdateList();
                MessageBox.Show($"Все базовые продукты с именем '{TxtBaseName.Text}' удалены.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Продукт с именем '{TxtBaseName.Text}' в базовом списке не найден.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // ДЕЙСТВИЯ ПАНЕЛИ КЛАССА-ПОТОМКА

        // [П1] Добавление детального продукта по полному набору параметров
        private void BtnAddDetailedFull_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDetName.Text))
            {
                MessageBox.Show("Введите название детального продукта!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var detailed = new DetailedFoodProduct(
                    TxtDetName.Text,
                    Convert.ToDouble(TxtDetProteins.Text),
                    Convert.ToDouble(TxtDetCarbs.Text),
                    Convert.ToDouble(TxtDetFats.Text),
                    TxtDetCategory.Text,
                    Convert.ToDouble(TxtDetCalories.Text),
                    TxtDetManufacturer.Text
                );

                DetailedFoodProduct.AddDetailedProduct(detailed);
                UpdateList();
                MessageBox.Show($"Детальный продукт '{TxtDetName.Text}' добавлен в конец очереди!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте числовые параметры детального продукта (включая калории)!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // [П2] Перегрузка добавления: только имя и калорийность (остальное по умолчанию)
        private void BtnAddDetailedQuick_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDetName.Text))
            {
                MessageBox.Show("Укажите имя для быстрого добавления в очередь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DetailedFoodProduct.AddDetailedProduct(TxtDetName.Text, Convert.ToDouble(TxtDetCalories.Text));
                UpdateList();
                MessageBox.Show("Продукт добавлен по имени и калориям.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Поле 'Калории' должно содержать число!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // [П3] Перегрузка добавления: объект-потомок со значениями по умолчанию
        private void BtnAddDetailedDefault_Click(object sender, RoutedEventArgs e)
        {
            var detailed = new DetailedFoodProduct("Дефолтный десерт", 0, 10, 2, "Выпечка", 150, "Цех №1");
            DetailedFoodProduct.AddDetailedProduct(detailed); UpdateList();
            MessageBox.Show("В очередь добавлен шаблонный десерт.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // [П1] Удаление детального продукта: извлечение строго первого элемента
        private void BtnDeleteDetailedObj_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли элементы в очереди
            if (DetailedFoodProduct.DetailedProductsQueue.Count > 0)
            {
                DetailedFoodProduct.RemoveDetailedProduct();
                UpdateList();
                MessageBox.Show("Первый продукт извлечен из головы очереди (FIFO).", "Очередь", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Очередь детальных продуктов пуста! Извлекать нечего.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // [П2] Перегрузка удаления: поиск в очереди по тексту имени из текстового поля
        private void BtnDeleteDetailedKey_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDetName.Text))
            { 
                MessageBox.Show("Введите название детального продукта для его удаления из очереди!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            bool exists = DetailedFoodProduct.DetailedProductsQueue.Any(p => p.Name.Equals(TxtDetName.Text, StringComparison.OrdinalIgnoreCase)); if (exists)
            {
                DetailedFoodProduct.RemoveDetailedProduct(TxtDetName.Text); // Перегрузка удаления по строке имени
                UpdateList();
                MessageBox.Show($"Продукт '{TxtDetName.Text}' удален (очередь пересобрана).", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Продукт '{TxtDetName.Text}' в текущей очереди не найден.", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // [П3] Перегрузка удаления: удаление конкретного выделенного в списке объекта
        private void BtnDeleteDetailedIndex_Click(object sender, RoutedEventArgs e)
        {
            int baseCount = FoodProduct.BaseProductsList.Count;
            int totalCount = baseCount + DetailedFoodProduct.DetailedProductsQueue.Count;
            // Проверяем, что выбранный индекс попадает в диапазон элементов Очереди
            if (LstProducts.SelectedIndex >= baseCount && LstProducts.SelectedIndex < totalCount)
            {
                int queueIndex = LstProducts.SelectedIndex - baseCount;
                var target = DetailedFoodProduct.DetailedProductsQueue.ElementAt(queueIndex);
                DetailedFoodProduct.RemoveDetailedProduct(target); // Перегрузка удаления по ссылке на объект
                UpdateList();
                MessageBox.Show("Выбранный детальный продукт удален из очереди.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите ДЕТАЛЬНЫЙ продукт в списке для удаления по объекту!", "Ошибка выбора", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // --- КНОПКИ ЗАПРОСОВ LINQ ---

        // LINQ-запрос фильтрации: отображает только высокобелковые продукты (>15г) из обеих коллекций
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            var filtered = FoodProduct.BaseProductsList
                .Concat(DetailedFoodProduct.DetailedProductsQueue.Cast<FoodProduct>())
                .Where(p => p.Proteins > 15)
                .Select(p => p.GetInfo()).ToList();
            if (filtered.Count > 0)
            {
                LstProducts.ItemsSource = filtered;
                MessageBox.Show($"Найдено {filtered.Count} продуктов с содержанием белка > 15г.", "LINQ Фильтр", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Продукты с белком > 15г не обнаружены.", "LINQ Фильтр", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // LINQ-запрос сортировки: объединяет коллекции и сортирует их по убыванию
        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            var sorted = FoodProduct.BaseProductsList
                .Concat(DetailedFoodProduct.DetailedProductsQueue.Cast<FoodProduct>())
                .OrderByDescending(p => p.GetQuality())
                .Select(p => p.GetInfo()).ToList();
            if (sorted.Count > 0)
            {
                LstProducts.ItemsSource = sorted;
            }
            else
            {
                MessageBox.Show("Коллекции пусты. Сортировать нечего.", "LINQ Сортировка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Сброс результатов фильтрации/сортировки и отображение полного списка данных
        private void BtnReset_Click(object sender, RoutedEventArgs e) => UpdateList();
    }
}
