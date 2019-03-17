using System;
using System.Text.RegularExpressions;

namespace AnaDron.SDTest.Model {
	sealed class LoginValidator:ILoginValidator {
		public void Validate(string login) {
			if (login == null) throw new ArgumentNullException(nameof(login));
			// if (!Regex.Match(login, "^(?=.*[A-Za-z0-9]$)[A-Za-z][A-Za-z\\d_]{2,15}$").Success) throw new ArgumentException("Login must be alphanumeric (include special symbol '_')", nameof(login));
		}
	}
}