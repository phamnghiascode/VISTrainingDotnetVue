using TrainingDotnetVue1.Models;
namespace TrainingDotnetVue1.Services.Interface;
public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetTasksAsync();
    Task<IEnumerable<TaskItem>> FilterTasksByStatusAsync(Status status);
    Task<IEnumerable<TaskItem>> SortTasksAsync(string sortBy, bool ascending = true);
    Task<string> GetStatisticsReportAsync();
}
