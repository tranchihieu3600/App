using App.DAO;
using App.DTO;
using App.Utils; // <- Namespace của các lớp tiện ích
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Menu = App.DTO.Menu;

// CÁC USING LIÊN QUAN ĐẾN VOICE/API ĐÃ ĐƯỢC CHUYỂN SANG fChatPopup.cs

namespace App
{
    public partial class fTableManager : Form
    {
        // === CÁC BIẾN CỦA FORM CHÍNH ===
        private List<Menu> currentBillItems = new List<Menu>();
        private Button selectedTableButton;
        private Account loginAccount;

        // === CÁC BIẾN SQLDEPENDENCY ===
        private string connectionString = DataProvider.Instance.connectionString;
        private SqlDependency depTable;
        private SqlDependency depBillInfo;

        // === CÁC BIẾN TỐI ƯU ẢNH ===
        private Image globalDefaultTableImage = null;
        private Image globalVipTableImage = null;

        // === CÁC BIẾN VOICE/CHAT ĐÃ BỊ XÓA (ĐÃ CHUYỂN SANG fChatPopup) ===
        // private VoiceHelper voiceHelper; 
        // private RasaClient rasaClient;   
        // private string openAiApiKey;     

        public Account LoginAccount
        {
            get => loginAccount;
            set
            {
                loginAccount = value;
                changeAccount(loginAccount.Type);
            }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;

            // Tải ảnh bàn 1 lần
            string defaultImagePath = Path.Combine(Application.StartupPath, "table.png");
            string vipImagePath = Path.Combine(Application.StartupPath, "tablevip.png");
            if (File.Exists(defaultImagePath))
                globalDefaultTableImage = Image.FromFile(defaultImagePath);
            if (File.Exists(vipImagePath))
                globalVipTableImage = Image.FromFile(vipImagePath);


            // Khởi động SqlDependency
            try
            {
                SqlDependency.Start(connectionString);
                Console.WriteLine(">>> SqlDependency started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> Lỗi khi Start SqlDependency: " + ex.Message);
            }

            // Load ban đầu và đăng ký dependencies
            LoadInitialData();
            SetupContextMenu();

            // === THAY ĐỔI: GẮN SỰ KIỆN MỞ CHAT CHO NÚT GHI ÂM CŨ ===
            // (Giả sử nút của bạn tên là btnVoice từ repo GitHub)
            if (this.btnOpenChat != null)
            {
                this.btnOpenChat.Text = "💬 Chat/Voice"; // Đổi chữ trên nút
                this.btnOpenChat.BackColor = System.Drawing.Color.DodgerBlue; // Đổi màu
                this.btnOpenChat.ForeColor = System.Drawing.Color.White;
                // Gỡ các sự kiện cũ (nếu có) và thêm sự kiện mới
                this.btnOpenChat.Click += new System.EventHandler(this.btnOpenChat_Click);
            }
            // === KẾT THÚC THAY ĐỔI ===

            // Dừng SqlDependency khi đóng form
            this.FormClosing += new FormClosingEventHandler(this.fTableManager_FormClosing);
        }

        #region Hàm Mới: Mở Popup Chat

        // HÀM MỚI ĐƯỢC THÊM VÀO: Mở form chat khi nhấn nút
        private void btnOpenChat_Click(object sender, EventArgs e)
        {
            // Tạo và hiển thị form chat
            // Chúng ta chỉ cần Show() nó. 
            // Khi chat trên popup, SqlDependency sẽ tự cập nhật fTableManager.
            fChatPopup chatForm = new fChatPopup();
            chatForm.Show(); // Dùng .Show() để nó không khóa fTableManager
        }

        #endregion

        #region SqlDependency Init / Register / Handlers (Giữ nguyên)

        private void LoadInitialData()
        {
            LoadTable();
            LoadCategoryList(0);
            RegisterTableDependency();
            RegisterBillInfoDependency();
        }

        private void RegisterTableDependency()
        {
            try
            {
                if (depTable != null)
                {
                    depTable.OnChange -= OnTableDataChanged;
                    depTable = null;
                }
                SqlConnection conn = new SqlConnection(connectionString);
                string sql = "SELECT idTable, tableName, status FROM dbo.TableFood";
                SqlCommand cmd = new SqlCommand(sql, conn);
                depTable = new SqlDependency(cmd);
                depTable.OnChange += OnTableDataChanged;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) { }
                Console.WriteLine(">>> Da dang ky theo doi TABLEFOOD thanh cong.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> LOI KHI DANG KY TABLEFOOD: " + ex.Message);
            }
        }

