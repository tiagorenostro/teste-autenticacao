namespace Autenticacao.Comum.Configuracao;

public class ConfiguracaoRabbitMQ
{
	public string Host { get; set; }
    public ushort Porta { get; set; }
    public string Usuario { get; set; }
    public string Senha { get; set; }
    public string VirtualHost { get; set; }

}