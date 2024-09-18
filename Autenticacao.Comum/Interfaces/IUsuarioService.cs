namespace Autenticacao.Comum.Interfaces;

public interface IUsuarioService
{
    Usuario CriarUsuario(CadastroUsuarioDto dto);
    ResultadoValidacao EfetuarLogin(LoginDto loginDto);
    ResultadoValidacao ExisteCadastro(string email, string celular);
    ResultadoValidacao AtivarUsuario(long usuarioId, long codigoAtivacao);
    ResultadoValidacao AlterarSenha(long usuarioId, string novaSenha);
    bool ExisteEmailCadastrado(string email);
}