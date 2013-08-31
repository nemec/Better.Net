using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace BetterDotNet
{
    /// <summary>
    /// Extends the built in Uri class to make it easier to
    /// access parts of a URI.
    /// </summary>
    public class ExtendedUri : Uri
    {
        /// <summary>
        /// Create a new instance of the ExtendedUri class.
        /// </summary>
        /// <param name="uri"></param>
        public ExtendedUri(Uri uri)
            : this(uri.ToString())
        {
        }

        /// <summary>
        /// Create a new instance of the ExtendedUri class.
        /// </summary>
        /// <param name="uriString"></param>
        public ExtendedUri(string uriString)
            : base(uriString)
        {
            PathSegmentParameterSeparator = ';';
            PathSegmentKeyValueSeparator = '=';
            QueryParameters = new NameValueCollection();
            InitProperties(this);
        }

        /// <summary>
        /// The username portion of the uri.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The password portion of the uri.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// The character that separates path segment parameters from
        /// the path segument. E.g. the ; in /home;key=value/downloads.
        /// </summary>
        public static char PathSegmentParameterSeparator { get; set; }

        /// <summary>
        /// The character that separates path segment parameter keys
        /// from their values. E.g. the = in /home;key=value/downloads.
        /// </summary>
        public static char PathSegmentKeyValueSeparator { get; set; }

        /// <summary>
        /// Gets an array containing the path segments
        /// that make up the specified URI.
        /// </summary>
        public new PathSegment[] Segments { get; private set; }

        /// <summary>
        /// Individual query parameters stored as key-value pairs.
        /// </summary>
        public NameValueCollection QueryParameters { get; private set; }

        private void InitProperties(Uri uri)
        {
            var auth = uri.UserInfo.Split(':');
            Username = auth[0];
            if (auth.Length == 2)
            {
                Password = auth[1];
            }
            else if(auth.Length > 2)
            {
                throw new ArgumentException(String.Format(
                    "User info ({0}) cannot have more than two parts.",
                    uri.UserInfo));
            }

            Segments = new PathSegment[uri.Segments.Length];
            for(var i = 0; i < uri.Segments.Length; i++)
            {
                Segments[i] = new PathSegment(uri.Segments[i]);
            }

            if (Query.Length > 0)
            {
                foreach (var pairStr in uri.Query.Substring(1).Split('&'))
                {
                    var pair = pairStr.Split('=');
                    if (pair.Length == 1)
                    {
                        // ReSharper disable AssignNullToNotNullAttribute
                        QueryParameters.Add(null, pair[0]);
                        // ReSharper restore AssignNullToNotNullAttribute
                    }
                    else if (pair.Length == 2)
                    {
                        QueryParameters.Add(pair[0], pair[1]);
                    }
                    else
                    {
                        throw new UriFormatException(String.Format(
                            "Query parameter '{0}' must be in key=value format",
                            pairStr));
                    }
                }
            }
        }

        /// <summary>
        /// A single path segment belonging to a URI.
        /// </summary>
        public class PathSegment
        {
            /// <summary>
            /// Create a new path segment.
            /// </summary>
            /// <param name="segment"></param>
            public PathSegment(string segment)
            {
                _originalSegment = segment;

                segment = segment.TrimEnd('/');

                var parts = segment.Split(PathSegmentParameterSeparator);
                Path = parts[0];
                MatrixParameters = new KeyValuePair<string, string>[parts.Length - 1];
                for (var i = 0; i < parts.Length - 1; i++)
                {
                    var param = parts[i + 1].Split(PathSegmentKeyValueSeparator);
                    if (param.Length == 1)
                    {
                        MatrixParameters[i] = new KeyValuePair<string, string>(param[0], null);
                    }
                    else if (param.Length == 2)
                    {
                        MatrixParameters[i] = new KeyValuePair<string, string>(param[0], param[1]);
                    }
                    else
                    {
                        throw new UriFormatException(String.Format(
                            "Key-value pair in path segment is malformed: {0}",
                            segment));
                    }
                }
            }

            private readonly string _originalSegment;

            /// <summary>
            /// The path part of the segment.
            /// </summary>
            public string Path { get; private set; }

            /// <summary>
            /// Matrix parameters can follow any path segment in a URI, 
            /// so they can be specified on an inner path segment too. 
            /// Multiple matrix parameters can be used in a given URI. 
            /// Note that a matrix parameter is preceded by a semicolon.
            /// 
            /// E.g. /path;key=value/other/path
            /// </summary>
            public KeyValuePair<string, string>[] MatrixParameters { get; private set; }

            /// <summary>
            /// Convert the PathSegment into a string.
            /// </summary>
            /// <param name="segment"></param>
            /// <returns></returns>
            public static implicit operator string(PathSegment segment)
            {
                return segment._originalSegment;
            }

            public override string ToString()
            {
                return _originalSegment;
            }
        }
    }

    /// <summary>
    /// HTTP related extensions
    /// </summary>
    public static class HttpExtensions
    {
        private const char KeySeparator = '=';
        private const char PairSeparator = '&';

        /// <summary>
        /// Encode a dictionary of key/values into a query string.
        /// All keys and values are sent through
        /// <see cref="ExtendedUri.EscapeUriString"/>.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static string BuildQueryString(IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                return "";
            }

            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                builder.Append(PairSeparator);
                builder.Append(Uri.EscapeUriString(property.Key));
                builder.Append(KeySeparator);
                builder.Append(Uri.EscapeUriString(property.Value.ToString()));
            }
            builder.Remove(0, 1);

            return builder.ToString();
        }

        /// <summary>
        /// Encode a <see cref="NameValueCollection"/> into a URL query string.
        /// All keys and values are sent through
        /// <see cref="ExtendedUri.EscapeUriString"/>.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static string BuildQueryString(NameValueCollection properties)
        {
            if (properties == null)
            {
                return "";
            }

            var builder = new StringBuilder();
            foreach (var property in properties.AllKeys)
            {
                builder.Append(PairSeparator);
                builder.Append(Uri.EscapeUriString(property));
                builder.Append(KeySeparator);
                builder.Append(Uri.EscapeUriString(properties[property]));
            }
            builder.Remove(0, 1);
            return builder.ToString();
        }

        /// <summary>
        /// Encode an (anonymous) object into a URL query string.
        /// All keys and values are sent through
        /// <see cref="ExtendedUri.EscapeUriString"/>.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static string BuildQueryString(Object properties)
        {
            if (properties == null)
            {
                return "";
            }

            var builder = new StringBuilder();
            foreach (var props in properties.GetType().GetProperties())
            {
                builder.Append(PairSeparator);
                builder.Append(Uri.EscapeUriString(props.Name));
                builder.Append(KeySeparator);
                builder.Append(Uri.EscapeUriString(
                    props.GetValue(properties, null).ToString()));
            }
            builder.Remove(0, 1);
            return builder.ToString();
        }
    }
}
