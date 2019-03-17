namespace AnaDron.SDTest.Model {
	public interface IUserDTO {
		int Id { get; }
		string Login { get; }
		string Password { get; }
		string Name { get; }
		double Balance { get; set; }
	}
}
