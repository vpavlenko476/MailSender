using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace MailSender.UI.ValidationRules
{
	class Message : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (!(value is string message)) return new ValidationResult(false, "Некорректные данные");
			if (string.IsNullOrEmpty(message)) return new ValidationResult(false, "Вы ничего не ввели");			
			return ValidationResult.ValidResult;
		}
	}
}
