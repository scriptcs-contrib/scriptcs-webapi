using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptCs.WebApi
{
    public class FormatterBuilder
    {
        private Func<Type, bool> _canReadType;
        private Func<Type, bool> _canWriteType;
        private Func<Type, Stream, HttpContent, IFormatterLogger, Task<object>> _readFromStream;
        private Func<Type, object, Stream, HttpContent, TransportContext, CancellationToken, Task> _writeToStream;
        private readonly IList<MediaTypeMapping> _mappings;
        private readonly IList<MediaTypeHeaderValue> _supportedMediaTypes;
        private readonly IList<Encoding> _supportedEncodings;

        public FormatterBuilder()
        {
            _mappings = new List<MediaTypeMapping>();
            _supportedMediaTypes = new List<MediaTypeHeaderValue>();
        }

        public FormatterBuilder CanReadType(Func<Type, bool> condition)
        {
            _canReadType = condition;
            return this;
        }

        public FormatterBuilder CanWriteType(Func<Type, bool> condition)
        {
            _canWriteType = condition;
            return this;
        }

        public FormatterBuilder ReadFromStream(
            Func<Type, Stream, HttpContent, IFormatterLogger, Task<object>> readFromStream)
        {
            _readFromStream = readFromStream;
            return this;
        }

        public FormatterBuilder WriteToStream(
            Func<Type, object, Stream, HttpContent,TransportContext, CancellationToken, Task> writeToStream)
        {
            _writeToStream = writeToStream;
            return this;
        }

        public FormatterBuilder SupportMediaType(MediaTypeHeaderValue mediaType)
        {
            _supportedMediaTypes.Add(mediaType);
            return this;
        }

        public FormatterBuilder SupportMediaType(string mediaType)
        {
           _supportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
            return this;
        }

        public FormatterBuilder SupportEncoding(Encoding encoding)
        {
            _supportedEncodings.Add(encoding);
            return this;
        }

        public FormatterBuilder MapQueryString(
            string parameterName, 
            string parameterValue,
            MediaTypeHeaderValue mediaType)
        {
            _mappings.Add(new QueryStringMapping(parameterName, parameterValue, mediaType));
            return this;
        }

        public FormatterBuilder MapQueryString(
            string parameterName,
            string parameterValue,
            string mediaType)
        {
            _mappings.Add(new QueryStringMapping(parameterName, parameterValue, mediaType));
            return this;
        }

        public FormatterBuilder MapRequestHeader(
            string headerName, 
            string headerValue, 
            System.StringComparison valueComparison, 
            bool isValueSubstring, 
            string mediaType)
        {
            _mappings.Add(new RequestHeaderMapping(headerName, headerValue, valueComparison, isValueSubstring, mediaType));
            return this;
        }

        public FormatterBuilder MapRequestHeader(
            string headerName,
            string headerValue,
            System.StringComparison valueComparison,
            bool isValueSubstring,
            MediaTypeHeaderValue mediaType
        )
        {
            _mappings.Add(new RequestHeaderMapping(headerName, headerValue, valueComparison, isValueSubstring, mediaType));
            return this;
        }

        public FormatterBuilder MapUriExtension(
            string extension,
            string mediaType
        )
        {
            _mappings.Add(new UriPathExtensionMapping(extension, mediaType));
            return this;
        }

        public FormatterBuilder MapUriExtension(
            string extension,
            MediaTypeHeaderValue mediaType
            )
        {
            _mappings.Add(new UriPathExtensionMapping(extension, mediaType));
            return this;
        }

        public MediaTypeFormatter Build()
        {
            var formatter = new Formatter(_canReadType, _canWriteType, _readFromStream, _writeToStream);

            foreach (var mediaType in _supportedMediaTypes)
            {
                formatter.SupportedMediaTypes.Add(mediaType);
            }

            foreach (var mapping in _mappings)
            {
                formatter.MediaTypeMappings.Add(mapping);
            }

            foreach (var encoding in _supportedEncodings)
            {
                formatter.SupportedEncodings.Add(encoding);
            }

            return formatter;
        }
    }
}