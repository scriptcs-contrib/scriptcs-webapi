using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace ScriptCs.WebApi
{
    public class ReadFromStreamArgs
    {
        public ReadFromStreamArgs(Type type, Stream stream, HttpContent content, IFormatterLogger logger)
        {
            Type = type;
            Stream = stream;
            Content = content;
            Logger = logger;
        }
        
        public Type Type { get; private set; }
        public Stream Stream { get; private set; }
        public Object Instance { get; private set; }
        public HttpContent Content { get; private set; }
        public IFormatterLogger Logger { get; private set; }
    }
}