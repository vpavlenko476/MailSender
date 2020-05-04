using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MailSender.UI.ValidationRules
{
	class SmtpPort : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{			
			if (string.IsNullOrEmpty(value.ToString())) return new ValidationResult(false, "Вы ничего не ввели");
			if (!Regex.IsMatch(value.ToString(), @"^\d+$"))
				return new ValidationResult(false, "Некорректный порт");
			return ValidationResult.ValidResult;
		}
	}
}
