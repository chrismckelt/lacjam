using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Framework.Exceptions
{
    public class ValidationException : Exception, IEnumerable<KeyValuePair<string,object>>
    {
        public List<KeyValuePair<string, object>> Errors { get; set; }

        public ValidationException(string message)
            : this(new KeyValuePair<string, object>(string.Empty,message))
        {
        }

        public ValidationException(params KeyValuePair<string, object>[] errors )
        {
            Errors = errors.ToList();
        }

        public ValidationException(string key, string error)
            : this(new KeyValuePair<string, object>(key, error))
        {
        }

        public void Add(string key, object payload)
        {
            Errors.Add(new KeyValuePair<string, object>(key,payload));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string Message
        {
            get
            {
                return Errors.Aggregate(String.Empty, (s, pair) =>
                {

                    if (pair.Value == null) 
                        return s;

                    if (s.Contains(pair.Value.ToString()))
                    {
                        return s;
                    }

                   return  s + " " + pair.Value;
                });
            }
        }

		public void Enforce()
		{
			if (Errors.Any())
			{
				throw this;
			}
		}
    }
}