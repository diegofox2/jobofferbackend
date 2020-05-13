using System;
using System.Collections.Generic;
using System.Text;

namespace JobOffer.Domain.Base
{
    public abstract class BaseAgregate
    {
        protected StringBuilder _errors = new StringBuilder();

        public static bool operator ==(BaseAgregate source, BaseAgregate reference)
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

            return results.TrueForAll(value => value == true);
        }

        public static bool operator !=(BaseAgregate source, BaseAgregate reference)
        {
            return !(source.Equals(reference));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return this == (BaseAgregate)obj;
        }

        public override int GetHashCode()
        {
            var hashCode = new StringBuilder();

            foreach (var property in this.GetType().GetProperties())
            {
                if (property.GetValue(this) != null)
                {
                    hashCode.Append(property.GetValue(this).GetHashCode().ToString());
                }
            }

            return hashCode.ToString().GetHashCode();
        }

        public abstract void Validate();

        protected void ThrowExceptionIfErrors()
        {
            if (_errors.Length != 0)
                throw new InvalidOperationException($"Error: {_errors}");
        }

    }
}
