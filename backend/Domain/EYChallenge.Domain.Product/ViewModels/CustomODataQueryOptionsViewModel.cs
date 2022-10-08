using Community.OData.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Domain.Product.ViewModels
{
    public class CustomODataQueryOptionsViewModel : IODataQueryOptions
    {
        public IReadOnlyCollection<string> Filters { get; set; }

        public string OrderBy { get; set; }

        public string Top { get; set; }

        public string Skip { get; set; }

        public string Select { get; set; }

        public string Expand { get; set; }
    }
}
