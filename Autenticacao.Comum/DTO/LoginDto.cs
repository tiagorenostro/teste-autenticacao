namespace Autenticacao.Comum.DTO;

public sealed record LoginDto
{
    [Required(ErrorMessage = "E-mail ou número de celular deve ser informado.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Senha para o login deve ser informado.")]
    public string Senha { get; set; }
}

