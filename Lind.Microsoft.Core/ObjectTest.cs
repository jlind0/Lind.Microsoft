using System;
using System.Collections.Generic;
using System.Text;

namespace Lind.Microsoft.Core
{
    public static class ObjectTest
    {
        public static T TestForNull<T>(this T value)
        {
            var obj = value as object;
            if (obj == null)
                throw new NullReferenceException();
            return value;
        }
    }
}
