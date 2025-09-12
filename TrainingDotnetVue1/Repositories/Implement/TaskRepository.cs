using System.Text.Json;
using TrainingDotnetVue1.Models;
using TrainingDotnetVue1.Repository.Interface;

namespace TrainingDotnetVue1.Repository.Implement;
public class TaskRepository : ITaskRepository
{
    private readonly string _filePath;

    public TaskRepository(string filePath = "tasks.json")
    {
        _filePath = filePath;
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        if(!File.Exists(_filePath))
        {
            Console.WriteLine($"Không tìm thấy file có đường dẫn '{_filePath}'");
            return Enumerable.Empty<TaskItem>();
        }

        string jsonContent = await File.ReadAllTextAsync(_filePath);
        if(string.IsNullOrEmpty(jsonContent))
        {
            Console.WriteLine($"File '{_filePath}' rỗng");
            return Enumerable.Empty<TaskItem>();
        }

        var result = JsonSerializer.Deserialize<List<TaskItem>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result ?? Enumerable.Empty<TaskItem>();
    }
}
