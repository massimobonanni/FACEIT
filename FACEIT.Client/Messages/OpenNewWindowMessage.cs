using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Messages
{
    internal class OpenNewWindowMessage : ValueChangedMessage<string>
    {
        public OpenNewWindowMessage(string value) : base(value)
        {

        }

        public object Data { get; set; }
    }
}
