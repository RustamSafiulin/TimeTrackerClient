using System.Windows;
using ToastNotifications.Core;

namespace ToastNotifications.Messages.Information
{
    /// <summary>
    /// Interaction logic for InformationDisplayPart.xaml
    /// </summary>
    public partial class InformationDisplayPart : NotificationDisplayPart
    {
        private readonly InformationMessage _viewModel;

        public InformationDisplayPart(InformationMessage information)
        {
            InitializeComponent();

            _viewModel = information;
            DataContext = information;
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
