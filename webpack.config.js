const path = require("path");
const webpack = require("webpack");
const { AureliaPlugin, ModuleDependenciesPlugin, GlobDependenciesPlugin } = require("aurelia-webpack-plugin");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const extractCSS = new ExtractTextPlugin("vendor.css");
const bundleOutputDir = "./wwwroot/dist";

module.exports = (env, argv) => {
	if ((!argv || !argv.mode) && process.env.ASPNETCORE_ENVIRONMENT === "Development") {
		argv = { mode: "development" };
	}
	console.log("mode=", argv.mode);
	const isDevBuild = argv.mode !== "production";
	const cssLoader = { loader: isDevBuild ? "css-loader" : "css-loader?minimize" };
  const isAurelia = (module) => {
    // Return true if it's an Aurelia module, with the exception of aurelia-bootstrapper.
    return module && module.context && module.context.indexOf('aurelia-') !== -1 && module.context.indexOf('aurelia-bootstrapper') === -1;
  };
  const isNodeModule = (module) => {
    // Return true if it's a node module, with the exception of aurelia-bootstrapper.
    return module && module.context && module.context.indexOf('node_modules') !== -1 && module.context.indexOf('aurelia-bootstrapper') === -1 && !isBootstrap(module);
  };
  const isBootstrap = (module) => {
    return module && module.context && module.context.indexOf('node_modules/bootstrap') !== -1;
  };
  return [{
		target: "web",
		mode: isDevBuild ? "development" : "production",
		entry: { "app": ["es6-promise/auto", "aurelia-bootstrapper"] },
		resolve: {
			extensions: [".ts", ".js"],
			modules: ["ClientApp", "node_modules"]
		},
		output: {
			path: path.resolve(bundleOutputDir),
			publicPath: "/dist/",
			filename: "[name].js",
			chunkFilename: "[name].js"
		},
		module: {
			rules: [
				{ test: /\.(woff|woff2)(\?|$)/, loader: "url-loader?limit=1" },
				{ test: /\.(png|eot|ttf|svg)(\?|$)/, loader: "url-loader?limit=100000" },
				{ test: /\.ts$/i, include: [/ClientApp/, /node_modules/], use: "awesome-typescript-loader" },
				{ test: /\.html$/i, use: "html-loader" },
				{ test: /\.css(\?|$)/, include: [/node_modules/], loader: extractCSS.extract([isDevBuild ? 'css-loader' : 'css-loader?minimize']) },
				{ test: /\.css$/i, exclude: [/node_modules/], issuer: /\.html$/i, use: cssLoader },
				{ test: /\.css$/i, exclude: [/node_modules/], issuer: [{ not: [{ test: /\.html$/i }] }], use: ["style-loader", cssLoader] },
				{ test: /\.scss$/i, issuer: /(\.html|empty-entry\.js)$/i, use: [cssLoader, "sass-loader"] },
				{ test: /\.scss$/i, issuer: /\.ts$/i, use: ["style-loader", cssLoader, "sass-loader"] }
			]
		},
		optimization: {
			splitChunks: {
				cacheGroups: {
          commons: {
						test: /[\\/]node_modules[\\/]/,
						name: "vendor",
						chunks: "all"
					}
          // vendor: {
          //   chunks: 'all',
          //   enforce: true,
          //   name: 'vendor',
          //   test: (module) => { return isNodeModule(module) && !isAurelia(module); }
          // },
          // aurelia: {
          //   chunks: 'all',
          //   enforce: true,
          //   name: 'aurelia',
          //   test: (module) => { return isNodeModule(module) && isAurelia(module); }
          // },
          // bootstrap: {
          //   chunks: 'all',
          //   enforce: true,
          //   name: 'bootstrap',
          //   test: (module) => { return isBootstrap(module); }
          // }
				}
			}
		},
		devtool: isDevBuild ? "source-map" : false,
		performance: {
			hints: false
		},
		plugins: [
      new webpack.ContextReplacementPlugin(/moment[/\\]locale$/, /nl/),
      new webpack.DefinePlugin({ IS_DEV_BUILD: JSON.stringify(isDevBuild) }),
			new webpack.ProvidePlugin({ $: "jquery", jQuery: "jquery", "window.jQuery": "jquery" }),
			new AureliaPlugin({ aureliaApp: "boot" }),
			new GlobDependenciesPlugin({ "boot": ["ClientApp/**/*.{ts,html}"] }),
			new ModuleDependenciesPlugin({}),
			extractCSS
		]
	}];
};
