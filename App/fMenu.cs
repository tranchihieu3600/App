using App.DAO;
using App.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace App
{
    public partial class fMenu : Form
    {
        public fMenu()
        {
            InitializeComponent();
            LoadFormMenu();
        }

        // --------------------------------------------------------------------------
        // # I. CÁC HÀM XỬ LÝ DỮ LIỆU & LAYOUT
        // --------------------------------------------------------------------------

        void LoadFormMenu()
        {
            dtgvIngredient.ReadOnly = true;
            dtgvIngredient.AllowUserToAddRows = false;
            LoadDynamicMenu();
        }

        // Hàm giúp các control con tự điều chỉnh kích thước khi form resize
        void ResizeDynamicControls()
        {
            // Lấy chiều rộng hiện tại của FlowLayoutPanel để áp dụng cho controls con
            int currentWidth = flpMenuContainer.ClientSize.Width - 10;

            foreach (Control control in flpMenuContainer.Controls)
            {
                if (control is Label || control is DataGridView)
                {
                    control.Width = currentWidth;
                }
            }
        }

        void LoadIngredientDetail(int foodId)
        {
            DataTable data = FoodIngredientDAO.Instance.GetFoodIngredientDetail(foodId);
            dtgvIngredient.DataSource = data;

            // RẤT QUAN TRỌNG: Tắt chế độ Fill để có thể chỉnh sửa chiều rộng từng cột
            dtgvIngredient.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Ẩn các cột ID
            if (dtgvIngredient.Columns.Contains("idFood")) dtgvIngredient.Columns["idFood"].Visible = false;
            if (dtgvIngredient.Columns.Contains("idIngredient")) dtgvIngredient.Columns["idIngredient"].Visible = false;

            // --- CẤU HÌNH CÁC CỘT HIỂN THỊ VÀ VỊ TRÍ (DisplayIndex) ---

            // 1. Tên Nguyên liệu (Vị trí 0)
            if (dtgvIngredient.Columns.Contains("ingredientName"))
            {
                dtgvIngredient.Columns["ingredientName"].HeaderText = "Tên Nguyên liệu";
                dtgvIngredient.Columns["ingredientName"].Width = 150;
                dtgvIngredient.Columns["ingredientName"].Visible = true;
                dtgvIngredient.Columns["ingredientName"].DisplayIndex = 0; // Vị trí đầu tiên
            }

            // 2. ĐỊNH LƯỢNG (Vị trí 1)
            if (dtgvIngredient.Columns.Contains("quantity"))
            {
                dtgvIngredient.Columns["quantity"].HeaderText = "Định lượng";
                dtgvIngredient.Columns["quantity"].Width = 90; // Đặt chiều rộng cố định
                dtgvIngredient.Columns["quantity"].Visible = true;
                dtgvIngredient.Columns["quantity"].DisplayIndex = 1; // Vị trí thứ hai
            }

            // 3. Đơn vị (FIX: Đặt Vị trí 2, ra sau Định lượng)
            if (dtgvIngredient.Columns.Contains("unit"))
            {
                dtgvIngredient.Columns["unit"].HeaderText = "Đơn vị";
                dtgvIngredient.Columns["unit"].Width = 65;
                dtgvIngredient.Columns["unit"].Visible = true;
                dtgvIngredient.Columns["unit"].DisplayIndex = 2; // Vị trí thứ ba
            }
        }

        // --- HÀM CHÍNH: LOAD MENU ĐỘNG ---
        void LoadDynamicMenu()
        {
            flpMenuContainer.SuspendLayout();
            flpMenuContainer.Controls.Clear();

            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();

            bool firstFoodFound = false;
            DataGridView firstDtgv = null;

            // Lấy chiều rộng ban đầu (sẽ được cập nhật khi resize)
            int panelWidth = flpMenuContainer.ClientSize.Width - 10;

            foreach (Category cat in listCategory)
            {
                // 1. TẠO HEADER DANH MỤC (Label)
                Label lblHeader = new Label
                {
                    Text = cat.CategoryName.ToUpper(),
                    Font = new Font(this.Font, FontStyle.Bold),
                    AutoSize = true,
                    ForeColor = Color.DarkBlue,
                    Margin = new Padding(3, 15, 3, 5),
                    Width = panelWidth
                };
                flpMenuContainer.Controls.Add(lblHeader);

                // 2. TẠO DATAGRIDVIEW CHO MÓN ĂN THUỘC DANH MỤC NÀY
                DataGridView dtgvCategoryFood = new DataGridView
                {
                    Name = "dtgv_" + cat.IdCategory,
                    DataSource = FoodDAO.Instance.GetListFoodByCategoryID(cat.IdCategory),
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToResizeRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    ColumnHeadersVisible = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    Width = panelWidth
                };

                // 3. ĐIỀU CHỈNH CỘT VÀ KÍCH THƯỚC
                dtgvCategoryFood.DataBindingComplete += (s, e) =>
                {
                    // Logic điều chỉnh cột (giữ nguyên)
                    if (dtgvCategoryFood.Columns["Price"] != null)
                    {
                        dtgvCategoryFood.Columns["Price"].DefaultCellStyle.Format = "c0";
                        dtgvCategoryFood.Columns["Price"].DefaultCellStyle.FormatProvider = new CultureInfo("vi-VN");
                    }

                    // Tính lại chiều cao dựa trên số hàng
                    if (dtgvCategoryFood.Rows.Count > 0)
                    {
                        dtgvCategoryFood.Height = dtgvCategoryFood.Rows.Count * dtgvCategoryFood.RowTemplate.Height + dtgvCategoryFood.ColumnHeadersHeight + 5;
                    }
                    else
                    {
                        dtgvCategoryFood.Height = 30; // Chiều cao tối thiểu nếu rỗng
                    }
                };

                // 4. KẾT NỐI SỰ KIỆN SELECTIONCHANGED
                dtgvCategoryFood.SelectionChanged += DynamicDtgv_SelectionChanged;

                flpMenuContainer.Controls.Add(dtgvCategoryFood);

                // GHI NHẬN DATAGRIDVIEW ĐẦU TIÊN CÓ MÓN ĂN
                if (!firstFoodFound && dtgvCategoryFood.Rows.Count > 0)
                {
                    firstFoodFound = true;
                    firstDtgv = dtgvCategoryFood;
                }
            }

            flpMenuContainer.ResumeLayout();

            // KÍCH HOẠT CHỌN DÒNG ĐẦU TIÊN (AUTOLOAD INGREDIENT)
            if (firstDtgv != null)
            {
                firstDtgv.Rows[0].Selected = true;
                firstDtgv.CurrentCell = firstDtgv.Rows[0].Cells[0];
            }
        }

        // --------------------------------------------------------------------------
        // # II. XỬ LÝ SỰ KIỆN
        // --------------------------------------------------------------------------

        // KẾT NỐI VỚI Design File: flpMenuContainer.Resize
        private void flpMenuContainer_Resize(object sender, EventArgs e)
        {
            ResizeDynamicControls();
        }

        private void DynamicDtgv_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView clickedDtgv = sender as DataGridView;

            if (clickedDtgv.CurrentRow == null || clickedDtgv.CurrentRow.Index == -1 || clickedDtgv.Rows.Count == 0) return;

            try
            {
                DataGridViewRow selectedRow = clickedDtgv.CurrentRow;

                // Lấy ID món ăn từ hàng được chọn
                object idObject = selectedRow.Cells["IdFood"].Value;

                if (idObject != null && int.TryParse(idObject.ToString(), out int foodId))
                {
                    LoadIngredientDetail(foodId);
                }
            }
            catch (Exception)
            {
                // Bỏ qua lỗi index hoặc lỗi dữ liệu (thường xảy ra khi grid đang reload)
            }
        }
    }
}