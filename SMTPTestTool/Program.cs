using System;
using System.Net;
using System.Net.Mail;
using System.CommandLine;

namespace Lacuna.SMTPTestTool;
internal class Program {
	static async Task<int> Main(string[] args) { 
		var serverHostOption = new Option<string>(name: "--serverHost", description: "SMTP ServerHost") { IsRequired = true };
		var serverPortOption = new Option<int>(name: "--serverPort", description: "SMTP serverPort") { IsRequired = true };
		var usernameOption = new Option<string>(name: "--username", description: "Username") { IsRequired = true };
		var passwordOption = new Option<string>(name: "--password", description: "Password") { IsRequired = true };
		var senderAddressOption = new Option<string>(name: "--senderAddress", description: "Sender Address") { IsRequired = true };
		var senderNameOption = new Option<string>(name: "--senderName", description: "Sender Name") { IsRequired = true };
		var emailRecipientOption = new Option<string>(name: "--email", description: "Email recipient") { IsRequired = true };

		var rootCommand = new RootCommand("Lacuna test e-mail Sender");
		rootCommand.AddOption(serverHostOption);
		rootCommand.AddOption(serverPortOption);
		rootCommand.AddOption(usernameOption);
		rootCommand.AddOption(passwordOption);
		rootCommand.AddOption(senderAddressOption);
		rootCommand.AddOption(senderNameOption);
		rootCommand.AddOption(emailRecipientOption);

		rootCommand.SetHandler( (serverHost, serverPort, username, password, senderAddress, senderName, emailRecipient) => {
			var smtpClient = new SmtpClient(serverHost, serverPort) {
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(username, password),
				EnableSsl = true
			};

			var mailMessage = new MailMessage {
				From = new MailAddress(senderAddress, senderName),
				Subject = "EmailSenderTester",
				Body = "This is a test email.",
				IsBodyHtml = true,
			};
			mailMessage.To.Add(new MailAddress(emailRecipient));

			try {
				smtpClient.Send(mailMessage);
				Console.WriteLine("Email sent successfully.");
			} catch (Exception ex) {
				Console.WriteLine("Failed to send the email.\n" + ex.Message);
			}

		}, serverHostOption, serverPortOption, usernameOption, passwordOption, senderAddressOption, senderNameOption, emailRecipientOption);
		 return await rootCommand.InvokeAsync(args);
	}
}
