using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc.Mail
{
    /// <summary>
    /// 邮件辅助类
    /// </summary>
    public class MailUtil
    {
        public static void SendMail(string sendTo, string title = "", string content = "")
        {
            string sendFrom = "", password = "";//从数据库读
            SmtpClient client = new SmtpClient("smtp.163.com");//默认网易
            client.Port = 25;
            client.Credentials = new System.Net.NetworkCredential(sendFrom, password);
            client.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(sendFrom);//和上面的对应
            mail.To.Add(sendTo);
            mail.Subject = title;//标题
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = content;
            mail.Priority = System.Net.Mail.MailPriority.High;//邮件优先级
            mail.IsBodyHtml = true;
            client.SendAsync(mail, "myUserToken");//异步发送第二个参数时一个用户定义对象，此对象将被传递给完成异步操作时所调用的方法,参数默认即可
        }
    }
}
