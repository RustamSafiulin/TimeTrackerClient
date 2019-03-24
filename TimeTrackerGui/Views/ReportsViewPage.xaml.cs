
using System;
using System.Windows.Controls;

using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class ReportsViewPage : Page
    {
        public ReportsViewPage(ReportsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            this.DataContext = _viewModel;
        }

        #region Props and Fields

        private ReportsViewModel _viewModel;

        #endregion
    }
}
