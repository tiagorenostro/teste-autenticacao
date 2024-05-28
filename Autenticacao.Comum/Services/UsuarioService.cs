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

	public Usuario CriarUsuario(CadastroUsuarioDto dto)
    {
        var usuario = new Usuario(dto, GerarCodigoAtivacao());
        usuarioRepositorio.SalvarUsuario(usuario);
        
        return usuario;
    }

    public bool EfetuarLogin(LoginDto loginDto, out string mensagem)
    {
        mensagem = "";

        Usuario usuario = null;
        
        if (loginDto.Login.EhEmail()) 
            usuario = usuarioRepositorio.ObterUsuarioPorEmail(loginDto.Login);

        if (loginDto.Login.EhCelular())
            usuario = usuarioRepositorio.ObterUsuarioPorCelular(loginDto.Login.AplicarMascara("00000-0000"));

        if (usuario is null)
        {
            mensagem = MensagemCadastroNaoEncontrado;
            return false;
        }

        if (usuario.Status == StatusUsuario.EmAtivacao)
        {
            mensagem = MensagemCadastroNaoAtivo;
            return false;
        }
        
        if (!loginDto.Senha.CriptografarSenha().Equals(usuario.Senha))
            return false;

        return true;
    }
    
    public bool ExisteCadastro(string email, string celular, out string mensagem)
    {
        mensagem = "";
        
        if (usuarioRepositorio.ExisteEmailCadastrado(email))
        {
            mensagem = MensagemEmailCadastrado;
            return true;
        }

        if (usuarioRepositorio.ExisteCelularCadastrado(celular))
        {
            mensagem = MensagemCelularCadastrado;
            return true;
        }

        return false;
    }

    public bool AtivarUsuario(long usuarioId, long codigoAtivacao, out string mensagem)
    {
        mensagem = "";
        
        var usuario = usuarioRepositorio.ObterUsuarioPorId(usuarioId);

        if (usuario is null)
        {
            mensagem = MensagemCadastradoNaoEncontradoAtivar;
            return false;
        }

        if (usuario.Status == StatusUsuario.Ativo)
        {
            mensagem = MensagemCadastroJaAtivo;
            return false;
        }

        if (codigoAtivacao != usuario.CodigoAtivacao)
        {
            mensagem = MensagemCodigoAtivacaoInvalido;
            return false;
        }

        usuario.Status = StatusUsuario.Ativo;
        usuarioRepositorio.SalvarUsuario(usuario);

        return true;
    }

    public bool AlterarSenha(long usuarioId, string novaSenha, out string mensagem)
    {
        mensagem = "";
        
        var usuario = usuarioRepositorio.ObterUsuarioPorId(usuarioId);

        if (usuario is null)
        {
            mensagem = MensagemCadastroNaoEncontradoAlterarSenha;
            return false;
        }

        usuario.Senha = novaSenha.CriptografarSenha();
        usuarioRepositorio.SalvarUsuario(usuario);

        return true;
    }

    public bool ExisteEmailCadastrado(string email) => usuarioRepositorio.ExisteEmailCadastrado(email);

    private static long GerarCodigoAtivacao() => new Random().NextInt64(100_000, 1_000_000);
}

