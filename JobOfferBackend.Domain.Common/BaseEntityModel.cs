﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JobOfferBackend.Domain.Common
{
    public abstract class BaseEntity<T> : IIdentity<T>
    {
        public string Id { get; set; }

        public bool HasIdCreated { get => !string.IsNullOrEmpty(Id); }

        protected StringBuilder _errors = new StringBuilder();

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

        public override int GetHashCode() => Id.GetHashCode();

        public bool ComparePropertiesTo(T other)
        {
            var results = new List<bool>();

            foreach (var property in this.GetType().GetProperties())
            {
                if (property.GetValue(this) == null && other.GetType().GetProperty(property.Name).GetValue(other) == null)
                {
                    results.Add(true);
                }
                else
                {
                    results.Add(property.GetValue(this).Equals(other.GetType().GetProperty(property.Name).GetValue(other)));
                }
            }

            return results.TrueForAll(value => value == true);
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
