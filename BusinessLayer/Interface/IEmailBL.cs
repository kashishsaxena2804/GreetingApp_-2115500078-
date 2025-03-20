using System;

namespace BusinessLayer.Interfaces
{
    public interface IEmailBL
    {
        void SendEmail(string to, string subject, string body);
    }
}
