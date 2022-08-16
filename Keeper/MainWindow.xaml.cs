using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Keeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly GridLength Zero = new(0);
        private static GridLength glMemory = new(0);
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_OnLoaded;
            Unloaded += MainWindow_OnUnloaded;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_OnLoaded;
            editorPanel.PreviewMouseWheel += EditorPanel_OnMouseWheel;
        }
        private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= MainWindow_OnUnloaded;
            // Detach mouse wheel CTRL-key zoom support
            PreviewMouseWheel -= EditorPanel_OnMouseWheel;
        }

        private void ExpandLeftPanelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ExpandLeftPanel();
        }

        private void CollapseLeftPanelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            CollapseLeftPane();
        }

        private void EditorPanel_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control) return;
            double fontSize = this.editorPanel.FontSize + e.Delta / 25.0;
            editorPanel.FontSize = fontSize switch
            {
                < 6 => 6,
                > 200 => 200,
                _ => fontSize
            };
            e.Handled = true;
        }

        private void LeftGridSplitter_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (leftPanel.RenderSize.Width < leftPanelMenuBar.MinWidth) CollapseLeftPane();
        }

        private void CollapseLeftPane()
        {
            expandLeftPanel.Visibility = Visibility.Visible;
            leftPanel.Visibility = Visibility.Collapsed;
            leftGridSplitter.Visibility = Visibility.Collapsed;
            glMemory = mainGrid.ColumnDefinitions[0].Width;
            mainGrid.ColumnDefinitions[0].Width = Zero;
        }

        private void ExpandLeftPanel()
        {
            leftPanel.Visibility = Visibility.Visible;
            leftGridSplitter.Visibility = Visibility.Visible;
            expandLeftPanel.Visibility = Visibility.Collapsed;
            leftPanel.RenderSize = new Size(200, leftPanel.Height);
            mainGrid.ColumnDefinitions[0].Width = glMemory.Value < new GridLength(200).Value ? new GridLength(200) : glMemory;
        }
    }
}
