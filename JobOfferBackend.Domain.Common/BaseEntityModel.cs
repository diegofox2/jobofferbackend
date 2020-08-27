using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobOfferBackend.Domain.Common
{
    public abstract class BaseEntity<T> : IIdentity<T> where T : class
    {
        public string Id { get; set; }

        public bool HasIdCreated { get => !string.IsNullOrEmpty(Id); }

        protected StringBuilder _errorLines = new StringBuilder();

        public static bool operator ==(BaseEntity<T> source, BaseEntity<T> reference)
        {
            if (source is null)
            {
                return reference is null;
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

        public bool HasSamePropertyValuesThan(T other)
        {
            CompareLogic compareLogic = new CompareLogic();
            return compareLogic.Compare(this, other).AreEqual;
        }

        public abstract void Validate();

        protected void ThrowExceptionIfErrors()
        {
            if (_errorLines != null && !string.IsNullOrEmpty(_errorLines.ToString()))
            {
                var exception = new InvalidOperationException("Error. See property 'Data' to get errors details");

                var listErrors = _errorLines.ToString().TrimEnd().Split("\r\n");

                for(int a = 0; a<listErrors.Length; a++ )
                {
                    string value = listErrors[a];
                    exception.Data.Add(a, value);
                }

                throw exception;
            }
        }


    }
}
