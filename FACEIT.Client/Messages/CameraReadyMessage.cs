using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FACEIT.Client.Messages
{
    public sealed class CameraReadyMessage : ValueChangedMessage<bool>
    {
        public CameraReadyMessage(bool value) : base(value)
        {
        }
    }
}
