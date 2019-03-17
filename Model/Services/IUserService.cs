using System.Collections.Generic;

namespace AnaDron.SDTest.Model {
	public interface IUserService {
		void Register(string name, string login, string password, string passwordConfirm, double balance);
		UserModel Authenticate(string login, string password);

		UserModel Get(int userId);
		IEnumerable<UserModel> GetOther(int userId, string template = null, int limit = 5);
	}
}