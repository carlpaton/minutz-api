namespace Minutz.Models.Entities
{
	public class Subscription
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Term { get; set; }
		public int Cost { get; set; }
	}
}