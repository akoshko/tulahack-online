import {dotnet} from './dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const debugMode = globalThis.window.location.origin.includes("localhost")
const authUrl = debugMode ?
	`${globalThis.window.location.origin}/debug-token.json` : `${globalThis.window.location.origin}/oauth2/auth`;
const response = await fetch(authUrl);

const token = debugMode ?
	(await response.json()).token : response.headers.get('X-Auth-Request-Access-Token');

if (!token)
	globalThis.window.location.reload();

const boot_request = await fetch(`${globalThis.window.location.origin}/blazor.boot.json`);
const boot_json = await boot_request.json();

const totalModulesCount =
	Object.keys(boot_json.resources?.assembly ?? []).length + 	// Application and library assemblies
	Object.keys(boot_json.resources?.pdb ?? []).length +      	// Debug symbols
	Object.keys(boot_json.resources?.vfs ?? []).length +      	// Virtual file system files: https://github.com/platformdotnet/Platform.VirtualFileSystem
	Object.keys(boot_json.resources?.icu ?? []).length +    		// International Components for Unicode
	Object.keys(boot_json.resources?.satelliteResources ?? []).length;	// Localization

const progressbar = document.getElementById("progressbar");
// Here are advanced examples of dotnet API usage:
// https://github.com/dotnet/runtime/blob/a270140281a13ab82a4401dff3da6d27fe499087/src/mono/sample/wasm/browser-advanced/main.js#L41
// and Github issue related to it
// https://github.com/dotnet/runtime/issues/93941
// .withModuleConfig() was inspired by these samples
const {setModuleImports, runMain, getConfig} = await dotnet
	.withDiagnosticTracing(false)
	.withApplicationArgumentsFromQuery()
	// 'withModuleConfig' is internal lower level API
	// here we show how emscripten could be further configured
	// It is preferred to use specific 'with***' methods instead in all other cases.
	.withConfig({
		// Set startupMemoryCache: true to save .wasm files in browser storage
		// this way .wasm files will be downloaded once and then cached on client`s side
		startupMemoryCache: false,
		resources: {
			modulesAfterConfigLoaded: {
				"advanced.lib.module.js": ""
			}
		}
	})
	.withModuleConfig({
		configSrc: `${globalThis.window.location.origin}/blazor.boot.json`,
		onConfigLoaded: (config) => {
			// This is called during emscripten `dotnet.wasm` instantiation, after we fetched config.
			console.log('WASM Module.onConfigLoaded');
			// config is loaded and could be tweaked before the rest of the runtime startup sequence
			// config.environmentVariables["MONO_LOG_LEVEL"] = "debug";
			// config.browserProfilerOptions = {};
		},
		preInit: () => {
			console.log('WASM Module.preInit');
		},
		preRun: () => {
			console.log('WASM Module.preRun');
		},
		onRuntimeInitialized: () => {
			console.log('WASM Module.onRuntimeInitialized');
			// here we could use API passed into this callback
			// Module.FS.chdir("/");
		},
		onDotnetReady: () => {
			// This is called after all assets are loaded.
			console.log('WASM Module.onDotnetReady');
		},
		onDownloadResourceProgress: (resourcesLoaded, totalResources) => {
			const value = parseInt((resourcesLoaded * 100.0) / totalModulesCount);
			progressbar.innerHTML = value + '%';
		},
		postRun: () => {
			console.log('WASM Module.postRun');
		},
	})
	.create();

setModuleImports("main.js", {
	window: {
		location: {
			origin: () => {
				return globalThis.window.location.origin.includes("localhost") ?
					"http://localhost:8080" : globalThis.window.location.origin;
			}
		}
	},
	auth: {
		token: () => token
	},
	openInNewTab: (newTabUrl) => window.open(newTabUrl, '_blank'),
	getAsync: async (url) => {
		try {
			console.log("Trying get data from", url, "...")
			const basePath = globalThis.window.location.origin.includes("localhost") ?
				"http://localhost:8080" : globalThis.window.location.origin;
			const response = await fetch(
				`${basePath}/api/${url}`,
				{
					method: 'GET'
				});
			debugger;
			if (!response.ok) {
				console.error('GetAsync: response is not ok!:', error);
				throw new Error(`HTTP error! Status: ${response.status}`);
			}
			const json = await response.json();
			return json;
		} catch (error) {
			console.error('Fetch error:', error);
			throw error;
		}
	},
	postAsync: async (url, data) => {
		try {
			const basePath = globalThis.window.location.origin.includes("localhost") ?
				"http://localhost:8080" : globalThis.window.location.origin;
			const response = await fetch(
				`${basePath}/api/${url}`,
				{
					method: 'POST',
					body: JSON.stringify(data)
				});
			if (!response.ok) {
				console.error('PostAsync: response is not ok!:', error);
				throw new Error(`HTTP error! Status: ${response.status}`);
			}
			return await response.json();
		} catch (error) {
			console.error('Fetch error:', error);
			throw error;
		}
	},
	patchAsync: async (url, data) => {
		try {
			const basePath = globalThis.window.location.origin.includes("localhost") ?
				"http://localhost:8080" : globalThis.window.location.origin;
			const response = await fetch(
				`${basePath}/api/${url}`,
				{
					method: 'PATCH',
					body: JSON.stringify(data)
				});
			if (!response.ok) {
				console.error('PatchAsync: response is not ok!:', error);
				throw new Error(`HTTP error! Status: ${response.status}`);
			}
			return await response.json();
		} catch (error) {
			console.error('Fetch error:', error);
			throw error;
		}
	},
	putAsync: async (url, data) => {
		try {
			const basePath = globalThis.window.location.origin.includes("localhost") ?
				"http://localhost:8080" : globalThis.window.location.origin;
			const response = await fetch(
				`${basePath}/api/${url}`,
				{
					method: 'PUT',
					body: JSON.stringify(data)
				});
			if (!response.ok) {
				console.error('PutAsync: response is not ok!:', error);
				throw new Error(`HTTP error! Status: ${response.status}`);
			}
			return await response.json();
		} catch (error) {
			console.error('Fetch error:', error);
			throw error;
		}
	},
	deleteAsync: async (url) => {
		try {
			const basePath = globalThis.window.location.origin.includes("localhost") ?
				"http://localhost:8080" : globalThis.window.location.origin;
			const response = await fetch(
				`${basePath}/api/${url}`,
				{
					method: 'DELETE'
				});
			if (!response.ok) {
				console.error('DeleteAsync: response is not ok!:', error);
				throw new Error(`HTTP error! Status: ${response.status}`);
			}
			return await response.json();
		} catch (error) {
			console.error('Fetch error:', error);
			throw error;
		}
	}
});

const config = getConfig();

console.log('running', config.mainAssemblyName);
await runMain(config.mainAssemblyName, [window.location.search]);