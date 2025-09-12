using Microsoft.Extensions.DependencyInjection;
using TrainingDotnetVue1.Models;
using TrainingDotnetVue1.Repository.Implement;
using TrainingDotnetVue1.Repository.Interface;
using TrainingDotnetVue1.Services.Implement;
using TrainingDotnetVue1.Services.Interface;

namespace TrainingDotnetVue1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var serviceProvider = new ServiceCollection()
            .AddSingleton<ITaskRepository, TaskRepository>()
            .AddTransient<ITaskService, TaskService>()
            .BuildServiceProvider();

            var taskService = serviceProvider.GetService<ITaskService>();
            if (taskService == null)
            {
                Console.WriteLine("Không thể khởi tạo dịch vụ TaskService.");
                return;
            }

            Console.WriteLine("Chào mừng đến với ứng dụng Nhắc việc!");

            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await DisplayAllTasks(taskService);
                        break;
                    case "2":
                        await DisplayTasksByStatus(taskService, Status.DueSoon, "--- Công việc sắp đến hạn ---");
                        break;
                    case "3":
                        await DisplayTasksByStatus(taskService, Status.Overdue, "--- Công việc đã quá hạn ---");
                        break;
                    case "4":
                        await DisplaySortedTasks(taskService);
                        break;
                    case "5":
                        await DisplayStatistics(taskService);
                        break;
                    case "0":
                        Console.WriteLine("Tạm biệt!");
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }
                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
            }
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("========== MENU ==========");
            Console.WriteLine("1. Hiển thị tất cả công việc");
            Console.WriteLine("2. Hiển thị công việc sắp đến hạn");
            Console.WriteLine("3. Hiển thị công việc đã quá hạn");
            Console.WriteLine("4. Sắp xếp và hiển thị công việc");
            Console.WriteLine("5. Xem thống kê nhanh");
            Console.WriteLine("0. Thoát");
            Console.Write("Nhập lựa chọn của bạn: ");
        }

        static void PrintTasks<T>(IEnumerable<T> tasks, string title) where T : TaskItem
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('-', title.Length));

            if (!tasks.Any())
            {
                Console.WriteLine("Không có công việc nào để hiển thị.");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Tiêu đề",-40} {"Deadline",-22} {"Ưu tiên",-10} {"Trạng thái"}");
            Console.WriteLine(new string('=', 100));

            foreach (var task in tasks)
            {
                var status = GetTaskStatus(task);
                var statusText = GetStatusDisplayText(status);
                var priorityText = task.Priority.ToString();

                Console.ForegroundColor = GetStatusColor(status);

                Console.WriteLine($"{task.Id,-5} {task.Title,-40} {task.Deadline,-22:dd/MM/yyyy HH:mm} {priorityText,-10} {statusText}");

                Console.ResetColor();
            }
        }

        static Status GetTaskStatus(TaskItem task)
        {
            return task switch
            {
                { CompletedAt: not null } => Status.Completed,
                { CompletedAt: null, Deadline: var dl } when dl < DateTime.Now => Status.Overdue,
                { CompletedAt: null, Deadline: var dl } when dl <= DateTime.Now.AddDays(3) => Status.DueSoon,
                _ => Status.NotStarted
            };
        }

        static string GetStatusDisplayText(Status status) => status switch
        {
            Status.Completed => "Đã hoàn thành",
            Status.Overdue => "Quá hạn",
            Status.DueSoon => "Sắp đến hạn",
            Status.NotStarted => "Chưa bắt đầu",
            _ => "Không xác định"
        };

        static ConsoleColor GetStatusColor(Status status) => status switch
        {
            Status.Completed => ConsoleColor.Green,
            Status.Overdue => ConsoleColor.Red,
            Status.DueSoon => ConsoleColor.Yellow,
            _ => ConsoleColor.Gray
        };

        static async Task DisplayAllTasks(ITaskService taskService)
        {
            var allTasks = await taskService.GetTasksAsync();
            PrintTasks(allTasks, "--- Danh sách tất cả công việc ---");
        }

        static async Task DisplayTasksByStatus(ITaskService taskService, Status status, string title)
        {
            var filteredTasks = await taskService.FilterTasksByStatusAsync(status);
            PrintTasks(filteredTasks, title);
        }

        static async Task DisplaySortedTasks(ITaskService taskService)
        {
            Console.WriteLine("\nSắp xếp công việc theo:");
            Console.WriteLine("1. Deadline");
            Console.WriteLine("2. Mức độ ưu tiên (Priority)");
            Console.Write("Chọn tiêu chí (mặc định là deadline): ");
            var sortBy = Console.ReadLine() == "2" ? "priority" : "deadline";

            Console.Write("Sắp xếp tăng dần (y/n, mặc định là y): ");
            var ascending = Console.ReadLine()?.ToLower() != "n";

            var sortedTasks = await taskService.SortTasksAsync(sortBy, ascending);
            var sortOrder = ascending ? "tăng dần" : "giảm dần";
            PrintTasks(sortedTasks, $"--- Công việc đã sắp xếp theo {sortBy} ({sortOrder}) ---");
        }

        static async Task DisplayStatistics(ITaskService taskService)
        {
            var report = await taskService.GetStatisticsReportAsync();
            Console.WriteLine($"\n{report}");
        }
    }
}
