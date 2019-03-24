
using System;
using System.Windows;
using System.Collections.Generic;

using TimeTracker.Helpers;

namespace TimeTracker
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
            var modelType = typeof(TModel);
            if (!_registeredModelDialogCache.ContainsKey(modelType))
            {
                _registeredModelDialogCache[modelType] = typeof(TDialog);
                return;
            }

            throw new InvalidOperationException($"Dialog {nameof(TModel)} has already been registered");
        }

        public bool? ShowDialog<TModel>(TModel modalDialogModel)
            where TModel : ModalDialogBase
        {
            if (!_registeredModelDialogCache.ContainsKey(typeof(TModel)))
            {
                throw new InvalidOperationException($"Dialog {nameof(TModel)} doesn't registered");
            }

            var dialogType = _registeredModelDialogCache[typeof(TModel)];
            var dialog = CreateDialog(dialogType);
            dialog.DataContext = modalDialogModel;
            dialog.Owner = _dialogsOwner;

            return dialog.ShowDialog();
        }

        public void SetDialogsOwner(Window dialogsOwner)
        {
            _dialogsOwner = dialogsOwner;
        }

        private Window CreateDialog(Type dialogType)
        {
            if (dialogType == null)
                throw new ArgumentNullException(dialogType.Name);

            var instance = Activator.CreateInstance(dialogType);
            var dialog = instance as Window;
            if (dialog != null)
            {
                return dialog;
            }

            throw new ArgumentException($"Dialog {dialogType.Name} doesn't supported");
        }

        public DialogService()
        {
            _registeredModelDialogCache = new Dictionary<Type, Type>();
        }
    }
}
