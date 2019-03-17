using System;

namespace AnaDron.SDTest.Model {
	public class UserNotFoundException:Exception {
		public UserNotFoundException(string name = "User") {
			Name = name;
		}

		public string Name { get; }

		public override string Message => $"{Name} not found";
	}
}