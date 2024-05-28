namespace Autenticacao.Comum.Database.Repositorio;

public class Repositorio(ISessionFactory sessionFactory)
{
	private ISession _sessao;

	protected ISession Sessao
	{
		get
		{
			if (_sessao is null || !_sessao.IsOpen)
				_sessao = sessionFactory.OpenSession();

			return _sessao;
		}
	}
}

