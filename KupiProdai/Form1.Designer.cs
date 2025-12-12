namespace KupiProdai
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl1;
        private TabPage tabProducts;
        private TabPage tabMovements;
        private Label label1, label2, label3, label4, label5, label6;
        private TextBox txtProductName, txtProductCode, txtPrice, txtQuantity;
        private ComboBox cmbCategory, cmbUnit;
        private Button btnAddProduct, btnUpdateProduct, btnDeleteProduct;
        private DataGridView dgvProducts;
        private TextBox txtSearch;
        private Label lblProductCount;
        private Button btnExport;
        private Button btnGenerateReport;

        // Movements tab
        private Label label7, label8, label9, label10, label11, label12;
        private TextBox txtMovementProduct, txtMovementQty, txtMovementPrice;
        private ComboBox cmbMovementType, cmbWarehouse;
        private DateTimePicker dtpMovementDate;
        private Button btnAddMovement;
        private DataGridView dgvMovements;
        private Label lblMovementCount;
        private Label lblSummary;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            this.Text = "Купи-Продай - Учет товаров";
            this.Size = new Size(1100, 700);  // Увеличили ширину
            this.StartPosition = FormStartPosition.CenterScreen;

            // TabControl
            tabControl1 = new TabControl();
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 10F);

            // =========== ВКЛАДКА ТОВАРОВ ===========
            tabProducts = new TabPage();
            tabProducts.Text = "Товары";
            tabProducts.Padding = new Padding(10);

            // Панель поиска
            Panel searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Height = 50;
            searchPanel.BackColor = Color.LightGray;

            txtSearch = new TextBox();
            txtSearch.Location = new Point(10, 10);
            txtSearch.Size = new Size(300, 25);
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.PlaceholderText = "Поиск товаров...";
            txtSearch.TextChanged += txtSearch_TextChanged;

            searchPanel.Controls.Add(txtSearch);

            // Панель формы товара - УВЕЛИЧИВАЕМ ВЫСОТУ
            Panel productFormPanel = new Panel();
            productFormPanel.Dock = DockStyle.Top;
            productFormPanel.Height = 220;  // Было 200
            productFormPanel.BorderStyle = BorderStyle.FixedSingle;
            productFormPanel.Padding = new Padding(10);

            // Поля формы
            int y = 10;
            int labelWidth = 120;
            int fieldWidth = 200;
            int spacing = 35;

            label1 = new Label { Text = "Наименование:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            txtProductName = new TextBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25) };
            y += spacing;

            label2 = new Label { Text = "Код:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            txtProductCode = new TextBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25) };
            y += spacing;

            label3 = new Label { Text = "Категория:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            cmbCategory = new ComboBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            label4 = new Label { Text = "Единица:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            cmbUnit = new ComboBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            label5 = new Label { Text = "Цена:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            txtPrice = new TextBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25) };
            y += spacing;

            label6 = new Label { Text = "Количество:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            txtQuantity = new TextBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25) };

            // Кнопки - сдвигаем правее
            int buttonX = 500;  // Было 450
            btnAddProduct = new Button { Text = "Добавить", Location = new Point(buttonX, 10), Size = new Size(100, 30) };
            btnUpdateProduct = new Button { Text = "Обновить", Location = new Point(buttonX, 50), Size = new Size(100, 30) };
            btnDeleteProduct = new Button { Text = "Удалить", Location = new Point(buttonX, 90), Size = new Size(100, 30) };
            btnExport = new Button { Text = "Экспорт", Location = new Point(buttonX, 130), Size = new Size(100, 30) };
            btnGenerateReport = new Button { Text = "Отчет", Location = new Point(buttonX, 170), Size = new Size(100, 30) };

            btnAddProduct.Click += btnAddProduct_Click;
            btnUpdateProduct.Click += btnUpdateProduct_Click;
            btnDeleteProduct.Click += btnDeleteProduct_Click;
            btnExport.Click += btnExport_Click;
            btnGenerateReport.Click += btnGenerateReport_Click;

            // Добавление контролов на панель
            productFormPanel.Controls.AddRange(new Control[] {
        label1, txtProductName, label2, txtProductCode,
        label3, cmbCategory, label4, cmbUnit,
        label5, txtPrice, label6, txtQuantity,
        btnAddProduct, btnUpdateProduct, btnDeleteProduct,
        btnExport, btnGenerateReport
    });

            // Таблица товаров
            dgvProducts = new DataGridView();
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;

            // Статус товаров
            lblProductCount = new Label();
            lblProductCount.Dock = DockStyle.Bottom;
            lblProductCount.Height = 25;
            lblProductCount.TextAlign = ContentAlignment.MiddleLeft;
            lblProductCount.BackColor = Color.LightBlue;
            lblProductCount.Padding = new Padding(5, 0, 0, 0);

            // Панель для таблицы с отступом
            Panel tablePanel = new Panel();
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.Padding = new Padding(0, 10, 0, 0);  // Отступ сверху
            tablePanel.Controls.Add(dgvProducts);
            tablePanel.Controls.Add(lblProductCount);

            // Компоновка вкладки товаров
            tabProducts.Controls.Add(tablePanel);
            tabProducts.Controls.Add(productFormPanel);
            tabProducts.Controls.Add(searchPanel);

            // =========== ВКЛАДКА ДВИЖЕНИЙ ===========
            tabMovements = new TabPage();
            tabMovements.Text = "Движение товаров";
            tabMovements.Padding = new Padding(10);

            // Панель формы движения - УВЕЛИЧИВАЕМ ШИРИНУ
            Panel movementFormPanel = new Panel();
            movementFormPanel.Dock = DockStyle.Top;
            movementFormPanel.Height = 160;  // Было 150
            movementFormPanel.BorderStyle = BorderStyle.FixedSingle;
            movementFormPanel.Padding = new Padding(10);

            y = 10;
            label7 = new Label { Text = "Товар:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            txtMovementProduct = new TextBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25) };
            y += spacing;

            label8 = new Label { Text = "Тип операции:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            cmbMovementType = new ComboBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            y += spacing;

            label9 = new Label { Text = "Склад:", Location = new Point(10, y), Size = new Size(labelWidth, 25) };
            cmbWarehouse = new ComboBox { Location = new Point(labelWidth + 20, y), Size = new Size(fieldWidth, 25), DropDownStyle = ComboBoxStyle.DropDownList };

            // Вторая колонка полей - ДВИГАЕМ ПРАВЕЕ
            int col2X = 450;  // Было 400
            y = 10;
            label10 = new Label { Text = "Количество:", Location = new Point(col2X, y), Size = new Size(labelWidth, 25) };
            txtMovementQty = new TextBox { Location = new Point(col2X + labelWidth + 10, y), Size = new Size(fieldWidth, 25) };
            y += spacing;

            label11 = new Label { Text = "Цена:", Location = new Point(col2X, y), Size = new Size(labelWidth, 25) };
            txtMovementPrice = new TextBox { Location = new Point(col2X + labelWidth + 10, y), Size = new Size(fieldWidth, 25) };
            y += spacing;

            label12 = new Label { Text = "Дата:", Location = new Point(col2X, y), Size = new Size(labelWidth, 25) };
            dtpMovementDate = new DateTimePicker { Location = new Point(col2X + labelWidth + 10, y), Size = new Size(fieldWidth, 25) };

            // Кнопка добавления движения - ЕЩЕ ПРАВЕЕ
            btnAddMovement = new Button
            {
                Text = "Добавить движение",
                Location = new Point(850, 10),  // Было 700
                Size = new Size(150, 30)
            };
            btnAddMovement.Click += btnAddMovement_Click;

            movementFormPanel.Controls.AddRange(new Control[] {
        label7, txtMovementProduct, label8, cmbMovementType,
        label9, cmbWarehouse, label10, txtMovementQty,
        label11, txtMovementPrice, label12, dtpMovementDate,
        btnAddMovement
    });

            // Таблица движений
            dgvMovements = new DataGridView();
            dgvMovements.Dock = DockStyle.Fill;
            dgvMovements.AllowUserToAddRows = false;
            dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Статус движений
            lblMovementCount = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.LightGreen,
                Padding = new Padding(5, 0, 0, 0)
            };

            lblSummary = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.LightYellow,
                Padding = new Padding(5, 0, 0, 0)
            };

            // Панель для таблицы с отступом
            Panel movementsTablePanel = new Panel();
            movementsTablePanel.Dock = DockStyle.Fill;
            movementsTablePanel.Padding = new Padding(0, 10, 0, 0);  // Отступ сверху

            // Панель статуса
            Panel statusPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            statusPanel.Controls.Add(lblMovementCount);
            statusPanel.Controls.Add(lblSummary);

            movementsTablePanel.Controls.Add(dgvMovements);
            movementsTablePanel.Controls.Add(statusPanel);

            // Компоновка вкладки движений
            tabMovements.Controls.Add(movementsTablePanel);
            tabMovements.Controls.Add(movementFormPanel);

            // Добавление вкладок
            tabControl1.TabPages.Add(tabProducts);
            tabControl1.TabPages.Add(tabMovements);

            this.Controls.Add(tabControl1);
        }
    }
}