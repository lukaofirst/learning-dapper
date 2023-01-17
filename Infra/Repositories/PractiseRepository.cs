
using System.Data;
using Core.Entities;
using Core.Interfaces;
using Dapper;
using Infra.Context;

namespace Infra.Repositories
{
	public class PractiseRepository : IPractiseRepository
	{
		private readonly DapperContext _context;

		public PractiseRepository(DapperContext context)
		{
			_context = context;
		}

		public async Task<int> CountRecords()
		{
			var sqlQuery = "SELECT COUNT(*) FROM \"Leagues\";";

			using var connection = _context.CreateConnection();

			var result = await connection.ExecuteScalarAsync<int>(sqlQuery);

			return result;
		}

		public async Task<List<League>> GetAll()
		{
			using var connection = _context.CreateConnection();
			var sqlQuery = "SELECT * FROM \"Leagues\";";
			var entities = await connection.QueryAsync<League>(sqlQuery);

			return entities.ToList();
		}

		public async Task<League> GetById(int id)
		{
			using var connection = _context.CreateConnection();
			var sqlQuery = $"SELECT * FROM \"Leagues\" WHERE \"Id\" = @Id;";
			var entity = await connection.QuerySingleOrDefaultAsync<League>(sqlQuery, new { id });

			return entity;
		}

		public async Task<League> QueryingMultiple(int leagueId)
		{
			var teamIds = new int[] { 1, 2 };

			var parameters = new DynamicParameters();
			parameters.Add("leagueId", leagueId);
			parameters.Add("teamIds", teamIds);

			using var connection = _context.CreateConnection();
			var sqlQuery = $"SELECT * FROM \"Leagues\" WHERE \"Id\" = @leagueId; SELECT * FROM \"Teams\" WHERE \"Id\" = ANY(@teamIds);";

			using var multi = await connection.QueryMultipleAsync(sqlQuery, parameters);

			var league = multi.ReadSingle<League>();
			var teams = multi.Read<Team>().ToList();

			var entity = new League
			{
				Id = league.Id,
				Name = league.Name,
				Teams = teams
			};

			return entity;
		}

		public async Task<bool> Create(string leagueName)
		{
			using var connection = _context.CreateConnection();

			var parameters = new { leagueName };

			var sqlQuery = "INSERT INTO public.\"Leagues\"(\"Name\") VALUES (@leagueName);";

			var result = await connection.ExecuteAsync(sqlQuery, parameters);

			return result > 0;
		}


		public async Task<bool> Update(string updatedLeagueName)
		{
			using var connection = _context.CreateConnection();

			var id = 4;

			var parameters = new { id, updatedLeagueName };

			var sqlQuery = "UPDATE public.\"Leagues\" SET \"Name\" = @updatedLeagueName WHERE \"Id\" = @id;";

			var result = await connection.ExecuteAsync(sqlQuery, parameters);

			return result > 0;
		}

		public async Task<bool> Delete(int id)
		{
			using var connection = _context.CreateConnection();

			var parameters = new { id };

			var sqlQuery = "DELETE FROM public.\"Leagues\" WHERE \"Id\" = @id;";

			var result = await connection.ExecuteAsync(sqlQuery, parameters);

			return result > 0;
		}

		public async Task SqlTransactions()
		{
			var leagues = new List<League>()
			{
				new League { Name = "Serie A" },
				new League { Name = "Bundesliga" },
			};

			using var connection = _context.CreateConnection();
			connection.Open();

			using var transaction = connection.BeginTransaction();

			try
			{
				var sqlQuery = "INSERT INTO public.\"Leagues\"(\"Name\") VALUES (@leagueName);";

				foreach (var league in leagues)
				{
					var parameters = new DynamicParameters();
					parameters.Add("leagueName", league.Name, DbType.String);
					await connection.ExecuteAsync(sqlQuery, parameters, transaction: transaction);
				}

				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}

		public async Task<List<Team>> QueryingOneToOne()
		{
			using var connection = _context.CreateConnection();
			var sqlQuery = $"select * from \"Teams\" t inner join \"Leagues\" l on l.\"Id\" = t.\"LeagueId\";";

			var leagues = await connection.QueryAsync<Team, League, Team>(sqlQuery, (team, league) =>
			{
				team.League = league;
				return team;
			});


			return leagues.ToList();
		}

		public async Task<List<League>> QueryingOneToMany()
		{
			using var connection = _context.CreateConnection();
			var sqlQuery = $"select * from \"Leagues\" l inner join \"Teams\" t on l.\"Id\" = t.\"LeagueId\";";

			var leagueDict = new Dictionary<int, League>();

			var leagues = await connection.QueryAsync<League, Team, League>(sqlQuery, (league, team) =>
			{
				if (!leagueDict.TryGetValue(league.Id, out var currentLeague))
				{
					currentLeague = league;
					leagueDict.Add(currentLeague.Id, currentLeague);
				}

				currentLeague.Teams!.Add(team);
				return currentLeague;
			});

			// return leagues.Distinct().ToList();
			return leagueDict.Select(x => x.Value).ToList();
		}

		public async Task<bool> UsingStoredProcedure()
		{
			using var connection = _context.CreateConnection();

			var parameters = new { /* parameters here */ };
			var storedProcedure = $"MyStoredProcedure";

			var result = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

			return result > 0;
		}
	}
}