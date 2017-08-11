using System;
using System.Collections.Generic;
using System.Text;

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
            set { this.SetValue(ref valueType, value); }
        }
        public JanusAttribute(Type valueType)
        {
            this.ValueType = valueType;
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
}
