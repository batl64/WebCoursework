using System.ComponentModel.DataAnnotations;

namespace WebAppShares.Models
{
	public class User
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public int Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
