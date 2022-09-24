using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            // Fetch data from database - domail WalkDifficulty
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();

            // Convert Domain WalkDiffilcuties to DTO WalkDiffilcuties
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            // Return response
            return Ok(walkDifficultiesDTO);
        }

       [HttpGet]
       [Route("{id:guid}")]
       [ActionName("GetAllWalkDifficultiesAsync")]
       public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            // Get WalkDifficulty Domain from database
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            // Conver Domain object to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            // Return response
            return Ok(walkDifficultyDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Convert DTO to Domain Object
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };

            // Pass Domain object to Repository to persist this
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            // Convert back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code,
            };

            // Send DTO response back to Client
            return CreatedAtAction(nameof(GetAllWalkDifficultiesAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to Domain Model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            // Update WalkDifficulty using repository - (Get Domain object in response (or null)
            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            // If Null the Not Found
            if (walkDifficulty == null)
            {
                return NotFound("Walk Difficultu Not Found!");
            }

            // Convert Domain back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficulty.Id,
                Code = walkDifficulty.Code,
            };

            // Return response
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkdifficultyAsync(Guid id)
        {
            // Get WalkDifficulty from database
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            // If null NotFound
            if (id == null)
            {
                return NotFound("Walk Difficulty Not Found");
            }

            // Convert response back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);


        }
    }
}
