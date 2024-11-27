using CommunityToolkit.Mvvm.Messaging.Messages;
using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Messages
{
    internal class PersonRecognizedMessage : ValueChangedMessage<Client.Entities.RecognizedPerson>
    {
        public PersonRecognizedMessage(Client.Entities.RecognizedPerson value) : base(value)
        {

        }
    }
}
