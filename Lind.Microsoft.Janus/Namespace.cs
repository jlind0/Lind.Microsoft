using System;
using System.ComponentModel;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System.Reflection;
using System.Collections.Concurrent;

namespace Lind.Microsoft.Janus
{
    public interface IJanusNamesapce : IJanusNotificationChange, IDisposable
    {
        Uri Id { get; }
        string Name { get; }
        bool IsReady { get; }
        bool IsDirty { get; }
        bool IsDead { get; }
    }
    public abstract class JanusNamespace: DynamicObject, IJanusNamesapce
    {
        protected CancellationTokenSource CancelSource { get; private set; } = new CancellationTokenSource();
        private Uri id;
        [JanusNamespace(@"http://janus.wwidew.net/id")]
        public Uri Id
        {
            get { return id; }
            protected set
            {
                this.SetValue(ref id, value);
            }
        }
        private string name;
        [JanusNamespace(@"http://janus.wwidew.net/name")]
        public string Name
        {
            get { return name; }
            protected set { this.SetValue(ref name, value); }
        }

        private bool isReady = false;
        [JanusNamespace(@"http://janus.wwidew.net/IsReady")]
        public bool IsReady
        {
            get { return isReady; }
            protected set { this.SetValue(ref isReady, value); }
        }
        private bool isDirty = false;
        [JanusNamespace(@"http://janus.wwidew.net/IsDirty")]
        public bool IsDirty
        {
            get { return isDirty; }
            protected set { this.SetValue(ref isDirty, value); }
        }
        private bool isDead = false;
        [JanusNamespace(@"http://janus.wwidew.net/IsDead")]
        public bool IsDead
        {
            get { return isDead; }
            protected set { this.SetValue(ref isDead, value); }
        }
        public event JanusNotificationChange NotificationChange;

        protected bool SetValue<T>(ref T storage, T newValue, [CallerMemberName]string property = null)
        {
            if (newValue?.Equals(storage) ?? storage == null)
                return false;
            NotificationChange?.Invoke(this.Id, GetAttributeNamespace(property), ChangeType.Update, storage, newValue);
            storage = newValue;
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.CancelSource.Cancel();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void ExecuteLogic(Action logic)
        {
            try
            {
                logic();
            }
            catch (OperationCanceledException) { this.IsDead = true; }
            catch (Exception) { this.IsDead = true; this.Dispose(true); }
        }
        private ConcurrentDictionary<string, string> NSLookup { get; set; } = new ConcurrentDictionary<string, string>();
        protected virtual Uri GetAttributeNamespace(string propertyName)
        {
            string ns;
            if (!NSLookup.TryGetValue(propertyName, out ns))
                ns = this.GetType().GetProperty(propertyName)?.GetCustomAttribute<JanusNamespaceAttribute>()?.Id;
            if(!string.IsNullOrEmpty(ns))
                NSLookup[propertyName] = ns;
            return new Uri(ns);
        }
    }
}
