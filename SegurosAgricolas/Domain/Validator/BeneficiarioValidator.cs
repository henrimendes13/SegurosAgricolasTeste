using SegurosAgricolas.Domain.Entities;

namespace SegurosAgricolas.Domain.Validator
{
    public class BeneficiarioValidator
    {
        public ValidationResult Validate(BeneficiarioEntity entity)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(entity.Nome))
                errors.Add("Nome é obrigatório");
            if (string.IsNullOrEmpty(entity.CNPJ))
                errors.Add("CNPJ é obrigatório");
            if (entity.CNPJ != null && !IsValidCNPJ(entity.CNPJ))
                errors.Add("CNPJ inválido");

            if (errors.Any())
            {
                return new ValidationResult(errors);
            }

            return new ValidationResult(errors);
        }

        private bool IsValidCNPJ(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                return false;
            }

            // Remove qualquer caractere que não seja dígito
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // Verifica se o CNPJ tem 14 dígitos
            if (cnpj.Length != 14)
            {
                return false;
            }

            // Verifica se todos os dígitos são iguais
            if (new string(cnpj.Distinct().ToArray()).Length == 1)
            {
                return false;
            }

            // Calcula os dígitos verificadores
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            }

            int resto = (soma % 11);
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            }

            resto = (soma % 11);
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();

            // Verifica se os dígitos calculados são iguais aos dígitos do CNPJ informado
            return cnpj.EndsWith(digito);
        }
    }
}
