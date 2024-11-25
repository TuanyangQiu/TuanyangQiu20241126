using System.ComponentModel.DataAnnotations;

namespace Handler.Model
{
	public class User
	{
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }
	}

}
