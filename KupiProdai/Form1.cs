using KupiProdai.Data;
using KupiProdai.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KupiProdai
{
    public partial class Form1 : Form
    {
        private JsonRepository _repository;
        private List<Product> _products;
        private List<Supplier> _suppliers;
        private List<Warehouse> _warehouses;
        private List<StockMovement> _movements;

        public Form1()
        {
            InitializeComponent();
            _repository = new JsonRepository();
            LoadData();
            InitializeControls();
        }

        private void LoadData()
        {
            _products = _repository.GetProducts();
            _suppliers = _repository.GetSuppliers();
            _warehouses = _repository.GetWarehouses();
            _movements = _repository.GetStockMovements();
        }

        private void InitializeControls()
        {
            // Заполняем комбо-боксы
            cmbCategory.Items.AddRange(new string[] { "Товар", "Услуга" });
            cmbUnit.Items.AddRange(new string[] { "шт.", "кг", "л", "м", "упак." });
            cmbWarehouse.Items.AddRange(_warehouses.Select(w => w.Name).ToArray());
            cmbMovementType.Items.AddRange(new string[] { "Поступление", "Реализация" });

            // Загружаем данные в таблицы
            RefreshProductsGrid();
            RefreshMovementsGrid();
        }

        // =========== ТОВАРЫ ===========
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var product = new Product
                {
                    Name = txtProductName.Text,
                    Code = txtProductCode.Text,
                    Category = cmbCategory.SelectedItem?.ToString(),
                    Unit = cmbUnit.SelectedItem?.ToString(),
                    Price = decimal.TryParse(txtPrice.Text, out decimal price) ? price : 0,
                    Quantity = int.TryParse(txtQuantity.Text, out int qty) ? qty : 0
                };

                if (string.IsNullOrEmpty(product.Name))
                {
                    MessageBox.Show("Введите наименование товара", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _repository.AddProduct(product);
                LoadData();
                RefreshProductsGrid();
                ClearProductForm();
                MessageBox.Show("Товар успешно добавлен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is Product selectedProduct)
            {
                selectedProduct.Name = txtProductName.Text;
                selectedProduct.Code = txtProductCode.Text;
                selectedProduct.Category = cmbCategory.SelectedItem?.ToString();
                selectedProduct.Unit = cmbUnit.SelectedItem?.ToString();
                selectedProduct.Price = decimal.TryParse(txtPrice.Text, out decimal price) ? price : 0;
                selectedProduct.Quantity = int.TryParse(txtQuantity.Text, out int qty) ? qty : 0;

                _repository.UpdateProduct(selectedProduct);
                LoadData();
                RefreshProductsGrid();
                MessageBox.Show("Товар успешно обновлен!", "Успех");
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is Product selectedProduct)
            {
                var result = MessageBox.Show($"Удалить товар '{selectedProduct.Name}'?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _repository.DeleteProduct(selectedProduct.Id);
                    LoadData();
                    RefreshProductsGrid();
                    ClearProductForm();
                }
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is Product selectedProduct)
            {
                txtProductName.Text = selectedProduct.Name;
                txtProductCode.Text = selectedProduct.Code;
                cmbCategory.SelectedItem = selectedProduct.Category;
                cmbUnit.SelectedItem = selectedProduct.Unit;
                txtPrice.Text = selectedProduct.Price.ToString("0.00");
                txtQuantity.Text = selectedProduct.Quantity.ToString();
            }
        }

        private void RefreshProductsGrid()
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = _products;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lblProductCount.Text = $"Товаров: {_products.Count}";
        }

        private void ClearProductForm()
        {
            txtProductName.Clear();
            txtProductCode.Clear();
            cmbCategory.SelectedIndex = -1;
            cmbUnit.SelectedIndex = -1;
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        // =========== ДВИЖЕНИЯ ТОВАРОВ ===========
        private void btnAddMovement_Click(object sender, EventArgs e)
        {
            try
            {
                var movement = new StockMovement
                {
                    Date = dtpMovementDate.Value,
                    Type = cmbMovementType.SelectedItem?.ToString(),
                    ProductName = txtMovementProduct.Text,
                    Quantity = int.TryParse(txtMovementQty.Text, out int qty) ? qty : 0,
                    Price = decimal.TryParse(txtMovementPrice.Text, out decimal price) ? price : 0,
                    WarehouseName = cmbWarehouse.SelectedItem?.ToString()
                };

                movement.Total = movement.Quantity * movement.Price;

                if (string.IsNullOrEmpty(movement.ProductName))
                {
                    MessageBox.Show("Введите наименование товара", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _repository.AddStockMovement(movement);

                // Обновляем количество товара на складе
                UpdateProductQuantity(movement);

                LoadData();
                RefreshMovementsGrid();
                RefreshProductsGrid();
                ClearMovementForm();
                MessageBox.Show("Движение товара успешно добавлено!", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProductQuantity(StockMovement movement)
        {
            var product = _products.FirstOrDefault(p => p.Name == movement.ProductName);
            if (product != null)
            {
                if (movement.Type == "Поступление")
                    product.Quantity += movement.Quantity;
                else if (movement.Type == "Реализация")
                    product.Quantity -= movement.Quantity;

                _repository.UpdateProduct(product);
            }
        }

        private void RefreshMovementsGrid()
        {
            dgvMovements.DataSource = null;
            dgvMovements.DataSource = _movements;
            dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            lblMovementCount.Text = $"Операций: {_movements.Count}";

            // Рассчитываем итоги
            decimal totalIncome = _movements
                .Where(m => m.Type == "Поступление")
                .Sum(m => m.Total);
            decimal totalOutcome = _movements
                .Where(m => m.Type == "Реализация")
                .Sum(m => m.Total);
            lblSummary.Text = $"Итого: Поступление = {totalIncome:C2}, Реализация = {totalOutcome:C2}";
        }

        private void ClearMovementForm()
        {
            txtMovementProduct.Clear();
            txtMovementQty.Clear();
            txtMovementPrice.Clear();
            cmbMovementType.SelectedIndex = -1;
            cmbWarehouse.SelectedIndex = -1;
            dtpMovementDate.Value = DateTime.Now;
        }

        // =========== ПОИСК ===========
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            var filtered = _products
                .Where(p => p.Name.ToLower().Contains(searchText) ||
                           p.Code.ToLower().Contains(searchText) ||
                           p.Category.ToLower().Contains(searchText))
                .ToList();

            dgvProducts.DataSource = filtered;
            lblProductCount.Text = $"Найдено: {filtered.Count}";
        }

        // =========== ЭКСПОРТ ===========
        private void btnExport_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                FileName = $"товары_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(_products,
                    Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(saveDialog.FileName, json);
                MessageBox.Show("Данные успешно экспортированы!", "Успех");
            }
        }

        // =========== ОТЧЕТЫ ===========
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            var lowStockProducts = _products.Where(p => p.Quantity < 10).ToList();

            if (lowStockProducts.Count > 0)
            {
                string report = "=== ТОВАРЫ С НИЗКИМ ОСТАТКОМ ===\n\n";
                foreach (var product in lowStockProducts)
                {
                    report += $"• {product.Name} ({product.Code}): {product.Quantity} {product.Unit}\n";
                }

                MessageBox.Show(report, "Отчет по остаткам",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Нет товаров с низким остатком", "Отчет",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}