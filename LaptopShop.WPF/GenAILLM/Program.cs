using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenAILLM
{
    /// <summary>
    /// Chương trình Trợ lý AI tích hợp dành cho dự án LaptopShop.
    /// Sử dụng Local LLM (PhoGPT) qua LM Studio để hỗ trợ quy trình phát triển.
    /// </summary>
    internal static class Program
    {
        // Địa chỉ Local API Server của LM Studio
        private static readonly string ApiUrl = "http://127.0.0.1:1234/v1/chat/completions";

        // Tối ưu hóa: Khởi tạo HttpClient 1 lần duy nhất (Best Practice)
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };

        static async Task Main(string[] args)
        {
            // Thiết lập Console hỗ trợ tiếng Việt có dấu
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                ShowHeader();

                // 1. CHỌN TÊN FILE ĐỂ XỬ LÝ (Giao diện mới nổi bật hơn)
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== BƯỚC 1: CHỌN FILE ĐỂ AI PHÂN TÍCH ===");
                Console.ResetColor();
                Console.Write("Nhập chính xác tên file (VD: ProductService.cs, CartPage.xaml.cs)\nHoặc chỉ cần nhấn [Enter] để dùng mặc định 'OrderService.cs': ");

                string inputFileName = Console.ReadLine()?.Trim();
                string targetFile = string.IsNullOrEmpty(inputFileName) ? "OrderService.cs" : inputFileName;

                // Tự động tìm thư mục gốc của Solution (LaptopShop.WPF)
                string slnRoot = FindSolutionRoot();

                PrintColor($"\n[*] Đang tìm kiếm file '{targetFile}' trong toàn bộ Solution...", ConsoleColor.DarkGray);

                // THUẬT TOÁN TÌM KIẾM THÔNG MINH: Quét toàn bộ các thư mục con trong Solution
                string[] allFoundPaths = Directory.GetFiles(slnRoot, targetFile, SearchOption.AllDirectories);

                string sourcePath = string.Empty;

                // Lọc bỏ các file rác sinh ra trong quá trình build (nằm trong thư mục bin, obj)
                foreach (var path in allFoundPaths)
                {
                    if (!path.Contains(@"\bin\") && !path.Contains(@"\obj\"))
                    {
                        sourcePath = path;
                        break;
                    }
                }

                // Nếu quét mọi ngóc ngách mà vẫn không thấy file
                if (string.IsNullOrEmpty(sourcePath))
                {
                    PrintColor($"\n[LỖI] Không tìm thấy bất kỳ file nào có tên '{targetFile}' trong toàn bộ hệ thống!", ConsoleColor.Red);
                    PrintColor($"-> Mẹo: Hãy kiểm tra lại đúng tên file bên Visual Studio (bao gồm cả chữ hoa/chữ thường).", ConsoleColor.Yellow);

                    Console.WriteLine("\nNhấn phím bất kỳ để thử lại, hoặc đóng cửa sổ này để thoát hoàn toàn...");
                    Console.ReadKey();
                    return; // Thoát vòng lặp hiện tại để chạy lại
                }

                // 2. MENU CHỨC NĂNG DEMO
                Console.WriteLine($"\n[ĐÃ TÌM THẤY FILE CHÍNH XÁC TẠI]:");
                Console.WriteLine(Path.GetFullPath(sourcePath));
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("DANH SÁCH TÁC VỤ AI HỖ TRỢ:");
                Console.WriteLine("  [1] Tự động tạo Unit Test (xUnit)");
                Console.WriteLine("  [2] Viết tài liệu XML Documentation (Clean Code)");
                Console.WriteLine("  [3] Phân tích & Giải trình Logic (Technical Report)");
                Console.WriteLine("  [0] Thoát chương trình");
                Console.Write("\nNhập lựa chọn của bạn (0-3): ");

                string choice = Console.ReadLine() ?? string.Empty;
                if (choice == "0")
                {
                    PrintColor("Đang thoát chương trình. Hẹn gặp lại!", ConsoleColor.Green);
                    break;
                }

                // 3. TẠO CÂU LỆNH (PROMPT) GỬI CHO AI
                string sourceCode = await File.ReadAllTextAsync(sourcePath);
                string prompt = ConstructPrompt(choice, sourceCode);

                if (string.IsNullOrWhiteSpace(prompt))
                {
                    PrintColor("\nLựa chọn không hợp lệ!", ConsoleColor.Red);
                    await Task.Delay(1000);
                    continue;
                }

                // 4. GỌI API ĐẾN LM STUDIO
                await ExecuteAiTask(choice, prompt, targetFile);

                Console.WriteLine("\nNhấn phím bất kỳ để quay lại Menu chính...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hàm tự động lùi thư mục để tìm thư mục gốc chứa file Solution (.sln).
        /// Giúp code chạy ổn định dù bật bằng Terminal hay F5.
        /// </summary>
        private static string FindSolutionRoot()
        {
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (currentDir != null)
            {
                if (currentDir.GetFiles("*.sln").Length > 0)
                {
                    return currentDir.FullName;
                }
                currentDir = currentDir.Parent;
            }
            // Fallback nếu không thấy file sln
            return Path.GetFullPath("..");
        }

        private static void ShowHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"************************************************************");
            Console.WriteLine(@"* LAPTOPSHOP - GENERATIVE AI INTEGRATION TOOL              *");
            Console.WriteLine(@"* Demo: Ứng dụng Local LLM trong quy trình kiểm thử phần mềm *");
            Console.WriteLine(@"************************************************************");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static string ConstructPrompt(string choice, string code)
        {
            // Đã tối ưu hóa Prompt: Dễ hiểu, mạch lạc, trực diện để PhoGPT-4B không bị lỗi rác
            return choice switch
            {
                "1" => $"Hãy viết một file Unit Test hoàn chỉnh bằng thư viện xUnit và Moq cho class C# dưới đây. Trả về DUY NHẤT mã C#, tuyệt đối không giải thích.\n\nMã nguồn:\n```csharp\n{code}\n```",

                "2" => $"Hãy thêm đầy đủ chú thích XML (/// <summary>, /// <param>, /// <returns>) bằng tiếng Việt cho toàn bộ class C# dưới đây. Trả về DUY NHẤT mã C# sau khi đã thêm chú thích, tuyệt đối không giải thích.\n\nMã nguồn:\n```csharp\n{code}\n```",

                "3" => $"Hãy phân tích đoạn mã C# dưới đây và viết báo cáo kỹ thuật bằng tiếng Việt theo đúng dàn ý 7 phần sau:\n\n" +
                       "1. Tổng quan (Mục đích class và vai trò)\n" +
                       "2. Cấu trúc (Fields, Properties, Constructor)\n" +
                       "3. Luồng xử lý chính\n" +
                       "4. Phân tích chi tiết từng phương thức\n" +
                       "5. Điểm mạnh của code\n" +
                       "6. Điểm yếu / Rủi ro tiềm ẩn\n" +
                       "7. Đề xuất cải thiện\n\n" +
                       $"Đoạn mã cần phân tích:\n```csharp\n{code}\n```",
                _ => string.Empty
            };
        }

        private static async Task ExecuteAiTask(string choice, string prompt, string targetFile)
        {
            var request = new
            {
                model = "local-model",
                messages = new[] {
                    new { role = "system", content = "Bạn là trợ lý AI chuyên về lập trình .NET. Hãy tuân thủ tuyệt đối cấu trúc được yêu cầu, trả lời bằng tiếng Việt rõ ràng, mạch lạc." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.2, // Rất thấp để AI không bịa ra chữ rác
                top_p = 0.8,       // Tăng tính ổn định cho từ vựng của AI
                max_tokens = 2500  // Đủ không gian để viết báo cáo dài
            };

            try
            {
                PrintColor("\n[AI] Đang gửi yêu cầu đến LM Studio... Vui lòng đợi trong giây lát (có thể mất 1-2 phút).", ConsoleColor.Yellow);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Lỗi từ Server: {response.StatusCode}. Hãy đảm bảo Server LM Studio đã được 'Start'.");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                string aiText = result.choices[0].message.content.ToString();

                // Dọn dẹp rác Markdown (nếu AI cố tình nhét code vào block ```csharp)
                if ((choice == "1" || choice == "2") && aiText.Contains("```"))
                {
                    var lines = aiText.Split('\n').ToList();
                    aiText = string.Join("\n", lines.Where(l => !l.Trim().StartsWith("```")));
                }

                aiText = aiText.Trim();

                // 5. LƯU KẾT QUẢ VÀO FILE (Tự động đặt tên theo file gốc)
                string baseName = Path.GetFileNameWithoutExtension(targetFile);
                string fileName = choice switch
                {
                    "1" => $"{baseName}Tests.cs",
                    "2" => $"{baseName}Documented.cs",
                    "3" => $"{baseName}_Explanation.md",
                    _ => "Result.txt"
                };

                // Lưu vào thư mục LaptopShop.Tests
                string slnRootPath = FindSolutionRoot();
                string outputDir = Path.Combine(slnRootPath, "LaptopShop.Tests");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                string outputPath = Path.Combine(outputDir, fileName);
                await File.WriteAllTextAsync(outputPath, aiText);

                PrintColor("\n[THÀNH CÔNG] AI đã hoàn tất tác vụ!", ConsoleColor.Green);
                Console.WriteLine($"-> Kết quả đã được lưu tại thư mục: {Path.GetFullPath(outputPath)}");

                Console.WriteLine("\n--- XEM TRƯỚC PHẢN HỒI ---");
                Console.WriteLine(aiText.Length > 800 ? aiText.Substring(0, 800) + "\n\n...[Đã cắt bớt để xem trước, vui lòng mở file để xem toàn bộ]..." : aiText);
            }
            catch (Exception ex)
            {
                PrintColor($"\n[LỖI KẾT NỐI API]: {ex.Message}", ConsoleColor.Red);
                Console.WriteLine("Cách khắc phục:");
                Console.WriteLine("1. Mở LM Studio, chọn tab Local Server (<->).");
                Console.WriteLine("2. Đảm bảo Model (PhoGPT) đã được Load vào bộ nhớ.");
                Console.WriteLine("3. Bật nút 'Start Server' tại địa chỉ [http://127.0.0.1:1234](http://127.0.0.1:1234)");
            }
        }

        // Hàm hỗ trợ in chữ có màu trên Console
        private static void PrintColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}