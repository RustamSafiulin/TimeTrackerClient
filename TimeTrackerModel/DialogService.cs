
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

using UssManagement.Helpers;
using UssManagement.Localization;

namespace UssManagement
{
    public class ModalDialogBase : DomainModelBase
    {
        #region Props and Fields

        private String _informationText = String.Empty;
        public String InformationText
        {
            get { return _informationText; }
            set { SetProperty(ref _informationText, value); }
        }

        private String _title = String.Empty;
        public String Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion

        public ModalDialogBase()
        { }
    }

    public class InformationDialogModel : ModalDialogBase
    {
        public InformationDialogModel()
        { }
    }

    public class ConfirmationDialogModel : ModalDialogBase
    {
        public ConfirmationDialogModel()
        { }
    }

    public interface IDialogService
    {
        void RegisterDialog<TModel, TDialog>()
            where TModel : ModalDialogBase
            where TDialog : Window;

        bool? ShowDialog<TModel>(TModel modalDialogModel)
            where TModel : ModalDialogBase;

        void SetDialogsOwner(Window dialogsOwner);
    }

    public sealed class DialogService : IDialogService
    {
        #region Props and Fields

        private Dictionary<Type, Type> _registeredModelDialogCache;
        private Window _dialogsOwner;

        #endregion

        public void RegisterDialog<TModel, TDialog>()
            where TModel : ModalDialogBase
            where TDialog : Window
        {
            InteropCommandService.LogStack();

            var modelType = typeof(TModel);
            if (!_registeredModelDialogCache.ContainsKey(modelType))
            {
                _registeredModelDialogCache[modelType] = typeof(TDialog);
                return;
            }

            throw new InvalidOperationException(Strings.exceptionDialogAlreadyRegistered);
        }

        public bool? ShowDialog<TModel>(TModel modalDialogModel)
            where TModel : ModalDialogBase
        {
            InteropCommandService.LogStack();

            if (!_registeredModelDialogCache.ContainsKey(typeof(TModel)))
            {
                throw new InvalidOperationException(Strings.exceptionDialogForModelNotRegistered);
            }

            var dialogType = _registeredModelDialogCache[typeof(TModel)];
            var dialog = CreateDialog(dialogType);
            dialog.DataContext = modalDialogModel;
            dialog.Owner = _dialogsOwner;

            return dialog.ShowDialog();
        }

        public void SetDialogsOwner(Window dialogsOwner)
        {
            InteropCommandService.LogStack();

            _dialogsOwner = dialogsOwner;
        }

        private Window CreateDialog(Type dialogType)
        {
            InteropCommandService.LogStack();

            if (dialogType == null)
                throw new ArgumentNullException(dialogType.Name);

            var instance = Activator.CreateInstance(dialogType);
            var dialog = instance as Window;
            if (dialog != null)
            {
                return dialog;
            }

            throw new ArgumentException(Strings.exceptionDialogNotSupported + dialogType.Name);
        }

        public DialogService()
        {
            _registeredModelDialogCache = new Dictionary<Type, Type>();
        }
    }
}
