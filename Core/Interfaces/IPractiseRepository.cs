using Core.Entities;

namespace Core.Interfaces
{
	public interface IPractiseRepository
	{
		Task<int> CountRecords();
		Task<List<League>> GetAll();
		Task<League> GetById(int id);
		Task<League> QueryingMultiple(int leagueId);
		Task<bool> Create(string leagueName);
		Task<bool> Update(string updatedLeagueName);
		Task<bool> Delete(int id);
		Task SqlTransactions();
		Task<List<Team>> QueryingOneToOne();
		Task<List<League>> QueryingOneToMany();
		Task<bool> UsingStoredProcedure();
	}
}