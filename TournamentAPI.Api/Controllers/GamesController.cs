using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;
using TournamentAPI.Data.Repositories;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(string? title)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync(title);
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return _mapper.Map<GameDto>(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _unitOfWork.TournamentRepository.AnyAsync(game.TournamentId))
            {
                return NotFound();
            }

            var gameEntity = await _unitOfWork.GameRepository.GetAsync(id);

            if (gameEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(game, gameEntity);

            _unitOfWork.GameRepository.Update(gameEntity);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDto game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _unitOfWork.TournamentRepository.AnyAsync(game.TournamentId))
            {
                return NotFound();
            }

            var finalGame = _mapper.Map<Game>(game);

            _unitOfWork.GameRepository.Add(finalGame);
            await _unitOfWork.CompleteAsync();

            var createdGame = _mapper.Map<GameDto>(finalGame);

            return CreatedAtAction("GetGame", new { id = finalGame.Id }, createdGame);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _unitOfWork.GameRepository.Remove(game);
            //todo: add exception handling to return status code 500
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult> PatchGame(int gameId,
            JsonPatchDocument<GameDto> patchDocument)
        {
            var gameEntity = await _unitOfWork.GameRepository.GetAsync(gameId);
            if (gameEntity == null)
            {
                return NotFound();
            }

            var gameToPatch = _mapper.Map<GameDto>(gameEntity);

            patchDocument.ApplyTo(gameToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(gameToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(gameToPatch, gameEntity);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await _unitOfWork.GameRepository.AnyAsync(id);
        }
    }
}
