using LucidbrndClear.Data.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucidbrndClear
{
    public static class EmailService
    {
        public static async Task SendEmailToCreatorsAsync(string EmailResiver,string Adress,string FIO,string PhoneNumber,string order)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ТЕСТSENDER", "lucid.delivery@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("ТЕСТRECIEVER", "lucid.delivery@gmail.com"));
            emailMessage.Subject = "Новый заказ Lucidbrnd";
            string NewOrderInformation = null;
            NewOrderInformation += "------------ИНФОРМАЦИЯ О ЗАКАЗЕ------------" + "\n" +
                                    "Заказ:            " + order + "\n\n";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "------------ИНФОРМАЦИЯ О ПОЛУЧАТЕЛЕ------------" + "\n" +
                       "Адрес:              " + Adress + "\n" +
                       "ФИО:                " + FIO + "\n" +
                       "Номер телефона:     " + PhoneNumber + "\n" +
                       "Email:              " + EmailResiver + "\n\n\n\n\n" +
                       "------------ИНФОРМАЦИЯ О ЗАКАЗЕ------------" + "\n" +
                       NewOrderInformation
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("lucid.delivery@gmail.com", "baretople");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public static async Task SendEmailToCustomer(string EmailResiver,string check)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ТЕСТSENDER", "lucid.delivery@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("ТЕСТRECIEVER", EmailResiver));
            emailMessage.Subject = "Ваш чек к заказу Lucidbrnd";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = check
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("lucid.delivery@gmail.com", "baretople");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public static async Task SendErrorEmail()
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ТЕСТSENDER", "lucid.delivery@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("ТЕСТRECIEVER", "lucid.delivery@gmail.com"));
            emailMessage.Subject = "!!!!!!!НА САЙТЕ ПРОИЗОШЛА ОШИБКА!!!!!!!!";

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "!!!!!!!НА САЙТЕ ПРОИЗОШЛА ОШИБКА!!!!!!!!"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("lucid.delivery@gmail.com", "baretople");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
