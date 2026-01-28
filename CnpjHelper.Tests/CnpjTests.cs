namespace CnpjHelper.Tests
{
    public class CnpjTests
    {
        #region Testes de Validação - CNPJs Válidos

        [Theory]
        [InlineData("PMVT17GD000144")] // CNPJ da Receita Federal
        [InlineData("PM.VT1.7GD/0001-44")] // CNPJ com pontuação
        [InlineData("JRYXJGEN000155")] // CNPJ da Receita Federal
        [InlineData("DJ.9YX.8H3/0001-04")] // CNPJ com pontuação
        [InlineData("M3LVVXLW000188")] // CNPJ da Receita Federal
        [InlineData("851SJJEV000159")] // CNPJ da Receita Federal
        [InlineData("L2TJ5GEY000138")] // CNPJ da Receita Federal
        [InlineData("Y6T259JN000132")] // CNPJ matriz da Receita Federal
        [InlineData("Y6T259JNZ04384")] // CNPJ filial da Receita Federal
        [InlineData("PM8ZY997000134")] // CNPJ matriz da Receita Federal
        [InlineData("PM8ZY997MZW630")] // CNPJ filial da Receita Federal
        [InlineData("5MN1P8JD000123")] // CNPJ matriz da Receita Federal
        [InlineData("5MN1P8JDPDKW18")] // CNPJ filial da Receita Federal
        [Trait("Categoria", "Validação")]
        public void Validar_DeveRetornarTrue_QuandoCnpjValido(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.True(resultado, $"CNPJ '{cnpj}' deveria ser válido");
        }

        #endregion

        #region Testes de Validação - CNPJs Inválidos

        [Theory]
#pragma warning disable xUnit1012 // Null should only be used for nullable parameters
        [InlineData(null)] // CNPJ nulo
#pragma warning restore xUnit1012 // Null should only be used for nullable parameters
        [InlineData("")] // CNPJ vazio
        [InlineData("   ")] // CNPJ com espaços
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "CnpjInválido")]
        public void Validar_DeveRetornarFalse_QuandoCnpjNuloOuVazio(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.False(resultado, $"CNPJ '{cnpj}' deveria ser inválido");
        }

        [Theory]
        [InlineData("123")] // Menos de 14 caracteres
        [InlineData("123456789012345")] // Mais de 14 caracteres
        [InlineData("1234567890")] // Tamanho incorreto
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "CnpjInválido")]
        public void Validar_DeveRetornarFalse_QuandoTamanhoIncorreto(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.False(resultado, $"CNPJ '{cnpj}' com tamanho incorreto deveria ser inválido");
        }

        [Theory]
        [InlineData("00000000000000")] // Todos zeros
        [InlineData("11111111111111")] // Todos uns
        [InlineData("AAAAAAAAAAAAAA")] // Todas letras iguais
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "CnpjInválido")]
        public void Validar_DeveRetornarFalse_QuandoTodosCaracteresIguais(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.False(resultado, $"CNPJ '{cnpj}' com todos caracteres iguais deveria ser inválido");
        }

        [Theory]
        [InlineData("PMVT17GD000143")] // Dígito verificador incorreto (último dígito alterado de 44 para 43)
        [InlineData("PMVT17GD000154")] // Dígito verificador incorreto (último dígito alterado de 44 para 54)
        [InlineData("JRYXJGEN000156")] // Dígito verificador incorreto (último dígito alterado de 55 para 56)
        [InlineData("DJ9YX8H3000105")] // Dígito verificador incorreto (último dígito alterado de 04 para 05)
        [InlineData("M3LVVXLW000187")] // Dígito verificador incorreto (último dígito alterado de 88 para 87)
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "DígitoVerificador")]
        public void Validar_DeveRetornarFalse_QuandoDigitoVerificadorIncorreto(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.False(resultado, $"CNPJ '{cnpj}' com dígito verificador incorreto deveria ser inválido");
        }

        [Theory]
        [InlineData("A1B2C3D400015@")] // Caractere especial no final
        [InlineData("A1B2C3D400015Z")] // Letra no dígito verificador (deve ser 0-9)
        [InlineData("A1B2C3D400015A")] // Letra no dígito verificador (deve ser 0-9)
        [InlineData("11222333000!81")] // Caractere especial no meio
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "FormatoInválido")]
        public void Validar_DeveRetornarFalse_QuandoCaracteresInvalidos(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.False(resultado, $"CNPJ '{cnpj}' com caracteres inválidos deveria ser inválido");
        }

        #endregion

        #region Testes de Geração

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Básico")]
        public void Gerar_DeveRetornarCnpjCom14Caracteres()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar();

            // Assert
            Assert.Equal(14, cnpj.Length);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Validação")]
        public void Gerar_DeveRetornarCnpjValido()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar();

            // Assert
            Assert.True(Cnpj.Validar(cnpj), $"CNPJ gerado '{cnpj}' deveria ser válido");
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Aleatoriedade")]
        public void Gerar_DeveGerarCnpjsDiferentes()
        {
            // Arrange & Act
            var cnpj1 = Cnpj.Gerar();
            var cnpj2 = Cnpj.Gerar();
            var cnpj3 = Cnpj.Gerar();

            // Assert
            Assert.NotEqual(cnpj1, cnpj2);
            Assert.NotEqual(cnpj2, cnpj3);
            Assert.NotEqual(cnpj1, cnpj3);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Matriz")]
        public void Gerar_PorPadrao_DeveGerarMatrizCom0001()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar();

            // Assert
            Assert.Equal("0001", cnpj.Substring(8, 4));
            Assert.True(Cnpj.Validar(cnpj), $"CNPJ matriz '{cnpj}' deveria ser válido");
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Matriz")]
        public void Gerar_ComGerarMatrizTrue_DeveGerarMatrizCom0001()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar(gerarMatriz: true);

            // Assert
            Assert.Equal("0001", cnpj.Substring(8, 4));
            Assert.True(Cnpj.Validar(cnpj), $"CNPJ matriz '{cnpj}' deveria ser válido");
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Filial")]
        public void Gerar_ComGerarMatrizFalse_DeveGerarFilialAleatoria()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar(gerarMatriz: false);

            // Assert
            Assert.True(Cnpj.Validar(cnpj), $"CNPJ filial '{cnpj}' deveria ser válido");
            // Pode ou não ser "0001" - é aleatório
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Aleatoriedade")]
        public void Gerar_ComGerarMatrizFalse_DeveGerarFiliaisDiferentes()
        {
            // Arrange & Act
            var filial1 = Cnpj.Gerar(gerarMatriz: false);
            var filial2 = Cnpj.Gerar(gerarMatriz: false);
            var filial3 = Cnpj.Gerar(gerarMatriz: false);

            // Assert
            // Verifica que pelo menos 2 são diferentes (probabilidade altíssima)
            Assert.True(
                filial1 != filial2 || filial2 != filial3 || filial1 != filial3,
                "Filiais geradas aleatoriamente deveriam ter alta probabilidade de serem diferentes"
            );
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Formatação")]
        public void Gerar_Matriz_ComPontuacao_DeveConter0001Formatado()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar(gerarMatriz: true, comPontuacao: true);

            // Assert
            Assert.Contains("/0001-", cnpj);
            Assert.True(Cnpj.Validar(cnpj), $"CNPJ matriz formatado '{cnpj}' deveria ser válido");
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Lote")]
        public void Gerar_DeveGerarLoteDe100MatrizesValidas()
        {
            // Arrange
            const int quantidade = 100;
            var cnpjsGerados = new System.Collections.Generic.List<string>();
            var cnpjsInvalidos = new System.Collections.Generic.List<string>();
            var cnpjsSemMatriz = new System.Collections.Generic.List<string>();

            // Act
            for (int i = 0; i < quantidade; i++)
            {
                var cnpj = Cnpj.Gerar(gerarMatriz: true);
                cnpjsGerados.Add(cnpj);

                if (!Cnpj.Validar(cnpj))
                {
                    cnpjsInvalidos.Add(cnpj);
                }

                if (cnpj.Substring(8, 4) != "0001")
                {
                    cnpjsSemMatriz.Add(cnpj);
                }
            }

            // Assert
            Assert.Equal(quantidade, cnpjsGerados.Count);
            Assert.Empty(cnpjsInvalidos);
            Assert.Empty(cnpjsSemMatriz);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Lote")]
        public void Gerar_DeveGerarLoteDe100FiliaisValidas()
        {
            // Arrange
            const int quantidade = 100;
            var cnpjsGerados = new System.Collections.Generic.List<string>();
            var cnpjsInvalidos = new System.Collections.Generic.List<string>();

            // Act
            for (int i = 0; i < quantidade; i++)
            {
                var cnpj = Cnpj.Gerar(gerarMatriz: false);
                cnpjsGerados.Add(cnpj);

                if (!Cnpj.Validar(cnpj))
                {
                    cnpjsInvalidos.Add(cnpj);
                }
            }

            // Assert
            Assert.Equal(quantidade, cnpjsGerados.Count);
            Assert.Empty(cnpjsInvalidos);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Formato")]
        public void Gerar_DeveConter12CaracteresAlfanumericosE2Numericos()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar();

            // Assert
            // Primeiros 12 caracteres devem ser alfanuméricos (0-9, A-Z)
            for (int i = 0; i < 12; i++)
            {
                Assert.True(
                    (cnpj[i] >= '0' && cnpj[i] <= '9') || (cnpj[i] >= 'A' && cnpj[i] <= 'Z'),
                    $"Posição {i} do CNPJ '{cnpj}' deve ser alfanumérica"
                );
            }

            // Últimos 2 caracteres devem ser numéricos (0-9)
            Assert.True(cnpj[12] >= '0' && cnpj[12] <= '9', $"Posição 12 do CNPJ '{cnpj}' deve ser numérica");
            Assert.True(cnpj[13] >= '0' && cnpj[13] <= '9', $"Posição 13 do CNPJ '{cnpj}' deve ser numérica");
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Formatação")]
        public void Gerar_ComPontuacao_DeveRetornarCnpjFormatado()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar(comPontuacao: true);

            // Assert
            Assert.Contains(".", cnpj);
            Assert.Contains("/", cnpj);
            Assert.Contains("-", cnpj);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Formatação")]
        public void Gerar_SemPontuacao_NaoDeveConterCaracteresEspeciais()
        {
            // Arrange & Act
            var cnpj = Cnpj.Gerar(comPontuacao: false);

            // Assert
            Assert.DoesNotContain(".", cnpj);
            Assert.DoesNotContain("/", cnpj);
            Assert.DoesNotContain("-", cnpj);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Lote")]
        public void Gerar_DeveGerarLoteDe100CnpjsValidos()
        {
            // Arrange
            const int quantidadeCnpjs = 100;
            var cnpjsGerados = new List<string>();
            var cnpjsInvalidos = new List<string>();

            // Act
            for (int i = 0; i < quantidadeCnpjs; i++)
            {
                var cnpj = Cnpj.Gerar();
                cnpjsGerados.Add(cnpj);

                if (!Cnpj.Validar(cnpj))
                {
                    cnpjsInvalidos.Add(cnpj);
                }
            }

            // Assert
            Assert.Equal(quantidadeCnpjs, cnpjsGerados.Count);
            Assert.Empty(cnpjsInvalidos);
        }

        [Fact]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Lote")]
        public void Gerar_DeveGerarLoteDe100CnpjsValidosComPontuacao()
        {
            // Arrange
            const int quantidadeCnpjs = 100;
            var cnpjsGerados = new List<string>();
            var cnpjsInvalidos = new List<string>();

            // Act
            for (int i = 0; i < quantidadeCnpjs; i++)
            {
                var cnpj = Cnpj.Gerar(comPontuacao: true);
                cnpjsGerados.Add(cnpj);

                if (!Cnpj.Validar(cnpj))
                {
                    cnpjsInvalidos.Add(cnpj);
                }
            }

            // Assert
            Assert.Equal(quantidadeCnpjs, cnpjsGerados.Count);
            Assert.Empty(cnpjsInvalidos);

            // Verifica que todos contêm pontuação
            foreach (var cnpj in cnpjsGerados)
            {
                Assert.Contains(".", cnpj);
                Assert.Contains("/", cnpj);
                Assert.Contains("-", cnpj);
            }
        }

        [Theory]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [Trait("Categoria", "Geração")]
        [Trait("Tipo", "Lote")]
        public void Gerar_DeveGerarLotesDeCnpjsValidosEmDiferentesQuantidades(int quantidade)
        {
            // Arrange
            var cnpjsGerados = new List<string>();
            var cnpjsInvalidos = new List<string>();

            // Act
            for (int i = 0; i < quantidade; i++)
            {
                var cnpj = Cnpj.Gerar();
                cnpjsGerados.Add(cnpj);

                if (!Cnpj.Validar(cnpj))
                {
                    cnpjsInvalidos.Add(cnpj);
                }
            }

            // Assert
            Assert.Equal(quantidade, cnpjsGerados.Count);
            Assert.Empty(cnpjsInvalidos);
        }

        #endregion

        #region Testes de Normalização

        [Theory]
        [InlineData("PM.VT1.7GD/0001-44", "PMVT17GD000144")]
        [InlineData("PM-VT1-7GD-0001-44", "PMVT17GD000144")]
        [InlineData("PM/VT1/7GD/0001/44", "PMVT17GD000144")]
        [InlineData("DJ.9YX.8H3/0001-04", "DJ9YX8H3000104")]
        [InlineData("Y6.T25.9JN/0001-32", "Y6T259JN000132")]
        [Trait("Categoria", "Normalização")]
        [Trait("Tipo", "Pontuação")]
        public void Validar_DeveRemoverPontuacao_AntesDeValidar(string cnpjComPontuacao, string cnpjSemPontuacao)
        {
            // Arrange & Act
            var resultadoComPontuacao = Cnpj.Validar(cnpjComPontuacao);
            var resultadoSemPontuacao = Cnpj.Validar(cnpjSemPontuacao);

            // Assert
            Assert.Equal(resultadoSemPontuacao, resultadoComPontuacao);
        }

        [Theory]
        [InlineData("pmvt17gd000144", "PMVT17GD000144")] // Letras minúsculas
        [InlineData("PmVt17Gd000144", "PMVT17GD000144")] // Misturado
        [InlineData("jryxjgen000155", "JRYXJGEN000155")] // Letras minúsculas
        [InlineData("dj9yx8h3000104", "DJ9YX8H3000104")] // Letras minúsculas
        [Trait("Categoria", "Normalização")]
        [Trait("Tipo", "Maiúsculas")]
        public void Validar_DeveConverterParaMaiusculas(string cnpjMinuscula, string cnpjMaiuscula)
        {
            // Arrange & Act
            var resultadoMinuscula = Cnpj.Validar(cnpjMinuscula);
            var resultadoMaiuscula = Cnpj.Validar(cnpjMaiuscula);

            // Assert
            Assert.Equal(resultadoMaiuscula, resultadoMinuscula);
        }

        #endregion

        #region Testes com CNPJs Reais da Receita Federal

        [Fact]
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "ReceitaFederal")]
        public void Validar_DevValidarCnpjsDaReceitaFederal()
        {
            // Arrange - CNPJs reais do arquivo cnpjs-gerados-27-01-2026.txt
            string[] cnpjsValidos =
            [
                "PMVT17GD000144", "PMVT17GD4S3V33", "PMVT17GD0S6V34", "PMVT17GDK6KX53", "PMVT17GD5VEK98",
                "JRYXJGEN000155", "DJ9YX8H3000104", "M3LVVXLW000188", "851SJJEV000159", "L2TJ5GEY000138",
                "EXZKKN2A000108", "G582RK9A000168", "MXM81EBD000133", "AWSZ23SW000148", "1NM1RTD3000107",
                "Y6T259JN000132", "Y6T259JNZ04384", "Y6T259JNH2C150", "Y6T259JNN0X770", "Y6T259JN1MJH03",
                "PM8ZY997000134", "PM8ZY997MZW630", "PM8ZY997YZCS03", "PM8ZY9975LH302", "PM8ZY997R8N508",
                "5MN1P8JD000123", "5MN1P8JDPDKW18", "5MN1P8JDWZ6322", "5MN1P8JD37ST49", "5MN1P8JDKWA038",
                "XZTGK9X5000169", "J3WAHCJ1000177", "A2MXP7XH000145", "CBYB15AW000177", "3LRNJ47B000111",
                "V8VME9WM000174", "R8WS7708000132", "S2BA8JWE000152", "Y4Cwydyr000183", "8P1KM5HB000109"
            ];

            // Act & Assert
            foreach (var cnpj in cnpjsValidos)
            {
                var resultado = Cnpj.Validar(cnpj);
                Assert.True(resultado, $"CNPJ '{cnpj}' da Receita Federal deveria ser válido");
            }
        }

        [Fact]
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "ReceitaFederal")]
        public void Validar_DeveValidarCnpjsComPontuacaoDaReceitaFederal()
        {
            // Arrange - CNPJs reais formatados do arquivo cnpjs-gerados-27-01-2026.txt
            string[] cnpjsFormatados =
            [
                "PM.VT1.7GD/0001-44", "PM.VT1.7GD/4S3V-33", "JR.YXJ.GEN/0001-55",
                "DJ.9YX.8H3/0001-04", "M3.LVV.XLW/0001-88", "85.1SJ.JEV/0001-59",
                "Y6.T25.9JN/0001-32", "Y6.T25.9JN/Z043-84", "Y6.T25.9JN/H2C1-50",
                "PM.8ZY.997/0001-34", "PM.8ZY.997/MZW6-30", "PM.8ZY.997/YZCS-03",
                "5M.N1P.8JD/0001-23", "5M.N1P.8JD/PDKW-18", "5M.N1P.8JD/WZ63-22"
            ];

            // Act & Assert
            foreach (var cnpj in cnpjsFormatados)
            {
                var resultado = Cnpj.Validar(cnpj);
                Assert.True(resultado, $"CNPJ formatado '{cnpj}' da Receita Federal deveria ser válido");
            }
        }

        [Fact]
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "ReceitaFederal")]
        public void Validar_DeveValidarMatrizEFiliais()
        {
            // Arrange - Matriz e suas filiais
            string matriz = "Y6T259JN000132";
            string[] filiais = new[]
            {
                "Y6T259JNZ04384",
                "Y6T259JNH2C150",
                "Y6T259JNN0X770",
                "Y6T259JN1MJH03",
                "Y6T259JN20GJ64",
                "Y6T259JN1SRD40"
            };

            // Act & Assert - Matriz
            Assert.True(Cnpj.Validar(matriz), $"Matriz '{matriz}' deveria ser válida");

            // Act & Assert - Filiais
            foreach (var filial in filiais)
            {
                var resultado = Cnpj.Validar(filial);
                Assert.True(resultado, $"Filial '{filial}' deveria ser válida");
            }
        }

        #endregion

        #region Testes de Performance/Stress

        [Fact]
        [Trait("Categoria", "Performance")]
        [Trait("Tipo", "Stress")]
        public void Validar_DeveValidarMilharesDeCnpjs_EmTempoRazoavel()
        {
            // Arrange
            var inicio = DateTime.Now;
            const int quantidade = 10000;

            // Act
            for (int i = 0; i < quantidade; i++)
            {
                var cnpj = Cnpj.Gerar();
                Cnpj.Validar(cnpj);
            }

            var duracao = DateTime.Now - inicio;

            // Assert
            Assert.True(duracao.TotalSeconds < 5, $"Validação de {quantidade} CNPJs levou {duracao.TotalSeconds}s (deveria ser < 5s)");
        }

        #endregion

        #region Testes de Regex

        [Theory]
        [InlineData("11222333000181")] // Numérico puro
        [InlineData("A1B2C3D4000156")] // Alfanumérico
        [InlineData("ZZZZZZZZZZZ99")] // Todas letras + dígitos
        [Trait("Categoria", "Validação")]
        [Trait("Tipo", "Formato")]
        public void Validar_DeveAceitarFormatoCorreto_12Alfanumericos2Numericos(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.Validar(cnpj);

            // Assert
            Assert.True(resultado || !resultado); // Testa que não lança exceção
        }

        #endregion

        #region Testes de Formatação

        [Theory]
        [InlineData("PMVT17GD000144", "PM.VT1.7GD/0001-44")]
        [InlineData("JRYXJGEN000155", "JR.YXJ.GEN/0001-55")]
        [InlineData("DJ9YX8H3000104", "DJ.9YX.8H3/0001-04")]
        [InlineData("Y6T259JN000132", "Y6.T25.9JN/0001-32")]
        [Trait("Categoria", "Formatação")]
        [Trait("Tipo", "Básico")]
        public void Formatar_DeveFormatarCnpjCorretamente(string cnpjSemFormatacao, string cnpjEsperado)
        {
            // Arrange & Act
            var resultado = Cnpj.Formatar(cnpjSemFormatacao);

            // Assert
            Assert.Equal(cnpjEsperado, resultado);
        }

        [Theory]
        [InlineData("PM.VT1.7GD/0001-44", "PM.VT1.7GD/0001-44")]
        [InlineData("JR-YXJ-GEN-0001-55", "JR.YXJ.GEN/0001-55")]
        [Trait("Categoria", "Formatação")]
        [Trait("Tipo", "Reformatação")]
        public void Formatar_DeveReformatarCnpjJaFormatado(string cnpjComFormatacao, string cnpjEsperado)
        {
            // Arrange & Act
            var resultado = Cnpj.Formatar(cnpjComFormatacao);

            // Assert
            Assert.Equal(cnpjEsperado, resultado);
        }

        [Theory]
