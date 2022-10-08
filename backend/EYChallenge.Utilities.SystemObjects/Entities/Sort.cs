using EYChallenge.Utilities.SystemObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Entities
{
    public class Sort<T>
    {
        public SortDirection Direction { get; set; }
        public Func<T, object> Action { get; set; }

        public Sort(SortDirection direction, Func<T, object> action)
        {
            Direction = direction;
            Action = action;
        }
    }
}
