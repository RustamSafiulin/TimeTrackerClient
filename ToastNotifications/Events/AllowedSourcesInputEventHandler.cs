using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ToastNotifications.Events
{
    public class AllowedSourcesInputEventHandler : IKeyboardEventHandler
    {
        private readonly IEnumerable<Type> _allowedSources;

        public AllowedSourcesInputEventHandler(IEnumerable<Type> allowedSources)
        {
            _allowedSources = allowedSources;
        }

        public void Handle(KeyEventArgs eventArgs)
        {
            var source = eventArgs.Source.GetType();
            var originalSource = eventArgs.Source.GetType();

            var doNotBlock = _allowedSources.Any(x => x == source || x == originalSource);

            eventArgs.Handled = doNotBlock == false;
        }
    }
}