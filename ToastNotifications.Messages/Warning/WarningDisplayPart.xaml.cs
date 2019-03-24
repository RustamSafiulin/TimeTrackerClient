using System.Windows;
using ToastNotifications.Core;

namespace ToastNotifications.Messages.Warning
{
    /// <summary>
    /// Interaction logic for WarningDisplayPart.xaml
    /// </summary>
    public partial class WarningDisplayPart : NotificationDisplayPart
    {
        private readonly WarningMessage _viewModel;

        public WarningDisplayPart(WarningMessage warning)
        {
            InitializeComponent();

            _viewModel = warning;
            DataContext = warning;
        }

        public override string GetMessage()
        {
            return this._viewModel.Message;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            _viewModel.Close();
        }

        public override MessageOptions GetOptions()
        {
            return this._viewModel.Options;
        }
    }
}
