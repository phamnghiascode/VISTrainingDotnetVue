using System.Text.Json.Serialization;

namespace TrainingDotnetVue1.Models
{
    public record TaskItem(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("deadline")] DateTime Deadline,
        [property: JsonPropertyName("completed_at")] DateTime? CompletedAt,
        [property: JsonPropertyName("priority")] Priority Priority
    );

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum Status
    {
        Completed,
        Overdue,
        DueSoon,
        NotStarted
    }
}