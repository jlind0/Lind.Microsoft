using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace Lind.Microsoft.Janus
{
    public interface IJanusSchema : IJanusNamesapce
    {
        IJanusSchema ParentSchema { get; }
        IReadOnlyCollection<IJanusAttribute> Attributes { get; }
    }
    public class JanusSchema : JanusNamespace, IJanusSchema
    {
        public IJanusSchema ParentSchema { get; private set; }
        protected JanusDictionary<Uri, IJanusAttribute> InternalAttributes { get; private set; } = new JanusDictionary<Uri, IJanusAttribute>();
        protected IJanusRepository Repository { get; private set; }
        public JanusSchema(IJanusRepository repository, string name, IJanusSchema parent = null, params IJanusAttribute[] attribs)
        {
            this.Repository = repository;
            foreach(var attr in attribs)
            {
                this.InternalAttributes.Add(attr.Id, attr);
            }
            this.ParentSchema = parent;
            this.Name = name;
            Create();
        }
        public JanusSchema(IJanusRepository repository, Uri uri)
        {
            this.Repository = repository;
            this.Id = uri;
        }
        protected void Create()
        {
            this.ExecuteLogic(async () =>
            {
                this.IsReady = false;
                this.IsDirty = true;
                await Pre();
                await PreCreate();
                this.Id = await this.Repository.GetNewUri(this.Name, this.ParentSchema?.Id, this.CancelSource.Token);
                await this.Repository.AddAttributes(this.InternalAttributes.Values, this.Id, this.CancelSource.Token);
                await Post();
                await PostCreate();
                this.IsDirty = false;
                this.IsReady = true;
            });
        }
        protected virtual Task PreCreate() { return Task.CompletedTask; }
        protected virtual Task PostCreate() { return Task.CompletedTask; }
        protected void Load()
        {
            this.ExecuteLogic(async () =>
            {
                this.IsReady = false;
                this.IsDirty = true;
                await Pre();
                await PreLoad();
                var schema = await this.Repository.GetSchema(this.Id, this.CancelSource.Token);
                if (schema == null)
                    throw new NullReferenceException();
                this.Name = schema.Name;
                foreach (var attr in schema.Attributes)
                    this.InternalAttributes.Add(attr.Id, attr);
                this.ParentSchema = schema.ParentSchema;
                await Post();
                await PostLoad();
                this.IsDirty = false;
                this.IsReady = true;
            });
        }
        protected virtual Task PreLoad() { return Task.CompletedTask; }
        protected virtual Task PostLoad() { return Task.CompletedTask; }
        protected virtual Task Pre() { return Task.CompletedTask; }
        protected virtual Task Post()
        {
            return Task.CompletedTask;
        }
        protected virtual void OnAttributeChange(object sender, PropertyChangedEventArgs args)
        {
        }
        public IReadOnlyCollection<IJanusAttribute> Attributes => InternalAttributes.Values.ToList().AsReadOnly();
        internal async Task AddAttribute(IJanusAttribute attribute)
        {
            try
            {
                this.IsDirty = true;
                await this.Repository.AddAttribute(attribute, this.Id, CancelSource.Token);
                this.InternalAttributes.Add(attribute.Id, attribute);
                this.IsDirty = false;
            }
            catch (OperationCanceledException) { }
            catch (Exception)
            {
                throw;
            }
        }
        //public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        //{
        //    switch(binder.Operation)
        //    {
        //        case ExpressionType.DivideAssign
        //    }
        //}
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dynamic test = null;
            test.janus.wwidew.net_First /= typeof(string);
            test.First *= test.janus.wwidew.net_First;
            return base.TrySetMember(binder, value);
        }
    }
}