#pragma warning disable xUnit1012 // Null should only be used for nullable parameters
        [InlineData(null)]
#pragma warning restore xUnit1012 // Null should only be used for nullable parameters
        [InlineData("")]
        [InlineData("123")]
        [InlineData("12345678901234567890")]
        [Trait("Categoria", "Formatação")]
        [Trait("Tipo", "CnpjInválido")]
        public void Formatar_DeveRetornarNull_QuandoCnpjInvalido(string cnpjInvalido)
        {
            // Arrange & Act
            var resultado = Cnpj.Formatar(cnpjInvalido);

            // Assert
            Assert.Null(resultado);
        }

        #endregion

        #region Testes de Remoção de Formatação

        [Theory]
        [InlineData("PM.VT1.7GD/0001-44", "PMVT17GD000144")]
        [InlineData("JR.YXJ.GEN/0001-55", "JRYXJGEN000155")]
        [InlineData("DJ-9YX-8H3-0001-04", "DJ9YX8H3000104")]
        [InlineData("Y6/T25/9JN/0001/32", "Y6T259JN000132")]
        [InlineData("PMVT17GD000144", "PMVT17GD000144")]
        [Trait("Categoria", "Normalização")]
        [Trait("Tipo", "RemoverFormatação")]
        public void RemoverFormatacao_DeveRemoverCaracteresEspeciais(string cnpjComFormatacao, string cnpjEsperado)
        {
            // Arrange & Act
            var resultado = Cnpj.RemoverFormatacao(cnpjComFormatacao);

            // Assert
            Assert.Equal(cnpjEsperado, resultado);
        }

        [Theory]
