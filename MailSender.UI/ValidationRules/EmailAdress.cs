using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MailSender.UI.ValidationRules
{
	class EmailAddress : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (!(value is string address)) return new ValidationResult(false, "Некорректные данные");
			if (string.IsNullOrEmpty(address)) return new ValidationResult(false, "Вы ничего не ввели");
			if (!Regex.IsMatch(address, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
				return new ValidationResult(false, "Некорректный адрес");
			return ValidationResult.ValidResult;
		}
	}
}
