using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Lind.Microsoft.Core;
using System.Runtime.CompilerServices;

namespace Lind.Microsoft.Janus
{
    public interface IJanusRepository
    {
        Task<IJanusSchema> GetSchema(Uri id, CancellationToken token = default(CancellationToken));
        Task<Uri> GetNewUri(string name, Uri parent = null, CancellationToken token = default(CancellationToken));
        Task<IJanusObject> GetObject(Uri id, CancellationToken token = default(CancellationToken));
        Task<ICollection<IJanusAttribute>> GetAttributes(Uri id, CancellationToken token = default(CancellationToken));
        Task<ICollection<IJanusAttributeValue>> GetAttributeValues(Uri id, CancellationToken token = default(CancellationToken));
        Task<ICollection<IJanusAttributeValue<T>>> GetAttributeValues<T>(Uri id, CancellationToken token = default(CancellationToken));
        Task AddAttributes(ICollection<IJanusAttribute> attributes, Uri id, CancellationToken token = default(CancellationToken));
        Task AddAttribute(IJanusAttribute attributes, Uri id, CancellationToken token = default(CancellationToken));
    }
}
