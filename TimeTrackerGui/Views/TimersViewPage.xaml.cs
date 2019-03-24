
using System;
using System.Windows.Controls;

using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class TimersViewPage : Page
    {
        public TimersViewPage(TimersViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            this.DataContext = _viewModel;
        }

        #region Props and Fields

        private TimersViewModel _viewModel;

        #endregion
    }
}
