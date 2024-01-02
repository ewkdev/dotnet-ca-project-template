using CAWebProject.Api.Example.V1.Models;
using CAWebProject.Application.Features.Example.Commands;
using CAWebProject.Application.Features.Example.Models;
using Riok.Mapperly.Abstractions;

namespace CAWebProject.Api.Example.V1;

[Mapper]
public partial class ExampleMapper
{
        public partial CreateExampleCommand ToCommand(CreateExampleRequest request);
        public partial ExampleResponse ToResponse(ExampleDto example);
        public partial ExampleCollectionResponse ToResponse(ExampleCollectionDto exampleCollection);
}
