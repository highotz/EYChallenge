using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EYChallenge.Utilities.SystemObjects.Extensions
{
    public static class ObjectExtensions
    {

        public static Object GetPropertyValue(this Object obj, String propName)
        {
            string[] nameParts = propName.Split('.');

            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach (String part in nameParts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }
    }
}
