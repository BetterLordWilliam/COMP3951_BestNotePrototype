using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951.Services
{
    public class AlertService
    {
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current!.MainPage!.DisplayAlert(title, message, cancel);
        }
    }
}
