
using MahApps.Metro.Controls;

using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class EntryWindow : MetroWindow
    {
        public EntryWindow(EntryViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            this.DataContext = _viewModel;
            this.Closing += (sender, e) => { _viewModel.OnAppNeedExitHandler(); };
        }

        #region Props and Fields

        private EntryViewModel _viewModel;

        #endregion
    }
}
