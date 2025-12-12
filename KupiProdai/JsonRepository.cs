using KupiProdai.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KupiProdai.Data
{
    public class JsonRepository
    {
        private string _dataFolder = "Data";

        public JsonRepository()
        {
            if (!Directory.Exists(_dataFolder))
                Directory.CreateDirectory(_dataFolder);
        }

        // Products
        public List<Product> GetProducts()
        {
            return Load<List<Product>>("products.json") ?? new List<Product>();
        }

        public void SaveProducts(List<Product> products)
        {
            Save("products.json", products);
        }

        public void AddProduct(Product product)
        {
            var products = GetProducts();
            product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            product.CreatedDate = DateTime.Now;
            products.Add(product);
            SaveProducts(products);
        }

        public void UpdateProduct(Product product)
        {
            var products = GetProducts();
            var index = products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
                products[index] = product;
            SaveProducts(products);
        }

        public void DeleteProduct(int id)
        {
            var products = GetProducts();
            products.RemoveAll(p => p.Id == id);
            SaveProducts(products);
        }

        // Suppliers
        public List<Supplier> GetSuppliers()
        {
            return Load<List<Supplier>>("suppliers.json") ?? new List<Supplier>();
        }

        public void SaveSuppliers(List<Supplier> suppliers)
        {
            Save("suppliers.json", suppliers);
        }

        public void AddSupplier(Supplier supplier)
        {
            var suppliers = GetSuppliers();
            supplier.Id = suppliers.Count > 0 ? suppliers.Max(s => s.Id) + 1 : 1;
            suppliers.Add(supplier);
            SaveSuppliers(suppliers);
        }

        // Stock Movements
        public List<StockMovement> GetStockMovements()
        {
            return Load<List<StockMovement>>("movements.json") ?? new List<StockMovement>();
        }

        public void AddStockMovement(StockMovement movement)
        {
            var movements = GetStockMovements();
            movement.Id = movements.Count > 0 ? movements.Max(m => m.Id) + 1 : 1;
            movements.Add(movement);
            Save("movements.json", movements);
        }

        // Warehouses
        public List<Warehouse> GetWarehouses()
        {
            return Load<List<Warehouse>>("warehouses.json") ?? new List<Warehouse>();
        }

        public void SaveWarehouses(List<Warehouse> warehouses)
        {
            Save("warehouses.json", warehouses);
        }

        // Helper methods
        private T Load<T>(string fileName) where T : class
        {
            var filePath = Path.Combine(_dataFolder, fileName);
            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private void Save<T>(string fileName, T data)
        {
            var filePath = Path.Combine(_dataFolder, fileName);
            // ЯВНО указываем Newtonsoft.Json.Formatting
            var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}