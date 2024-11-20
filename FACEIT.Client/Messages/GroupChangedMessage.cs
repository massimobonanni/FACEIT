using CommunityToolkit.Mvvm.Messaging.Messages;
using FACEIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.Messages
{
    internal enum GroupChangeType
    {
        Updated,
        Deleted,
        Created
    }

    internal class GroupChangedMessage: ValueChangedMessage<Group>
    {
        public GroupChangedMessage(Group value, GroupChangeType changeType) : base(value)
        {
            this.ChangeType = changeType;
        }

        public GroupChangeType ChangeType { get; private set; }
    }
}
