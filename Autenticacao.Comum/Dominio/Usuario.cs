namespace Autenticacao.Comum.Dominio;

public class Usuario
{
    public virtual long Id { get; set; }
    public virtual string Nome { get; set; }
    public virtual string Email { get; set; }
    public virtual string Celular { get; set; }
    public virtual string Senha { get; set; }
    public virtual StatusUsuario Status { get; set; } = StatusUsuario.EmAtivacao;
    public virtual DateTime DataHoraCadastro { get; init; } = DateTime.Now;
    public virtual long CodigoAtivacao { get; set; }
    public virtual bool PermiteReceberNotificacoes { get; set; }

    protected Usuario() { }

    public Usuario(CadastroUsuarioDto dto, long codigoAtivacao)
	{
        Nome = dto.Nome;
        Email = dto.Email;
        Celular = dto.Celular;
        Senha = dto.Senha.CriptografarSenha();
        PermiteReceberNotificacoes = dto.PermiteReceberNotificacoes;
        CodigoAtivacao = codigoAtivacao;
	}
}

