using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobOffer.Domain.Base
{
    public abstract class BaseValueObject 
    {
        protected StringBuilder _errors = new StringBuilder();

        public static bool operator ==(BaseValueObject source, BaseValueObject reference)
        {
            var results = new List<bool>();

            foreach (var property in source.GetType().GetProperties())
            {
                if (property.GetValue(source) == null && reference.GetType().GetProperty(property.Name).GetValue(reference) == null)
                {
                    results.Add(true);
                }
                else
                {
                    results.Add(property.GetValue(source).Equals(reference.GetType().GetProperty(property.Name).GetValue(reference)));
                }
            }

            return results.All(p=> p == true);
        }

        public static bool operator !=(BaseValueObject source, BaseValueObject reference)
        {
            return !(source == reference);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return this == (BaseValueObject)obj;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            foreach (var property in this.GetType().GetProperties())
            {
                hashCode += property.GetValue(this).GetHashCode();
            }

            return hashCode;
        }

        public abstract void Validate();

        protected void ThrowExceptionIfErrors()
        {
            if (_errors != null && !string.IsNullOrEmpty(_errors.ToString()))
            {
                throw new InvalidOperationException($"Error: {_errors}");
            }
        }
    }
}
