namespace Autenticacao.Comum.DTO;

public record ErroDto
{
	public string Detalhe { get; set; }
	public IEnumerable<Campo> Campos { get; set; }

	public ErroDto(string detalhe) => Detalhe = detalhe;
	public ErroDto(IEnumerable<Campo> campos) => Campos = campos;
}

public sealed record Campo(string Nome, string Mensagem)
{
	public string Nome { get; set; } = Nome;
	public string Mensagem { get; set; } = Mensagem;
}

