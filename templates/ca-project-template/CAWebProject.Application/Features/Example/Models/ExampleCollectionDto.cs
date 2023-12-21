namespace CAWebProject.Application.Features.Example.Models;

public class ExampleCollectionDto
{
    public int Total { get; init; }
    public int Limit { get; init; }
    public int Offset { get; init; }
    public List<ExampleDto> Items { get; init; } = [];
}