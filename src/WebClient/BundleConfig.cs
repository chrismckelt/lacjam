using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using BundleTransformer.CleanCss.Minifiers;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using BundleTransformer.Core.Translators;
using BundleTransformer.Less.Translators;
using BundleTransformer.TypeScript.Translators;
using BundleTransformer.UglifyJs;
using BundleTransformer.UglifyJs.Configuration;
using BundleTransformer.UglifyJs.Minifiers;

namespace Lacjam.WebClient
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var engine = new JavaScriptEngineSwitcher.V8.V8JsEngine();
            var uglySettings = new UglifySettings();
            uglySettings.Js.CodeGeneration.Beautify = true;
            bundles.UseCdn = false;
            var uglifyJsMinifier = new UglifyJsMinifier(() => engine, uglySettings)
            {
                CompressionOptions = { Angular = true, },
                ParsingOptions = new ParsingOptions() { Strict = true },
                CodeGenerationOptions = new CodeGenerationOptions() { }
            
            };
           
            var lessTranslator = new LessTranslator();
            var nullBuilder = new DefaultBundleBuilder();
            var cssTransformer = new CssTransformer(new CleanCssMinifier(), new ITranslator[] { lessTranslator });

            var tsTranslater = new TypeScriptTranslator();

            var jsTransformer = new JsTransformer(uglifyJsMinifier,new List<ITranslator>(){tsTranslater});
            var nullOrderer = new NullOrderer();


            var jsBundle = new CustomScriptBundle("~/bundles/js");
            jsBundle.IncludeDirectory("~/scripts", "*.js", false);
            jsBundle.IncludeDirectory("~/scripts/angular-ui", "*.js", false);

            jsBundle.Builder = new DefaultBundleBuilder();
            jsBundle.Orderer = new JsBundlerOrderer();
            jsBundle.Transforms.Add(jsTransformer);
            bundles.Add(jsBundle);

            var typeScriptBundle = new CustomScriptBundle("~/bundles/ts");
            typeScriptBundle.IncludeDirectory("~/app", "*.js", true);
            typeScriptBundle.Transforms.Add(jsTransformer);
            jsBundle.Builder = new DefaultBundleBuilder();
            jsBundle.Orderer = new JsBundlerOrderer();
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
 
            var with = files.Where(a => a.VirtualFile.Name.Contains(_name)).OrderBy(a => a.VirtualFile.Name).OrderBy(a => a.VirtualFile.Name).ToList();
            var without = files.Where(a => !a.VirtualFile.Name.Contains(_name)).OrderBy(a => a.VirtualFile.Name);
            with.AddRange(without);
            var rf =
                with.Where(
                    bundleFile => bundleFile.VirtualFile.Name != null 
                        && !bundleFile.VirtualFile.Name.Contains(".min.")
                        && !bundleFile.VirtualFile.Name.Contains("-min.")
                        );
            return rf;
        }
    }

    public class JsBundlerOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
           const string name = "jquery";
           var bundleFiles = files as BundleFile[] ?? files.ToArray();

           var start = bundleFiles
                .Where(a => a.IncludedVirtualPath.Contains(name) )
                .Where(b => b.IncludedVirtualPath.ToCharArray().Count(c => c== Convert.ToChar(@"/")) < 2)
                .OrderBy(a => a.IncludedVirtualPath)
                .ToList();
           var middle = bundleFiles
                .Where(a => !a.IncludedVirtualPath.Contains(name))
                 .Where(b => b.IncludedVirtualPath.ToCharArray().Count(c => c == Convert.ToChar(@"/")) < 2)
                .OrderBy(a => a.IncludedVirtualPath)
                .ToList();


           var end = bundleFiles
                .Where(b => b.IncludedVirtualPath.ToCharArray().Count(c => c == Convert.ToChar(@"/")) >= 2)
                .OrderBy(a => a.IncludedVirtualPath)
                ;
             
            middle.AddRange(end);
            start.AddRange(middle);
            return start;
        }
    }
}
