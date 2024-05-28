namespace Autenticacao.Comum.Mensagem;

public sealed record EnviarCodigoAtivacao
{
	public long CodigoAtivacao { get; set; }
	public string Email { get; set; }
}

