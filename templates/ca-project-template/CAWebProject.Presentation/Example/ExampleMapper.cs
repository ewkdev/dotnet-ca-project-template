using CAWebProject.Application.Features.Example.Commands;
using CAWebProject.Presentation.Example.Models.V1;
using Riok.Mapperly.Abstractions;

namespace CAWebProject.Presentation.Example;

[Mapper]
public partial class ExampleMapper
{
        public partial CreateExampleCommand ToCommand(CreateExampleRequest request);
}