        private void RegisterBillInfoDependency()
        {
            try
            {
                if (depBillInfo != null)
                {
                    depBillInfo.OnChange -= OnBillInfoDataChanged;
                    depBillInfo = null;
                }
                SqlConnection conn = new SqlConnection(connectionString);
                string sql = "SELECT idBill, idFood, [count] FROM dbo.BillInfo";
                SqlCommand cmd = new SqlCommand(sql, conn);
                depBillInfo = new SqlDependency(cmd);
                depBillInfo.OnChange += OnBillInfoDataChanged;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) { }
                Console.WriteLine(">>> Da dang ky theo doi BILLINFO thanh cong.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> LOI KHI DANG KY BILLINFO: " + ex.Message);
            }
        }

        private void OnTableDataChanged(object sender, SqlNotificationEventArgs e)
        {
            if (e == null)
            {
                RegisterTableDependency();
                return;
            }
            if (e.Info == SqlNotificationInfo.Invalid)
            {
                try { RegisterTableDependency(); } catch { }
                return;
            }
            BeginInvoke(new MethodInvoker(() =>
            {
                Console.WriteLine(">>> NHAN DUOC THONG BAO: TABLEFOOD DA THAY DOI!");
                List<Table> updatedTableList = TableDAO.Instance.LoadTableList();
                UpdateTableControls(updatedTableList);
                if (lsvBill.Tag is Table t)
                {
                    Table currentTableLatestData = updatedTableList.FirstOrDefault(tbl => tbl.ID == t.ID);
                    if (currentTableLatestData != null)
                    {
                        lsvBill.Tag = currentTableLatestData;
                        ShowBill(currentTableLatestData.ID);
                    }
                    else
                    {
                        lsvBill.Items.Clear();
                        txbtotalPrice.Text = 0.ToString("c");
                        lsvBill.Tag = null;
                    }
                }
                RegisterTableDependency();
            }));
        }

        private void OnBillInfoDataChanged(object sender, SqlNotificationEventArgs e)
        {
            if (e == null)
            {
                RegisterBillInfoDependency();
                return;
            }
            if (e.Info == SqlNotificationInfo.Invalid)
            {
                try { RegisterBillInfoDependency(); } catch { }
                return;
            }
            BeginInvoke(new MethodInvoker(() =>
            {
                Console.WriteLine(">>> NHAN DUOC THONG BAO: BILLINFO DA THAY DOI!");
                List<Table> updatedTableList = TableDAO.Instance.LoadTableList();
                UpdateTableControls(updatedTableList);
                if (lsvBill.Tag is Table t)
                {
                    Table currentTableLatestData = updatedTableList.FirstOrDefault(tbl => tbl.ID == t.ID);
                    if (currentTableLatestData != null)
                    {
                        lsvBill.Tag = currentTableLatestData;
                        ShowBill(currentTableLatestData.ID);
                    }
                }
                RegisterBillInfoDependency();
            }));
        }

        private void fTableManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SqlDependency.Stop(connectionString);
                Console.WriteLine(">>> SqlDependency stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> Lỗi khi Stop SqlDependency: " + ex.Message);
            }
            globalDefaultTableImage?.Dispose();
            globalVipTableImage?.Dispose();
        }

        #endregion

        #region Các hàm Logic nghiệp vụ Form (Giữ nguyên)

        void changeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
        }

        private void fTableManager_Load(object sender, EventArgs e) { }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                table = TableDAO.Instance.GetTakeAwayTable();
                if (table == null)
                {
                    MessageBox.Show("Không tìm thấy bàn 'Mang Về'.");
                    return;
                }
                lsvBill.Tag = table;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int idFood = (cbFood.SelectedItem as Food).IdFood;
            int count = (int)nmFoodCount.Value;
            var notEnough = GetNotEnoughIngredients(idFood, count);
            if (notEnough.Count > 0)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Không đủ nguyên liệu:");
                foreach (var item in notEnough)
                {
                    msg.AppendLine($"• {item.IngredientName}: cần {item.Quantity} {item.Unit}");
                }
                MessageBox.Show(msg.ToString(), "Thiếu nguyên liệu");
                return;
            }

            if (idBill == -1)
            {
                string createdBy = fLogin.LoggedInUserName;
                BillDAO.Instance.InsertBill(table.ID, createdBy);
                idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            ShowBill(table.ID);
            UpdateSingleTable(table.ID);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) return;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            if (idBill != -1)
            {
                if (MessageBox.Show($"Thanh toán {table.Name}?", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    float totalPrice = 0;
                    foreach (ListViewItem item in lsvBill.Items)
                    {
                        if (item.SubItems.Count >= 4)
                        {
                            totalPrice += float.Parse(item.SubItems[3].Text, NumberStyles.Currency, CultureInfo.CurrentCulture);
                        }
                    }
                    if (table.Name.ToLower().Contains("vip"))
                    {
                        totalPrice += 20000;
                    }
                    txbtotalPrice.Text = totalPrice.ToString("c");
                    bool userWantsPrint = MessageBox.Show("Bạn có muốn in hóa đơn không?", "In hóa đơn", MessageBoxButtons.YesNo) == DialogResult.Yes;
                    if (userWantsPrint)
                    {
                        string filePath = InvoiceFileHelper.GetInvoiceFilePath(true);
                        string fullDisplayName = BillDAO.Instance.GetDisplayNameByBillID(idBill);
                        PDFExporter.ExportBillToPDF(lsvBill, table, fullDisplayName, txbtotalPrice.Text, filePath, true);
                    }
                    BillDAO.Instance.CheckOut(idBill);
                    UpdateSingleTable(table.ID);
                    ShowBill(table.ID);
                }
            }
            ShowLowStockWarning();
        }

        private Panel CreateTablePanel(Table item, Image defaultTableImage, Image vipTableImage)
        {
            Panel panel = new Panel
            {
                Width = 110 + 15,
                Height = 150,
                Margin = new Padding(10),
                Tag = item.ID
            };
            Button btn = new Button
            {
                Width = 110,
                Height = 110,
                FlatStyle = FlatStyle.Flat,
                Text = "",
                Tag = item,
                BackColor = Color.White,
                Location = new Point(0, 0)
            };
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, btn.Width, btn.Height);
            btn.Region = new Region(path);
            btn.Paint += (s, e) =>
            {
                Table table = (s as Button).Tag as Table;
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color borderColor = table.Status == "Trống" ? Color.DodgerBlue : Color.Red;
                if (s == selectedTableButton)
                    borderColor = Color.Green;
                using (Pen pen = new Pen(borderColor, 5))
                {
                    g.DrawEllipse(pen, 1, 1, btn.Width - 3, btn.Height - 3);
                }
                Image tableImage = item.Name.ToLower().Contains("vip") ? vipTableImage : defaultTableImage;
                if (tableImage != null)
                {
                    int imgSize = 80;
                    int imgX = (btn.Width - imgSize) / 2;
                    int imgY = (btn.Height - imgSize) / 2;
                    g.DrawImage(tableImage, new Rectangle(imgX, imgY, imgSize, imgSize));
                }
            };
            btn.Click += Btn_Click;
            btn.Tag = item;
            Label lblTableName = new Label
            {
                Text = item.Name,
                Width = 110,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(0, btn.Height + 5)
            };
            panel.Controls.Add(btn);
            panel.Controls.Add(lblTableName);
            return panel;
        }

        private void LoadTable()
        {
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            int? selectedTableId = (selectedTableButton?.Tag as Table)?.ID;
            flowLayoutNormalTables.Controls.Clear();
            flowLayoutVipTables.Controls.Clear();
            foreach (Table item in tableList)
            {
                Panel panel = CreateTablePanel(item, globalDefaultTableImage, globalVipTableImage);
                if (item.Name.ToLower().Contains("vip"))
                    flowLayoutVipTables.Controls.Add(panel);
                else
                    flowLayoutNormalTables.Controls.Add(panel);
                if (selectedTableId.HasValue && selectedTableId.Value == item.ID)
                {
                    selectedTableButton = panel.Controls.OfType<Button>().First();
                    selectedTableButton.BackColor = Color.LightGreen;
                    selectedTableButton.Invalidate();
                }
            }
        }

        private void UpdateTableControls(List<Table> updatedTableList)
        {
            var allTablePanels = flowLayoutNormalTables.Controls.Cast<Panel>()
                                .Concat(flowLayoutVipTables.Controls.Cast<Panel>()).ToList();
            Dictionary<int, Panel> existingPanels = new Dictionary<int, Panel>();
            Dictionary<int, Button> existingButtons = new Dictionary<int, Button>();
            Dictionary<int, Table> existingTables = new Dictionary<int, Table>();
            foreach (Panel panel in allTablePanels)
            {
                Button btn = panel.Controls.OfType<Button>().FirstOrDefault();
                if (btn != null && btn.Tag is Table table)
                {
                    if (!existingPanels.ContainsKey(table.ID))
                    {
                        existingPanels.Add(table.ID, panel);
                        existingButtons.Add(table.ID, btn);
                        existingTables.Add(table.ID, table);
                    }
                }
            }
            foreach (var pair in existingTables)
            {
                int tableId = pair.Key;
                Table oldTable = pair.Value;
                Table newTableData = updatedTableList.FirstOrDefault(t => t.ID == tableId);
                if (newTableData != null)
                {
                    if (oldTable.Status != newTableData.Status || oldTable.Name != newTableData.Name)
                    {
                        Button btn = existingButtons[tableId];
                        btn.Tag = newTableData;
                        Label lbl = existingPanels[tableId].Controls.OfType<Label>().FirstOrDefault();
                        if (lbl != null && lbl.Text != newTableData.Name)
                        {
                            lbl.Text = newTableData.Name;
                        }
                        btn.Invalidate();
                    }
                }
                else
                {
                    Panel panelToRemove = existingPanels[tableId];
                    if (panelToRemove.Parent != null)
                    {
                        panelToRemove.Parent.Controls.Remove(panelToRemove);
                    }
                    panelToRemove.Dispose();
                }
            }
            foreach (Table newTable in updatedTableList)
            {
                if (!existingTables.ContainsKey(newTable.ID))
                {
                    Panel newPanel = CreateTablePanel(newTable, globalDefaultTableImage, globalVipTableImage);
                    if (newTable.Name.ToLower().Contains("vip"))
                        flowLayoutVipTables.Controls.Add(newPanel);
                    else
                        flowLayoutNormalTables.Controls.Add(newPanel);
                }
            }
        }

        private void UpdateSingleTable(int tableID)
        {
            foreach (Control panel in flowLayoutNormalTables.Controls.Cast<Control>().Concat(flowLayoutVipTables.Controls.Cast<Control>()))
            {
                if (panel is Panel && (int)panel.Tag == tableID)
                {
                    Button btn = panel.Controls.OfType<Button>().FirstOrDefault();
                    if (btn != null)
                    {
                        Table updatedTable = TableDAO.Instance.GetTableByID(tableID);
                        if (updatedTable != null)
                        {
                            btn.Tag = updatedTable;
                            btn.Invalidate();
                        }
                        break;
                    }
                }
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            float totalPrice = 0;
            CultureInfo culture = new CultureInfo("vi-VN");
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            currentBillItems = MenuDAO.Instance.GetListMenuByTable(id);
            foreach (Menu item in listBillInfo)
            {
                totalPrice += item.TotalPrice;
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("c", culture));
                lsvItem.SubItems.Add(item.TotalPrice.ToString("c", culture));
                lsvItem.SubItems.Add(item.IdBill.ToString());
                lsvBill.Items.Add(lsvItem);
            }
            Table table = TableDAO.Instance.GetTableByID(id);
            if (table != null && table.Name.ToLower().Contains("vip"))
            {
                totalPrice += 20000;
            }
            txbtotalPrice.Text = totalPrice.ToString("c", culture);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int tableID = (clickedButton.Tag as Table).ID;
            lsvBill.Tag = clickedButton.Tag;
            if (selectedTableButton != null)
            {
                selectedTableButton.BackColor = Color.White;
                selectedTableButton.Invalidate();
            }
            selectedTableButton = clickedButton;
            selectedTableButton.BackColor = Color.LightGreen;
            selectedTableButton.Invalidate();
            ShowBill(tableID);
        }

        void LoadCategoryList(int id)
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "CategoryName";
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "foodName";
            cbFood.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        #endregion

        #region Các hàm Menu Item Handlers (Giữ nguyên)

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfle f = new fAccountProfle(loginAccount);
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void CbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null) return;
            Category selectedCategory = cb.SelectedItem as Category;
            if (selectedCategory != null)
            {
                id = selectedCategory.IdCategory;
            }
            LoadFoodListByCategoryID(id);
        }

        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) return;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            if (idBill == -1)
            {
                MessageBox.Show("Chưa có hóa đơn để in!");
                return;
            }
            string filePath = InvoiceFileHelper.GetInvoiceFilePath(false);
            PDFExporter.ExportBillToPDF(lsvBill, table, fLogin.LoggedInUserName, txbtotalPrice.Text, filePath, false);
            MessageBox.Show("Đã tạo hóa đơn tạm.");
        }

        private void updatethongtincanhan_Click(object sender, EventArgs e)
        {
            Staff staff = StaffDAO.Instance.GetStaffByAccount(LoginAccount.Username);
            if (staff != null)
            {
                Infomation f = new Infomation(LoginAccount.Username);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tài khoản này chưa được gán với nhân viên nào!");
            }
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void nhậpNguyênLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fImport f = new fImport();
            f.ShowDialog();
        }

        private List<FoodIngredient> GetNotEnoughIngredients(int foodId, int count)
        {
            var requiredIngredients = FoodIngredientDAO.Instance.GetIngredientsByFoodId(foodId);
            List<FoodIngredient> notEnough = new List<FoodIngredient>();
            foreach (var usage in requiredIngredients)
            {
                Ingredient ingredient = IngredientDAO.Instance.GetIngredientByID(usage.IdIngredient);
                decimal requiredAmount = usage.Quantity * count;
                if (ingredient == null || ingredient.Quantity < requiredAmount)
                {
                    var clone = new FoodIngredient
                    {
                        IdFood = usage.IdFood,
                        IdIngredient = usage.IdIngredient,
                        IngredientName = usage.IngredientName,
                        Unit = usage.Unit,
                        Quantity = requiredAmount
                    };
                    notEnough.Add(clone);
                }
            }
            return notEnough;
        }

        public static void ShowLowStockWarning()
        {
            var lowStockList = IngredientDAO.Instance.GetLowStockIngredients();
            if (lowStockList.Count == 0) return;
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Các nguyên liệu sắp hết kho:\n");
            foreach (var item in lowStockList)
            {
                messageBuilder.AppendLine($"• {item.IngredientName,-20} : {item.Quantity,8:0.###} {item.Unit}");
            }
            MessageBox.Show(messageBuilder.ToString(), "Cảnh báo nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (lsvBill.SelectedItems.Count > 0)
            {
                string foodName = lsvBill.SelectedItems[0].Text;
                Table table = lsvBill.Tag as Table;
                if (table == null) return;
                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int idFood = FoodDAO.Instance.GetFoodIdByName(foodName);
                if (MessageBox.Show($"Bạn có chắc muốn xóa món '{foodName}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BillInfoDAO.Instance.DeleteFoodFromBill(idBill, idFood);
                    ShowBill(table.ID);
                }
            }
        }

        private void SetupContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Xóa món");
            deleteItem.Click += DeleteItem_Click;
            contextMenu.Items.Add(deleteItem);
            lsvBill.ContextMenuStrip = contextMenu;
        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvBill.SelectedItems.Count == 0)
                return;
            ListViewItem selectedItem = lsvBill.SelectedItems[0];
            string selectedFoodName = selectedItem.SubItems[0].Text;
            int selectedCount = int.Parse(selectedItem.SubItems[1].Text);
            nmFoodCount.Value = selectedCount;
            int foodId = FoodDAO.Instance.GetFoodIdByName(selectedFoodName);
            if (foodId == -1)
                return;
            Food food = FoodDAO.Instance.GetFoodById(foodId);
            if (food == null)
                return;
            cbFood.SelectedItem = cbFood.Items.Cast<Food>().FirstOrDefault(f => f.IdFood == food.IdFood);
            cbCategory.SelectedItem = cbCategory.Items.Cast<Category>().FirstOrDefault(c => c.IdCategory == food.IdCategory);
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fMenu f = new fMenu(); // Khởi tạo form Menu
            f.Show();        // Hiển thị form lên
        }

        #endregion

        // === CÁC HÀM XỬ LÝ VOICE/CHAT ĐÃ BỊ XÓA (ĐÃ CHUYỂN SANG fChatPopup) ===
        // btnVoice_Click
        // OnDataAvailable
        // OnRecordingStopped
        // ConvertSpeechToText_OpenAI
        // HandleRasaResponse

    } // <--- Đóng class fTableManager
} // <--- Đóng namespace App