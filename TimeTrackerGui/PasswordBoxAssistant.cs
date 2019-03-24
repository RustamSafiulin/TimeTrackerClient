
using System;
using System.Windows;
using System.Windows.Controls;

namespace TimeTracker
{
    public class PasswordBoxAssistant : DependencyObject
    {
        #region PlaceHolderText

        public static bool GetPlaceHolderText(DependencyObject obj)
        {
            return (bool)obj.GetValue(PlaceHolderTextProperty);
        }
        public static void SetPlaceHolderText(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceHolderTextProperty, value);
        }

        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.RegisterAttached("PlaceHolderText", typeof(string),
                typeof(PasswordBoxAssistant),
                new UIPropertyMetadata(string.Empty, PlaceHolderTextChanged));
        private static void PlaceHolderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PasswordBox)) return;
            ((PasswordBox)d).PasswordChanged +=
                (sender, args) =>
                {
                    var pb = sender as PasswordBox;
                    pb.SetValue(HasPasswordProperty, (pb.Password.Length > 0));
                };
        }

        #endregion

        #region HasPassword

        public bool HasPassword
        {
            get { return (bool)GetValue(HasPasswordProperty); }
            set { SetValue(HasPasswordProperty, value); }
        }
        private static readonly DependencyProperty HasPasswordProperty =
            DependencyProperty.RegisterAttached("HasPassword",
                typeof(bool), typeof(PasswordBoxAssistant),
                new FrameworkPropertyMetadata(false));

        #endregion

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(String), typeof(PasswordBoxAssistant),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
                    DependencyProperty.RegisterAttached("Attach",
                    typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
                    DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
                    typeof(PasswordBoxAssistant));

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static String GetPassword(DependencyObject dp)
        {
            return (String)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, String value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
