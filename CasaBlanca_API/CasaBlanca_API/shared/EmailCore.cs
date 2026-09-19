using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;


namespace CasaBlanca_API.shared
{
    public class EmailCore
    {


        public async Task SendEmail(string password, string to, string subject, CancellationToken cancellationToken = default)
        {
            try
            {

                var message = new MimeMessage();

                // REMITENTE
                message.From.Add(new MailboxAddress("Casa Blanca Admin.", "rgarrido@officenet.com.mx"));

                // DESTINATARIO
                message.To.Add(new MailboxAddress("Destinatario", to));

                // ASUNTO
                message.Subject = subject;

                // CUERPO
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"""
                                <p><strong>Hola</strong></p>
                                <p>Este es tu contrase&ntilde;a temporal para entrar el sitio web de Casa Blanca : <strong>{password}</strong></p>
                                <p>&nbsp;Una vez que hayas entrado debr&aacute;s cambiar con contrase&ntilde;a para poder entrar al sitio.</p>
                                <p><span style="color: #ff0000;"><em><span style="text-decoration: underline;">No contestar este email</span></em></span></p>
                                <p>&nbsp;</p>
                                <p>Saludos,</p>
                                <p>Casa Blanca Admin</p>
                                """
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();

                // CONEXIÓN SMTP GMAIL
                await smtp.ConnectAsync("ws117.win.arvixe.com", 465, SecureSocketOptions.SslOnConnect);

                // AUTENTICACIÓN
                await smtp.AuthenticateAsync("rgarrido@officenet.com.mx", "7v0SN6vXO2oyhTi");

                // ENVÍO
                await smtp.SendAsync(message);

                // DESCONECTAR
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
