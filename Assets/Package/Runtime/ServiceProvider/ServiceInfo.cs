using System;
using System.Collections.Generic;

namespace ADM
{
    internal class ServiceInfo
    {
        public Type Interface                   { get; set; }
        public Type Implementation              { get; set; }
        public IEnumerable<Type> Dependencies   { get; set; }
        public bool IsSingleton                 { get; set; }
        public object Instance                  { get; set; }
    }
}
