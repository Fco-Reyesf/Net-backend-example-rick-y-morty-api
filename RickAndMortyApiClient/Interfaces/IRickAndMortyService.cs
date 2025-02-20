using System.Collections.Generic;
using System.Threading.Tasks;
using RickAndMortyApiClient.Models;

namespace RickAndMortyApiClient.Interfaces
{
    public interface IRickAndMortyService
    {
        Task<List<Result>> GetAllEpisodesAsync();
        Task<Result> GetEpisodeByIdAsync(int id);
    }
}