namespace Autenticacao.Comum.Database.Repositorio;

public class UsuarioRepositorio(ISessionFactory sessionFactory) : Repositorio(sessionFactory)
{
	public void SalvarUsuario(Usuario usuario)
	{
		using var transacao = Sessao.BeginTransaction();
		Sessao.SaveOrUpdate(usuario);
		transacao.Commit();
	}

	public Usuario ObterUsuarioPorEmail(string email) =>
		Queryable().FirstOrDefault(x => x.Email == email);
	
	public Usuario ObterUsuarioPorCelular(string celular) =>
		Queryable().FirstOrDefault(x => x.Celular.Contains(celular));
	
	public Usuario ObterUsuarioPorId(long usuarioId) =>
		Queryable().FirstOrDefault(x => x.Id == usuarioId);
	
	public bool ExisteEmailCadastrado(string email) =>
		Queryable().Any(x => x.Email == email);
	
	public bool ExisteCelularCadastrado(string celular) =>
		Queryable().Any(x => x.Celular == celular);

	private IQueryable<Usuario> Queryable() =>
		Sessao.Query<Usuario>();
}

