using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using BundleTransformer.CleanCss.Minifiers;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Transformers;
using BundleTransformer.Core.Translators;
using BundleTransformer.Less.Translators;
using BundleTransformer.TypeScript.Translators;
using BundleTransformer.UglifyJs;
using BundleTransformer.UglifyJs.Configuration;
using BundleTransformer.UglifyJs.Minifiers;
using JavaScriptEngineSwitcher.V8;
using Lacjam.Framework.Extensions;


namespace Lacjam.WebClient
{
    /// <summary>
    ///     http://stackoverflow.com/questions/11980458/bundler-not-including-min-files
    /// </summary>
    public class BundleConfig
    {
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //   ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }


        public static void RegisterBundles(BundleCollection bundles)
        {
            // bundles.IgnoreList.Clear();
            //  AddDefaultIgnorePatterns(bundles.IgnoreList);
            //NOTE: it's bundles.DirectoryFilter in Microsoft.AspNet.Web.Optimization.1.1.3 and not bundles.IgnoreList

            var engine = new V8JsEngine();

            var uglySettings = new UglifySettings();

            uglySettings.Js.CodeGeneration.Beautify = true;

            bundles.UseCdn = false;

            var uglifyJsMinifier = new UglifyJsMinifier(() => engine, uglySettings)

            {
                CompressionOptions = { Angular = true },
                ParsingOptions = new ParsingOptions { Strict = true },
                CodeGenerationOptions = new CodeGenerationOptions()
            };


            var lessTranslator = new LessTranslator();

            var cssTransformer = new CssTransformer(new CleanCssMinifier(), new ITranslator[] { lessTranslator });

            var tsTranslater = new TypeScriptTranslator();

            var jsTransformer = new JsTransformer(uglifyJsMinifier, new List<ITranslator> { tsTranslater });

            var jsBundle = new CustomScriptBundle("~/bundles/js");

            jsBundle.IncludeDirectory("~/scripts", "*.js", true);
            jsBundle.IncludeDirectory("~/scripts", "*.map", true);
            
            bundles.IgnoreList.Ignore("angular-mocks.js");
            bundles.IgnoreList.Ignore("angular-scenario.js");

         //   jsBundle.IncludeDirectory("~/scripts/angular-dialog-service-5.1.2", "*.js", true);

            jsBundle.Builder = new DefaultBundleBuilder();

            jsBundle.Orderer = new JsBundlerOrderer();

            jsBundle.Transforms.Add(jsTransformer);

            bundles.Add(jsBundle);


            var typeScriptBundle = new CustomScriptBundle("~/bundles/ts");    //// Typescript generatred locally via visual studio and checked in - not done on the fly

            typeScriptBundle.IncludeDirectory("~/app", "*.js", true);

            typeScriptBundle.Transforms.Add(jsTransformer);

            typeScriptBundle.Builder = new DefaultBundleBuilder();

            typeScriptBundle.Orderer = new JsBundlerOrderer();

            bundles.Add(typeScriptBundle);


            var lessCssBundle = new Bundle("~/bundles/less");

            lessCssBundle.Include("~/Content/bootstrap/bootstrap.less");

            lessCssBundle.Builder = new DefaultBundleBuilder();

            lessCssBundle.Transforms.Add(cssTransformer);

            lessCssBundle.Orderer = new DefaultBundleOrderer();

            bundles.Add(lessCssBundle);

            var cssBundle = new StyleBundle("~/bundles/css");

            cssBundle.IncludeDirectory("~/Content/", "*.css", true);

            cssBundle.Builder = new DefaultBundleBuilder();

            cssBundle.Transforms.Add(cssTransformer);

            cssBundle.Orderer = new PushToTopOrderer("bootstrap");

            bundles.Add(cssBundle);
        }
    }


    public class PushToTopOrderer : IBundleOrderer
    {
        private readonly string _name;


