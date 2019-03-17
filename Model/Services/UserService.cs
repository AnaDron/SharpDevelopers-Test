using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AnaDron.SDTest.Model {
	sealed class UserService:ServiceBase, IUserService {
		readonly ILoginValidator _loginValidator;
		readonly IPasswordValidator _passwordValidator;

		public UserService(ILoginValidator loginValidator, IPasswordValidator passwordValidator, IUnitOfWork unitOfWork) : base(unitOfWork) {
			_loginValidator = loginValidator;
			_passwordValidator = passwordValidator;
		}

		public void Register(string name, string login, string password, string passwordConfirm, double balance) {
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
			_loginValidator.Validate(login);
			_passwordValidator.Validate(password);
			if (passwordConfirm != password) throw new ArgumentException("Password confirmation wrong", nameof(passwordConfirm));

			var user = Users.FindByLogin(login);
			if (user != null) throw new DuplicateLoginException();

			Users.Add(name, login, password, balance);
		}

		public UserModel Authenticate(string login, string password) {
			var user = Users.FindByLogin(login).Exist();
			if (password != user.Password) throw new ArgumentException("Password doesn't match", nameof(password));

			return user.ToModel();
		}

		public UserModel Get(int userId) {
			return Users.GetExist(userId).ToModel();
		}

		public IEnumerable<UserModel> GetOther(int userId, string template = null, int limit = 5) {
			Users.CheckExist(userId);
			if (limit <= 0 || limit > 10) throw new ArgumentOutOfRangeException(nameof(limit));

			return Users.GetOther(userId, template, limit).Select(x => x.ToModel());
		}
	}
}