﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace RealTimeChatApp
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
