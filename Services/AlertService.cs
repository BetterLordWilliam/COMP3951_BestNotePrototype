using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// March 12 2025
///
namespace BestNote_3951.Services
{
    /// <summary>
    /// Used to generate UI alerts.
    /// </summary>
    public class AlertService
    {
        /// <summary>
        /// Asynchronously show an alert
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current!.MainPage!.DisplayAlert(title, message, cancel);
        }
    }
}
