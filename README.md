# CnpjHelper

[![.NET Standard 2.1](https://img.shields.io/badge/.NET%20Standard-2.1-blue)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Tests](https://img.shields.io/badge/tests-95%20passing-brightgreen)]()

**CnpjHelper** é uma biblioteca .NET para **validação, geração e formatação de CNPJs alfanuméricos** brasileiros, seguindo o padrão da Receita Federal.

## Características
✅ Validação de CNPJs alfanuméricos (0-9, A-Z)  
✅ Geração de CNPJs válidos (matriz ou filial)  
✅ Formatação automática (XX.XXX.XXX/XXXX-XX)  
✅ Remoção de formatação  
✅ Geração de filiais a partir de matriz  
✅ Validado com CNPJs reais da Receita Federal  
✅ Vários testes unitários

<!--
## Instalação
Você pode instalar o CnpjHelper via NuGet Package Manager:
```bash
Install-Package Cvllo.CnpjHelper
```
-->

## Uso Rápido

```csharp
using CnpjHelper;

// Validar CNPJ 
bool valido = Cnpj.Validar("PMVT17GD000144");  // true 
bool invalido = Cnpj.Validar("PMVT17GD000143");  // false

// Gerar CNPJ matriz (0001) 
string matriz = Cnpj.Gerar();  // "PMVT17GD000144"

// Gerar CNPJ filial (aleatório) 
string filial = Cnpj.Gerar(gerarMatriz: false);  // "PMVT17GD5VEK98"

// Formatar CNPJ 
string formatado = Cnpj.Formatar("PMVT17GD000144");  // "PM.VT1.7GD/0001-44"

// Gerar filial da matriz 
string novaFilial = Cnpj.GerarFilial(matriz);  // "PMVT17GDK6KX53"

// Remover formatação 
string semFormatacao = Cnpj.RemoverFormatacao(formatado);  // "PMVT17GD000144"
```


## API

### `Cnpj.Validar(string cnpj)` → `bool`
Valida um CNPJ alfanumérico.

### `Cnpj.Gerar(bool gerarMatriz = true, bool comPontuacao = false)` → `string`
Gera um CNPJ válido.

### `Cnpj.GerarFilial(string cnpjMatriz, bool comPontuacao = false)` → `string?`
Gera filial a partir de uma matriz.

### `Cnpj.Formatar(string cnpj)` → `string?`
Formata CNPJ no padrão XX.XXX.XXX/XXXX-XX.

### `Cnpj.RemoverFormatacao(string cnpj)` → `string`
Remove pontuação de um CNPJ.

## Formato CNPJ Alfanumérico
```
XX.XXX.XXX/XXXX-DD 
│  │   │   │    └─ 2 dígitos verificadores (0-9) 
│  │   │   └────── 4 caracteres estabelecimento (0-9, A-Z) 
│  │   └────────── 3 caracteres (0-9, A-Z) 
│  └────────────── 3 caracteres (0-9, A-Z) 
└───────────────── 2 caracteres (0-9, A-Z)
```

**Exemplo:** `PM.VT1.7GD/0001-44`
- Base: `PMVT17GD`
- Estabelecimento: `0001` (matriz)
- Dígitos: `44`

## Testes

```bash
dotnet test

# Executar testes específicos por categoria ou tipo

dotnet test --filter "Categoria=Validação" dotnet test --filter "Categoria=Geração" dotnet test --filter "Tipo=Lote"
```

## Validar lote
Você pode validar uma lista de CNPJs em lote:
```csharp
string[] cnpjs = { "PMVT17GD000144", "Y6T259JN000132" }; 
foreach (var cnpj in cnpjs) { 
    Console.WriteLine($"{cnpj}: {Cnpj.Validar(cnpj)}"); 
}
```
## Performance
- Validação: ~0.5ms por CNPJ
- Geração: ~1ms por CNPJ
- 10.000 CNPJs: < 5 segundos

## Licença
MIT License

## Contato
- Autor: Daniel Corvello
- **Github:** [danielcorvello](https://github.com/danielcorvello)
- **LinkedIn:** [danielcorvello](https://www.linkedin.com/in/danielcorvello/)

---

<div align="center">
  
**Feito com ❤️ para a comunidade .NET brasileira**

</div>