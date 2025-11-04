using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Roles;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Roles;
using Xunit;

namespace Api.Tests.Integration.Roles;