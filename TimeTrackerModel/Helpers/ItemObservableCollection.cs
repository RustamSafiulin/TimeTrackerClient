
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace TimeTracker.Helpers
{
    public class ItemObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
            where T : EditableObject<T>
    {
        private List<String> _trackableProps;
        private Action _itemChangedCallback;

        public event PropertyChangedEventHandler PropertyChanged;

        public ItemObservableCollection(IEnumerable<String> props, Action itemChangedCallback)
        {
            _trackableProps = props.ToList();
            _itemChangedCallback = itemChangedCallback;

            base.CollectionChanged += CollectionChanged_Handler;
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

        public void AcceptChanges()
        {
            foreach (var item in Items)
            {
                if (item.IsDirty)
                {
                    item.AcceptChanges();
                }
            }

            IsDirty = false;
        }

        public void RejectChanges()
        {
            foreach (var item in Items)
            {
                if (item.IsDirty)
                {
                    item.RejectChanges();
                }
            }

            IsDirty = false;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        private void CollectionChanged_Handler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (T x in e.OldItems)
                    x.PropertyChanged -= ItemChanged;
            }

            if (e.NewItems != null)
            {
                foreach (T x in e.NewItems)
                    x.PropertyChanged += ItemChanged;
            }
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_trackableProps.FirstOrDefault(prop => prop == e.PropertyName) != null)
            {
                var item = sender as EditableObject<T>;
                if (item != null)
                {
                    if (!item.IsTrackable)
                        return;
                }

                if (_itemChangedCallback != null)
                    _itemChangedCallback();
                IsDirty = true;
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}