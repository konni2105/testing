using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Poc.Tests.Integration
{
    public class CareerApiIntegrationTest
     : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public CareerApiIntegrationTest(
            WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        //No token

        [Fact]
        public async Task GetCareers_WithoutToken_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/Careers");


            Console.WriteLine(
                $"Actual HTTP Status: {(int)response.StatusCode} {response.StatusCode}");

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }


        //UserToken 
        [Fact]
        public async Task CreateCareer_WithUserToken_ReturnsForbidden()
        {
            var token = GenerateToken("User");

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", token);

            var response = await _client.PostAsync(
                "/api/Careers",
                new StringContent(
                    "{}",
                    Encoding.UTF8,
                    "application/json"));

            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }

        //Admin token

        [Fact]
        public async Task CreateCareer_WithAdminToken_ReturnsCreated()
        {
            var token = GenerateToken("Admin");

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", token);

            var career = new
            {
                Title = "Integration Test Career",
                Description = "Career created during integration testing",
                MinimumSalary = 30000m,
                MaximumSalary = 60000m,
                RequiredExperience = 2
            };

            var json = System.Text.Json.JsonSerializer.Serialize(career);

            var response = await _client.PostAsync(
                "/api/Careers",
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"));

            Console.WriteLine(
    $"Actual HTTP Status: {(int)response.StatusCode} {response.StatusCode}");

            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);
        }




        private string GenerateToken(string role)
        {
            var factory = _factory;
            var configuration = factory.Services
                .GetRequiredService<IConfiguration>();

            var issuer = configuration["JwtSettings:Issuer"]!;
            var audience = configuration["JwtSettings:Audience"]!;
            var secretKey = configuration["JwtSettings:SecretKey"]!;

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, "999"),
        new Claim(JwtRegisteredClaimNames.UniqueName, "testuser"),
        new Claim(ClaimTypes.Role, role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}