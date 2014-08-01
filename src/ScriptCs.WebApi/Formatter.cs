using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptCs.WebApi
{
    internal class Formatter : MediaTypeFormatter
    {
        private Func<Type, bool> _canReadType;
        private Func<Type, bool> _canWriteType;
        private Func<ReadFromStreamArgs, Task<object>> _readFromStream;
        private Func<WriteToStreamArgs, Task> _writeToStream;

        public Formatter(
            Func<Type, bool> canReadType,
            Func<Type, bool> canWriteType,
            Func<ReadFromStreamArgs, Task<object>> readFromStream,
            Func<WriteToStreamArgs, Task> writeToStream
            )
        {
            _canReadType = canReadType;
            _canWriteType = canWriteType;
            _readFromStream = readFromStream;
            _writeToStream = writeToStream;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return _readFromStream(new ReadFromStreamArgs(type, readStream, content, formatterLogger));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext, CancellationToken cancellationToken)
        {
            return _writeToStream(new WriteToStreamArgs(type, value, writeStream, content, transportContext, cancellationToken));
        }

        public override bool CanReadType(Type type)
        {
            return _canReadType(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _canWriteType(type);
        }
    }
}