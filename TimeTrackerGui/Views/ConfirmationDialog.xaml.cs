
using System;
using System.Windows;

using MahApps.Metro.Controls;

namespace TimeTracker.Views
{
    public partial class ConfirmationDialog : MetroWindow
    {
        public ConfirmationDialog()
        {
            InitializeComponent();

            btnNo.Click += (sender, e) =>
            {
                DialogResult = false;
            };

            btnYes.Click += (sender, e) =>
            {
                DialogResult = true;
            }; 
        }
    }
}
