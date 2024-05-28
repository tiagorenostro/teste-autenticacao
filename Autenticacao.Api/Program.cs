var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder();

var configuracao = InjecaoDependencia.Registrar(builder.Services, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseNLog();

builder.Services.AddMassTransit(x =>
{
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

builder.Services.AddTransient<IUsuarioService, UsuarioService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ping", () => "pong");

app.MapPost("/login", (LoginDto loginDto, IUsuarioService usuarioService) =>
{
    try
    {
        return usuarioService.EfetuarLogin(loginDto, out var mensagem) ? 
            Results.Ok("Login efetuado com sucesso.") :
            Results.Json(new ErroDto(mensagem), contentType: MediaTypeNames.Application.Json, 
                statusCode: StatusCodes.Status401Unauthorized);
    }
    catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Json(new ErroDto("Ocorreu um erro inesperado ao efetuar o login. Tente novamente."), 
            contentType: MediaTypeNames.Application.Json,
            statusCode: (int)HttpStatusCode.InternalServerError);
    }
});

app.MapPost("/usuario/cadastrar", async (CadastroUsuarioDto cadastroUsuarioDto,
    IPublishEndpoint publicador, IUsuarioService usuarioService) =>
{
    try
    {
        if (cadastroUsuarioDto is null)
            return Results.BadRequest(new ErroDto("Dados para cadastro de usuário inválidos."));

        if (!MiniValidator.TryValidate(cadastroUsuarioDto, out var erros))
            return Results.BadRequest(new ErroDto(erros.Select(x => x.Value.Select(y => new Campo(x.Key, y))).SelectMany(x => x)));

        if (!cadastroUsuarioDto.AceitoPoliticaPrivacidade)
            return Results.BadRequest(new ErroDto("Termo de politica privacidade não aceito."));

        if (!cadastroUsuarioDto.Senha.Equals(cadastroUsuarioDto.ConfirmacaoSenha))
            return Results.BadRequest(new ErroDto("Senhas não conferem."));

        if (usuarioService.ExisteCadastro(cadastroUsuarioDto.Email, cadastroUsuarioDto.Celular, out var mensagem))
            return Results.Json(new ErroDto(mensagem), contentType: MediaTypeNames.Application.Json, statusCode: StatusCodes.Status412PreconditionFailed);
        
        var usuario = usuarioService.CriarUsuario(cadastroUsuarioDto);

        await publicador.Publish(new EnviarCodigoAtivacao
        {
            Email = usuario.Email,
            CodigoAtivacao = usuario.CodigoAtivacao
        });
            
        return Results.Ok(new UsuarioIdDto
        {
            Id = usuario.Id
        });
    }
    catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Json(new ErroDto("Ocorreu um erro inesperado ao efetuar o cadastro. Tente novamente."), 
            contentType: MediaTypeNames.Application.Json,
            statusCode: (int)HttpStatusCode.InternalServerError);
    }
});

app.MapPost("/usuario/recuperar/senha", async (RecuperarSenhaDto recuperarSenhaDto,
    IPublishEndpoint publicador, IUsuarioService usuarioService) =>
{
    try
    {
        if (recuperarSenhaDto is null)
            return Results.BadRequest(new ErroDto("E-mail para a recuperação deve ser informado."));

        if (!MiniValidator.TryValidate(recuperarSenhaDto, out var erros))
            return Results.BadRequest(new ErroDto(erros.Select(x => x.Value.Select(y => new Campo(x.Key, y))).SelectMany(x => x)));

        if (usuarioService.ExisteEmailCadastrado(recuperarSenhaDto.Email))
            await publicador.Publish(new RecuperarSenha
            {
                Email = recuperarSenhaDto.Email
            });
        
        return Results.Accepted(value: "Em breve será enviado um e-mail para recuperação.");
    }
    catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Json(new ErroDto("Ocorreu um erro inesperado para enviar o e-mail de recuperação de senha. Tente novamente."), 
            contentType: MediaTypeNames.Application.Json,
            statusCode: (int)HttpStatusCode.InternalServerError);
    }
});

app.MapPut("/usuario/{usuarioId:long}/ativar/{codigoAtivacao:long}", (long usuarioId, long codigoAtivacao,
    IUsuarioService usuarioService) =>
{
    try
    {
        return usuarioService.AtivarUsuario(usuarioId, codigoAtivacao, out var mensagem) ?
            Results.Ok("Cadastro ativado com sucesso.") :
            Results.BadRequest(new ErroDto(mensagem));
    }
    catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Json(new ErroDto("Ocorreu um erro inesperado ao ativar sua conta. Tente novamente."), 
            contentType: MediaTypeNames.Application.Json,
            statusCode: (int)HttpStatusCode.InternalServerError);
    }
});

app.MapPut("/usuario/{usuarioId:long}/alterar/senha", (long usuarioId, 
    CadastroSenhaDto redefinirSenhaDto, IUsuarioService usuarioService) =>
{
    try
    {
        if (redefinirSenhaDto is null)
            return Results.BadRequest(new ErroDto("Novas senhas para alteração inválidos."));

        if (!MiniValidator.TryValidate(redefinirSenhaDto, out var erros))
            return Results.BadRequest(new ErroDto(erros.Select(x => x.Value.Select(y => new Campo(x.Key, y))).SelectMany(x => x)));

        if (!redefinirSenhaDto.Senha.Equals(redefinirSenhaDto.ConfirmacaoSenha))
            return Results.BadRequest(new ErroDto("Senhas não conferem."));

        return usuarioService.AlterarSenha(usuarioId, redefinirSenhaDto.Senha, out var mensagem) ? 
            Results.Ok("Senha alterada com sucesso.") :
            Results.BadRequest(new ErroDto(mensagem));
    }
    catch (Exception ex)
    {
        logger.Error(ex);
        return Results.Json(new ErroDto("Ocorreu um erro inesperado ao alterar a senha. Tente novamente."), contentType: MediaTypeNames.Application.Json,
            statusCode: (int)HttpStatusCode.InternalServerError);
    }
});

app.Run();

public partial class Program;