
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TimeTracker.Helpers
{
    public class EditableObject<T> : INotifyPropertyChanged, ITrackable
    {
        #region Fields and Properties

        private Memento<T> Memento { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Boolean IsTrackable { get; set; }

        public Boolean IsValid
        {
            get { return _validationErrors.Count == 0; }
        }

        private Boolean _isDirty = false;
        public Boolean IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public void BeginChanges()
        {
            if (IsTrackable)
            {
                Memento.SaveOrUpdate(this);
                IsDirty = true;
            }
        }

        public virtual void RejectChanges()
        {
            if (IsTrackable)
            {
                Memento.Restore(this);
                Memento.Dispose();
                IsDirty = false;

                _validationErrors.Clear();
            }
        }

        public virtual void AcceptChanges()
        {
            if (IsTrackable)
            {
                Memento.Dispose();
                IsDirty = false;

                _validationErrors.Clear();
            }
        }

        protected bool SetProperty<TVal>(ref TVal storage, TVal value, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<TVal>.Default.Equals(storage, value))
                return false;

            if (IsTrackable && !_isDirty)
            {
                BeginChanges();
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            this.OnPropertyChanged("IsValid");

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public EditableObject()
        {
            this.Memento = new Memento<T>();
            IsTrackable = true;
        }

        #region Validation helpers

        private Dictionary<string, string> _validationErrors = new Dictionary<string, string>();
        public void AddError(string propertyName, string error)
        {
            _validationErrors[propertyName] = error;
        }

        public void RemoveError(string propertyName)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);
        }

        public Boolean ErrorExists(string propertyName)
        {
            return _validationErrors.ContainsKey(propertyName);
        }

        private Boolean CanGetValueFromString<TProp>(String value)
        {
            var typeConverter = TypeDescriptor.GetConverter(typeof(TProp));
            if (typeConverter != null && typeConverter.CanConvertFrom(typeof(string)) && typeConverter.IsValid(value))
            {
                return true;
            }

            return false;
        }

        private TProp GetValueFromString<TProp>(String value)
        {
            return (TProp)Convert.ChangeType(value, typeof(TProp));
        }

        protected String ValidateIntegralProperty<TProp>(String propertyName, TProp min, TProp max, String current)
        {
            var errorText = String.Empty;
            /*
            if (CanGetValueFromString<TProp>(current))
            {
                if (Comparer<TProp>.Default.Compare(GetValueFromString<TProp>(current), min) < 0 ||
                    Comparer<TProp>.Default.Compare(GetValueFromString<TProp>(current), max) > 0)
                {
                    errorText = Strings.validationOutOfRange + min + " - " + max + ".";
                    AddError(propertyName, errorText);
                }
                else
                {
                    RemoveError(propertyName);
                }
            }
            else
            {
                errorText = Strings.validationOutOfRange + min + " - " + max + ".";
                AddError(propertyName, errorText);
            }

            OnPropertyChanged("IsValid");*/
            return errorText;
        }

        #endregion
    }
}