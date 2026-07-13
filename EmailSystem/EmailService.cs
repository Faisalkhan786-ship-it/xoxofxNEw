using Dapper;
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

namespace EmailSystem
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool SendEmailCommonRegistrtion(string EmailID, string subject, string body, bool IsHTML = true)
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
                    subject = subject,
                    Type = "Registration OTP"
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

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailCommononeWithDrawal(string EmailID, string subject, string body, bool IsHTML = true)
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
                    subject = subject,
                    Type = "WithDrawal"
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

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailCommononeFundRequest(string EmailID, string subject, string body, bool IsHTML = true)
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
                    subject = subject,
                    Type = "Fund Request"
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

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailCommononeWelcomeletter(string EmailID, string subject, string body, bool IsHTML = true)
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
                    subject = subject,
                    Type = "Welcome letter"
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

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool SendEmailCommononeForgotPassword(string EmailID, string subject, string body, bool IsHTML = true)
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
                    subject = subject,
                    Type = "Forgot Password"
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

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailCommononeUpdateProfile(string EmailID, string subject, string body, bool IsHTML = true)
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
                    subject = subject,
                    Type = "Update Profile"
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

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //-----------------Send OTP User Registration
        public void SendOtpEmailForUser(string otp, string emailId, string purpose = "XOXOFX")
        {
            try
            {
                string subject = "One-Time Passcode (OTP)";
                string name = "Dear User";
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
            Thank you for registering with <strong>XOXOFX</strong>.
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
        <p style='margin:4px 0;'>Thank you,<br/>The XOXOFXTeam</p>

        <div style='margin-top:12px;'>
            <a href='#' style='margin:0 8px;'><img src='https://cdn-icons-png.flaticon.com/24/174/174855.png' width='24'/></a>
            <a href='#' style='margin:0 8px;'><img src='https://cdn-icons-png.flaticon.com/24/733/733547.png' width='24'/></a>
            <a href='#' style='margin:0 8px;'><img src='https://cdn-icons-png.flaticon.com/512/5968/5968958.png' width='24'/></a>
        </div>
    </div>
</div>";

                bool sent = false;
                sent = SendEmailCommonRegistrtion(emailId.Trim(), subject, body, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error: " + ex.Message);
            }
        }

        //----------------Send OTP Withdrawal Request

        public void SendOtpEmailForRequestFundWithdrawal(string otp, string emailId, string WalletAddress, string name, string ukTime)
        {
            try
            {
                string subject = $"One-Time Passcode (OTP) — UK Time: {ukTime}";
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

        <p style='color:#444;font-size:15px;margin:0;'>Thank you for choosing <strong>XOXOFX</strong>.</p>
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
        <p style='margin:4px 0;'>Thank you,<br/>The XOXOFX Team</p>
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

                sent = SendEmailCommononeWithDrawal(emailId.Trim(), subject, body, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Fund Request): " + ex.Message);
            }
        }


        //----------------Send OTP Fund Request
        public void SendOtpEmailForRequestFund(string otp, string emailId, string name, string purpose = "XOXOFX")
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
        <p style='margin:4px 0;'>Thank you,<br/>The XOXOFX Technology Team</p>

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
                sent = SendEmailCommononeFundRequest(emailId.Trim(), subject, body, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Fund Request): " + ex.Message);
            }
        }


        //----------------------Welcome letter

        public void SendOtpEmailForUserRegistrationWelcomletter(string authLogin, string plainPassword, string emailid, string Name)
        {
            string EmailID = emailid.Trim();

            try
            {
                string subject = "Welcome to XOXOFX, " + Name;
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
                            🚀 Welcome to XOXOFX, {Name}!
                        </h2>

                        <p style='text-align:center; color:#666; margin-top:0;'>
                            We're thrilled to have you on board.
                        </p>

                        <!-- CONTENT -->
                        <p style='color:#444; line-height:1.6; text-align:justify;'>
                            The future isn’t coming — it’s already here, and it’s <b>Assetes</b>. 
                            With XOXOFX, you now have access to a powerful ecosystem where Assets work for you, 
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
                            Welcome aboard {Name}. Let’s create passive income — the XOXOFXway.
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
                            © {DateTime.Now.Year} XOXOFX. All rights reserved.
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
                sent = SendEmailCommononeWelcomeletter(emailid.Trim(), subject, body, true);

            }
            catch { }
        }

        //----------------------Forgost Password

        public void SendOtpEmailForForgotPassword(string authLogin, string authPass, string emailId)
        {
            try
            {
                string userName = "XOXOFXUser";
                string messageIntro = "As requested, here are your login credentials:";
                string emailTo = emailId?.Trim();

                // Subject
                string subject = "Your XOXOFXLogin Credentials";

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
                html.Append("<p style='color:#333;font-size:14px;margin-top:8px;'>Thank you for choosing <strong>XOXOFX</strong>.</p>");
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
                html.Append("<p style='margin-top:10px;color:#aaa;'>© 2026 XOXOFX. All rights reserved.</p>");
                html.Append("</div>");
                html.Append("</div>");

                string body = html.ToString();
                bool sent = false;
                sent = SendEmailCommononeForgotPassword(emailTo, subject, body, true);

            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Forgot Password): " + ex.Message);
            }
        }


        //        //profile Update 
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
             
                    sent = SendEmailCommononeUpdateProfile(emailId.Trim(), subject, body, true);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP Email Error (Fund Request): " + ex.Message);
            }
        }

     
    }
}
