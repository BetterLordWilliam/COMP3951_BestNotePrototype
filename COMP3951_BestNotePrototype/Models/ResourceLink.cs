using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.PdfViewer;

namespace BestNote_3951.Models
{
    /// <summary>
    /// The ResourceLink class represents the link between a BestFile and its PDF resource. It 
    /// contains a path to the PDF path that it is linking to, as well as a Bookmark with the page
    /// number that the the Bookmark is linked to.
    /// </summary>
    internal class ResourceLink
    {

        /// <summary>
        /// The Bookmark containing the page number and name of the link.
        /// </summary>
        public Bookmark ResourceBookmark { get; set; }

        /// <summary>
        /// The path of the PDF resource that this resource is referencing.
        /// </summary>
        public string ResourcePath { get; set; }

        /// <summary>
        /// Initializes the ResourceBookmark and ResourcePath fields with the passed parameters.
        /// </summary>
        /// <param name="resourceBookmark">a Bookmark object</param>
        /// <param name="resourcePath">a String representing the path to a PDF</param>
        public ResourceLink(Bookmark resourceBookmark, String resourcePath)
        {
            ResourceBookmark = resourceBookmark;
            ResourcePath = resourcePath;
        }
    }
}
