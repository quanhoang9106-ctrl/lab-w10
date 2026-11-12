
# Smart Home Mini - Lab 10

Bài thực hành C# về 3 Design Pattern: Factory, Singleton và Facade.

## 1. Chạy chương trình

Sử dụng C# và .NET 8.

Chạy chương trình:
```bash
dotnet run
```

Chạy kiểm thử T1–T5:
```bash
dotnet run -- --test
```

## 2. Các Design Pattern sử dụng

### Factory
- File: `Patterns/DeviceFactory.cs`
- Tạo các thiết bị SmartLight, SmartFan, SmartDoor.
- Giúp chương trình không cần gọi trực tiếp constructor của từng lớp.

### Singleton
- File: `Patterns/HomeConfiguration.cs`
- Quản lý cấu hình chung của ngôi nhà.
- Đảm bảo chương trình dùng chung một đối tượng cấu hình.

### Facade
- File: `Patterns/SmartHomeFacade.cs`
- Cung cấp hai chế độ GoToWork() và GoToSleep().
- Giúp điều khiển nhiều thiết bị thông qua một lệnh.

## 3. Lợi ích

- Mã nguồn được chia thành các lớp riêng biệt.
- Dễ quản lý và sử dụng các thiết bị.
- Giảm sự phụ thuộc giữa các lớp.
- Có thể mở rộng thêm thiết bị.

## 4. Hạn chế

- Chưa kiểm tra giới hạn độ sáng từ 0 đến 100.
- Thiết bị mới tạo chưa được thêm vào hệ thống điều khiển chính.
- Chưa lưu trạng thái thiết bị khi thoát chương trình.

## 5. Trả lời câu hỏi

1. Factory che giấu việc lựa chọn lớp và khởi tạo đối tượng.
2. Singleton đảm bảo dùng chung một cấu hình, nhưng dữ liệu chung có thể bị thay đổi ngoài ý muốn.
3. Factory dùng để tạo đối tượng, Facade dùng để đơn giản hóa việc điều khiển nhiều đối tượng.
4. Nếu không có Facade, chương trình phải gọi từng phương thức của từng thiết bị.

## 6. UML và kiểm thử

- Sơ đồ lớp: `docs/ClassDiagram.md`
- Kiểm thử: `Tests/PatternTests.cs`
- Gồm 5 trường hợp kiểm thử T1–T5.
