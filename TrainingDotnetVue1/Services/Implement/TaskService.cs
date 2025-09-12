using TrainingDotnetVue1.Models;
using TrainingDotnetVue1.Repository.Interface;
using TrainingDotnetVue1.Services.Interface;

namespace TrainingDotnetVue1.Services.Implement;
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItem>> FilterTasksByStatusAsync(Status status)
    {
        var allTasks = await _taskRepository.GetAllTasksAsync();
        Func<TaskItem, bool> predicate = task => GetTaskStatus(task) == status;
        return allTasks.Where(predicate);
    }

    public async Task<string> GetStatisticsReportAsync()
    {
        var allTasks = (await _taskRepository.GetAllTasksAsync()).ToList();

        if (!allTasks.Any()) return "Không có công việc nào để thống kê.";

        var totalTasks = allTasks.Count();
        var completedTasks = allTasks.Count(t => GetTaskStatus(t) == Status.Completed);
        var overdueTasks = allTasks.Count(t => GetTaskStatus(t) == Status.Overdue);
        var dueSoonTasks = allTasks.Count(t => GetTaskStatus(t) == Status.DueSoon);
        var highPriorityTasks = allTasks.Count(t => t.Priority == Priority.High && t.CompletedAt == null);

        double completionPercentage = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;

        var report = new System.Text.StringBuilder();
        report.AppendLine("--- Báo cáo thống kê nhanh ---");
        report.AppendLine($"Tổng số công việc: {totalTasks}");
        report.AppendLine($"Đã hoàn thành: {completedTasks} ({completionPercentage:F2}%)");
        report.AppendLine($"Đã quá hạn: {overdueTasks}");
        report.AppendLine($"Sắp đến hạn: {dueSoonTasks}");
        report.AppendLine($"Chưa hoàn thành & ưu tiên Cao: {highPriorityTasks}");
        report.AppendLine("-----------------------------");

        return report.ToString();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksAsync()
    {
        return await _taskRepository.GetAllTasksAsync();
    }

    public async Task<IEnumerable<TaskItem>> SortTasksAsync(string sortBy, bool ascending = true)
    {
        var allTasks = await _taskRepository.GetAllTasksAsync();

        var sortedTasks = sortBy.ToLower() switch
        {
            "deadline" => ascending
                            ? allTasks.OrderBy(t => t.Deadline)
                            : allTasks.OrderByDescending(t => t.Deadline),
            "priority" => ascending
                            ? allTasks.OrderBy(t => t.Priority)
                            : allTasks.OrderByDescending(t => t.Priority),
            _ => allTasks.OrderBy(t => t.Id)
        };
        return sortedTasks;
    }

    private Status GetTaskStatus(TaskItem task)
    {
        return task switch
        {
            { CompletedAt: not null } => Status.Completed,
            { CompletedAt: null, Deadline: var dl } when dl < DateTime.Now => Status.Overdue,
            { CompletedAt: null, Deadline: var dl } when dl <= DateTime.Now.AddDays(3) => Status.DueSoon,
            _ => Status.NotStarted
        };
    }
}
