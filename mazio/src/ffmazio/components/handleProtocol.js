const MAZIOPROT_HANDLER_CONTRACTID = "@mozilla.org/network/protocol;1?name=mazio";
const MAZIOPROT_HANDLER_CID = Components.ID("{2bfd84a9-948f-4319-99d4-23a402478c69}");

const NS_IOSERVICE_CID = "{9ac9e770-18bc-11d3-9337-00104ba0fd40}";
const NS_PREFSERVICE_CONTRACTID = "@mozilla.org/preferences-service;1";
const URI_CONTRACTID = "@mozilla.org/network/simple-uri;1";
const NS_WINDOWWATCHER_CONTRACTID = "@mozilla.org/embedcomp/window-watcher;1";
const STREAMIOCHANNEL_CONTRACTID = "@mozilla.org/network/stream-io-channel;1";
const STRING_STREAM_CTRID = "@mozilla.org/io/string-input-stream;1";

const nsIProtocolHandler = Components.interfaces.nsIProtocolHandler;
const nsIURI = Components.interfaces.nsIURI;
const nsISupports = Components.interfaces.nsISupports;
const nsIIOService = Components.interfaces.nsIIOService;
const nsIPrefService = Components.interfaces.nsIPrefService;
const nsIWindowWatcher = Components.interfaces.nsIWindowWatcher;
const nsIChannel = Components.interfaces.nsIChannel;

function LOG(msg)
{
//    var consoleService = Components.classes["@mozilla.org/consoleservice;1"].
//            getService(Components.interfaces.nsIConsoleService);
//    consoleService.logStringMessage(msg);
}

function getSystemRoot()
{
    var envGetter = Components.classes["@mozilla.org/process/environment;1"].
            createInstance(Components.interfaces.nsIEnvironment);
    var systemRoot = envGetter.get("SystemRoot");

    LOG("system root=" + systemRoot);
    return systemRoot;
}


function scanForDotNetFrameworks()
{
    var systemRoot = getSystemRoot();

    var file = Components.classes["@mozilla.org/file/local;1"].
            createInstance(Components.interfaces.nsILocalFile);
    file.initWithPath(systemRoot + "\\Microsoft.NET\\Framework");

    var entries = file.directoryEntries;
    var versionsList = "";
    while (entries.hasMoreElements())
    {
        var entry = entries.getNext();
        entry.QueryInterface(Components.interfaces.nsIFile);

        //A version of dot net is present if there is a directory that looks like:
        //    vN.N.NXXXX
        if (entry.isDirectory())
        {
            //Use the following to detect some versions of the CLR whose folder looks
            //like "v3.0", without the extended version number.
            if (/^(v\d+\.\d+\.\d+|v\d+\.\d+)$/m.test(entry.leafName))
            {
                if (versionsList != "")
                    versionsList += " ";

                versionsList += entry.leafName.substring(1);
            }
        }
    }

    if (versionsList == "")
        versionsList = "NONE";

	return versionsList;
}

function MazioProtocolHandler(scheme)
{
    this.scheme = scheme;
}

// attribute defaults
MazioProtocolHandler.prototype.defaultPort = -1;
MazioProtocolHandler.prototype.protocolFlags = nsIProtocolHandler.URI_NORELATIVE;

MazioProtocolHandler.prototype.allowPort = function(aPort, aScheme)
{
    return false;
}

MazioProtocolHandler.prototype.newURI = function(aSpec, aCharset, aBaseURI)
{
    var uri = Components.classes[URI_CONTRACTID].createInstance(nsIURI);
    uri.spec = aSpec;
    return uri;
}

MazioProtocolHandler.prototype.newChannel = function(aURI)
{
    var handle = aURI.spec.substr("mazio:".length);

    var versionsData = scanForDotNetFrameworks();
    var mazioWasRun = false;
    if (versionsData != "NONE")
    {
        if (versionsData != "")
        {
            var versions = versionsData.split(' ');

            for(var i = 0; i < versions.length; i++)
            {
                LOG(".NET version " + versions[i]);
                if (/^[2-9]/.test(versions[i]))
                {
                    runMazio(handle);
                    mazioWasRun = true;
                    LOG("mazio was run");
                    break;
                }
            }
        }
    }
    if (!mazioWasRun)
    {
        LOG("mazio was not run");
        var prompts = Components.classes["@mozilla.org/embedcomp/prompt-service;1"]
                .getService(Components.interfaces.nsIPromptService);
        prompts.alert(null, "Info", "Mazio requires .NET runtime version 2.0 or higher.\n"
                + "Please install it from http://windowsupdate.microsoft.com/");
    }

    return new PhonyChannel();
}

function runMazio(handle)
{
    var componentFile = __LOCATION__;
    var fileName = componentFile.parent.path + "\\mazio.exe";
    LOG("fileName=" + fileName);

    // create an nsILocalFile for the executable
    var file = Components.classes["@mozilla.org/file/local;1"].createInstance(Components.interfaces.nsILocalFile);
    file.initWithPath(fileName);

	// create an nsIProcess
    var process = Components.classes["@mozilla.org/process/util;1"].createInstance(Components.interfaces.nsIProcess);
    process.init(file);

	// Run the process.
    // If first param is true, calling thread will be blocked until
    // called process terminates.
    // Second and third params are used to pass command-line arguments
    // to the process.
    var args = [handle];
    LOG("running " + fileName + " " + args);
    process.run(false, args, args.length);
}

function PhonyChannel()
{
}

PhonyChannel.prototype.open = function phony_open()
{
    throw Components.results.NS_ERROR_NOT_IMPLEMENTED;
}

PhonyChannel.prototype.asyncOpen = function phony_aopen(streamListener, context)
{
    throw Components.results.NS_ERROR_NOT_IMPLEMENTED;
}

function MazioProtocolHandlerFactory(scheme)
{
    this.scheme = scheme;
}

MazioProtocolHandlerFactory.prototype.createInstance = function(outer, iid)
{
    if (outer != null) throw Components.results.NS_ERROR_NO_AGGREGATION;

    if (!iid.equals(nsIProtocolHandler) && !iid.equals(nsISupports))
        throw Components.results.NS_ERROR_INVALID_ARG;

    return new MazioProtocolHandler(this.scheme);
}

var factory_mazio = new MazioProtocolHandlerFactory("mazio");

var MazioModule = new Object();

MazioModule.registerSelf = function(compMgr, fileSpec, location, type)
{
    compMgr = compMgr.QueryInterface(Components.interfaces.nsIComponentRegistrar);

    compMgr.registerFactoryLocation(MAZIOPROT_HANDLER_CID,
            "Mazio pseudo-protocol handler",
            MAZIOPROT_HANDLER_CONTRACTID,
            fileSpec, location, type);
}

MazioModule.unregisterSelf = function(compMgr, fileSpec, location)
{
    compMgr = compMgr.QueryInterface(Components.interfaces.nsIComponentRegistrar);

    compMgr.unregisterFactoryLocation(MAZIOPROT_HANDLER_CID, fileSpec);
}

MazioModule.getClassObject = function(compMgr, cid, iid)
{
    if (!iid.equals(Components.interfaces.nsIFactory))
        throw Components.results.NS_ERROR_NOT_IMPLEMENTED;

    if (cid.equals(MAZIOPROT_HANDLER_CID)) return factory_mazio;

    throw Components.results.NS_ERROR_NO_INTERFACE;
}

MazioModule.canUnload = function(compMgr)
{
    return true;
}

function NSGetModule(compMgr, fileSpec)
{
    return MazioModule;
}

