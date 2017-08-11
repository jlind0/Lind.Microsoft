using System;
using System.Collections.Generic;
using System.Text;

namespace Lind.Microsoft.Janus
{
    public interface IJanusObject : IJanusNamesapce
    {
        IJanusSchema Schema { get; }

        IReadOnlyCollection<IJanusAttributeValue> Members { get; }
    }
    public class JanusObject : JanusNamespace, IJanusObject
    {
        public IJanusSchema Schema => throw new NotImplementedException();

        public IReadOnlyCollection<IJanusAttributeValue> Members => throw new NotImplementedException();
    }
}
