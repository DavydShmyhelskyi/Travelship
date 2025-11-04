using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Travels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Travels;
using Xunit;

namespace Api.Tests.Integration.Travels;