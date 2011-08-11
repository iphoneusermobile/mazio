using System.Collections.Specialized;

namespace mazio
{
    interface ServerTypeCommandLineParser
    {
        NameValueCollection getParams(string cmdline);
    }
}
