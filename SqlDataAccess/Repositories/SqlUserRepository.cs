using System;
using System.Collections.Generic;
using System.Linq;
using AnaDron.SDTest.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class SqlUserRepository:IUserRepository {
		readonly SystemContext _context;

		public SqlUserRepository(SystemContext context) {
			_context = context;
		}

		public bool Any() => _context.Users.Any();

		public IUserDTO Get(int id) => _context.Users.Find(id);

		public IUserDTO FindByLogin(string login) => _context.Users.FirstOrDefault(x => x.Login == login);

		public IEnumerable<IUserDTO> GetAll() => _context.Users;

		public IEnumerable<IUserDTO> GetOther(int userId, string template = null, int limit = 5) {
			IQueryable<User> query = _context.Users;

			template = template?.Trim() ?? "";
			if (template == "")
				query = query
					.Where(x => x.UserId != userId)
					.OrderBy(x => x.Name)
					.Take(limit);
			else
				// Дополнительно сначала сортируем имена по индексу вхождения шаблона
				query = query.FromSql(
$@"SELECT TOP({limit}) [UserId], [Balance], [Login], [Name], [Password]
FROM (
	SELECT *, CHARINDEX({template}, [Name]) AS [Index]
	FROM [Users]
	WHERE [UserId] <> {userId}
) as T1
WHERE [Index] > 0
ORDER BY [Index], [Name]");

			return query.ToList();
		}

		public void Add(string name, string login, string password, double balance) {
			_context.Users.Add(new User {
				Name = name,
				Login = login,
				Password = password,
				Balance = balance
			});
		}

		public void UpdateBalance(IUserDTO user) {
			if (!(user is User)) throw new InvalidOperationException();
		}
	}
}