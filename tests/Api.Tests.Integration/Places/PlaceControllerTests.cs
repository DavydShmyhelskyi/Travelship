using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Places;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Places;
using Xunit;

namespace Api.Tests.Integration.Places;