````markdown
# 🍽️ Hệ Thống Quản Lý Quán Ăn (Restaurant Management System)

[![GitHub repo size](https://img.shields.io/github/repo-size/tranchihieu3600/App)](https://github.com/tranchihieu3600/App)
[![GitHub contributors](https://img.shields.io/github/contributors/tranchihieu3600/App)](https://github.com/tranchihieu3600/App/graphs/contributors)
[![GitHub stars](https://img.shields.io/github/stars/tranchihieu3600/App?style=social)](https://github.com/tranchihieu3600/App/stargazers)

Đây là một ứng dụng phần mềm được thiết kế để quản lý toàn diện các hoạt động của một quán ăn/nhà hàng. Hệ thống bao quát mọi quy trình một cách chặt chẽ: từ khâu nhập nguyên liệu, định lượng tạo nên món ăn, quản lý danh mục, cho đến quá trình phục vụ, xuất hóa đơn, phân quyền nhân sự, và đặc biệt là **tích hợp AI hỗ trợ đặt món thông minh**.

---

## ✨ Tính năng nổi bật

Phần mềm được chia thành các module chính nhằm tối ưu hóa quy trình vận hành của quán ăn:

### 1. 🤖 Tích hợp AI & Tương tác thông minh (Smart Ordering)
* **Rasa Order Bot:** Chatbot AI thông minh được xây dựng bằng Rasa, hỗ trợ tương tác tự động với khách hàng/nhân viên trong quá trình đặt món.
* **Xử lý Giọng nói & Văn bản (Voice/Text to Action):** Hệ thống có khả năng nhận diện giọng nói hoặc tin nhắn văn bản, phân tích ngữ nghĩa (NLP) và tự động chuyển đổi thành các thao tác (actions) thực tế như: *gọi món, thêm món, xóa/hủy món ăn khỏi order...* một cách mượt mà.

### 2. ⚡ Giao diện Thời gian thực (Real-time UI)
* Trạng thái bàn ăn, order mới, cập nhật món và thông tin doanh thu được đồng bộ hóa và hiển thị theo thời gian thực trên toàn hệ thống mà không cần tải lại trang/ứng dụng.

### 3. 📦 Quản lý Kho & Nguyên liệu
* **Nhà cung cấp:** Quản lý thông tin chi tiết của các đối tác, nhà cung cấp nguyên liệu cho quán.
* **Quản lý nguyên liệu:** Theo dõi danh sách nguyên liệu, số lượng, và tình trạng nhập/xuất kho.
* **Định lượng món ăn:** Tính năng đặc biệt cho phép liên kết chi tiết từng loại nguyên liệu cấu thành nên một món ăn hoàn chỉnh.

### 4. 🍳 Quản lý Thực đơn (Menu)
* **Danh mục món ăn:** Phân loại món ăn theo từng nhóm (Ví dụ: Món nướng, Lẩu, Đồ uống, Khai vị...).
* **Quản lý món ăn & Giá cả:** Thêm, sửa, xóa các món ăn trong thực đơn và quản lý giá bán chi tiết.

### 5. 🧾 Quản lý Giao dịch & Hóa đơn
* **Hóa đơn (Bill):** Khởi tạo và lưu trữ thông tin hóa đơn cho từng bàn/khách hàng.
* **Chi tiết hóa đơn (Bill Info):** Ghi nhận chi tiết từng món ăn, số lượng và thành tiền trong một hóa đơn cụ thể. Theo dõi trạng thái hóa đơn (đã thanh toán / chưa thanh toán).

### 6. 👥 Quản lý Nhân sự & Phân quyền
* **Quản lý Người dùng (User):** Lưu trữ thông tin của đội ngũ nhân sự làm việc tại quán.
* **Quản lý Tài khoản (Account):** Liên kết với người dùng để cấp quyền truy cập.
* **Phân quyền linh hoạt:** * `Quản lý (Manager):` Có toàn quyền truy cập hệ thống.
  * `Nhân viên (Staff):` Giới hạn quyền hạn, chủ yếu thao tác nghiệp vụ bán hàng.

---

## 🛠️ Công nghệ sử dụng

*(Bạn có thể chỉnh sửa phần này cho phù hợp với dự án thực tế)*

* **Ngôn ngữ lập trình:** [Ví dụ: Python, C#, JavaScript...]
* **Cơ sở dữ liệu:** [Ví dụ: SQL Server, MySQL, MongoDB...]
* **AI & NLP:** Rasa Framework, [Tên thư viện nhận diện giọng nói, VD: SpeechRecognition, Google Speech API...]
* **Real-time:** [Ví dụ: WebSocket, SignalR, Socket.io...]
* **Framework / Giao diện:** [Ví dụ: React, Vue, .NET, Flask, FastAPI...]

---

## 🚀 Hướng dẫn cài đặt

Để chạy thử dự án trên máy cá nhân của bạn, vui lòng làm theo các bước sau:

1. **Clone repository về máy:**
   ```bash
   git clone [https://github.com/tranchihieu3600/App.git](https://github.com/tranchihieu3600/App.git)
````

2.  **Thiết lập Cơ sở dữ liệu:**
      * Import file CSDL đi kèm trong dự án vào hệ quản trị CSDL của bạn.
      * Cập nhật chuỗi kết nối (Connection String) trong file cấu hình.
3.  **Cài đặt môi trường cho Rasa (Bot AI):**
      * Cài đặt các thư viện Python cần thiết (`pip install -r requirements.txt`).
      * Train model Rasa: `rasa train`.
      * Khởi chạy Rasa Action Server và Rasa Server.
4.  **Mở dự án & Chạy (Build & Run):**
      * Mở dự án bằng IDE tương ứng và khởi chạy ứng dụng chính.

-----

## 🔒 Thông tin đăng nhập mặc định (Dành cho Test)

  * **Tài khoản Quản lý:** `admin` / Mật khẩu: `admin123`
  * **Tài khoản Nhân viên:** `staff` / Mật khẩu: `staff123`

-----

## 📞 Thông tin liên hệ

  * **Tác giả:** Trần Chí Hiếu
  * **GitHub:** [tranchihieu3600](https://www.google.com/search?q=https://github.com/tranchihieu3600)
  * **Email:** tranchihieu3600@gmail.com
  * **SĐT:** 0901036971

-----

*Cảm ơn bạn đã quan tâm đến dự án này. Mọi đóng góp (Pull Request) hoặc báo lỗi (Issues) đều được chào đón\!*
