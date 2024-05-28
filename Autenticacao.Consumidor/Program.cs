await Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        var configuracao = InjecaoDependencia.Registrar(services, hostContext.Configuration);

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddNLog(hostContext.Configuration);
        });
        
        services.AddTransient<IEmailService, EmailService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<RecuperarSenhaConsumidor, RecuperarSenhaDefinition>();
            x.AddConsumer<EnviarCodigoAtivacaoConsumidor, EnviarCodigoAtivacaoDefinition>();

            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuracao.RabbitMQ.Host, configuracao.RabbitMQ.Porta, configuracao.RabbitMQ.VirtualHost, h =>
                {
                    h.Username(configuracao.RabbitMQ.Usuario);
                    h.Password(configuracao.RabbitMQ.Senha);
                });

                config.Durable = true;
                config.ConfigureEndpoints(context);
            });
        });
    })
    .Build()
    .RunAsync();
    