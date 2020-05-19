using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InOutLogging
{
    public class InOutLoggingOptions
    {
        public PathString[] ExcludedRoutes { get; set; } = Array.Empty<PathString>();

        public PathString[] NoContentRoutes { get; set; } = Array.Empty<PathString>();

    }
}