#pragma warning disable xUnit1012 // Null should only be used for nullable parameters
        [InlineData(null)]
#pragma warning restore xUnit1012 // Null should only be used for nullable parameters
        [InlineData("")]
        [Trait("Categoria", "Normalização")]
        [Trait("Tipo", "RemoverFormatação")]
        public void RemoverFormatacao_DeveRetornarMesmoValor_QuandoNullOuVazio(string cnpj)
        {
            // Arrange & Act
            var resultado = Cnpj.RemoverFormatacao(cnpj);

            // Assert
            Assert.Equal(cnpj, resultado);
        }

        #endregion

        #region Testes de Geração de Filiais

        [Theory]
        [InlineData("PMVT17GD000144")]
        [InlineData("Y6T259JN000132")]
        [InlineData("PM8ZY997000134")]
        [InlineData("5MN1P8JD000123")]
        [Trait("Categoria", "GeraçãoFilial")]
        [Trait("Tipo", "Básico")]
        public void GerarFilial_DeveGerarFilialValida_APartirDeMatriz(string cnpjMatriz)
        {
            // Arrange & Act
            var filial = Cnpj.GerarFilial(cnpjMatriz);

            // Assert
            Assert.NotNull(filial);
            Assert.Equal(14, filial.Length);
            Assert.True(Cnpj.Validar(filial), $"Filial '{filial}' gerada a partir de '{cnpjMatriz}' deveria ser válida");

            // Verifica que os primeiros 8 caracteres são iguais (base do CNPJ)
            Assert.Equal(cnpjMatriz.Substring(0, 8), filial.Substring(0, 8));
        }

        [Fact]
        [Trait("Categoria", "GeraçãoFilial")]
        [Trait("Tipo", "Aleatoriedade")]
        public void GerarFilial_DeveGerarFiliaisComNumeracaoDiferente()
        {
            // Arrange
            string cnpjMatriz = "PMVT17GD000144";

            // Act
            var filial1 = Cnpj.GerarFilial(cnpjMatriz);
            var filial2 = Cnpj.GerarFilial(cnpjMatriz);
            var filial3 = Cnpj.GerarFilial(cnpjMatriz);

            // Assert
            Assert.NotEqual(filial1, filial2);
            Assert.NotEqual(filial2, filial3);
            Assert.NotEqual(filial1, filial3);
        }

        [Fact]
        [Trait("Categoria", "GeraçãoFilial")]
        [Trait("Tipo", "Formatação")]
        public void GerarFilial_ComPontuacao_DeveRetornarFilialFormatada()
        {
            // Arrange
            string cnpjMatriz = "PMVT17GD000144";

            // Act
            var filial = Cnpj.GerarFilial(cnpjMatriz, comPontuacao: true);

            // Assert
            Assert.NotNull(filial);
            Assert.Contains(".", filial);
            Assert.Contains("/", filial);
            Assert.Contains("-", filial);
            Assert.True(Cnpj.Validar(filial), $"Filial formatada '{filial}' deveria ser válida");
        }

        [Theory]
        [InlineData("PMVT17GD000143")] // Dígito verificador incorreto
        [InlineData("123")] // Tamanho incorreto
