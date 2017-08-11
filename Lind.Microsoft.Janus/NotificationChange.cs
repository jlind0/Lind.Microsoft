using System;
using System.Collections.Generic;
using System.Text;

namespace Lind.Microsoft.Janus
{
    public delegate void JanusNotificationChange(Uri instanceId, Uri attributeId, ChangeType changeType, object oldValue, object newValue);

    public enum ChangeType
    {
        Create,
        Add,
        Remove,
        Update,
        Replace,
        Destroy
    }
    public interface IJanusNotificationChange
    {
        event JanusNotificationChange NotificationChange;
    }
}
