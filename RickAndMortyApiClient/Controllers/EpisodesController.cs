using Microsoft.AspNetCore.Mvc;
using RickAndMortyApiClient.Interfaces;
using RickAndMortyApiClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RickAndMortyApiClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : ControllerBase
    {
        private readonly IRickAndMortyService _rickAndMortyService;

        public EpisodesController(IRickAndMortyService rickAndMortyService)
        {
            _rickAndMortyService = rickAndMortyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Result>>> GetAllEpisodes()
        {
            var episodes = await _rickAndMortyService.GetAllEpisodesAsync();
            return Ok(episodes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetEpisodeById(int id)
        {
            var episode = await _rickAndMortyService.GetEpisodeByIdAsync(id);
            if (episode == null)
            {
                return NotFound();
            }
            return Ok(episode);
        }
    }
}