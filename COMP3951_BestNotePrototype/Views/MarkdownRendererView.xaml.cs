using System;
using BestNote_3951.Messages;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Specialized;
using System.Web;

namespace BestNote_3951.Views;

public partial class MarkdownRendererView : ContentView
{
	public MarkdownRendererView()
	{
		InitializeComponent();
        MarkdownWebView.Navigating += MarkdownWebView_Navigating;
    }

	/// <summary>
	///  if the event args contain a url that has the bestnote:// scheme it cancels
	///  the default behavior and we turn the url into a uri for local navigation.
	///  we ge tthe page number by parsing the query string and then send the page
	///  number to the pdfViewer that is registered for the event.
	///  
	/// sadly it doesn't actually GO to the page, it just updates the page number and the
	/// user has to physically press enter because the syncfusion pdfViewer doesn't
	/// have a public API that lets us simulate a keypress event for unknown arcane 
	/// reasons of reat mystery and confusion.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void MarkdownWebView_Navigating(object sender, WebNavigatingEventArgs e)
	{
		if (e.Url.StartsWith("bestnote://"))
		{
			Uri uri = new Uri(e.Url);

			NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
			int pageNumber;
			bool isParsed = int.TryParse(query["pg"], out pageNumber);

			if (isParsed)
			{
				MarkdownLinkClickedMessage msg = new MarkdownLinkClickedMessage(pageNumber);
				WeakReferenceMessenger.Default.Send(msg);
			}
		}

		if (e.Url.Contains("#"))
		{
			e.Cancel = true;

			var anchor = e.Url.Substring(e.Url.IndexOf('#') + 1);
			(sender as WebView)?.Eval($"document.getElementById('{anchor}')?.scrollIntoView();");
		}
	}
}