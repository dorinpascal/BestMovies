﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Clients;

public partial class BestMoviesApiClient
{
    public async Task SaveUser(UserDto user)
    {
        var userJson = JsonSerializer.Serialize(user);
        var userStringContent = new StringContent(
            userJson,
            Encoding.UTF8,
            "application/json"
        );
        var responseMessage = await _client.PostAsync("users", userStringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
    }

    public async Task<UserDto> GetUser(string identifier)
    {
        var responseMessage = await _client.GetAsync($"users/{identifier}");
        if (!responseMessage.IsSuccessStatusCode)
        {
            await responseMessage.ThrowBasedOnStatusCode();
        }
        
        var content = await responseMessage.ReadContentSafe();
        return JsonSerializer.Deserialize<UserDto>(content, _jsonSerializerOptions)!;
    }
}