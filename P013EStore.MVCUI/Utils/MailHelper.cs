using P013EStore.Core.Entities;
using System.Net;
using System.Net.Mail;

namespace P013EStore.MVCUI.Utils
{
	public class MailHelper
	{
		public static async Task SendMailAsync(Contact contact)
		{
			SmtpClient smtpClient = new("mail.siteadresi.com", 587);//1.parametre mail sunucusu, 2.parametre mail portu
			smtpClient.Credentials = new NetworkCredential("email kullanıcı adı","email şifre");
			smtpClient.EnableSsl = false; //email sunucusu ssl ile çalışıyorsa true ver
			MailMessage message = new();
			message.From = new MailAddress("info@siteadi.com");// mesajın gönderildiği adres
			message.To.Add("info@siteadi.com"); // mesajın gönderileceği mail adresi
			message.To.Add("test@siteadi.com"); //1den fazla adrese mail gönderebiliriz
			message.Subject = "Siteden mesaj geldi";
			message.Body = $"Mail Bilgileri: <hr /> Ad Soyad : {contact.Name} {contact.Surname} <hr /> Email : {contact.Email} <hr /> Telefon : {contact.Phone} <hr /> Mesajı : {contact.Message} <hr /> Gönderilme Tarihi : {contact.CreateDate}";//mesajın içeriği
			message.IsBodyHtml = true;//gönderimde html kodu kullandıysak bu ayarı aktif etmeliyiz. 
			//smtpClient.Send(message);//mesajı senkron olarak gönderir
			await smtpClient.SendMailAsync(message);// asenkron olarak attık
			smtpClient.Dispose(); // nesneyi bellekten at
		}
	}
}
