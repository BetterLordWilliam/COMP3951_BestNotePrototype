using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using BestNote_3951.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.Maui.PdfViewer;
using BestNote_3951.Messages;



/// <summary>
/// AUTHOR: Olivia Grace worked on the EmbeddedPdfView files
/// SOURCES:
/// Used the following Syncfusion PDF viewer documentatiion to customize the toolbar:
/// https://help.syncfusion.com/maui/pdf-viewer/toolbar
/// </summary>
namespace BestNote_3951.Views;

/// <summary>
/// Contains functionality for opening a pdf from the user's file system.
/// </summary>
public partial class EmbeddedPdfView : ContentView
{
    /// <summary>
    /// Initializes this EmbeddedPdfView, binds this EmbeddedPdfView to the EmbeddedPdfViewModel
    /// and customizes the PDF viewer tool bar.
    /// </summary>
	public EmbeddedPdfView()
	{
		InitializeComponent();
        CustomizePDFToolbar();
        pdfViewer.DocumentLoaded += PdfViewer_DocumentLoaded;

        if (this.BindingContext is EmbeddedPdfViewModel vm)
        {
            vm.PropertyChanged += OnPropertyChanged;
        }

        WeakReferenceMessenger.Default.Register<MarkdownLinkClickedMessage>(this, (recipient, message) =>
        {
            
            if (pdfViewer.PageCount > 0 && message.Value > 0)
            {
                if (pdfViewer.GoToPageCommand.CanExecute(message.Value))
                {
                    pdfViewer.GoToPageCommand.Execute(message.Value);                   
                    pdfViewer.Unfocus();
                }
            }

            // pdfViewer.ZoomMode = ZoomMode.FitToWidth;
        });

    }


    /// <summary>
    /// Removes the printer, annotations, previous page, next page, and page layour icons from
    /// the Syncfusion PDF Viewer toolbar.
    /// </summary>
    private void CustomizePDFToolbar()
    {
        var printer = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Print");
        var annotations = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Annotations");
        var prevPage = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Previous page");
        var nextPage = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Next page");
        var pageLayout = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Page layout mode");

        if (printer != null && annotations != null && prevPage != null && nextPage != null && pageLayout != null)
        {
            printer.IsVisible = false;
            annotations.IsVisible = false;
            prevPage.IsVisible = false;
            nextPage.IsVisible = false;
            pageLayout.IsVisible = false;
        }
    }


    /// <summary>
    /// When the view model's PageNum changes, we use the GoToPageCommand to navigate.
    /// </summary>
    //private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    if (sender is EmbeddedPdfViewModel vm && e.PropertyName == nameof(vm.PageNum))
    //    {
    //        if (pdfViewer.PageCount > 0 && vm.PageNum > 0)
    //        {
    //            if (pdfViewer.GoToPageCommand.CanExecute(vm.PageNum))
    //            {
    //                pdfViewer.GoToPageCommand.Execute(vm.PageNum);
    //                pdfViewer.Unfocus();
    //            }
    //        }
    //    }
    //}

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is EmbeddedPdfViewModel vm && e.PropertyName == nameof(vm.PageNum))
        {
            if (pdfViewer.PageCount > 0 && vm.PageNum > 0)
            {
                if (pdfViewer.GoToPageCommand.CanExecute(vm.PageNum))
                {
                    pdfViewer.GoToPageCommand.Execute(vm.PageNum);
                    pdfViewer.Unfocus();
                }
            }
        }
    }



    /// <summary>
    /// When the document loads, navigate to the page specified in the view model.
    /// </summary>
    private async void PdfViewer_DocumentLoaded(object sender, System.EventArgs e)
    {
        if (BindingContext is EmbeddedPdfViewModel vm && vm.PageNum > 0)
        {
            if (pdfViewer.PageCount > 0)
            {
                if (pdfViewer.GoToPageCommand.CanExecute(vm.PageNum))
                {
                    pdfViewer.GoToPageCommand.Execute(vm.PageNum);
                    pdfViewer.Unfocus();
                }
            }
        }
    }

}