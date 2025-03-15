using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951.ViewModels
{
    public partial class MainPanelViewModel : ObservableObject
    {
        #region ViewModels
        public EmbeddedPdfViewModel EmbeddedPdfViewModel { get; private set; }
        public MarkdownEditorViewModel MarkdownEditorViewModel { get; private set; }
        public MarkdownRendererViewModel MarkdownRendererViewModel { get; private set; }
        #endregion

        public MainPanelViewModel(
            EmbeddedPdfViewModel embeddedPdfViewModel,
            MarkdownEditorViewModel markdownEditorViewModel,
            MarkdownRendererViewModel markdownRendererViewModel
        )
        {
            EmbeddedPdfViewModel = embeddedPdfViewModel;
            MarkdownEditorViewModel = markdownEditorViewModel;
            MarkdownRendererViewModel = markdownRendererViewModel;
        }
    }
}
