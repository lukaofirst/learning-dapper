namespace Core.Entities
{
	public class Match
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public List<Team> Teams { get; set; } = new();
	}
}