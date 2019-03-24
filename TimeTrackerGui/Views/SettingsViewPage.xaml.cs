
using System;
using System.Windows.Controls;

using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class SettingsViewPage : Page
    {
        public SettingsViewPage(SettingsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            this.DataContext = _viewModel;
        }

        #region Props and Fields

        private SettingsViewModel _viewModel;

        #endregion
    }
}
