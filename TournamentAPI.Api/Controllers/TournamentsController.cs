using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class TournamentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentsController(IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return _mapper.Map<TournamentDto>(tournament);
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDto tournament)
        {
            if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
            {
                return BadRequest();
            }

            var tournamentEntity = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(tournament, tournamentEntity);

            _unitOfWork.TournamentRepository.Update(tournamentEntity);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentExists(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournament(Tournament tournament)
        {
            var finalTournament = _mapper.Map<Tournament>(tournament);

            _unitOfWork.TournamentRepository.Add(finalTournament);
            await _unitOfWork.CompleteAsync();

            var createdTournament = _mapper.Map<TournamentDto>(finalTournament);

            return CreatedAtAction("GetTournament", new { id = finalTournament.Id }, createdTournament);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> TournamentExists(int id)
        {
            return await _unitOfWork.TournamentRepository.AnyAsync(id);
        }
    }
}