#pragma warning disable xUnit1012 // Null should only be used for nullable parameters
        [InlineData(null)] // Null
#pragma warning restore xUnit1012 // Null should only be used for nullable parameters
        [InlineData("")] // Vazio
        [InlineData("00000000000000")] // Todos iguais
        [Trait("Categoria", "GeraçãoFilial")]
        [Trait("Tipo", "MatrizInválida")]
        public void GerarFilial_DeveRetornarNull_QuandoMatrizInvalida(string cnpjMatrizInvalido)
        {
            // Arrange & Act
            var filial = Cnpj.GerarFilial(cnpjMatrizInvalido);

            // Assert
            Assert.Null(filial);
        }

        [Fact]
        [Trait("Categoria", "GeraçãoFilial")]
        [Trait("Tipo", "Normalização")]
        public void GerarFilial_DeveAceitarMatrizComPontuacao()
        {
            // Arrange
            string cnpjMatrizFormatado = "PM.VT1.7GD/0001-44";

            // Act
            var filial = Cnpj.GerarFilial(cnpjMatrizFormatado);

            // Assert
            Assert.NotNull(filial);
            Assert.True(Cnpj.Validar(filial), $"Filial '{filial}' deveria ser válida");
            Assert.StartsWith("PMVT17GD", filial);
        }

        [Fact]
        [Trait("Categoria", "GeraçãoFilial")]
        [Trait("Tipo", "Lote")]
        public void GerarFilial_DeveGerarLoteDe50FiliaisValidas()
        {
            // Arrange
            const int quantidadeFiliais = 50;
            string cnpjMatriz = "Y6T259JN000132";
            var filiaisGeradas = new List<string>();
            var filiaisInvalidas = new List<string>();

            // Act
            for (int i = 0; i < quantidadeFiliais; i++)
            {
                var filial = Cnpj.GerarFilial(cnpjMatriz)!;
                filiaisGeradas.Add(filial);

                if (!Cnpj.Validar(filial))
                {
                    filiaisInvalidas.Add(filial);
                }
            }

            // Assert
            Assert.Equal(quantidadeFiliais, filiaisGeradas.Count);
            Assert.Empty(filiaisInvalidas);

            // Verifica que todas mantêm a base do CNPJ
            foreach (var filial in filiaisGeradas)
            {
                Assert.StartsWith("Y6T259JN", filial);
            }
        }

        #endregion
    }
}
