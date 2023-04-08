using CAWebProject.Application.Features.Example.Models;
using Riok.Mapperly.Abstractions;

namespace CAWebProject.Application.Features.Example;

[Mapper]
public partial class ExampleMapper
{
    public partial ExampleDto ToDto(Project.Domain.Example.Example example);
}