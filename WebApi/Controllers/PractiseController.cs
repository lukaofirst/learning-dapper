using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PractiseController : ControllerBase
	{
		private readonly IPractiseRepository _practiseRepository;

		public PractiseController(IPractiseRepository practiseRepository)
		{
			_practiseRepository = practiseRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var entities = await _practiseRepository.GetAll();

			return Ok(entities);
		}

		[HttpGet("get-by-id/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var entity = await _practiseRepository.GetById(id);

			return Ok(entity);
		}

		[HttpGet("querying-multiple/{leagueId}")]
		public async Task<IActionResult> QueryingMultiple(int id)
		{
			var entity = await _practiseRepository.QueryingMultiple(id);

			return Ok(entity);
		}

		[HttpGet("count-records")]
		public async Task<IActionResult> CountRecords()
		{
			var count = await _practiseRepository.CountRecords();

			return Ok($"Total Records: {count}");
		}

		[HttpPost]
		public async Task<IActionResult> Create(string leagueName)
		{
			var result = await _practiseRepository.Create(leagueName);

			return Ok(result);
		}

		[HttpPut]
		public async Task<IActionResult> Update(string updatedLeagueName)
		{
			var result = await _practiseRepository.Update(updatedLeagueName);

			return Ok(result);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _practiseRepository.Delete(id);

			return Ok(result);
		}

		[HttpPost("transactions")]
		public async Task<IActionResult> SqlTransactions()
		{
			await _practiseRepository.SqlTransactions();

			return Ok("Command executed");
		}

		[HttpGet("queryingOneToOne")]
		public async Task<IActionResult> QueryingOneToOne()
		{
			var result = await _practiseRepository.QueryingOneToOne();

			return Ok(result);
		}

		[HttpGet("queryingOneToMany")]
		public async Task<IActionResult> QueryingOneToMany()
		{
			var result = await _practiseRepository.QueryingOneToMany();

			return Ok(result);
		}

		[HttpPost("usingStoredProcedure")]
		public async Task<IActionResult> UsingStoredProcedure()
		{
			var result = await _practiseRepository.UsingStoredProcedure();

			return Ok(result);
		}
	}
}