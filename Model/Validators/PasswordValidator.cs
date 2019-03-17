using System;
using System.Text.RegularExpressions;

namespace AnaDron.SDTest.Model {
	sealed class PasswordValidator:IPasswordValidator {
		public void Validate(string password) {
			if (password == null) throw new ArgumentNullException(nameof(password));
			if (!Regex.Match(password, "^((?!.*[\\s])(?=.*[A-Z])(?=.*\\d).{6,20})").Success) throw new ArgumentException("Password must contains capital letter, digit, be 6 or more length and can't contains whitespaces", nameof(password));
		}
	}
}