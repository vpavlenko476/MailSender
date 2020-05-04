using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace MailSender.UI.ValidationRules
{
	class SendDate : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (!(value is DateTime date)) return new ValidationResult(false, "Некорректные данные");
			if (date<DateTime.Now) return new ValidationResult(false, "Нельзя выбрать прошедшую дату");
			
			return ValidationResult.ValidResult;
		}
	}
}
