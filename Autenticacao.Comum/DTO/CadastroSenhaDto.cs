namespace Autenticacao.Comum.DTO;

public record CadastroSenhaDto
{
    [Required(ErrorMessage = "Senha deve ser informado.")]
    [RegularExpression("^(?=.*?[0-9])(?=.*?[#?!@$%^&*-+{}<>/]).{8,15}$",
        ErrorMessage = "A senha ter conter entre 8 e 15 dígitos, caracter especial e número.")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "Senha para confirmar deve ser informado.")]
    public string ConfirmacaoSenha { get; set; }
}