namespace Autenticacao.Comum.InjecaoDependencia;

public class InjecaoDependencia
{
	public static Configuracao.Configuracao Registrar(IServiceCollection services, IConfiguration configuration)
	{
		var configuracao = RegistrarConfiguracao(services, configuration);
		RegistrarNHibernate(services, configuracao);
		RegistrarRepositorios(services);

		return configuracao;
	}

	private static Configuracao.Configuracao RegistrarConfiguracao(IServiceCollection services, IConfiguration configuration)
	{
		var configuracao = configuration.GetSection("Configuracao").Get<Configuracao.Configuracao>();
		services.AddSingleton(configuracao);
		return configuracao;
	}

	private static void RegistrarNHibernate(IServiceCollection services, Configuracao.Configuracao configuracao)
	{
		var nhibernateConfig = new Configuration();
		nhibernateConfig.DataBaseIntegration(db =>
		{
			db.Driver<NHibernate.Extensions.Sqlite.SqliteDriver>();
			db.Dialect<NHibernate.Extensions.Sqlite.SqliteDialect>();
			db.ConnectionString = configuracao.ConnectionString;
			db.IsolationLevel = IsolationLevel.ReadCommitted;
			db.BatchSize = 100;
		});

		var mapper = new ModelMapper();
		mapper.AddMappings(Assembly.GetAssembly(typeof(UsuarioMap)).GetExportedTypes().Where(x => x.Name.EndsWith("Map")));

		nhibernateConfig.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

		services.AddSingleton(x => nhibernateConfig.BuildSessionFactory());
	}

	private static void RegistrarRepositorios(IServiceCollection services)
	{
		services.AddScoped<Repositorio>();
        services.AddScoped<UsuarioRepositorio>();
    }
}