using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;
using WeText.Common.Services;
using WeText.Services.Common;

namespace WeText.Services.Social
{
    public sealed class SocialService : Microservice
    {
        public SocialService(ICommandConsumer commandConsumer, IEventConsumer eventConsumer)
            : base(commandConsumer, eventConsumer)
        {

        }
    }
}
