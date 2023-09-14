using MassTransit;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using CommonLayer.Model;

namespace Collab_Consumer.MailService
{
    public class MailConsumer : IConsumer<CollabEmailModel>
    {
        public async Task Consume(ConsumeContext<CollabEmailModel> context)
        {
            var data = context.Message;
            
        }
    }   
}
