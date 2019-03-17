using System.Collections.Generic;

namespace AnaDron.SDTest.Model {
	sealed class UserDTO:IUserDTO {
		public int Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public double Balance { get; set; }
	}
}
