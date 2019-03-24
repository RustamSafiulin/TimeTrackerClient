
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using MahApps.Metro.Controls;

using TimeTracker.ViewModels;

namespace TimeTracker.Views
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow(IDialogService dialogService, ApplicationViewModel viewModel)
        {
            InitializeComponent();

            _dialogService = dialogService;
            _viewModel = viewModel;
            this.DataContext = _viewModel;

            lwPageChanger.SelectionChanged += PageChangedHandler;

            _navigationService = NavigationFrame.NavigationService;

            SourceInitialized += (obj, sender) =>
            {
                this.SetForegroundWindow();
            };

            Closing += MainViewClosing;
        }

        private void MainViewClosing(object sender, CancelEventArgs e)
        {
            _dialogService.SetDialogsOwner(this);

            bool? dialogResult = _dialogService.ShowDialog<ConfirmationDialogModel>(new ConfirmationDialogModel()
            {
                Title = this.Title,
                InformationText = "Вы действительно хотите выйти?"
            });

            if (dialogResult.Value)
            {
                e.Cancel = false;
                _viewModel.OnAppNeedExitHandler();
            }
            else
            {
                e.Cancel = true;
            }
        }

        #region Props and Fields

        private IDialogService _dialogService;
        private ApplicationViewModel _viewModel;

        private List<Page> _pages = new List<Page>();
        private readonly NavigationService _navigationService;

        #endregion

        public void AddPage(Page page)
        {
            _pages.Add(page);
        }

        public void Run()
        {
            _navigationService.Navigate(_pages.ElementAt(0));
        }

        private void PageChangedHandler(object sender, RoutedEventArgs eventArgs)
        {
            var listView = sender as System.Windows.Controls.ListView;

            if (listView != null)
            {
                var page = _pages.ElementAt(listView.SelectedIndex);
                _navigationService.Navigate(page);
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as System.Windows.Controls.ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void ListViewCollapsedAnimationEvent(object sender, EventArgs e)
        {
            lwStateLabel.Visibility = Visibility.Hidden;
            lwUpdatesLabel.Visibility = Visibility.Hidden;
            lwSettingsLabel.Visibility = Visibility.Hidden;
        }

        private void ListViewRiseAnimationEvent(object sender, EventArgs e)
        {
            lwStateLabel.Visibility = Visibility.Visible;
            lwUpdatesLabel.Visibility = Visibility.Visible;
            lwSettingsLabel.Visibility = Visibility.Visible;
        }
    }
}
