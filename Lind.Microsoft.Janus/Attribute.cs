using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq.Expressions;

namespace Lind.Microsoft.Janus
{
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class JanusNamespaceAttribute : Attribute
    {
        public string Id { get; set; }
        public JanusNamespaceAttribute(string id)
        {
            this.Id = id;
        }
        
    }
    public interface IJanusAttribute : IJanusNamesapce
    {
        Type ValueType { get; }
    }
    public class JanusAttribute : JanusNamespace, IJanusAttribute
    {
        private Type valueType;
        [JanusNamespace("http://janus.wwidew.net/type")]
        public Type ValueType
        {
            get { return valueType; }
            private set { this.SetValue(ref valueType, value); }
        }
        public JanusAttribute(Uri id, string name, Type valueType)
        {
            this.ValueType = valueType;
            this.Id = id;
            this.Name = name;
        }
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            if(binder.Operation == ExpressionType.DivideAssign)
            {
                Type a = arg as Type;
                if(a != null)
                {
                    this.ValueType = a;
                    result = this;
                    return true;
                }
            }
            result = this;
            return false;
        }
    }
    public interface IJanusAttributeValue : IJanusNamesapce
    {
        IJanusAttribute Attribute { get; }
        object ObjValue { get; set; }
    }
    public interface IOJanusAttributeValue<out T> : IJanusAttributeValue
    {
        T Value { get; }
    }
    public interface IIJanusAttributeValue<in T> : IJanusAttributeValue
    {
        T Value { set; }
    }
    public interface IJanusAttributeValue<T> : IOJanusAttributeValue<T>, IIJanusAttributeValue<T> { }

    public class JanusAttributeValue<T> : JanusNamespace, IIJanusAttributeValue<T>
    {
        public JanusAttributeValue(IJanusAttribute attr, T value = default(T))
        {
            attr.NotificationChange += Attr_NotificationChange;
        }

        private void Attr_NotificationChange(Uri instanceId, Uri attributeId, ChangeType changeType, object oldValue, object newValue)
        {
            if (attributeId == new Uri("http://janus.wwidew.net/type"))
                this.IsDead = true;
        }
        private T value;
        public T Value
        {
            get { if (!IsDead) return value; else throw new InvalidOperationException(); }
            set { this.SetValue(ref this.value, value); }
        }

        public IJanusAttribute Attribute { get; private set; }

        public object ObjValue { get { return this.Value; } set { this.Value = (T)value; } }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
            if (binder.Name.StartsWith("_"))
            {
                return ((DynamicObject)this.Attribute).TryGetMember(binder, out result);
            }
            return base.TryGetMember(binder, out result);
        }
    }
}
