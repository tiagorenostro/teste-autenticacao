namespace Autenticacao.Comum.DTO;

public sealed record CadastroUsuarioDto : CadastroSenhaDto
{
	[Required(ErrorMessage = "Nome deve ser informado.")]
	[MaxLength(255, ErrorMessage = "Tamanho máximo é 255 caracteres.")]
	public string Nome { get; set; }

    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [MaxLength(255, ErrorMessage = "Tamanho máximo é 255 caracteres.")]
    [Required(ErrorMessage = "E-mail deve ser informado.")]
    public string Email { get; set; }

    [RegularExpression(@"\([1-9]{2}\) 9[7-9]{1}[0-9]{3}\-[0-9]{4}$", ErrorMessage = "Número do celular inválido ou com formatação incorreta (00) 00000-0000.")]
    [Required(ErrorMessage = "Número do celular deve ser informado.")]
    public string Celular { get; set; }

	public bool AceitoPoliticaPrivacidade { get; set; }
	public bool PermiteReceberNotificacoes { get; set; }
}