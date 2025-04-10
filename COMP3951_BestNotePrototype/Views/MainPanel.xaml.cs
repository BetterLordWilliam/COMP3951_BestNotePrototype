using Microsoft.Maui.Controls;

namespace BestNote_3951.Views
{
    /**
     *
     * - We define five columns total: three for content (Editor, Renderer, PDF) and
     *   two for narrow 'splitter' columns. 
     * - The total width is split among the 3 content windows.
     * - The widths are adjusted when user drags the "splitter" columns
     * - When the window is resized, the width is redistributed to the 3 content windows, 
     * but the ratio of distribution is preserved.
     */
    public partial class MainPanel : ContentView
    {
        /// <summary>
        /// minimum width of the views in device pixels
        /// </summary>
        private int minWidth = 50;

        /// <summary>
        /// Total width to be distributed to the content columns
        /// </summary>
        private double _totalForContentColumns;

        /// <summary>
        /// doubles to store the column widths when dragging begins.
        /// col 1 & 3 are the "splitter" columns. 0, 2 ,4 are content columns.
        /// 0 & 4 only have one side that is adjustable, 2 has both sides.
        /// </summary>
        private double _col0WidthStart;
        private double _col2WidthStartLeft;
        private double _col2WidthStartRight;
        private double _col4WidthStart;

        /// <summary>
        /// initial mouse position when dragging a splitter
        /// </summary>
        private double _startX;

        public MainPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// when MAUI allocates the size for the window, we override it to distribute the available width to the content columns
        /// </summary>
        /// <param name="width"> width of the window</param>
        /// <param name="height"> height of the window</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width <= 0)
            {
                return;
            }

            // Subtract the splitter columns’ widths from the total available width.
            double splitterWidthTotal = Splitter1.WidthRequest + Splitter2.WidthRequest;
            double available          = width - splitterWidthTotal;

            // Current absolute widths of columns 0, 2, and 4.
            double w0 = MainGrid.ColumnDefinitions[0].Width.Value;
            double w2 = MainGrid.ColumnDefinitions[2].Width.Value;
            double w4 = MainGrid.ColumnDefinitions[4].Width.Value;
            double sum = w0 + w2 + w4;

            if (sum <= 0)
            {
                return;
            }

            // rescale the content colums so the sum == available width
            // for preserving the ratio when the window is resized
            double factor = available / sum;
            SetAbsoluteColumnWidth(0, w0 * factor);
            SetAbsoluteColumnWidth(2, w2 * factor);
            SetAbsoluteColumnWidth(4, w4 * factor);
        }

        /// <summary>
        /// sets a columns width in the xaml to be the desired width in pixels
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="width"></param>
        private void SetAbsoluteColumnWidth(int columnIndex, double width)
        {
            if (width < 0) width = 0;
            MainGrid.ColumnDefinitions[columnIndex].Width
                = new GridLength(width, GridUnitType.Absolute);
        }

        /// <summary>
        /// handles the drag gesture between columns 0 and 2.
        /// 
        /// when in the started state, captures the mouse position and absolute widths of col 0 & 2
        /// when in running, calculate the distance the mouse moveed and resize the columns accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPanUpdated_Splitter1(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    // capture hte widths for col 0 (editor) and col 2 (renderer)
                    _col0WidthStart = MainGrid.ColumnDefinitions[0].Width.Value;
                    _col2WidthStartLeft = MainGrid.ColumnDefinitions[2].Width.Value;
                    _startX = e.TotalX;
                    break;

                case GestureStatus.Running:
                    double delta = e.TotalX - _startX;

                    double newCol0 = _col0WidthStart + delta;

                    if (newCol0 < minWidth)
                    {
                        newCol0 = minWidth;
                    }

                    // make sure the total width is the same as it started
                    double sum     = _col0WidthStart + _col2WidthStartLeft;
                    double newCol2 = sum - newCol0;
                    if (newCol2 < minWidth)
                    {
                        newCol2 = minWidth;
                        newCol0 = sum - minWidth;

                        if (newCol0 < minWidth)
                        {
                            newCol0 = minWidth;
                        }
                    }

                    // apply the ne wwidths to editor and renderer
                    SetAbsoluteColumnWidth(0, newCol0);
                    SetAbsoluteColumnWidth(2, newCol2);
                    break;
            }
        }


        /// <summary>
        /// same idea as slitter1, except for the spitter between the renderer and the pdf view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPanUpdated_Splitter2(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _col2WidthStartRight = MainGrid.ColumnDefinitions[2].Width.Value;
                    _col4WidthStart      = MainGrid.ColumnDefinitions[4].Width.Value;
                    _startX = e.TotalX;
                    break;

                case GestureStatus.Running:
                    double delta   = e.TotalX - _startX;
                    double newCol2 = _col2WidthStartRight + delta;
                    if (newCol2 < 50) newCol2 = 50;

                    double sum     = _col2WidthStartRight + _col4WidthStart;
                    double newCol4 = sum - newCol2;
                    if (newCol4 < minWidth)
                    {
                        newCol4 = minWidth;
                        newCol2 = sum - 50;
                        if (newCol2 < 50) newCol2 = 50;
                    }

                    SetAbsoluteColumnWidth(2, newCol2);
                    SetAbsoluteColumnWidth(4, newCol4);
                    break;
            }
        }
    }
}
