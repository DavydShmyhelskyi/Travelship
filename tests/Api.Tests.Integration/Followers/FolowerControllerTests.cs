using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Followers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Folowers;
using Xunit;

namespace Api.Tests.Integration.Followers;