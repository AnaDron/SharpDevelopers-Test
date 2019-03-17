using System.Collections.Generic;

namespace AnaDron.SDTest.Model {
	public interface IUserRepository {
		bool Any();

		IUserDTO Get(int id);
		IUserDTO FindByLogin(string login);
		IEnumerable<IUserDTO> GetAll();
		IEnumerable<IUserDTO> GetOther(int userId, string template = null, int limit = 5);

		void Add(string name, string login, string password, double balance);

		void UpdateBalance(IUserDTO user);
	}
}