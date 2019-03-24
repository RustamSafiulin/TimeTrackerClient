
using System;
using System.Windows;
using MahApps.Metro.Controls;

using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class ProfileSettingsWindow : MetroWindow
    {
        public ProfileSettingsWindow(Window owner, ProfileSettingsViewModel viewModel)
        {
            InitializeComponent();

            _owner = owner;
            _viewModel = viewModel;
            this.DataContext = _viewModel;
            this.Closing += (sender, e) => { _viewModel.OnCloseProfileSettingsWindowHandler(); e.Cancel = true; };
            this.IsVisibleChanged += (sender, e) =>
            {
                switch (Visibility)
                {
                    case Visibility.Visible:
                        this.Owner = _owner;
                        _owner.Opacity = 0.3;
                        break;
                    case Visibility.Hidden:
                        _owner.Opacity = 1.0;
                        break;
                }
            };

            this.Topmost = true;
        }

        #region Props and Fields

        private Window _owner;
        private ProfileSettingsViewModel _viewModel;

        #endregion
    }
}
