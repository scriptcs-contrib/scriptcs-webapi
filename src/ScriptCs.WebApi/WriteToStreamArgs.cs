using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace ScriptCs.WebApi
{
    public class WriteToStreamArgs
    {
        public WriteToStreamArgs(Type type, object instance, Stream stream, HttpContent content, TransportContext context, CancellationToken token)
        {
            Type = type;
            Instance = instance;
            Stream = stream;
            Content = content;
            Context = context;
            Token = token;

        }

        public Type Type { get; private set; }
        public object Instance { get; private set; }
        public Stream Stream { get; private set; }
        public HttpContent Content { get; private set; }
        public TransportContext Context { get; private set; }
        public CancellationToken Token { get; private set; }
    }
}