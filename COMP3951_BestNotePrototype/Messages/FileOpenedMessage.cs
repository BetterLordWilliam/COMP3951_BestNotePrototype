using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models.FileSystem;

///
/// Will Otterbein
/// April 6th 2025
///  
/// Relevant MSDN Documentation for messaging between components in the MVVM way
/// https://learn.microsoft.com/en-us/dotnet/architecture/maui/communicating-between-components
/// 
namespace BestNote_3951.Messages
{
    /// <summary>
    /// Message for handling file opened events.
    /// 
    /// <list type="bullet">
    /// <item>Published from the file tree</item>
    /// <item>Susbcribed in the Main Page -- I'll get to why it's not just the editor.</item>
    /// <item>Needs to initialize a new editor, rendered and PDF for each window</item>
    /// <item>Potential for future expansion to tabbed view</item>
    /// </list>
    /// 
    /// </summary>
    public class FileOpenedMessage : ValueChangedMessage<IBNFile>
    {
        /// <summary>
        /// Constructor for the FileOpenedMessage, expects a type IBNFile.
        /// </summary>
        /// <param name="file"></param>
        public FileOpenedMessage(IBNFile file) : base(file) { }
    }
}
