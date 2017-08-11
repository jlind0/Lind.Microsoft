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
    
    public interface ISolution : IJanusObject
    {
        IReadOnlyCollection<IProject> Projects { get; set; }
    }
    public interface IProject : IJanusObject
    {
        ISolution Solution { get; }
    }
    
   
   
}
