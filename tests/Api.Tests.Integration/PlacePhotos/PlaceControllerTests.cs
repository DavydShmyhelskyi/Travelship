using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.PlacePhotos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.PlacePhotos;
using Xunit;

namespace Api.Tests.Integration.PlacePhotos;