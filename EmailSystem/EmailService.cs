using Dapper;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using static QRCoder.PayloadGenerator;
using static QRCoder.PayloadGenerator.ShadowSocksConfig;
using static System.Net.WebRequestMethods;

namespace EmailSystem
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //--------------Action Type 1 Smtp Wali Mail jayegi 
        public bool SendEmailCommonone(string EmailID, string subject, string body, bool IsHTML = true)
        {
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                MailMessage Email = new MailMessage();
                Email.To.Add(EmailID);
                Email.From = new MailAddress("noreply@smtpmails.online", "noreply@aichat.com");
                Email.Subject = subject;
                Email.Body = body;
                Email.IsBodyHtml = IsHTML;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "email-smtp.us-east-1.amazonaws.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("AKIAUHKMWX7RB4RJ72MG", "BHdwy2xUY3HkrXxtAYM5UK+dhBaMg5PXHK9awpnYOZom")
                };

                smtp.Send(Email);
                return true;
            }
            catch
            {
                return false;
            }
        }


        //----------Action Type 2 Rentelligence Wali Mail jayegi 

        public bool SendEmailCommonTWO(string emailId, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Rentelligence", "noreply@rentelligence.ai"));
                message.To.Add(new MailboxAddress(emailId, emailId));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    // Step 1: connect to your mail server
                    client.Connect("mail.rentelligence.ai", 465, SecureSocketOptions.SslOnConnect);
                    //  If "mail" doesn't work, try:
                    // client.Connect("webmail.rentelligence.ai", 465, SecureSocketOptions.SslOnConnect);

                    //  Step 2: authenticate using your mailbox credentials
                    client.Authenticate("noreply@rentelligence.ai", "j2~5h70Cp");

                    //  Step 3: send mail
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Email send failed: " + ex.Message);
                return false;
            }
        }


        //------------Pehle One try karega, fail to Two, aur vice versa
        public bool SendEmailWithFallback(string email, string subject, string body, bool isHtml = true)
        {
            // Pehle One try
            if (SendEmailCommonone(email, subject, body, isHtml))
                return true;

            // Agar One fail, to Two try
            if (SendEmailCommonTWO(email, subject, body, isHtml))
                return true;

            // Agar Two bhi fail, to false
            return false;
        }

        //-----------------Send OTP User Registration
        public void SendOtpEmailForUser(string otp, string emailId, int actionType = 1, string purpose = "SantrxGlobal")
        {
            try
            {
                string subject = "One-Time Passcode (OTP)";
                string name = "Dear User";

                string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public";

                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>

        <!-- 🔹 LOGO FIX START -->
        <div style='background:#000;display:inline-block;padding:10px 14px;border-radius:10px;margin-bottom:15px;'>
            <img src='{logoUrl}' alt='Santrix Global Logo' style='height:38px;display:block;' />
        </div>
        <!-- 🔹 LOGO FIX END -->

        <h2 style='color:#2c3e50;margin-bottom:10px;font-weight:600;'>Dear User,</h2>

        <p style='color:#444;font-size:15px;margin:0;'>
            Thank you for registering with <strong>SantrixGlobal</strong>.
        </p>

        <p style='color:#444;font-size:15px;margin-top:10px;'>
            To complete your email verification, please use the One-Time Passcode (OTP) below:
        </p>

        <div style='margin:25px 0;'>
            <span style='display:inline-block;padding:14px 24px;font-size:26px;
                color:#4A3AFF;
                background:#eef3ff;
                border:1px solid #d0d9ff;
                border-radius:10px;
                font-weight:600;
                letter-spacing:6px;'>🎯 {otp}</span>
        </div>

        <p style='color:#444;font-size:14px;'>
            Please enter this code in the verification screen to continue.
        </p>

        <p style='color:#777;font-size:13px;'>
            Note: This code is valid for a limited time. If you did not request this, please ignore this email.
        </p>
    </div>

    <div style='background-color:#fafafa;padding:16px 20px;text-align:center;font-size:12px;color:#999;'>
        <p style='margin:4px 0;'>Thank you,<br/>The SantrxGlobal Team</p>

        <div style='margin-top:12px;'>
            <a href='#' style='margin:0 8px;'><img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' width='24'/></a>
            <a href='#' style='margin:0 8px;'><img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' width='24'/></a>
            <a href='#' style='margin:0 8px;'><img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' width='24'/></a>
        </div>
    </div>
</div>";


                bool sent = false;

                // 🔹 ActionType == 1 → pehle One, fallback Two
                if (actionType == 1)
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);

                    if (!sent)
                        sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);
                }
                // 🔹 ActionType == 2 → pehle Two, fallback One
                else if (actionType == 2)
                {
                    sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);

                    if (!sent)
                        sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }
                // 🔹 Baaki sab (3, 4, 5... ya koi unknown) → sirf One
                else
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }

                if (!sent)
                {
                    Console.WriteLine("Both email methods failed!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error: " + ex.Message);
            }
        }

        //----------------Send OTP Withdrawal Request

        public void SendOtpEmailForRequestFundWithdrawal(string otp,string emailId,string WalletAddress,string name,int actionType,string ukTime)
        {
            try
            {
                string subject = $"One-Time Passcode (OTP) — UK Time: {ukTime}";
                string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public";

                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>
        <img src='{logoUrl}' alt='Rentelligence Logo' style='height:38px;margin-bottom:15px;' />
        <h2 style='color:#2c3e50;margin-bottom:10px;font-weight:600;'>Dear {name},</h2>

        <p style='color:#444;font-size:15px;margin:0;'>Thank you for choosing <strong>SantrixGlobal</strong>.</p>
        <p style='color:#444;font-size:15px;margin-top:10px;'>To proceed with your withdrawal request, please use the OTP provided below:</p>

        <div style='margin:25px 0;'>
            <span style='display:inline-block;padding:14px 24px;font-size:26px;
                color:#4A3AFF;
                background:#eef3ff;
                border:1px solid #d0d9ff;
                border-radius:10px;
                font-weight:600;
                letter-spacing:6px;'>🎯 {otp}</span>
        </div>

        <p style='color:#444;font-size:14px;margin-top:20px;'>Wallet Address:</p>
        <div style='margin:10px 0;'>
            <span style='display:inline-block;padding:10px 16px;font-size:15px;
                color:#333;
                background:#f9f9f9;
                border:1px solid #ddd;
                border-radius:8px;
                font-family:monospace;'>{WalletAddress}</span>
        </div>

       

        <p style='color:#444;font-size:14px;margin-top:20px;'>Please enter this code in the verification screen to proceed.</p>
        <p style='color:#777;font-size:13px;'>Note: This code is valid for a limited time. If you did not request this action, please ignore this email.</p>
    </div>

    <div style='background-color:#fafafa;padding:16px 20px;text-align:center;font-size:12px;color:#999;'>
        <p style='margin:4px 0;'>Thank you,<br/>The SantrixGlobal Team</p>
              <div style='margin-top:12px; text-align:center;'>
                   <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                       <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' alt='Instagram' style='width:24px;height:24px;' />
                   </a>
                   <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                       <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' alt='Facebook' style='width:24px;height:24px;' />
                   </a>
                   <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                       <img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' alt='X' style='width:24px;height:24px;' />
                   </a>
               </div>
    </div>
</div>";

                bool sent = false;

                if (actionType == 1)
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                    if (!sent) sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);
                }
                else if (actionType == 2)
                {
                    sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);
                    if (!sent) sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }
                else
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }

                if (!sent)
                {
                    Console.WriteLine("Both email methods failed!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Fund Request): " + ex.Message);
            }
        }

        //---------------- Forgot Password
        public void SendOtpEmailForForgotPassword(string authPass, string emailId)
        {
            try
            {
                string userName = "AIChatBot User";
                string messageIntro = "As requested, here are your login credentials:";
                string emailTo = emailId?.Trim();

                // Subject
                string subject = "Your AIChatBot Login Credentials";

                // HTML Body
                StringBuilder html = new StringBuilder();

                html.Append("<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;");
                html.Append("border:2px solid transparent;border-radius:12px;");
                html.Append("background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);");
                html.Append("background-origin: border-box;background-clip: content-box, border-box;");
                html.Append("box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>");

                // Inner content
                html.Append("<div style='background-color:#ffffff;padding:25px 20px;text-align:center;'>");

                // Logo
                html.Append("<img src='' alt='Rentelligence Logo' style='height:48px;margin-bottom:15px;' />");

                html.Append($"<h2 style='color:#2c3e50;margin-bottom:8px;font-weight:600;'>Dear {userName},</h2>");
                html.Append($"<p style='color:#444;font-size:15px;margin-top:0;'>{messageIntro}</p>");

                // Credentials box
                html.Append("<div style='margin:20px auto 25px auto;max-width:90%;background:#eef3ff;");
                html.Append("border:1px solid #d0d9ff;border-radius:10px;padding:16px;'>");

                html.Append($"<p style='margin:0;font-size:16px;'><strong>Login ID:</strong> {emailTo}</p>");
                html.Append($"<p style='margin:8px 0 0;font-size:16px;'><strong>Password:</strong> {authPass}</p>");
                html.Append("</div>");

                html.Append("<p style='color:#555;font-size:14px;'>For your security, we recommend changing your password after logging in.</p>");
                html.Append("<p style='color:#333;font-size:14px;margin-top:8px;'>Thank you for choosing <strong>Santrx</strong>.</p>");
                html.Append("</div>");

                // Footer with social links
                html.Append("<div style='background-color:#fafafa;padding:16px 20px;text-align:center;font-size:12px;color:#999;'>");
                html.Append("<p style='margin:4px 0;'>Follow us</p>");

                html.Append("<div style='margin-top:10px;'>");

                // Instagram
                html.Append("<a href='' target='_blank' style='margin: 0 10px;'>");
                html.Append("<img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' alt='Instagram' style='width:24px;height:24px;'>");
                html.Append("</a>");

                // Facebook
                html.Append("<a href='' target='_blank' style='margin: 0 10px;'>");
                html.Append("<img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' alt='Facebook' style='width:24px;height:24px;'>");
                html.Append("</a>");

                // X (Twitter)
                html.Append("<a href='' target='_blank' style='margin: 0 10px;'>");
                html.Append("<img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' alt='X' style='width:24px;height:24px;'>");
                html.Append("</a>");

                html.Append("</div>");
                html.Append("<p style='margin-top:10px;color:#aaa;'>© 2025 Santrx. All rights reserved.</p>");
                html.Append("</div>");
                html.Append("</div>");

                string body = html.ToString();
                bool sent = false;

                SendEmailCommonone(emailTo, subject, body, true);

                if (!sent)
                {
                    Console.WriteLine("Both email methods failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Forgot Password): " + ex.Message);
            }
        }

        //----------------Send OTP Fund Request
        public void SendOtpEmailForRequestFund(string otp, string emailId, string name, int actionType = 1, string purpose = "SantrixGlobal")
        {
            try
            {
                string subject = "One-Time Passcode (OTP)";
                string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public";

                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>
        <img src='{logoUrl}' alt='Rentelligence Logo' style='height:38px;margin-bottom:15px;' />
        <h2 style='color:#2c3e50;margin-bottom:10px;font-weight:600;'>Dear {name},</h2>

        <p style='color:#444;font-size:15px;margin:0;'>Thank you for choosing <strong>{purpose}</strong>.</p>
        <p style='color:#444;font-size:15px;margin-top:10px;'>To complete your transaction, please use the OTP provided below:</p>

        <div style='margin:25px 0;'>
            <span style='display:inline-block;padding:14px 24px;font-size:26px;
                color:#4A3AFF;
                background:#eef3ff;
                border:1px solid #d0d9ff;
                border-radius:10px;
                font-weight:600;
                letter-spacing:6px;'>🎯 {otp}</span>
        </div>
        <p style='color:#444;font-size:14px;margin-top:20px;'>Please enter this code in the verification screen to proceed.</p>
        <p style='color:#777;font-size:13px;'>Note: This code is valid for a limited time. If you did not request this action, please ignore this email.</p>
    </div>

    <div style='background-color:#fafafa;padding:16px 20px;text-align:center;font-size:12px;color:#999;'>
        <p style='margin:4px 0;'>Thank you,<br/>The SantrixGlobal Team</p>

        <div style='margin-top:12px; text-align:center;'>
            <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' alt='Instagram' style='width:24px;height:24px;' />
            </a>
            <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' alt='Facebook' style='width:24px;height:24px;' />
            </a>
            <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                <img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' alt='X' style='width:24px;height:24px;' />
            </a>
        </div>
    </div>
</div>";
                //SendEmailCommonone(emailId.Trim(), subject, body, true);

                bool sent = false;

                // 🔹 ActionType == 1 → pehle One, fallback Two
                if (actionType == 1)
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);

                    if (!sent)
                        sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);
                }
                // 🔹 ActionType == 2 → pehle Two, fallback One
                else if (actionType == 2)
                {
                    sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);

                    if (!sent)
                        sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }
                // 🔹 Baaki sab (3, 4, 5... ya koi unknown) → sirf One
                else
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }

                if (!sent)
                {
                    Console.WriteLine("Both email methods failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Fund Request): " + ex.Message);
            }
        }


        //----------------Send Welcome Letter

        public void SendOtpEmailForUserRegistrationWelcomletter(string plainPassword, string emailid, string Name)
        {
            string EmailID = emailid.Trim();

            try
            {
                string subject = "Welcome to AI Chat Bot, " + Name;
                string logoUrl = "";

                string body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
    <style>
        /* MOBILE FIX */
        @media only screen and (max-width: 600px) {{
            .main-container {{
                width: 100% !important;
                margin: 0 !important;
                padding: 0 !important;
            }}
            .inner-box {{
                width: 100% !important;
                padding: 15px !important;
                border-radius: 10px !important;
            }}
            .content {{
                padding: 10px !important;
            }}
            h2 {{ font-size: 20px !important; }}
            p {{ font-size: 14px !important; }}
        }}
    </style>
</head>

<body style='margin:0; padding:0; background:#f4f4f4; font-family:Arial;'>

<table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' bgcolor='#f4f4f4'>
<tr>
<td align='center'>

    <table role='presentation' class='main-container' width='100%' style='max-width:600px; width:100%; margin:0 auto; padding:0;' border='0' cellspacing='0' cellpadding='0'>
    <tr>
        <td>

            <table role='presentation' width='100%' 
                style='background:white; border-radius:15px; border:3px solid transparent;
                background-image:linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
                background-origin:border-box; background-clip:content-box, border-box;' 
                class='inner-box' cellpadding='0' cellspacing='0'>

                <tr>
                    <td class='content' style='padding:25px;'>

                        <!-- LOGO -->
                        <div style='text-align:center; margin-bottom:20px;'>
                            <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public' 
                                 style='width:140px; height:auto; border-radius:8px; border:1px solid #eee; padding:6px;'>
                        </div>

                        <!-- TITLE -->
                        <h2 style='text-align:center; color:#333; margin:0 0 8px;'>
                            🚀 Welcome to SantrixGlobal, {Name}!
                        </h2>

                        <p style='text-align:center; color:#666; margin-top:0;'>
                            We're thrilled to have you on board.
                        </p>

                        <!-- CONTENT -->
                        <p style='color:#444; line-height:1.6; text-align:justify;'>
                            The future isn’t coming — it’s already here, and it’s <b>Assetes</b>. 
                            With SantrixGlobal, you now have access to a powerful ecosystem where Assets work for you, 
                            generate consistent rewards, and free your time for what truly matters.
                        </p>

                        <h3 style='color:#333;'>🤖 Put Your Assets to Work</h3>
                        <p style='color:#555;'>Let them earn while you live smarter.</p>

                        <h3 style='color:#333;'>🛠️ Your Capital. Our Platform.</h3>
                        <p style='color:#555;'>Together, we build a future where money works smarter.</p>

                        <!-- LOGIN BOX -->
                        <table width='100%' style='background:#f8f8ff; border:1px solid #6c63ff; border-radius:8px; margin:25px 0;' cellpadding='12'>
                            <tr>
                                <td align='center'>
                                    <p style='margin:0; font-size:16px;'><b>Login ID:</b> {emailid}</p>
                                    <p style='margin-top:8px; font-size:16px;'><b>Password:</b> {plainPassword}</p>
                                </td>
                            </tr>
                        </table>

                        <p style='text-align:center; color:#666; font-size:15px;'>
                            Welcome aboard {Name}. Let’s create passive income — the Santrx way.
                        </p>

                        <hr style='border:none; border-top:1px solid #eee; margin:30px 0;'>

                        <!-- SOCIAL -->
                        <p style='text-align:center; color:#999; font-size:14px;'>Follow us on</p>

                        <div style='text-align:center; margin-top:10px;'>
                            <a href=''>
                                <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='margin:0 8px;'>
                            </a>
                            <a href=''>
                                <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='margin:0 8px;'>
                            </a>
                            <a href='#'>
                                <img src='https://cdn-icons-png.flaticon.com/24/5968/5968958.png' style='margin:0 8px; width:24px;'>
                            </a>
                        </div>

                        <p style='text-align:center; color:#bbb; font-size:12px; margin-top:15px;'>
                            © {DateTime.Now.Year} AICHatBOT. All rights reserved.
                        </p>

                    </td>
                </tr>

            </table>

        </td>
    </tr>
    </table>

</td>
</tr>
</table>

</body>
</html>";

                SendEmailCommonone(emailid.Trim(), subject, body, true);

                
            }
            catch { }
        }

        //Event Email User
        public void SendOtpEmailForEventUser(string otp, string emailId, int actionType = 1, string purpose = "SantrixGlobal")
        {
            try
            {
                string subject = "Your Event Booking Verification Code";

                string body = $@"
<div style='max-width:650px;margin:auto;padding:0;font-family:Arial,Helvetica,sans-serif;
background:#ffffff;border-radius:8px;border:3px solid #003399;'>   <!-- BLUE BOLD BORDER -->

    <!-- HEADER -->
    <div style='text-align:center;padding:30px 20px;background:#fff;border-bottom:1px solid #e5e5e5;'>
        <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/893ab68a-6977-4ac1-a97e-9c68846cf400/public'
             style='height:55px;margin-bottom:15px;' />
        <h2 style='font-size:22px;margin:0;color:#1a1a1a;'>Email Verification Code</h2>
    </div>

    <!-- BODY -->
    <div style='padding:25px 30px;color:#333;font-size:15px;line-height:1.6;'>

        <p style='margin:0;'>Dear User,</p>

        <p style='margin-top:10px;'>
            To verify your email address for your event booking, please enter the following code:
        </p>

        <div style='text-align:center;margin:25px 0;'>
            <div style='display:inline-block;padding:14px 26px;font-size:26px;
                color:#003399;background:#f4f6ff;border:1px solid #cdd6ff;
                border-radius:8px;font-weight:bold;letter-spacing:6px;'>
                🎯 {otp}
            </div>
        </div>

         <p style='margin-top:10px;'>
      If you have any questions or need further assistance, please do not hesitate to contact us at 
      <a href='mailto:support@santrixglobal.ai' style='color:#0047d1;text-decoration:none;font-weight:bold;'>
          support@Santrx.ai
      </a>.
      We are here to support you and ensure your experience with us is exceptional.
  </p>

        <p style='margin-top:25px;'>Best Regards,<br/>Santrx Team</p>
    </div>

    <!-- FOOTER SOCIAL -->
    <div style='text-align:center;padding:15px 0;background:#fafafa;border-top:1px solid #e5e5e5;'>
        <a href='' style='margin:0 8px;'>
            <img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' width='22' />
        </a>
        <a href='' style='margin:0 8px;'>
            <img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' width='22' />
        </a>
          <a href='' target='_blank' style='margin: 0 8px; text-decoration:none;'>
                        <img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' alt='X' style='width:24px;height:24px;' />
                    </a>
            </div>

    <!-- DISCLAIMER -->
    <div style='padding:15px 20px;font-size:11px;color:#7a7a7a;line-height:1.5;background:#fafafa;'>
        <p style='margin:0;'>Disclaimer: This email is for verification purposes only.</p>
    </div>
</div>";

                bool sent = false;

                if (actionType == 1)
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                    if (!sent)
                        sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);
                }
                else if (actionType == 2)
                {
                    sent = SendEmailCommonTWO(emailId.Trim(), subject, body, true);
                    if (!sent)
                        sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }
                else
                {
                    sent = SendEmailCommonone(emailId.Trim(), subject, body, true);
                }

                if (!sent)
                {
                    Console.WriteLine("Both email methods failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error: " + ex.Message);
            }
        }

    }
}
