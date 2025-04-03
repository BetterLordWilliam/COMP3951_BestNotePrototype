using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// April 1 2025
/// 
/// MVVM Doesn't work with the native DisplayAlert MAUI API, so alerts need to be shown otherwise.
/// These guys on this stack overflow thread pretty much solved this problem for me.
/// https://stackoverflow.com/questions/72429055/how-to-displayalert-in-a-net-maui-viewmodel
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

        /// <summary>
        /// Asynchronouslt show a confirmation box alert.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="accept"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel);
        }

        /// <summary>
        /// Show alert message, with callback.
        /// </summary>
        /// <param name="callback">Action to perform afterwards.</param>
        public void ShowConfirmation(string title, string message, Action<bool> callback,
                                     string accept = "Yes", string cancel = "No")
        {
            Application.Current!.MainPage!.Dispatcher.Dispatch(async () =>
            {
                bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(answer);
            });
        }
    }
}
