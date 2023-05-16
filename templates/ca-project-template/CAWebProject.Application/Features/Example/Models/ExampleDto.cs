namespace CAWebProject.Application.Features.Example.Models;

public class ExampleDto
{
    public Guid Id { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}