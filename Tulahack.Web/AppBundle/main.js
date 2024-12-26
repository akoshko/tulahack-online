import {dotnet} from './dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const {setModuleImports, getConfig, runMain, getAssemblyExports} = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

const url = globalThis.window.location.origin.includes("localhost") ? 
    "http://localhost:8080" : globalThis.window.location.origin;
const response = await fetch(`${url}/oauth2/auth`);

const token = response.headers.get('X-Auth-Request-Access-Token');
if (!token)
    globalThis.window.location.reload();

setModuleImports("main.js", {
    window: {
        location: {
            origin: () => url
        }
    },
    auth: {
        token: () => token
    },
    openInNewTab: (newTabUrl) => window.open(newTabUrl, '_blank')
});

const config = getConfig();

await runMain(config.mainAssemblyName, [window.location.search]);