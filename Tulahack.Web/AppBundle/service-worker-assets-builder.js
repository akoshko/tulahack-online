const fs = require('fs');

// Paths to the input and output files
const blazorBootJsonPath = './blazor.boot.json';
const serviceWorkerAssetsJsPath = './service-worker-assets.js';

fs.readFile(blazorBootJsonPath, 'utf8', (err, data) => {
	if (err) {
		console.error('Error reading blazor.boot.json:', err);
		return;
	}

	try {
		const blazorBoot = JSON.parse(data);

		// assetsManifest structure example
		let assetsManifest = {
			"assets": [{
				"hash": "sha256-taT3IZgnth\/SVDoEl9Kl5ippE\/3JFSALsWDWF8SaeTM=",
				"url": "AppBundle\/_framework\/Avalonia.Base.wasm"
			}], "version": "YVEr+xUg"
		};
		// clean up self.assetsManifest
		assetsManifest = {};

		// Extract assets from the blazor.boot.json sections
		const jsModuleNativeAssets = Object.entries(blazorBoot.resources.jsModuleNative || {}).map(([url, hash]) => ({
			hash, url
		}));

		const jsModuleRuntimeAssets = Object.entries(blazorBoot.resources.jsModuleRuntime || {}).map(([url, hash]) => ({
			hash, url
		}));

		const wasmNativeAssets = Object.entries(blazorBoot.resources.wasmNative || {}).map(([url, hash]) => ({
			hash, url
		}));

		const wasmSymbolsAssets = Object.entries(blazorBoot.resources.wasmSymbols || {}).map(([url, hash]) => ({
			hash, url
		}));

		const icuAssets = Object.entries(blazorBoot.resources.icu || {}).map(([url, hash]) => ({
			hash, url
		}));

		const assemblyAssets = Object.entries(blazorBoot.resources.assembly || {}).map(([url, hash]) => ({
			hash, url
		}));

		const pdbAssets = Object.entries(blazorBoot.resources.pdb || {}).map(([url, hash]) => ({
			hash, url
		}));

		const satelliteResourcesAssets = Object.entries(blazorBoot.resources.satelliteResources || {}).map(([url, hash]) => ({
			hash, url
		}));

		const vfsAssets = Object.entries(blazorBoot.resources.vfs['runtimeconfig.bin'] || {}).map(([url, hash]) => ({
			hash, url
		}));

		// Combine all assets
		assetsManifest.assets = [
			...jsModuleNativeAssets,
			...jsModuleRuntimeAssets,
			...wasmNativeAssets,
			...wasmSymbolsAssets,
			...icuAssets,
			...assemblyAssets,
			...pdbAssets,
			...satelliteResourcesAssets,
			...vfsAssets
		];
		assetsManifest.version = blazorBoot.resources.hash;

		// Write the content to service-worker-assets.js
		const serviceWorkerAssetsContent = `self.assetsManifest = {
		  "assets": ${JSON.stringify(assetsManifest.assets, null, 2)},
		  "version": ${JSON.stringify(assetsManifest.version, null, 2)}
		};`;
		fs.writeFile(serviceWorkerAssetsJsPath, serviceWorkerAssetsContent, 'utf8', (err) => {
			if (err) {
				console.error('Error writing service-worker-assets.js:', err);
				return;
			}
			console.log('service-worker-assets.js has been successfully generated.');
		});
	} catch (parseError) {
		console.error('Error parsing blazor.boot.json:', parseError);
	}
});