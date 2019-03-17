namespace AnaDron.SDTest.Model {
	public sealed class UserModel {
		internal UserModel(IUserDTO source) {
			Id = source.Id;
			Name = source.Name;
			Login = source.Login;
			Balance = source.Balance;
		}

		public int Id { get; }
		public string Name { get; }
		public string Login { get; }
		public double Balance { get; }
	}
}
