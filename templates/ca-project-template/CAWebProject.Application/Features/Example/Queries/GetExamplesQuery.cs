﻿using CAWebProject.Application.Features.Example.Models;
using FluentResults;
using MediatR;

namespace CAWebProject.Application.Features.Example.Queries;

public class GetExamplesQuery : IRequest<Result<ExampleCollectionDto>>
{
}