        public PushToTopOrderer(string name)
        {
            _name = name;
        }


        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            BundleFile[] bundleFiles = files as BundleFile[] ?? files.ToArray();
            foreach (BundleFile file in bundleFiles)
            {
                file.IncludedVirtualPath = file.IncludedVirtualPath.Replace(@"\", "/");
            }

            List<BundleFile> with =
                bundleFiles.Where(a => a.VirtualFile.Name.Contains(_name) || a.IncludedVirtualPath.Contains(_name))
                    .OrderBy(a => a.VirtualFile.Name)
                    .ToList();

            IOrderedEnumerable<BundleFile> without =
                bundleFiles.Where(a => !a.VirtualFile.Name.Contains(_name)).OrderBy(a => a.VirtualFile.Name);

            with.AddRange(without);

            IEnumerable<BundleFile> rf =
                with.Where(
                    bundleFile => bundleFile.VirtualFile.Name != null
                                  && !bundleFile.VirtualFile.Name.Contains(".min.")
                                  && !bundleFile.VirtualFile.Name.Contains("-min.")
                                  && !bundleFile.VirtualFile.Name.EndsWith(".less")
                    );

            return rf;
        }
    }

    public class JsBundlerOrderer : IBundleOrderer
    {
        private List<BundleFile> _filesInCorrectOrder;

        private void Add(BundleFile bf)
        {
            if (bf == null) return;
            if (!_filesInCorrectOrder.Contains(bf) && bf.VirtualFile != null)
            {
                if (!bf.VirtualFile.Name.Contains(@"_References"))
                    _filesInCorrectOrder.Add(bf);
            }
        }

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            var exclude = new List<string>
            {
                "require.js",
            };


            BundleFile[] bundleFiles = files as BundleFile[] ??
                                       files.Where(a => !exclude.Contains(a.VirtualFile.Name)).ToArray();


            var uptop = new List<string>
            {
                "jquery-1.10.2",
                "underscore",
                "modernizr-2.6.2",
                "q",
                "respond",
                "moment",
                "angular",
                "spin",
                "toastr",
                "angular-ui-router",
                "angular-ui",
                "bootstrap",
                "ui-utils",
                "ui-utils-ieshiv",
                "select2"
            };

            // leftovers  
            var downbelow = new List<string>
            {
              //  "dialogs",
               
            };

            _filesInCorrectOrder = new List<BundleFile>();

            //  uptop.Reverse();

            var filteredForImportantOnly = bundleFiles.Where(a => downbelow.All(b => b.ToJavaScriptExtension(true) != a.VirtualFile.Name));

            foreach (string name in uptop)
            {
                string name1 = name;

                var firstItems = filteredForImportantOnly
                    .Where(a => a.VirtualFile.Name.ToLowerInvariant() == name.ToJavaScriptExtension(true)
                        .ToLowerInvariant());

                firstItems.Each(a => Add(a));
                // Add(first);

                foreach (BundleFile bundleFile in filteredForImportantOnly.Where(
                    a => a.VirtualFile.Name.Contains(name1))
                    .Where(bundleFile => IsStartingFile(name, bundleFile.VirtualFile.Name))
                    .Where(bundleFile => !_filesInCorrectOrder.Contains(bundleFile)))
                {
                    _filesInCorrectOrder.Add(bundleFile);
                }
            }

            foreach (BundleFile bf in from name1 in uptop
                                      from bundleFile in filteredForImportantOnly.Where(
                                          a => a.VirtualFile.Name.Contains(name1))
                                      where !_filesInCorrectOrder.Contains(bundleFile)
                                      select bundleFile)
            {
                Add(bf);
            }

            foreach (BundleFile leftover in bundleFiles)
            {
                Add(leftover);
            }

            foreach (var source in bundleFiles.Where(a => downbelow.All(b => b.ToJavaScriptExtension(true).Contains(a.VirtualFile.Name))))
            {
                Add(source);
            }

            return _filesInCorrectOrder;
        }

        private bool IsStartingFile(string starter, string name)
        {
            bool xfile = name.ToLowerInvariant() == (starter.ToJavaScriptExtension(true)).ToLowerInvariant();
            return xfile;
        }
    }
}
