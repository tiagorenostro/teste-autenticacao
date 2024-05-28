namespace Autenticacao.Comum.Interfaces;

public interface IUsuarioService
{
    Usuario CriarUsuario(CadastroUsuarioDto dto);
    bool EfetuarLogin(LoginDto loginDto, out string mensagem);
    bool ExisteCadastro(string email, string celular, out string mensagem);
    bool AtivarUsuario(long usuarioId, long codigoAtivacao, out string mensagem);
    bool AlterarSenha(long usuarioId, string novaSenha, out string mensagem);
    bool ExisteEmailCadastrado(string email);
}