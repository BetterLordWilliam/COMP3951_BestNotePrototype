using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951.Models
{
    internal class ResourceLink
    {
        public string Name { get; set; }

        public int PageNumber { get; set; }

        public string ResourcePath { get; set; }

        public ResourceLink(String name, int pageNum, String resourcePath)
        {
            Name = name;
            PageNumber = pageNum;
            ResourcePath = resourcePath;
        }
    }
}
