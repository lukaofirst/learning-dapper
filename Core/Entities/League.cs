namespace Core.Entities
{
	public class League
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public List<Team>? Teams { get; set; } = new();
	}
}