namespace CAWebProject.Api.Example.V2.Models;

public record ExampleCollectionResponse(int Total, int Offset, int Limit, List<ExampleResponse> Items);