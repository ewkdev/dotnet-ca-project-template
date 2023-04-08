namespace CAWebProject.Application.Features.Example.Models;

public class ExampleCollectionDto
{
    public int Total { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public List<ExampleDto> Items { get; set; } = new();
}