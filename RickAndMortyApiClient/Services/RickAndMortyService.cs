using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RickAndMortyApiClient.Interfaces;
using RickAndMortyApiClient.Models;

namespace RickAndMortyApiClient.Services
{
    public class RickAndMortyService : IRickAndMortyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RickAndMortyService> _logger;

        public RickAndMortyService(HttpClient httpClient, ILogger<RickAndMortyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Result>> GetAllEpisodesAsync()
        {
            var allEpisodes = new List<Result>();
            var nextPageUrl = "https://rickandmortyapi.com/api/episode";

            while (!string.IsNullOrEmpty(nextPageUrl))
            {
                try
                {
                    _logger.LogInformation("Fetching data from: {NextPageUrl}", nextPageUrl);

                    var response = await _httpClient.GetAsync(nextPageUrl);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("API Response: {JsonResponse}", jsonResponse);

                    ApiResponse? apiResponse;
                    try
                    {
                        apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);
                        if (apiResponse == null)
                        {
                            _logger.LogError("Failed to deserialize the API response.");
                            continue;
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Failed to deserialize the API response.");
                        throw;
                    }

                    if (apiResponse.Results != null)
                    {
                        _logger.LogInformation("Found {Count} episodes in this page.", apiResponse.Results.Count);
                        allEpisodes.AddRange(apiResponse.Results);
                    }
                    else
                    {
                        _logger.LogWarning("No results found in the API response.");
                    }

                    nextPageUrl = apiResponse.Info?.next;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching data from the API.");
                    throw;
                }
            }

            _logger.LogInformation("Total episodes fetched: {TotalEpisodes}", allEpisodes.Count);
            return allEpisodes;
        }

        public async Task<Result> GetEpisodeByIdAsync(int id)
        {
            try
            {
                var url = $"https://rickandmortyapi.com/api/episode/{id}";
                _logger.LogInformation("Fetching data from: {Url}", url);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response: {JsonResponse}", jsonResponse);

                Result? episode;
                try
                {
                    episode = JsonSerializer.Deserialize<Result>(jsonResponse);
                    if (episode == null)
                    {
                        _logger.LogError("Failed to deserialize the API response.");
                        throw new Exception("Failed to deserialize the API response.");
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize the API response.");
                    throw;
                }

                return episode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching data from the API.");
                throw;
            }
        }
    }
}