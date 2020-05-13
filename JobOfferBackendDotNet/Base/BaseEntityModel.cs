using JobOffer.Domain.Entities;
using System;
using System.Text;

namespace JobOffer.Domain.Base
{
    public abstract class BaseEntity<T> : IIdentity<T>
    {
        public string Id { get; set; }

        protected StringBuilder _errors = new StringBuilder();

        public static bool operator ==(BaseEntity<T> source, BaseEntity<T> reference)
        {
            if (ReferenceEquals(source, null))
            {
                return ReferenceEquals(reference, null);
            }

            return source.Equals(reference);
        }

        public static bool operator !=(BaseEntity<T> source, BaseEntity<T> reference)
        {
            return !(source == reference);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Id == ((BaseEntity<T>)obj).Id;
        }

        public bool HasIdCreated { get => !string.IsNullOrEmpty(Id); }

        

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public abstract void Validate();

        protected void ThrowExceptionIfErrors()
        {
            if (_errors.Length != 0)
                throw new InvalidOperationException($"Error: {_errors}");
        }

    }
}
