namespace Autenticacao.Comum.Configuracao;

public sealed record ConfiguracaoEmail
{
    public string Host { get; set; }
    public int Porta { get; set; }
    public string Usuario { get; set; }
    public string Senha { get; set; }
}