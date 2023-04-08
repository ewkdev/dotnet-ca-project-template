namespace CAWebProject.Presentation.Example.Models.V1;

public record ExampleCollectionResponse(int Total, int Offset, int Limit, List<ExampleResponse> Items);