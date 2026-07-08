using Dapper;
using System.IO;
using System.Net;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using static QRCoder.PayloadGenerator;
using static QRCoder.PayloadGenerator.ShadowSocksConfig;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace EmailSystem
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public bool SendEmailCommonone(string EmailID, string subject, string body, bool IsHTML = true)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://apis.arbionai.com/api/commonEmail/commonSendOtp";

                var requestObj = new
                {
                    projectCode = "46.250.225.82",
                    templateCode = "Em$24237%dsfhjG2454#2023$frAll",
                    fromEmail = "noreply@xoxofx.com",
                    toEmail = EmailID,
                    body = body,
                    subject = subject
                };

                string json = JsonConvert.SerializeObject(requestObj);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.UserAgent = "Mozilla/5.0";
                request.Timeout = 300000;
                request.KeepAlive = false;

                byte[] data = Encoding.UTF8.GetBytes(json);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    //File.WriteAllText(@"C:\EmailResponse.txt", result);
                }

                return true;
            }
            catch (WebException ex)
            {
                string error = "";

                if (ex.Response != null)
                {
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        error = "HTTP Status : " + (int)response.StatusCode +
                                Environment.NewLine +
                                "Description : " + response.StatusDescription +
                                Environment.NewLine +
                                "Response :" +
                                Environment.NewLine +
                                reader.ReadToEnd();
                    }
                }
                else
                {
                    error = ex.ToString();
                }

                //File.WriteAllText(@"C:\EmailError.txt", error);

                return false;
            }
            catch (Exception ex)
            {
                //File.WriteAllText(@"C:\EmailError.txt", ex.ToString());
                return false;
            }
        }
        

        //----------Action Type 2 per ye wali

        public bool SendEmailCommonTWO(string emailId, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("arbion", "noreply@arbion.ai"));
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
        public void SendOtpEmailForUser(string otp, string emailId, int actionType = 1, string purpose = "XoxoFX")
        {
            try
            {
                string subject = "One-Time Passcode (OTP)";
                string name = "Dear User";
                //var logoUrl = Path.Combine(Directory.GetCurrentDirectory(), "EmailLogo", "Logo.png");
                //string logoUrl = "https://www.theia.org/sites/default/files/2024-11/Arbion%20logo.png";
                //string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/5eca37a2-f40e-4ded-152e-2df67488bc00/public";

                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>

      

        <h2 style='color:#2c3e50;margin-bottom:10px;font-weight:600;'>Dear User,</h2>

        <p style='color:#444;font-size:15px;margin:0;'>
            Thank you for registering with <strong>XoxoFX</strong>.
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
        <p style='margin:4px 0;'>Thank you,<br/>The XoxoFX Team</p>

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

        public void SendOtpEmailForRequestFundWithdrawal(string otp, string emailId, string WalletAddress, string name, int actionType, string ukTime)
        {
            try
            {
                string subject = $"One-Time Passcode (OTP) — UK Time: {ukTime}";
                //var logoUrl = Path.Combine(Directory.GetCurrentDirectory(), "EmailLogo", "Logo.png");
                //string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public";

                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>
        <h2 style='color:#2c3e50;margin-bottom:10px;font-weight:600;'>Dear {name},</h2>

        <p style='color:#444;font-size:15px;margin:0;'>Thank you for choosing <strong>XoxoFX</strong>.</p>
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
        <p style='margin:4px 0;'>Thank you,<br/>The XoxoFX  Team</p>
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

        public void SendOtpEmailForForgotPassword(string authPass, string emailId)
        {
            try
            {
                string userName = "XOXOFX  User";
                string emailTo = emailId?.Trim();

                string subject = "XoxoFX Password Reset Details";

                StringBuilder html = new StringBuilder();

                html.Append("<div style='max-width:520px;margin:auto;font-family:Arial,sans-serif;");
                html.Append("background:#ffffff;border-radius:12px;border:1px solid #e6e6e6;");
                html.Append("padding:25px 20px;text-align:center;'>");

                //// Logo
                //html.Append("<img src='' ");
                //html.Append("style='width:120px;margin-bottom:10px;display:block;margin-left:auto;margin-right:auto;' />");

                // Title
                html.Append("<h2 style='color:#222;margin-top:10px;margin-bottom:6px;font-weight:600;'>Password Reset Request</h2>");
                html.Append($"<p style='color:#555;margin:0 0 15px;font-size:14px;'>Hello {userName}, here are your updated login details.</p>");

                // Credentials Box
                html.Append("<div style='background:#f7f8ff;border:1px solid #d8ddff;border-radius:8px;");
                html.Append("padding:15px;text-align:left;margin:0 auto 18px auto;max-width:90%;'>");

                html.Append($"<p style='margin:0;font-size:15px;color:#333;'><strong>Login ID:</strong> {emailTo}</p>");
                html.Append($"<p style='margin:6px 0 0;font-size:15px;color:#333;'><strong>Password:</strong> {authPass}</p>");

                html.Append("</div>");

                // Info message
                html.Append("<p style='color:#666;font-size:13px;margin:0;'>For your security, please change your password after logging in.</p>");
                html.Append("<p style='color:#666;font-size:13px;margin-top:8px;'>If you did not request this, please ignore this email.</p>");

                // Footer
                html.Append("<div style='border-top:1px solid #eee;margin-top:22px;padding-top:15px;'>");

                html.Append("<p style='margin:0;color:#999;font-size:12px;'>Follow us</p>");

                html.Append("<div style='margin-top:8px;'>");
                html.Append("<a href='#' style='margin:0 6px;'><img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='width:22px;'></a>");
                html.Append("<a href='#' style='margin:0 6px;'><img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='width:22px;'></a>");
                html.Append("<a href='#' style='margin:0 6px;'><img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' style='width:22px;'></a>");
                html.Append("</div>");

                html.Append("<p style='margin-top:10px;color:#aaa;font-size:11px;'>© 2026 XOXOFX.tech. All rights reserved.</p>");
                html.Append("</div>");

                html.Append("</div>");

                string body = html.ToString();

                SendEmailCommonone(emailTo, subject, body, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Forgot Password): " + ex.Message);
            }
        }



        //profile Update 
        public void SendOtpEmailUserProfileUpdate(string otp, string emailId, string name, int actionType = 1, string purpose = "XOXOFX")
        {
            try
            {
                string subject = "One-Time Passcode (OTP)";                               
                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>
        <h2 style='color:#2c3e50;margin-bottom:10px;font-weight:600;'>Dear {name},</h2>

        <p style='color:#444;font-size:15px;margin:0;'>Thank you for choosing <strong>{purpose}</strong>.</p>
        <p style='color:#444;font-size:15px;margin-top:10px;'>To complete your verification, please use the One-Time Password (OTP) provided below:</p>

        <div style='margin:25px 0;'>
            <span style='display:inline-block;padding:14px 24px;font-size:26px;
                color:#4A3AFF;
                background:#eef3ff;
                border:1px solid #d0d9ff;
                border-radius:10px;
                font-weight:600;
                letter-spacing:6px;'>🎯 {otp}</span>
        </div>
        <p style='color:#444;font-size:14px;margin-top:20px;'>Please enter this code on the verification screen to proceed.</p>
<p style='color:#444;font-size:14px;margin-top:20px;'>For your security, this OTP is valid for a limited time and should not be shared with anyone.</p>
        <p style='color:#777;font-size:13px;'>Note: If you did not request this verification, please ignore this email or contact our support team immediately.</p>
    </div>

    <div style='background-color:#fafafa;padding:16px 20px;text-align:center;font-size:12px;color:#999;'>
        <p style='margin:4px 0;'>Thank you,<br/>The XoxoFX Team</p>

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

        //----------------Send OTP Fund Request
        public void SendOtpEmailForRequestFund(string otp, string emailId, string name, int actionType = 1, string purpose = "XOXOFX")
        {
            try
            {
                string subject = "One-Time Passcode (OTP)";
                //string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public";
                var logoUrl = Path.Combine(Directory.GetCurrentDirectory(), "EmailLogo", "Logo.png");


                string body = $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;
    border:2px solid transparent;
    border-radius:12px;
    background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);
    background-origin: border-box;
    background-clip: content-box, border-box;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>

    <div style='background-color:#ffffff;padding:25px 20px;text-align:center;border-bottom:1px solid #f0f0f0;'>
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
        <p style='margin:4px 0;'>Thank you,<br/>The XoxoFX Team</p>

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
                string subject = $"🚀 Welcome to XOXOFX , {Name}!";

                string body = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>

<body style='margin:0; padding:0; background:#0b0f1a; font-family:Arial;'>

<table width='100%' cellspacing='0' cellpadding='0'>
<tr>
<td align='center'>

<table width='100%' style='max-width:620px; margin:30px auto;'>

<tr>
<td style='background:#111827; border-radius:16px; padding:30px;
border:1px solid #1f2937; color:#e5e7eb;'>



<!-- TITLE -->
<h2 style='text-align:center; margin:0; color:#ffffff;'>
Welcome to XoxoFX, {Name}! 🚀
</h2>

<p style='text-align:center; color:#9ca3af; margin-top:8px;'>
Your AI-powered arbitrage engine is now live
</p>

<!-- CONTENT -->
<div style='margin-top:25px; line-height:1.7; font-size:15px;'>

<p>
Welcome to <b>XoxoFX  Engine</b> — your gateway to automated crypto arbitrage profits.
</p>

<p>
Our AI scans multiple networks in real-time to detect profitable opportunities across 
<b>SOL, ETH, and BSC</b>.
</p>

<h3 style='color:#60a5fa;'>⚡ Automated Arbitrage</h3>
<p>
Execute trades instantly by capturing price differences across exchanges.
</p>

<h3 style='color:#34d399;'>📊 Real-Time Opportunities</h3>
<p>
Track live spreads, monitor profits, and access high-frequency insights.
</p>

<h3 style='color:#fbbf24;'>🚀 Passive Income 24/7</h3>
<p>
Let the system work continuously with optimized execution strategies.
</p>

<h3 style='color:#a78bfa;'>🔐 Secure & Smart</h3>
<p>
Your funds stay in your control while XoxoFX handles analysis.
</p>

</div>

<!-- LOGIN DETAILS -->
<div style='background:#0f172a; border:1px solid #374151;
border-radius:10px; padding:15px; margin:25px 0; text-align:center;'>

<p style='margin:5px 0;'><b>Email:</b> {emailid}</p>
<p style='margin:5px 0;'><b>Password:</b> {plainPassword}</p>

</div>

<!-- CTA BUTTON -->
<div style='text-align:center; margin-top:20px;'>
<a href='https://xoxofx.com/'
style='display:inline-block; padding:14px 30px;
background:linear-gradient(90deg,#6366f1,#8b5cf6);
color:white; text-decoration:none; border-radius:8px;
font-weight:bold;'>

Start Profiting Now 🚀

</a>
</div>

<!-- FOOTER -->
<div style='text-align:center; margin-top:30px; color:#6b7280; font-size:13px;'>

<p>Follow us</p>

<img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='margin:0 6px;'/>
<img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='margin:0 6px;'/>
<img src='https://cdn-icons-png.flaticon.com/24/5968/5968958.png' style='margin:0 6px; width:24px;'/>

<p style='margin-top:15px;'>
© {DateTime.Now.Year} XoxoFX. All rights reserved.
</p>

</div>

</td>
</tr>

</table>

</td>
</tr>
</table>

</body>
</html>";

                SendEmailCommonone(EmailID, subject, body, true);
            }
            catch { }
        }
        //        public void SendOtpEmailForUserRegistrationWelcomletter(string plainPassword, string emailid, string Name)
        //        {
        //            string EmailID = emailid.Trim();

        //            try
        //            {
        //                string subject = $"Welcome to Arbion , {Name}!";
        //                string body = $@"
        //<!DOCTYPE html>
        //<html>
        //<head>
        //    <meta charset='UTF-8'>
        //    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        //    <style>
        //        @media only screen and (max-width: 600px) {{
        //            .container {{ width:100% !important; padding:0 !important; }}
        //            .card {{ padding:15px !important; border-radius:10px !important; }}
        //            h2 {{ font-size:22px !important; }}
        //            p {{ font-size:14px !important; }}
        //        }}
        //    </style>
        //</head>

        //<body style='margin:0; padding:0; background:#f3f4f6; font-family:Arial;'>
        //    <table width='100%' cellspacing='0' cellpadding='0'>
        //        <tr>
        //            <td align='center'>

        //                <table class='container' width='100%' style='max-width:600px; margin:20px auto;'>
        //                    <tr>
        //                        <td>

        //                            <table class='card' width='100%' 
        //                                style='background:#fff; padding:30px; border-radius:16px;
        //                                border:1px solid #ddd;'>

        //                                <!-- LOGO -->
        //                                <tr>
        //                                    <td align='center' style='padding-bottom:20px;'>
        //                                        <img src='https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/5eca37a2-f40e-4ded-152e-2df67488bc00/public'
        //                                        style='width:140px;' />
        //                                    </td>
        //                                </tr>

        //                                <!-- TITLE -->
        //                                <tr>
        //                                    <td align='center'>
        //                                        <h2 style='margin:0; color:#222;'>Welcome to Acanza Tech Assistant, {Name}!</h2>
        //                                        <p style='color:#666; margin-top:8px;'>
        //                                            Your personal AI companion is ready to help.
        //                                        </p>
        //                                    </td>
        //                                </tr>

        //                                <!-- MAIN CONTENT -->
        //                                <tr>
        //                                    <td style='color:#444; padding-top:15px; line-height:1.6;'>

        //                                        <p>
        //                                            You have successfully registered on our AI Chat Platform — 
        //                                            a smart assistant designed to make your daily tasks easier and more productive.
        //                                        </p>

        //                                        <h3 style='color:#333; margin-top:20px;'>🤖 Smart Conversations</h3>
        //                                        <p>
        //                                            Ask anything, get instant responses, create content, learn new things, 
        //                                            or automate your workflow.
        //                                        </p>

        //                                        <h3 style='color:#333; margin-top:15px;'>⚡ Boost Productivity</h3>
        //                                        <p>
        //                                            Whether it's writing, coding, planning, or brainstorming — 
        //                                            your AI assistant works with you 24/7.
        //                                        </p>

        //                                        <h3 style='color:#333; margin-top:15px;'>🧠 Learn & Create</h3>
        //                                        <p>
        //                                            Generate ideas, improve your skills, get explanations, summaries, 
        //                                            and much more in seconds.
        //                                        </p>

        //                                    </td>
        //                                </tr>

        //                                <!-- LOGIN DETAILS -->
        //                                <tr>
        //                                    <td>
        //                                        <table width='100%'
        //                                            style='background:#f7f7f7; border:1px solid #ccc; border-radius:10px; margin:25px 0;' cellpadding='12'>
        //                                            <tr>
        //                                                <td align='center' style='font-size:16px; color:#333;'>
        //                                                    <b>Login ID:</b> {emailid}<br>
        //                                                    <b>Password:</b> {plainPassword}
        //                                                </td>
        //                                            </tr>
        //                                        </table>
        //                                    </td>
        //                                </tr>

        //                                <!-- FOOTER MESSAGE -->
        //                                <tr>
        //                                    <td align='center'>
        //                                        <p style='color:#666; font-size:15px;'>
        //                                            We're excited to have you onboard.<br>
        //                                            Start chatting and experience the power of AI!
        //                                        </p>
        //                                    </td>
        //                                </tr>

        //                                <tr>
        //                                    <td>
        //                                        <hr style='border:none; border-top:1px solid #eee; margin:25px 0;'>
        //                                    </td>
        //                                </tr>

        //                                <!-- SOCIAL ICONS -->
        //                                <tr>
        //                                    <td align='center'>
        //                                        <p style='color:#999; font-size:14px; margin-bottom:10px;'>Follow us</p>

        //                                        <a href='#'><img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' style='margin:0 6px;'></a>
        //                                        <a href='#'><img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' style='margin:0 6px;'></a>
        //                                        <a href='#'><img src='https://cdn-icons-png.flaticon.com/24/5968/5968958.png' style='margin:0 6px; width:24px;'></a>

        //                                        <p style='color:#bbb; font-size:12px; margin-top:15px;'>
        //                                            © {DateTime.Now.Year} Acanza Tech. All rights reserved.
        //                                        </p>
        //                                    </td>
        //                                </tr>

        //                            </table>

        //                        </td>
        //                    </tr>
        //                </table>

        //            </td>
        //        </tr>
        //    </table>
        //</body>
        //</html>";

        //                SendEmailCommonone(emailid.Trim(), subject, body, true);
        //            }
        //            catch { }
        //        }


        public void SendOtpEmailForEventUser(string otp, string emailId, int actionType = 1, string purpose = "XoxoFX")
        {
            try
            {
                string subject = "Your Event XoxoFX Verification Code";

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
      <a href='' style='color:#0047d1;text-decoration:none;font-weight:bold;'>
          support@xoxofx.ai
      </a>.
      We are here to support you and ensure your experience with us is exceptional.
  </p>

        <p style='margin-top:25px;'>Best Regards,<br/>XoxoFX Team</p>
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


        public void SendOtpEmailForUserRegistrationWelcomletter(string authLogin, string plainPassword, string emailid, string Name, int actionType)
        {
            string EmailID = emailid.Trim();

            try
            {
                string subject = "Welcome to XoxoFX, " + Name;
                //string logoUrl = "https://imagedelivery.net/nq9qT5FHZv9Sg48UUnD1-A/222f2792-e560-422a-09c6-17e44f99fa00/public";

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

                       


                        <!-- TITLE -->
                        <h2 style='text-align:center; color:#333; margin:0 0 8px;'>
                            🚀 Welcome to XoxoFX, {Name}!
                        </h2>

                        <p style='text-align:center; color:#666; margin-top:0;'>
                            We're thrilled to have you on board.
                        </p>

                        <!-- CONTENT -->
                        <p style='color:#444; line-height:1.6; text-align:justify;'>
                            The future isn’t coming — it’s already here, and it’s <b>Assetes</b>. 
                            With XoxoFX, you now have access to a powerful ecosystem where Assets work for you, 
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
                                    <p style='margin:0; font-size:16px;'><b>Login ID:</b> {authLogin}</p>
                                    <p style='margin-top:8px; font-size:16px;'><b>Password:</b> {plainPassword}</p>
                                </td>
                            </tr>
                        </table>

                        <p style='text-align:center; color:#666; font-size:15px;'>
                            Welcome aboard {Name}. Let’s create passive income — the XoxoFX way.
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
                            © {DateTime.Now.Year} XoxoFX. All rights reserved.
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


                bool sent = false;

                // ActionType Logic
                if (actionType == 1)
                {
                    sent = SendEmailCommonone(emailid.Trim(), subject, body, true);
                    if (!sent) sent = SendEmailCommonTWO(emailid.Trim(), subject, body, true);
                }
                else if (actionType == 2)
                {
                    sent = SendEmailCommonTWO(emailid.Trim(), subject, body, true);
                    if (!sent) sent = SendEmailCommonone(emailid.Trim(), subject, body, true);
                }
                else
                {
                    sent = SendEmailCommonone(emailid.Trim(), subject, body, true);
                }
            }
            catch { }
        }
        public void SendOtpEmailForForgotPassword(string authLogin, string authPass, string emailId, int actionType)
        {
            try
            {
                string userName = "XoxoFX User";
                string messageIntro = "As requested, here are your login credentials:";
                string emailTo = emailId?.Trim();

                // Subject
                string subject = "Your XoxoFX Login Credentials";

                // HTML Body
                StringBuilder html = new StringBuilder();

                html.Append("<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;");
                html.Append("border:2px solid transparent;border-radius:12px;");
                html.Append("background-image: linear-gradient(white, white), linear-gradient(90deg, #4A3AFF, #00C6FF);");
                html.Append("background-origin: border-box;background-clip: content-box, border-box;");
                html.Append("box-shadow: 0 4px 12px rgba(0,0,0,0.06);'>");

                // Inner content
                html.Append("<div style='background-color:#ffffff;padding:25px 20px;text-align:center;'>");


                html.Append($"<h2 style='color:#2c3e50;margin-bottom:8px;font-weight:600;'>Dear {userName},</h2>");
                html.Append($"<p style='color:#444;font-size:15px;margin-top:0;'>{messageIntro}</p>");

                // Credentials box
                html.Append("<div style='margin:20px auto 25px auto;max-width:90%;background:#eef3ff;");
                html.Append("border:1px solid #d0d9ff;border-radius:10px;padding:16px;'>");

                html.Append($"<p style='margin:0;font-size:16px;'><strong>Login ID:</strong> {authLogin}</p>");
                html.Append($"<p style='margin:8px 0 0;font-size:16px;'><strong>Password:</strong> {authPass}</p>");
                html.Append("</div>");

                html.Append("<p style='color:#555;font-size:14px;'>For your security, we recommend changing your password after logging in.</p>");
                html.Append("<p style='color:#333;font-size:14px;margin-top:8px;'>Thank you for choosing <strong>XoxoFX</strong>.</p>");
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
                html.Append("<p style='margin-top:10px;color:#aaa;'>© 2026 XoxoFX. All rights reserved.</p>");
                html.Append("</div>");
                html.Append("</div>");

                string body = html.ToString();
                bool sent = false;

                // 🔹 ActionType == 1 → pehle One, fallback Two
                if (actionType == 1)
                {
                    sent = SendEmailCommonone(emailTo, subject, body, true);
                    if (!sent)
                        sent = SendEmailCommonTWO(emailTo, subject, body, true);
                }
                // 🔹 ActionType == 2 → pehle Two, fallback One
                else if (actionType == 2)
                {
                    sent = SendEmailCommonTWO(emailTo, subject, body, true);
                    if (!sent)
                        sent = SendEmailCommonone(emailTo, subject, body, true);
                }
                // 🔹 Baaki sab (3, 4, 5... ya koi unknown) → sirf One
                else
                {
                    sent = SendEmailCommonone(emailTo, subject, body, true);
                }

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
    }
}
