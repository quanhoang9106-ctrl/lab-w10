# Smart Home Mini - Lab 10

Ứng dụng console C# minh họa Factory, Singleton và Facade theo bài thực hành 10.

## 1. Chạy chương trình

Yêu cầu .NET SDK 8. Mở terminal tại thư mục `lab w10`:

```powershell
dotnet run
```

Menu cho phép chọn chế độ Đi làm/Đi ngủ, điều khiển riêng thiết bị, tạo thiết bị theo loại người dùng nhập và thay đổi mức sáng ban đêm từ 0 đến 100.

Chạy kiểm thử:

```powershell
dotnet run -- --test
```

## 2. Vị trí và lợi ích của các pattern

- **Factory — `DeviceFactory.cs`:** `Create(type, name)` chọn và khởi tạo `SmartLight`, `SmartFan` hoặc `SmartDoor`, trả về `IDevice`. Client không gọi constructor của từng thiết bị. Loại được chuẩn hóa bằng `Trim()` và `ToLowerInvariant()`; loại không hỗ trợ gây `ArgumentException`.
- **Singleton — `HomeConfiguration.cs`:** lớp `sealed`, constructor `private` và `Lazy<HomeConfiguration>` bảo đảm khởi tạo một thể hiện dùng chung, an toàn luồng khi khởi tạo. Cấu hình chứa `HomeName` và `NightLightLevel`; mức sáng chỉ nhận giá trị 0–100. Việc sửa các thuộc tính cấu hình không được đồng bộ hóa cho truy cập nhiều luồng.
- **Facade — `SmartHomeFacade.cs`:** giữ các phụ thuộc `IDevice` cho đèn/quạt và `ILockable` cho cửa. `GoToWork()` tắt đèn, tắt quạt, khóa cửa. `GoToSleep()` tắt quạt, khóa cửa, bật đèn rồi đọc và in mức sáng từ cấu hình chung. Client kích hoạt mỗi chế độ bằng một lời gọi.

`IDevice.cs` chứa bật/tắt; `ILockable.cs` tách riêng khóa/mở khóa. `SmartDoor` thực hiện cả hai interface; đèn và quạt chỉ thực hiện `IDevice`. Mỗi thiết bị nhận tên qua constructor và in trạng thái khi thao tác.

## 3. Hạn chế của thiết kế

Singleton tạo phụ thuộc vào trạng thái toàn cục, khiến các kiểm thử có thể ảnh hưởng lẫn nhau nếu không khôi phục cấu hình. Có thể cải tiến bằng cách truyền một abstraction cấu hình vào Facade và quản lý vòng đời qua Dependency Injection.

Facade hiện điều khiển ba thiết bị được truyền khi khởi tạo. Thiết bị tạo thêm từ menu được lưu để điều khiển riêng ở mục 3, nhưng không tự tham gia các chế độ của Facade. Chương trình chưa lưu dữ liệu sau khi thoát. Mức sáng ban đêm được đọc và thông báo theo yêu cầu đề; chưa mô phỏng thay đổi độ sáng vật lý của đèn.

## 4. Trả lời câu hỏi phần G

1. **Factory che giấu quyết định nào?** Che giấu việc chọn lớp cụ thể dựa trên loại thiết bị và cách khởi tạo lớp đó; client nhận `IDevice`.
2. **Singleton bảo đảm gì, có rủi ro nào?** Bảo đảm các lần lấy `HomeConfiguration.Instance` trả về cùng một đối tượng. Rủi ro là trạng thái chung có thể bị thay đổi từ nhiều nơi, tạo phụ thuộc ẩn và làm kiểm thử khó độc lập.
3. **Facade khác Factory thế nào?** Factory tập trung vào tạo đối tượng; Facade cung cấp thao tác mức cao để phối hợp các đối tượng đã có.
4. **Bỏ Facade thì client cần biết gì?** Client phải biết thiết bị nào cần bật/tắt/khóa, thứ tự gọi cho từng chế độ và cách đọc, thông báo mức sáng từ cấu hình.

## 5. Câu hỏi phân tích phần C và D

- Thêm `SmartCamera`: tạo lớp thực hiện `IDevice`, thêm nhánh `camera` trong Factory và cập nhật kiểm thử (T2 hiện giả định camera chưa hỗ trợ). Nếu đưa camera vào một chế độ thì bổ sung phụ thuộc và hành vi tương ứng của Facade.
- Constructor Singleton là `private` để client không tự tạo thể hiện mới.
- `sealed` ngăn kế thừa lớp cấu hình, giữ thiết kế Singleton rõ ràng.
- Singleton giữ trạng thái giữa các test và khó thay bằng cấu hình giả; các test phải khôi phục giá trị đã thay đổi.
- Với .NET DI và `AddSingleton`, không nhất thiết phải tự cài Singleton. Container có thể quản lý một thể hiện cho mỗi service provider, đồng thời cho phép truyền phụ thuộc và thay thế khi kiểm thử.

## 6. UML và kết quả kiểm thử

- Sơ đồ lớp: [ClassDiagram.png](ClassDiagram.png).
- Mã kiểm thử: `PatternTests.cs`.
- Kết quả chạy: [TestResults.txt](TestResults.txt), 24 kiểm tra đạt, 0 thất bại.

| Mã | Nội dung | Kết quả |
| --- | --- | --- |
| T1 | Factory tạo light, fan, door đúng kiểu | PASS |
| T2 | Loại camera không hỗ trợ gây ArgumentException | PASS |
| T3 | Hai lần lấy Instance cùng tham chiếu | PASS |
| T4 | Đi làm: đèn tắt, quạt tắt, cửa khóa, đúng thứ tự | PASS |
| T5 | Đi ngủ: quạt tắt, cửa khóa, đèn bật, in mức sáng cấu hình, đúng thứ tự | PASS |

Các kiểm tra bổ sung bao gồm loại/tên rỗng, chuẩn hóa loại trong culture Thổ Nhĩ Kỳ, biên mức sáng 0/100, từ chối -1/101, phụ thuộc null của Facade và trạng thái nguồn/khóa độc lập của cửa. Bộ kiểm thử khôi phục cấu hình và culture sau khi chạy; trả mã thoát 1 nếu có kiểm tra thất bại.
