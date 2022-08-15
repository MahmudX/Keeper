using System;
using System.Diagnostics;
using System.IO;
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
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_OnLoaded;
            Unloaded += MainWindow_OnUnloaded;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_OnLoaded;
            this.editorPanel.PreviewMouseWheel += EditorPanel_OnMouseWheel;
        }
        private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= MainWindow_OnUnloaded;

            // Detach mouse wheel CTRL-key zoom support
            this.PreviewMouseWheel -= EditorPanel_OnMouseWheel;
        }
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //string? text = await webView.ExecuteScriptAsync("function awesome(){return editor.getValue();}awesome();");
            //MessageBox.Show(text);
        }

        private void ExpandLeftPanelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            leftPanel.Visibility = Visibility.Visible;
            leftGridSplitter.Visibility = Visibility.Visible;
            expandLeftPanel.Visibility = Visibility.Collapsed;
        }

        private void CollapseLeftPanelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            CollapseLeftPane();
        }

        private void EditorPanel_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Debug.WriteLine($"e.Delta -> {e.Delta}");

            if (Keyboard.Modifiers != ModifierKeys.Control) return;
            double fontSize = this.editorPanel.FontSize + e.Delta / 25.0;

            this.editorPanel.FontSize = fontSize switch
            {
                < 6 => 6,
                > 200 => 200,
                _ => fontSize
            };

            e.Handled = true;

        }

        private void LeftPanel_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine("Changed" + e.NewSize.Width);
            //if (!(e.NewSize.Width < leftPanelMenuBar.MinWidth)) return;
            //CollapseLeftPane();
        }

        private void CollapseLeftPane()
        {
            expandLeftPanel.Visibility = Visibility.Visible;
            leftPanel.Visibility = Visibility.Collapsed;
            leftGridSplitter.Visibility = Visibility.Collapsed;
        }

        private void LeftGridSplitter_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            leftPanel.ShowGridLines = true;
            if(leftPanel.RenderSize.Width< leftPanelMenuBar.MinWidth)CollapseLeftPane();
            leftPanel.RenderSize = new Size(200, leftPanel.Height);
        }
    }
}
