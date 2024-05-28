namespace Autenticacao.Comum.Extensions;

public static class StringExtension
{
    public static bool EhCelular(this string celular) => long.TryParse(celular, out _);

    public static bool EhEmail(this string email) => email.Contains('@') && email.Contains('.');
    
    public static string AplicarMascara(this string texto, string mascara)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return null;

        var textoFormatado = new MaskedTextProvider(mascara);
        _ = textoFormatado.Add(texto, out _, out _);

        return textoFormatado.ToString();
    }
    
    public static string CriptografarSenha(this string input)
    {
        var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hash = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        return sb.ToString();
    }
}