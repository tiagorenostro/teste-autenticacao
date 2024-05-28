namespace Autenticacao.Consumidor.Definition;

public class RecuperarSenhaDefinition : ConsumerDefinition<RecuperarSenhaConsumidor>
{
	private const string NomeEndpoint = "autenticacao.recuperar.senha";

	public RecuperarSenhaDefinition() => EndpointName = NomeEndpoint;

	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RecuperarSenhaConsumidor> consumerConfigurator, IRegistrationContext context) =>
		endpointConfigurator.UseMessageRetry(static c => c.Interval(5, TimeSpan.FromMicroseconds(500)));
}

