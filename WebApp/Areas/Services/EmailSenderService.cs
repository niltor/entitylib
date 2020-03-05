using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Areas.Options;

namespace WebApp.Areas.Services
{
    public class EmailSenderService : IEmailSender
    {
        readonly string _apiKey;
        readonly string fromEmail = "no-response@entitylib.com";
        readonly IWebHostEnvironment _env;
        public EmailSenderService(IOptions<MailOptions> options, IWebHostEnvironment env)
        {
            _apiKey = options.Value.APIKey;
            _env = env;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var response = await SendAsync(fromEmail, email, subject, null, htmlMessage, "EntityLib");
            if (response.StatusCode != HttpStatusCode.OK
                && response.StatusCode != HttpStatusCode.Accepted)
            {
                // TODO:记录错误信息
                var result = await response.Body.ReadAsStringAsync();

            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromEamil"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="htmlContent"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <returns></returns>
        public async Task<Response> SendAsync(string fromEamil, string toEmail, string subject, string content = null, string htmlContent = null, string fromName = null, string toName = null)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(fromEamil, fromName);
            var to = new EmailAddress(toEmail, toName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;

        }

        /// <summary>
        /// 注册激活邮件
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> SendRegisterMail(string toEmail, string userId)
        {
            string host = "https://publist.vof.media";
            if (_env.IsDevelopment())
            {
                host = "http://localhost:4200";
            }
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var htmlContent = "<p>如果您没有注册【PubList】账户，请忽略该邮件</p>";
            htmlContent += "<p>请点击以下链接激活验证您的邮箱（两小时内有效）：</p>";
            htmlContent += $@"<a href=""{host}/home/verify_email?code={userId}&time={time}""
style=""background: #1e5b99;color:rgb(255, 255, 255);padding:8px 16px;text-decoration: none"">激活邮箱</a>";
            var response = await SendAsync(fromEmail, toEmail, "PubList账号激活", null, htmlContent, "PubList");
            if (response.StatusCode != HttpStatusCode.OK
                && response.StatusCode != HttpStatusCode.Accepted)
            {
                // TODO:记录错误信息
                var result = await response.Body.ReadAsStringAsync();
                Console.WriteLine(result);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> SendVerifyCodeAsync(string toEmail, string code)
        {
            var htmlContent = "<p>您请求的【PubList】验证码为：</p>";
            htmlContent += $@"<p style=""color:blue;font-size:20px""><strong>{code}</strong></p>";
            var response = await SendAsync(fromEmail, toEmail, "PubList验证码", null, htmlContent, "PubList");
            if (response.StatusCode != HttpStatusCode.OK
                && response.StatusCode != HttpStatusCode.Accepted)
            {
                // TODO:记录错误信息
                var result = await response.Body.ReadAsStringAsync();
                Console.WriteLine(result);
                return false;
            }
            return true;
        }
    }
}
