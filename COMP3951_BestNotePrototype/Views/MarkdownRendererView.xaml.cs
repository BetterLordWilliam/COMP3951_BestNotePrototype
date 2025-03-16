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
	/// Gets the UR
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
	}
}