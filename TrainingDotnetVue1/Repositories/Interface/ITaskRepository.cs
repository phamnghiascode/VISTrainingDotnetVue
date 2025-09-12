using TrainingDotnetVue1.Models;

namespace TrainingDotnetVue1.Repository.Interface;
public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
}
