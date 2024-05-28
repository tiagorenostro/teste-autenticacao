namespace Autenticacao.Comum.Database.Mapeamento;

public class UsuarioMap : ClassMapping<Usuario>
{
	public UsuarioMap()
	{
		Lazy(true);
		DynamicInsert(true);
		DynamicUpdate(true);
		Table(typeof(Usuario).Name);

		Id(x => x.Id, mapper => mapper.Generator(Generators.Identity));
		Property(x => x.Nome);
        Property(x => x.Email);
        Property(x => x.Celular);
        Property(x => x.Senha);
        Property(x => x.Status);
        Property(x => x.DataHoraCadastro);
        Property(x => x.CodigoAtivacao);
        Property(x => x.PermiteReceberNotificacoes);
    }
}