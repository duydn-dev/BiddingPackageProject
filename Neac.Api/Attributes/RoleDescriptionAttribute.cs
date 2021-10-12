using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public RoleDescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class RoleGroupDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public RoleGroupDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
