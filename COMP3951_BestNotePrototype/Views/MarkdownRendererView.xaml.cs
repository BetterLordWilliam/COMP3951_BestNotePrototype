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
	/// if the event args contain a markdown heading (#), then use javascript to
	/// scroll to that heading in the webview.
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
			string? pdfPath = query["pdf"];

            bool isParsed = int.TryParse(query["pg"], out pageNumber);
			

			if (isParsed && pdfPath != null)
			{

				MarkdownLinkClickedPathMessage pdfMsg = new MarkdownLinkClickedPathMessage(pdfPath);
				WeakReferenceMessenger.Default.Send(pdfMsg);

				

				MarkdownLinkClickedMessage msg = new MarkdownLinkClickedMessage(pageNumber);
				WeakReferenceMessenger.Default.Send(msg);
            }
		}

		// if there is a markdown heading in the link, cancel default behavior and use a lil bit of 
		// java script to scroll to the heading.
		if (e.Url.Contains("#"))
		{
			e.Cancel = true;

			string anchor = e.Url.Substring(e.Url.IndexOf('#') + 1);
			(sender as WebView)?.Eval($"document.getElementById('{anchor}')?.scrollIntoView();");
		}
	}
}