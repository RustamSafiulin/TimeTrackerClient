using System.Windows;
using System.Windows.Media;

namespace ToastNotifications.Utilities
{
    public static class DependencyObjectTreeHelper
    {
        public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var tmpFoundChild = GetChild<T>(childName, child);
                if (tmpFoundChild != null)
                {
                    foundChild = tmpFoundChild;
                    break;
                }
            }

            return foundChild;
        }

        private static T GetChild<T>(string childName, DependencyObject child) where T : DependencyObject
        {
            // If the child is not of the request child type child
            if ((child is T childType) == false)
            {
                // recursively drill down the tree
                return FindChild<T>(child, childName);
            }

            if (string.IsNullOrEmpty(childName) == false)
            {
                // If the child's name is set for search
                if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                {
                    return (T)child;
                }

                return null;
            }

            // child element found.
            return (T)child;
        }
    }
}
