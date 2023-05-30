using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail; // Para validar los correos
using System.Text.RegularExpressions;

public static class Validation
{
    private static Regex rUsuario = new Regex(@"^[a-zA-ZñÑ]{5,20}$");
    public static bool TryValidateEntityMsj<objeto>(objeto entity, out List<string> errors)
    {
        var validationContext = new ValidationContext(entity);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);
        errors = validationResults.Select(r => r.ErrorMessage).ToList();

        return isValid;
    }
    public static bool TryValidateEntity<T>(T entity)
    {
        var validationContext = new ValidationContext(entity);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

        return isValid;
    }
    public static string ValidarCorreo(string correo)
    {
        string mensaje = string.Empty;

        try
        {
            var mailAddress = new MailAddress(correo);
            string dominio = mailAddress.Host.ToLower();

            if (dominio != "gmail.com")
            {
                mensaje = "Solo se aceptan correos electrónicos de Google (gmail.com).";
            }
        }
        catch (FormatException)
        {
            mensaje = "El correo electrónico no tiene un formato válido.";
        }
        catch (ArgumentException)
        {
            mensaje = "El correo electrónico está vacío o tiene espacios en blanco.";
        }

        return mensaje;
    }
    public static string ValidarUsuario(string usuario)
    {
        if (!rUsuario.IsMatch(usuario))
        {
            return "El usuario no es válido. Solo se permiten letras (mayúsculas y minúsculas) y debe tener una longitud entre 5 y 20 caracteres.";
        }
        return string.Empty;
    }
}
