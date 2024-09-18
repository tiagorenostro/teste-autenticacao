namespace Autenticacao.Comum.Dominio;

public class ResultadoValidacao
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }

    public static ResultadoValidacao EmCasoSucesso() => new(){ Sucesso = true };
    public static ResultadoValidacao EmCasoErro(string mensagem) => new() { Sucesso = false, Mensagem = mensagem };
}