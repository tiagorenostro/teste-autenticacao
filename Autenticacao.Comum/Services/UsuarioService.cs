namespace Autenticacao.Comum.Services;

public class UsuarioService(UsuarioRepositorio usuarioRepositorio) : IUsuarioService
{
    private const string MensagemEmailCadastrado = "E-mail já cadastrado.";
    private const string MensagemCelularCadastrado = "Número de celular já cadastrado.";
    private const string MensagemCadastradoNaoEncontradoAtivar = "Cadastro não encontrado para ativação.";
    private const string MensagemCodigoAtivacaoInvalido = "Código de ativação inválido";
    private const string MensagemCadastroNaoEncontradoAlterarSenha = "Não possui cadastrado para redefinir a senha";
    private const string MensagemCadastroJaAtivo = "Cadastro já ativo.";
    private const string MensagemCadastroNaoEncontrado = "Verifique seu login ou se você é novo por aqui, realize seu cadastro.";
    private const string MensagemCadastroNaoAtivo = "Seu cadastro não está ativo. Verifique seu e-mail.";
    private const string MensagemSenhaInvalida = "Senha inválida.";

	public Usuario CriarUsuario(CadastroUsuarioDto dto)
    {
        var usuario = new Usuario(dto, GerarCodigoAtivacao());
        usuarioRepositorio.SalvarUsuario(usuario);
        
        return usuario;
    }

    public ResultadoValidacao EfetuarLogin(LoginDto loginDto)
    {
        Usuario usuario = null;
        
        if (loginDto.Login.EhEmail()) 
            usuario = usuarioRepositorio.ObterUsuarioPorEmail(loginDto.Login);

        if (loginDto.Login.EhCelular())
            usuario = usuarioRepositorio.ObterUsuarioPorCelular(loginDto.Login.AplicarMascara("00000-0000"));

        if (usuario is null)
            return ResultadoValidacao.EmCasoErro(MensagemCadastroNaoEncontrado);

        if (usuario.Status == StatusUsuario.EmAtivacao)
            return ResultadoValidacao.EmCasoErro(MensagemCadastroNaoAtivo);
        
        if (!loginDto.Senha.CriptografarSenha().Equals(usuario.Senha))
            return ResultadoValidacao.EmCasoErro(MensagemSenhaInvalida);

        return ResultadoValidacao.EmCasoSucesso();
    }
    
    public ResultadoValidacao ExisteCadastro(string email, string celular)
    {        
        if (usuarioRepositorio.ExisteEmailCadastrado(email))
            return ResultadoValidacao.EmCasoErro(MensagemEmailCadastrado);

        if (usuarioRepositorio.ExisteCelularCadastrado(celular))
            return ResultadoValidacao.EmCasoErro(MensagemCelularCadastrado);

        return ResultadoValidacao.EmCasoSucesso();
    }

    public ResultadoValidacao AtivarUsuario(long usuarioId, long codigoAtivacao)
    {   
        var usuario = usuarioRepositorio.ObterUsuarioPorId(usuarioId);

        if (usuario is null)
            return ResultadoValidacao.EmCasoErro(MensagemCadastradoNaoEncontradoAtivar);

        if (usuario.Status == StatusUsuario.Ativo)
            return ResultadoValidacao.EmCasoErro(MensagemCadastroJaAtivo);

        if (codigoAtivacao != usuario.CodigoAtivacao)
            return ResultadoValidacao.EmCasoErro(MensagemCodigoAtivacaoInvalido);

        usuario.Status = StatusUsuario.Ativo;
        usuarioRepositorio.SalvarUsuario(usuario);

        return ResultadoValidacao.EmCasoSucesso();
    }

    public ResultadoValidacao AlterarSenha(long usuarioId, string novaSenha)
    {   
        var usuario = usuarioRepositorio.ObterUsuarioPorId(usuarioId);

        if (usuario is null)
            return ResultadoValidacao.EmCasoErro(MensagemCadastroNaoEncontradoAlterarSenha);

        usuario.Senha = novaSenha.CriptografarSenha();
        usuarioRepositorio.SalvarUsuario(usuario);

        return ResultadoValidacao.EmCasoSucesso();
    }

    public bool ExisteEmailCadastrado(string email) => usuarioRepositorio.ExisteEmailCadastrado(email);

    private static long GerarCodigoAtivacao() => new Random().NextInt64(100_000, 1_000_000);
}

