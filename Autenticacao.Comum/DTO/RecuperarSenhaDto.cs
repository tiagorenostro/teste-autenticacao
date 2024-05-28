namespace Autenticacao.Comum.DTO;

public sealed record RecuperarSenhaDto
{
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [Required(ErrorMessage = "E-mail para a recuperação deve ser informado.")]
    public string Email { get; set; }
}