# 🍽️ Restaurant Management System (Hệ Thống Quản Lý Quán Ăn)

[![GitHub repo size](https://img.shields.io/github/repo-size/tranchihieu3600/App)](https://github.com/tranchihieu3600/App)
[![GitHub contributors](https://img.shields.io/github/contributors/tranchihieu3600/App)](https://github.com/tranchihieu3600/App/graphs/contributors)

Dự án phần mềm quản lý nhà hàng/quán ăn toàn diện. Hệ thống bao quát toàn bộ quy trình vận hành thực tế: từ quản lý kho, định lượng nguyên liệu, menu, bán hàng, cho đến việc tích hợp AI xử lý ngôn ngữ tự nhiên để hỗ trợ gọi món và đồng bộ dữ liệu theo thời gian thực.

---

## ✨ Tính năng nổi bật

### 🤖 1. Tích hợp AI & Tương tác thông minh (Smart Ordering)
* **Rasa Order Bot:** Chatbot AI được huấn luyện bằng Rasa để tự động hóa giao tiếp.
* **Voice/Text to Action:** Nhận diện và xử lý ngôn ngữ tự nhiên (NLP) từ giọng nói hoặc văn bản, tự động chuyển thành thao tác: *gọi món, thêm/xóa món, cập nhật order*.

### ⚡ 2. Giao diện Thời gian thực (Real-time)
* Mọi thay đổi về trạng thái bàn, order mới, hoặc cập nhật món ăn đều được đồng bộ hóa tức thì trên toàn hệ thống mà không cần tải lại trang.

### 📦 3. Quản lý Kho & Định lượng nguyên liệu
* **Nhà cung cấp:** Quản lý thông tin đối tác nhập hàng.
* **Kiểm kho:** Theo dõi lượng nguyên liệu tồn kho, nhập/xuất.
* **Định lượng món ăn (Công thức):** Liên kết chi tiết từng loại nguyên liệu cấu thành nên một món ăn (Ví dụ: 1 bát phở cần bao nhiêu gram thịt, bánh phở...).

### 🍳 4. Quản lý Menu & Hóa đơn
* **Menu:** Quản lý danh mục, thêm/sửa/xóa món ăn và cập nhật giá.
* **Hóa đơn (Bill):** Tạo hóa đơn theo bàn, lưu trữ chi tiết món, số lượng, thành tiền và trạng thái thanh toán.

### 👥 5. Phân quyền Nhân sự
* **Quản lý (Manager):** Toàn quyền hệ thống (Kho, Menu, Doanh thu, Nhân sự).
* **Nhân viên (Staff):** Quyền hạn giới hạn ở nghiệp vụ bán hàng, tạo order và thanh toán.

---

## 🛠️ Công nghệ sử dụng

| Thành phần | Công nghệ / Framework |
| :--- | :--- |
| **Ngôn ngữ chính** | *(C#, Python)* |
| **Cơ sở dữ liệu** | *(SQL Server)* |
| **Trí tuệ nhân tạo (AI)** | Rasa Framework, Voice/Text Recognition |
| **Real-time** | *(Lắng nghe thay đổi csdl)* |
| **Giao diện (Frontend)** | *(WinForms)* |

---

## 🚀 Hướng dẫn chạy dự án

**1. Clone mã nguồn về máy:**
```bash
git clone [https://github.com/tranchihieu3600/App.git](https://github.com/tranchihieu3600/App.git)
