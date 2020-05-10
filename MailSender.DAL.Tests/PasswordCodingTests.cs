using NUnit.Framework;
using Services;

namespace MailSender.DAL.Tests
{
	class PasswordCodingTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void DecryptPass_123abc_Returns234bcd()
		{
			string input = "123abc";
			string expected = "234bcd";
			var output = PasswordCoding.Encrypt(input);
			Assert.That(output, Is.EqualTo(expected));
		}

		[Test]
		public void EncryptPass_abc_bcd()
		{
			string input = "234bcd";
			string expected = "123abc";
			var output = PasswordCoding.Decrypt(input);
			Assert.That(output, Is.EqualTo(expected));
		}
	}
}
