namespace Autenticacao.Consumidor.Definition;

public class EnviarCodigoAtivacaoDefinition : ConsumerDefinition<EnviarCodigoAtivacaoConsumidor>
{
	private const string NomeEndpoint = "autenticacao.enviar.codigo.ativacao";

	public EnviarCodigoAtivacaoDefinition() => EndpointName = NomeEndpoint;
		
	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EnviarCodigoAtivacaoConsumidor> consumerConfigurator, IRegistrationContext context) =>
		endpointConfigurator.UseMessageRetry(static c => c.Interval(5, TimeSpan.FromMicroseconds(500)));
}

