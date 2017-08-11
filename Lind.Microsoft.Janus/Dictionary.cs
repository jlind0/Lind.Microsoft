using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Lind.Microsoft.Core;
using System.Linq;

namespace Lind.Microsoft.Janus
{
    public class JanusDictionary<K, T> : ObservableDictionary<K, T>, IJanusNotificationChange
        where T: IJanusNamesapce
    {
        public JanusDictionary()
        {
            this.CollectionChanged += JanusDictionary_CollectionChanged;
        }

        private void JanusDictionary_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IJanusNamesapce s = (IJanusNamesapce)sender;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<T>())
                    {
                        this.NotificationChange?.Invoke(s.Id, item.Id, ChangeType.Add, null, item);
                        item.NotificationChange += NewItem_NotificationChange;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems.OfType<T>())
                    {
                        this.NotificationChange?.Invoke(s.Id, item.Id, ChangeType.Remove, item, null);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.OldItems)
                    {
                        foreach (var newItem in e.NewItems.OfType<T>())
                        {
                            this.NotificationChange?.Invoke(s.Id, newItem.Id, ChangeType.Replace, item, newItem);
                            newItem.NotificationChange += NewItem_NotificationChange;
                        }
                    }
                    break;
            }
        }

        private void NewItem_NotificationChange(Uri instanceId, Uri attributeId, ChangeType changeType, object oldValue, object newValue)
        {
            this.NotificationChange?.Invoke(instanceId, attributeId, changeType, oldValue, newValue);
        }
        

        public event JanusNotificationChange NotificationChange;
    }
}
