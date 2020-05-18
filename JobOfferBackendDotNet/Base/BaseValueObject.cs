using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobOfferBackend.Domain.Base
{
    public abstract class BaseValueObject 
    {
        protected StringBuilder _errors = new StringBuilder();

        public static bool operator ==(BaseValueObject source, BaseValueObject reference)
        {
            if (!(source is null) && !(reference is null))
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

                return results.All(p => p == true);
            }

            if(source is null && reference is null)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(BaseValueObject source, BaseValueObject reference)
        {
            if(!(source is null) && !(reference is null))
            {
                return !(source == reference);
            }

            if (source is null && reference is null)
            {
                return false;
            }

            return true;
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
                if (property.GetValue(this) != null)
                {
                    hashCode += property.GetValue(this).GetHashCode();
                }
            }

            return hashCode;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
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
