using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace polestar
{
    public partial class SearchView : UserControl
    {
        public event EventHandler<string>? SearchSubmitted;
        public event EventHandler? CloseRequested;

        public SearchView()
        {
            InitializeComponent();

            BtnCloseSearch.Cursor = Cursors.Hand;
            BtnExecuteSearch.Cursor = Cursors.Hand;
            SearchPlaceholder.IsHitTestVisible = false;
            SearchTextBox.CaretBrush = Brushes.White;
            SearchTextBox.KeyDown += SearchTextBox_KeyDown;
        }

        public void FocusInput()
        {
            SearchTextBox.Focus();
            SearchTextBox.SelectAll();
        }

        private void BtnCloseSearch_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnExecuteSearch_Click(object sender, RoutedEventArgs e)
        {
            SubmitSearch();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchPlaceholder != null)
            {
                SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitSearch();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void SubmitSearch()
        {
            string query = SearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(query))
            {
                SearchSubmitted?.Invoke(this, query);
            }
        }
    }
}

