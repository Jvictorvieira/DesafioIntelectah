using System.ComponentModel.DataAnnotations;

namespace ConcessionariaAPP.Models.Attributes;

public class CpfAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string cpf) return false;
        cpf = cpf.Replace(".", "").Replace("-", "").Trim();
        if (cpf.Length != 11 || !cpf.All(char.IsDigit)) return false;
        // Validação de dígitos do CPF (simplificada)
        int[] mult1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] mult2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        string tempCpf = cpf[..9];
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult1[i];
        int rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;
        string digit = rest.ToString();
        tempCpf += digit;
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult2[i];
        rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;
        digit += rest.ToString();
        return cpf.EndsWith(digit);
    }
}