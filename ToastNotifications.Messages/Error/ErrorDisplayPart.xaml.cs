using System.Windows;
using ToastNotifications.Core;

namespace ToastNotifications.Messages.Error
{
    /// <summary>
    /// Interaction logic for ErrorDisplayPart.xaml
    /// </summary>
    public partial class ErrorDisplayPart : NotificationDisplayPart
    {
        private readonly ErrorMessage _viewModel;

        public ErrorDisplayPart(ErrorMessage error)
        {
            InitializeComponent();

            _viewModel = error;
            DataContext = error;
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
