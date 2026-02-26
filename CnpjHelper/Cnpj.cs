using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CnpjHelper
{
    /// <summary>
    /// Fornece métodos para validação, geração e formatação de CNPJs alfanuméricos.
    /// </summary>
    public static class Cnpj
    {
        private const int TamanhoCnpj = 14;
        private const int TamanhoBase = 12;
        private const int ValorAsciiZero = 48;
        private const int ModuloCalculo = 11;
        private const int LimiteResto = 2;

        private static readonly Regex CnpjRegex = new Regex(@"^[0-9A-Z]{12}[0-9]{2}$", RegexOptions.Compiled);
        private static readonly Regex FormatacaoRegex = new Regex(@"[.\-/]", RegexOptions.Compiled);
        private static readonly char[] ValoresAlfanumericos = new char[]
        {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// Valida um CNPJ alfanumérico verificando seu formato e dígitos verificadores.
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado (com ou sem formatação).</param>
        /// <returns>True se o CNPJ for válido; caso contrário, false.</returns>
        /// <example>
        /// <code>
        /// bool ehValido = Cnpj.Validar("PM.VT1.7GD/0001-44"); // true
        /// bool ehValido2 = Cnpj.Validar("PMVT17GD000144"); // true
        /// </code>
        /// </example>
        public static bool Validar(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return false;

            cnpj = RemoverFormatacao(cnpj);

            if (!CnpjRegex.IsMatch(cnpj))
                return false;

            if (cnpj.All(c => c == cnpj[0]))
                return false;

            (int d1, int d2) = CalcularDigitos(cnpj);

            return cnpj.EndsWith($"{d1}{d2}");
        }

        /// <summary>
        /// Gera um CNPJ alfanumérico válido aleatório.
        /// </summary>
        /// <param name="gerarMatriz">Se true (padrão), gera CNPJ matriz com "0001"; se false, gera com número de estabelecimento aleatório.</param>
        /// <param name="comPontuacao">Se true, retorna o CNPJ formatado (XX.XXX.XXX/XXXX-XX); caso contrário, sem formatação.</param>
        /// <returns>CNPJ válido gerado aleatoriamente.</returns>
        /// <example>
        /// <code>
        /// string cnpjMatriz = Cnpj.Gerar(); // "PMVT17GD000144" (matriz com 0001)
        /// string cnpjFilial = Cnpj.Gerar(gerarMatriz: false); // "PMVT17GD5VEK98" (filial aleatória)
        /// string cnpjFormatado = Cnpj.Gerar(comPontuacao: true); // "PM.VT1.7GD/0001-44"
        /// </code>
        /// </example>
        public static string Gerar(bool gerarMatriz = true, bool comPontuacao = false)
        {
            char[] cnpjArray = new char[TamanhoBase];

            // Gera os primeiros 8 caracteres (base do CNPJ)
            for (int i = 0; i < 8; i++)
                cnpjArray[i] = ValoresAlfanumericos[GerarNumeroAleatorio(0, ValoresAlfanumericos.Length)];

            // Gera os próximos 4 caracteres (número do estabelecimento)
            if (gerarMatriz)
            {
                // Matriz sempre usa "0001"
                cnpjArray[8] = '0';
                cnpjArray[9] = '0';
                cnpjArray[10] = '0';
                cnpjArray[11] = '1';
            }
            else
            {
                // Filial com número aleatório
                for (int i = 8; i < TamanhoBase; i++)
                    cnpjArray[i] = ValoresAlfanumericos[GerarNumeroAleatorio(0, ValoresAlfanumericos.Length)];
            }

            string cnpjBase = new string(cnpjArray);
            (int d1, int d2) = CalcularDigitos(cnpjBase);

            string cnpjCompleto = cnpjBase + d1.ToString() + d2.ToString();

            return comPontuacao ? Formatar(cnpjCompleto)! : cnpjCompleto;
        }

        /// <summary>
        /// Gera uma filial válida a partir de um CNPJ matriz.
        /// </summary>
        /// <param name="cnpjMatriz">CNPJ da matriz (com ou sem formatação).</param>
        /// <param name="comPontuacao">Se true, retorna o CNPJ formatado; caso contrário, sem formatação.</param>
        /// <returns>CNPJ de filial válido ou null se o CNPJ matriz for inválido.</returns>
        /// <example>
        /// <code>
        /// string filial = Cnpj.GerarFilial("PMVT17GD000144"); // "PMVT17GD5VEK98"
        /// string filialFormatada = Cnpj.GerarFilial("PM.VT1.7GD/0001-44", comPontuacao: true); // "PM.VT1.7GD/5VEK-98"
        /// </code>
        /// </example>
        public static string? GerarFilial(string cnpjMatriz, bool comPontuacao = false)
        {
            if (!Validar(cnpjMatriz))
                return null;

            cnpjMatriz = RemoverFormatacao(cnpjMatriz);

            string baseCnpj = cnpjMatriz[..8];

            char[] numeroFilial = new char[4];
            for (int i = 0; i < 4; i++)
                numeroFilial[i] = ValoresAlfanumericos[GerarNumeroAleatorio(0, ValoresAlfanumericos.Length)];

            string cnpjBase = baseCnpj + new string(numeroFilial);
            (int d1, int d2) = CalcularDigitos(cnpjBase);

            string cnpjCompleto = cnpjBase + d1.ToString() + d2.ToString();

            return comPontuacao ? Formatar(cnpjCompleto) : cnpjCompleto;
        }

        /// <summary>
        /// Formata um CNPJ no padrão XX.XXX.XXX/XXXX-XX.
        /// </summary>
        /// <param name="cnpj">CNPJ a ser formatado (com ou sem formatação prévia).</param>
        /// <returns>CNPJ formatado ou null se o CNPJ for inválido.</returns>
        /// <example>
        /// <code>
        /// string formatado = Cnpj.Formatar("PMVT17GD000144"); // "PM.VT1.7GD/0001-44"
        /// </code>
        /// </example>
        public static string? Formatar(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return null;

            cnpj = RemoverFormatacao(cnpj);

            if (cnpj.Length != TamanhoCnpj)
                return null;

            return $"{cnpj[..2]}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
        }

        /// <summary>
        /// Remove a formatação de um CNPJ (pontos, traços e barras).
        /// </summary>
        /// <param name="cnpj">CNPJ com ou sem formatação.</param>
        /// <returns>CNPJ sem formatação.</returns>
        /// <example>
        /// <code>
        /// string semFormatacao = Cnpj.RemoverFormatacao("PM.VT1.7GD/0001-44"); // "PMVT17GD000144"
        /// </code>
        /// </example>
        public static string RemoverFormatacao(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return cnpj;

            return FormatacaoRegex.Replace(cnpj, "").ToUpperInvariant();
        }

        private static (int digito1, int digito2) CalcularDigitos(string cnpjBase)
        {
            int[] multiplicador1 = new int[TamanhoBase] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[TamanhoBase + 1] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;
            for (int i = 0; i < TamanhoBase; i++)
                soma += (cnpjBase[i] - ValorAsciiZero) * multiplicador1[i];
            int resto = (soma % ModuloCalculo);
            resto = (resto < LimiteResto) ? 0 : ModuloCalculo - resto;
            int digito1 = resto;

            string cnpjComPrimeiroDigito = cnpjBase + digito1;

            soma = 0;
            for (int i = 0; i < TamanhoBase + 1; i++)
                soma += (cnpjComPrimeiroDigito[i] - ValorAsciiZero) * multiplicador2[i];
            resto = (soma % ModuloCalculo);
            resto = (resto < LimiteResto) ? 0 : ModuloCalculo - resto;
            int digito2 = resto;

            return (digito1, digito2);
        }

        private static int GerarNumeroAleatorio(int min, int max)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] data = new byte[4];
            rng.GetBytes(data);
            int value = Math.Abs(BitConverter.ToInt32(data, 0));
            return min + (value % (max - min));
        }
    }
}
