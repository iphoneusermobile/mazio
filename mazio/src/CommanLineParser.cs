using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using mazio.uploaders;

namespace mazio
{
    public class CommandLineParser
    {
        private readonly Dictionary<string, UploadType> uploaders = new Dictionary<string, UploadType>();
        private readonly Dictionary<string, ServerTypeCommandLineParser> parsers = new Dictionary<string, ServerTypeCommandLineParser>();

        private CommandLineParser()
        {
            uploaders["jira"] = new JiraUploadBuilder();
            uploaders["skitch"] = new SkitchUploadBuilder();
            uploaders["flickr"] = new FlickrUploadBuilder();
            uploaders["picasaweb"] = new PicasawebUploadBuilder();
            uploaders["meetingroom"] = new MeetingRoomUploadBuilder();

            parsers["jira"] = new JiraCommandLineParser();
            parsers["meetingroom"] = new MeetingRoomCommandLineParser();
        }

        private static CommandLineParser instance;

        public static CommandLineParser Instance
        {
            get
            {
                if (instance == null)
                    instance = new CommandLineParser();
                return instance;
            }
        }

        public UploadType getUploader(string args)
        {
            if (args == null)
                return null;

            if (!args.StartsWith("mazio:"))
                return null;

            string pars = args.Substring(args.IndexOf(':') + 1);
            int idx = pars.IndexOf(':');
            if (idx < 2)
                return null;
            string upl = pars.Substring(0, idx).ToLower();
            return !uploaders.ContainsKey(upl) ? null : uploaders[upl];
        }

        public string getStartupFileName(string args) {
            if (args == null) return null;

            const string mazioOpenfile = "mazio:openfile:";
           
            if (!args.StartsWith(mazioOpenfile) || args.Length < mazioOpenfile.Length + 1) return null;

            return HttpUtility.UrlDecode(args.Substring(mazioOpenfile.Length));
        }

        public NameValueCollection getArguments(string args)
        {
            if (args == null)
                return null;

            string pars = args.Substring(args.IndexOf(':') + 1);
            int idx1 = pars.IndexOf(':');
            if (idx1 < 2)
                return null;
            string serverType = pars.Substring(0, idx1).ToLower();
            return !parsers.ContainsKey(serverType) ? null : parsers[serverType].getParams(pars);
        }
    }
}
