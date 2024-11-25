using System.ComponentModel.DataAnnotations;

namespace Handler.Model
{
	public class User
	{
		[Required(ErrorMessage = "First Name is required.")]
		[MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
		[RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z ]+$", ErrorMessage = "First Name can only contain letters and spaces, and must include at least one letter.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is required.")]
		[MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
		[RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z ]+$", ErrorMessage = "Last Name can only contain letters and spaces, and must include at least one letter.")]
		public string LastName { get; set; }
	}

}
