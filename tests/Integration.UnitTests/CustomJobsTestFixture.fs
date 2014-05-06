namespace Lacjam.Integration.UnitTests
module CustomJobsTestFixture  =
    open FsUnit.Xunit
    open Lacjam
//    open Ploeh.AutoFixture
//    open Ploeh.AutoFixture.AutoFoq
//    open Ploeh.AutoFixture.DataAnnotations
// 
//    open Foq
//    open NServiceBus    
//    open NServiceBus.ObjectBuilder.Common
    open System
    open System.Linq
    open System.Collections
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Runtime
    open Lacjam.Core.Jobs
    open Lacjam.Core.Scheduling
    open Lacjam.Core.Utility
    open Lacjam.Integration
    open Xunit


   // let fixture = Fixture().Customize(AutoFoqCustomization())

    //#region SwellNet HTML

    [<Literal>]
    let htmlContent = """ 
    <!DOCTYPE html>
    <!--[if IEMobile 7]><html class="iem7"  lang="en" dir="ltr"><![endif]-->
    <!--[if lte IE 6]><html class="lt-ie9 lt-ie8 lt-ie7"  lang="en" dir="ltr"><![endif]-->
    <!--[if (IE 7)&(!IEMobile)]><html class="lt-ie9 lt-ie8"  lang="en" dir="ltr"><![endif]-->
    <!--[if IE 8]><html class="lt-ie9"  lang="en" dir="ltr"><![endif]-->
    <!--[if (gte IE 9)|(gt IEMobile 7)]><!--><html  lang="en" dir="ltr"><!--<![endif]-->

    <head profile="http://www.w3.org/1999/xhtml/vocab">

      <meta charset="utf-8" />
    <link rel="shortlink" href="/node/124" />
    <link rel="shortcut icon" href="http://www.swellnet.com/profiles/swellnet/themes/swellzen/favicon.ico" type="image/vnd.microsoft.icon" />
    <link rel="canonical" href="/reports/australia/new-south-wales/cronulla" />
    <link rel="canonical" href="http://www.swellnet.com/reports/australia/new-south-wales/cronulla" />
    <meta name="Generator" content="Drupal 7 (http://drupal.org)" />
    <script>var _sf_startpt=(new Date()).getTime();</script>
      <title>Cronulla Detailed Surf Report, Surf Photos, Live Winds, Tides and Weather | Swellnet</title>

          <meta name="MobileOptimized" content="width">
        <meta name="HandheldFriendly" content="true">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <meta http-equiv="cleartype" content="on">

      <link type="text/css" rel="stylesheet" href="http://www.swellnet.com/sites/default/files/css/css_791YXBaKKm1ORM_7huSKEsIV9tSWq6wmRkERhuXpN6w.css" media="all" />
    <link type="text/css" rel="stylesheet" href="http://www.swellnet.com/sites/default/files/css/css_KIExfB-v-nd8aWElmHFanaGceI1Pt7PojBIDig3YsVI.css" media="all" />
    <link type="text/css" rel="stylesheet" href="http://www.swellnet.com/sites/default/files/css/css_4ihpQDhUUDX5b85a1nWqAumjHt5azPdnazEvRwBBBps.css" media="all" />
    <link type="text/css" rel="stylesheet" href="http://www.swellnet.com/sites/default/files/css/css_PKwsSJN0-THLHs6aeJxcCaULMp71Ouj0TarpwGIixko.css" media="all" />
    <link type="text/css" rel="stylesheet" href="http://www.swellnet.com/sites/default/files/css/css_6IUmFFVDRcZ7NEbqORDT2Nz9aB7XHmWi9nJVcLAeXqY.css" media="all" />
      <script src="http://www.swellnet.com/sites/default/files/js/js_zzcIWOou_jnX0ZWAIA4sb6Xy_p5a8FZNA0GySvuWjPU.js"></script>
    <script src="http://www.swellnet.com/sites/default/files/js/js_HnDbOFSKer3VRUBUn6qRWdTFePROakXKJQPYQvJ7ETA.js"></script>
    <script src="http://www.swellnet.com/sites/default/files/js/js_Kf4BxwEptSusX1YzKxn2XpNw5mSlMDeiGO-4l2cXQVM.js"></script>
    <script src="http://www.swellnet.com/sites/default/files/js/js_ZvoCksJ7imp1ylshhPZW12I0cdeEeoCj1_FOVqfJX2o.js"></script>
    <script src="http://www.swellnet.com/sites/default/files/js/js_1I63EV2Un02YeSe_qhYfgYz2RuUCae25XoFp8zX-d6w.js"></script>
    <script>jQuery.extend(Drupal.settings, {"basePath":"\/","pathPrefix":"","ajaxPageState":{"theme":"swellzen","theme_token":"LVmW_IiXinxC-3yoIhtbu7L20HmKxyd34Buyvwfx7Ek","js":{"profiles\/swellnet\/modules\/contrib\/jquery_update\/replace\/jquery\/1.5\/jquery.min.js":1,"misc\/jquery.once.js":1,"misc\/drupal.js":1,"profiles\/swellnet\/modules\/contrib\/jquery_update\/replace\/ui\/ui\/minified\/jquery.ui.core.min.js":1,"profiles\/swellnet\/modules\/contrib\/jquery_update\/replace\/ui\/ui\/minified\/jquery.ui.widget.min.js":1,"profiles\/swellnet\/modules\/contrib\/jquery_update\/replace\/ui\/ui\/minified\/jquery.ui.accordion.min.js":1,"profiles\/swellnet\/modules\/contrib\/comment_notify\/comment_notify.js":1,"profiles\/swellnet\/modules\/contrib\/extlink\/extlink.js":1,"profiles\/swellnet\/modules\/contrib\/panels\/js\/panels.js":1,"profiles\/swellnet\/modules\/contrib\/quote\/quote.js":1,"profiles\/swellnet\/modules\/custom\/swellnet_location\/swellnet_location_report\/js\/swellnet_report.js":1,"profiles\/swellnet\/modules\/custom\/swellnet_location\/swellnet_location_report\/js\/2D.js":1,"profiles\/swellnet\/modules\/custom\/swellnet_location\/swellnet_location_report\/js\/swellnet_report_tides.js":1,"profiles\/swellnet\/modules\/custom\/swellnet_location\/swellnet_location_report\/js\/swellnet_report_wind.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/script.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/fresco\/fresco.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/formalize\/jquery.formalize.legacy.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/tipsy\/jquery.tipsy.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/d3\/d3.v3.min.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/colorbox\/jquery.colorbox-min.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/flexslider\/jquery.flexslider-min.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/masonry\/masonry.pkgd.min.js":1,"profiles\/swellnet\/themes\/swellzen\/js\/lib\/imagesloaded\/imagesloaded.pkgd.min.js":1},"css":{"modules\/system\/system.base.css":1,"modules\/system\/system.menus.css":1,"modules\/system\/system.messages.css":1,"modules\/system\/system.theme.css":1,"misc\/ui\/jquery.ui.core.css":1,"misc\/ui\/jquery.ui.theme.css":1,"misc\/ui\/jquery.ui.accordion.css":1,"profiles\/swellnet\/modules\/contrib\/comment_notify\/comment_notify.css":1,"modules\/comment\/comment.css":1,"profiles\/swellnet\/modules\/contrib\/date\/date_api\/date.css":1,"profiles\/swellnet\/modules\/contrib\/date\/date_popup\/themes\/datepicker.1.7.css":1,"modules\/field\/theme\/field.css":1,"profiles\/swellnet\/modules\/contrib\/logintoboggan\/logintoboggan.css":1,"profiles\/swellnet\/modules\/contrib\/mollom\/mollom.css":1,"modules\/node\/node.css":1,"modules\/search\/search.css":1,"modules\/user\/user.css":1,"profiles\/swellnet\/modules\/contrib\/video_filter\/video_filter.css":1,"profiles\/swellnet\/modules\/contrib\/extlink\/extlink.css":1,"modules\/forum\/forum.css":1,"profiles\/swellnet\/modules\/contrib\/views\/css\/views.css":1,"profiles\/swellnet\/modules\/contrib\/chartbeat\/chartbeat.css":1,"profiles\/swellnet\/modules\/contrib\/ckeditor\/ckeditor.css":1,"profiles\/swellnet\/modules\/contrib\/ctools\/css\/ctools.css":1,"profiles\/swellnet\/modules\/contrib\/panels\/css\/panels.css":1,"profiles\/swellnet\/modules\/contrib\/quote\/quote.css":1,"profiles\/swellnet\/themes\/swellzen\/layouts\/sixthree\/sixthree.css":1,"profiles\/swellnet\/themes\/zen\/system.menus.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/normalize.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/layouts\/fixed-width.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/tabs.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/pages.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/blocks.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/navigation.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/views-styles.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/nodes.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/comments.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/forms.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/fields.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/panels.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/style.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/users.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/print.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/wams.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/forecast.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/report.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/lib\/fresco\/fresco.css":1,"profiles\/swellnet\/themes\/swellzen\/css\/lib\/tipsy\/tipsy.css":1}},"chartbeat":{"uid":10032,"domain":"www.swellnet.com","useCanonical":true,"noCookies":false},"quote_nest":2,"video_filter":{"url":{"ckeditor":"\/video_filter\/dashboard\/ckeditor"},"modulepath":"profiles\/swellnet\/modules\/contrib\/video_filter"},"swellnet":{"before":1391845433,"start":1391931833,"end":1392018233,"offset":39600,"wind":{"1391925600":{"timestamp":1391925600,"time":"6:00am","day":"Sun","direction":"40","label":"NE","speed":"12","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391922000":{"timestamp":1391922000,"time":"5:00am","day":"Sun","direction":"50","label":"NE","speed":"10","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391918400":{"timestamp":1391918400,"time":"4:00am","day":"Sun","direction":"60","label":"ENE","speed":"9","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#000c70"},"1391914800":{"timestamp":1391914800,"time":"3:00am","day":"Sun","direction":"40","label":"NE","speed":"9","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#000c70"},"1391911200":{"timestamp":1391911200,"time":"2:00am","day":"Sun","direction":"20","label":"NNE","speed":"12","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391907600":{"timestamp":1391907600,"time":"1:00am","day":"Sun","direction":"10","label":"N","speed":"13","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391904000":{"timestamp":1391904000,"time":"12:00am","day":"Sun","direction":"10","label":"N","speed":"14","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391900400":{"timestamp":1391900400,"time":"11:00pm","day":"Sat","direction":"20","label":"NNE","speed":"14","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391896800":{"timestamp":1391896800,"time":"10:00pm","day":"Sat","direction":"30","label":"NNE","speed":"14","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391893200":{"timestamp":1391893200,"time":"9:00pm","day":"Sat","direction":"40","label":"NE","speed":"14","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391889600":{"timestamp":1391889600,"time":"8:00pm","day":"Sat","direction":"50","label":"NE","speed":"17","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#d9d801"},"1391886000":{"timestamp":1391886000,"time":"7:00pm","day":"Sat","direction":"50","label":"NE","speed":"17","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#d9d801"},"1391882400":{"timestamp":1391882400,"time":"6:00pm","day":"Sat","direction":"50","label":"NE","speed":"18","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#d9d801"},"1391878800":{"timestamp":1391878800,"time":"5:00pm","day":"Sat","direction":"50","label":"NE","speed":"21","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#da0000"},"1391875200":{"timestamp":1391875200,"time":"4:00pm","day":"Sat","direction":"50","label":"NE","speed":"22","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#da0000"},"1391871600":{"timestamp":1391871600,"time":"3:00pm","day":"Sat","direction":"50","label":"NE","speed":"21","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#da0000"},"1391868000":{"timestamp":1391868000,"time":"2:00pm","day":"Sat","direction":"50","label":"NE","speed":"20","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#da0000"},"1391864400":{"timestamp":1391864400,"time":"1:00pm","day":"Sat","direction":"40","label":"NE","speed":"18","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#d9d801"},"1391860800":{"timestamp":1391860800,"time":"12:00pm","day":"Sat","direction":"40","label":"NE","speed":"20","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#da0000"},"1391857200":{"timestamp":1391857200,"time":"11:00am","day":"Sat","direction":"40","label":"NE","speed":"16","layout":{"size":"16px","left":"9px","top":"9px"},"colour":"#d9d801"},"1391853600":{"timestamp":1391853600,"time":"10:00am","day":"Sat","direction":"40","label":"NE","speed":"14","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#00d900"},"1391850000":{"timestamp":1391850000,"time":"9:00am","day":"Sat","direction":"310","label":"NW","speed":"6","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#000c70"},"1391846400":{"timestamp":1391846400,"time":"8:00am","day":"Sat","direction":"360","label":"N","speed":"6","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#000c70"},"1391842800":{"timestamp":1391842800,"time":"7:00am","day":"Sat","direction":"350","label":"N","speed":"8","layout":{"size":"12px","left":"13px","top":"12px"},"colour":"#000c70"}},"tides":{"1391922300":{"timestamp":1391922300,"original_time":"2014-02-09T05:05","time":"5:05am","day":"Sun","type":"High","height":"1.5"},"1391947020":{"timestamp":1391947020,"original_time":"2014-02-09T11:57","time":"11:57am","day":"Sun","type":"Low","height":"0.6"},"1391968020":{"timestamp":1391968020,"original_time":"2014-02-09T17:47","time":"5:47pm","day":"Sun","type":"High","height":"1.1"},"1391988300":{"timestamp":1391988300,"original_time":"2014-02-09T23:25","time":"11:25pm","day":"Sun","type":"Low","height":"0.6"},"1392012240":{"timestamp":1392012240,"original_time":"2014-02-10T06:04","time":"6:04am","day":"Mon","type":"High","height":"1.5"},"1392036840":{"timestamp":1392036840,"original_time":"2014-02-10T12:54","time":"12:54pm","day":"Mon","type":"Low","height":"0.6"},"1392057960":{"timestamp":1392057960,"original_time":"2014-02-10T18:46","time":"6:46pm","day":"Mon","type":"High","height":"1.2"},"1392078060":{"timestamp":1392078060,"original_time":"2014-02-11T00:21","time":"12:21am","day":"Tue","type":"Low","height":"0.6"},"1392101700":{"timestamp":1392101700,"original_time":"2014-02-11T06:55","time":"6:55am","day":"Tue","type":"High","height":"1.6"},"1392125880":{"timestamp":1392125880,"original_time":"2014-02-11T13:38","time":"1:38pm","day":"Tue","type":"Low","height":"0.5"},"1392147180":{"timestamp":1392147180,"original_time":"2014-02-11T19:33","time":"7:33pm","day":"Tue","type":"High","height":"1.2"},"1392167340":{"timestamp":1392167340,"original_time":"2014-02-12T01:09","time":"1:09am","day":"Wed","type":"Low","height":"0.6"},"1392190680":{"timestamp":1392190680,"original_time":"2014-02-12T07:38","time":"7:38am","day":"Wed","type":"High","height":"1.6"},"1392214560":{"timestamp":1392214560,"original_time":"2014-02-12T14:16","time":"2:16pm","day":"Wed","type":"Low","height":"0.4"},"1392235980":{"timestamp":1392235980,"original_time":"2014-02-12T20:13","time":"8:13pm","day":"Wed","type":"High","height":"1.3"},"1392256260":{"timestamp":1392256260,"original_time":"2014-02-13T01:51","time":"1:51am","day":"Thu","type":"Low","height":"0.5"},"1392279360":{"timestamp":1392279360,"original_time":"2014-02-13T08:16","time":"8:16am","day":"Thu","type":"High","height":"1.6"},"1392303000":{"timestamp":1392303000,"original_time":"2014-02-13T14:50","time":"2:50pm","day":"Thu","type":"Low","height":"0.4"},"1392324420":{"timestamp":1392324420,"original_time":"2014-02-13T20:47","time":"8:47pm","day":"Thu","type":"High","height":"1.4"},"1392345000":{"timestamp":1392345000,"original_time":"2014-02-14T02:30","time":"2:30am","day":"Fri","type":"Low","height":"0.5"},"1392367980":{"timestamp":1392367980,"original_time":"2014-02-14T08:53","time":"8:53am","day":"Fri","type":"High","height":"1.7"},"1392391320":{"timestamp":1392391320,"original_time":"2014-02-14T15:22","time":"3:22pm","day":"Fri","type":"Low","height":"0.4"},"1392412860":{"timestamp":1392412860,"original_time":"2014-02-14T21:21","time":"9:21pm","day":"Fri","type":"High","height":"1.4"}}},"extlink":{"extTarget":"_blank","extClass":0,"extSubdomains":1,"extExclude":"","extInclude":"","extCssExclude":"","extCssExplicit":"","extAlert":0,"extAlertText":"This link will take you to an external web site.","mailtoClass":0}});</script>

          <!--[if lt IE 9]>
        <script src="/profiles/swellnet/themes/zen/js/html5-respond.js"></script>
        <![endif]-->
        <script>
        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-366603-1']);
        _gaq.push(['_trackPageview']);

        (function() {
          var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
          ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
          var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
    </head>
    <body id="swellnet-body" class="html not-front not-logged-in no-sidebars page-node page-node- page-node-124 node-type-surf-location panels-driven-layout main-colspan-9 main-nested-container section-reports page-panels" >
          <p id="skip-link">
          <a href="#main-menu" class="element-invisible element-focusable">Jump to navigation</a>
        </p>
  
        <div id="wrapper">

        <div id="prologue">
        <div id="user-bar">
            <div id='boxes-box-swellnet_facebook_global' class='boxes-box'><div class="boxes-box-content"><iframe allowtransparency="true" frameborder="0" scrolling="no" src="//www.facebook.com/plugins/like.php?href=http%3A%2F%2Fwww.facebook.com%2Fswellnet&amp;send=false&amp;layout=button_count&amp;width=300&amp;show_faces=true&amp;font=verdana&amp;colorscheme=dark&amp;action=like&amp;height=21" style="border:none; overflow:hidden; width:300px; height:21px;"></iframe>
    </div></div><nav id="site-user-menu">
      <ul class="menu"><li class="menu__item is-leaf first leaf" id="nav-log-in"><a href="/user/login" class="menu__link">Log in</a></li>
    <li class="menu__item is-leaf last leaf" id="nav-register"><a href="/user/register" class="menu__link">Register</a></li>
    </ul></nav>    </div>
      </div> <!-- /#prologue -->
  
      <div id="page">
          <div id="header" role="banner">
            <span class="site-logo">
      <a href="/" id="logo" rel="home" title="Return to the Swellnet home page"><img src="http://www.swellnet.com/profiles/swellnet/themes/swellzen/logo.png" alt="Swellnet logo" /></a></span><div id="site-main-menu" class="nav">
        <h2 class="block__title block-title">Main menu</h2>
        <ul class="menu"><li class="menu__item is-leaf first leaf" id="nav-home"><a href="/welcome" title="Home" class="menu__link">Home</a></li>
    <li class="menu__item is-leaf leaf" id="nav-surf-reports-and-forecasts"><a href="/reports" class="menu__link">Surf Reports &amp; Forecasts</a></li>
    <li class="menu__item is-leaf leaf" id="nav-surfcams"><a href="/surfcams" class="menu__link">Surfcams</a></li>
    <li class="menu__item is-leaf leaf" id="nav-news"><a href="/news" class="menu__link">News</a></li>
    <li class="menu__item is-leaf leaf" id="nav-photos"><a href="/photos" class="menu__link">Photos</a></li>
    <li class="menu__item is-leaf leaf" id="nav-forum"><a href="/forum" title="" class="menu__link">Forum</a></li>
    <li class="menu__item is-leaf last leaf new" id="nav-shop"><a href="http://shop.swellnet.com" title="The Swellnet Shop" class="menu__link">Shop</a></li>
    </ul></div><div id='boxes-box-swellnet_adblock_leaderboard' class='boxes-box'><div class="boxes-box-content"><span class="mcnamf" title="728x90"></span></div></div>    </div>
  
      <div id="main-section">

    
              <div id="main" role="main">
        


    <div class="layout-sixthree section panel-left">
      <div class="panel-pane pane-page-breadcrumb" >
  
      
  
      <nav class="breadcrumb" role="navigation"><h2 class="element-invisible">You are here</h2><ol><li><a href="/">Home</a> › </li><li><a href="/reports">Surf Reports &amp; Forecasts</a> › </li><li><a href="/reports/australia">Australia</a> › </li><li><a href="/reports/australia/new-south-wales">New South Wales</a> › </li><li>Cronulla</li></ol></nav>
  
      </div>
    <div class="panel-pane pane-page-tabs" >
  
      
  
      <div id="tabs"><h2 class="element-invisible">Primary tabs</h2><ul class="tabs-primary tabs primary"><li class="active"><a href="/reports/australia/new-south-wales/cronulla" class="active">Surf Report<span class="element-invisible">(active tab)</span></a></li>
    <li><a href="/reports/australia/new-south-wales/cronulla/forecast">Surf Forecast</a></li>
    <li><a href="/reports/australia/new-south-wales/cronulla/wams">WAMS</a></li>
    </ul></div>
  
      </div>
    <div class="panel-pane pane-views-panes pane-surf-reports-panel-pane" >
  
      
  
      <div class="view view-surf-reports view-id-surf_reports view-display-id-panel_pane view-dom-id-01b7c637858a3ad98583958b4a379ed4">
        
  
  
          <div class="view-content">
            <div class="views-row views-row-1 views-row-odd views-row-first views-row-last">
      
      <div class="views-field views-field-title">        <span class="field-content">Cronulla</span>  </div>  </div>
        </div>
  
  
          <div class="attachment attachment-after">
          <div class="view view-surf-reports view-id-surf_reports view-display-id-latest_report">
        
  
  
          <div class="view-content">
            <div class="views-row views-row-1 views-row-odd views-row-first views-row-last">
      
      <span class="views-field views-field-field-surf-report-date">    <span class="views-label views-label-field-surf-report-date">Updated: </span>    <span class="field-content"><span class="date-display-single">Sun 9 February 6:36am</span></span>  </span>  
      <span class="views-field views-field-nothing">    <span class="views-label views-label-nothing">Surf: </span>    <span class="field-content">clean 2-3ft ESE   </span>  </span>  
      <span class="views-field views-field-field-surf-report-wind">    <span class="views-label views-label-field-surf-report-wind">Winds: </span>    <span class="field-content">Light and variable N</span>  </span>  
      <span class="views-field views-field-field-surf-report-weather">    <span class="views-label views-label-field-surf-report-weather">Weather: </span>    <span class="field-content">mostly sunny</span>  </span>  
      <span class="views-field views-field-field-surf-report-rating">    <span class="views-label views-label-field-surf-report-rating">Rating: </span>    <span class="field-content">4/10</span>  </span>  
      <div class="views-field views-field-body">        <div class="field-content"><p>The dawn report says 2ft but some of the bigger sets are nudging 3ft this morning as the ESE swell continues to provide the better quality waves early on. Still a little bump on the waves surface from yesterday's onshores but overall not looking too shabby this morning. There are waves right along the strip with the Wall, Elouera and Midway probably the pick and attracting the bigger set waves more consistently. North Cronulla and Wanda still Ok but you'll have to be a little more patient for those bigger sets. Plenty of opportunity for a wave to yourself but the car parks a filling fast. That Nor Easter will not be too far away so get in early.</p></div>  </div>  </div>
        </div>
  
  
  
  
  
  
    </div><div class="view view-surf-reports view-id-surf_reports view-display-id-older">
        
  
  
          <div class="view-content">
            <div class="views-row views-row-1 views-row-odd views-row-first views-row-last">
      
      <span class="views-field views-field-field-surf-report-date">        <span class="field-content"><span class="date-display-single">Sun 9 February 6:02am</span>:</span>  </span>  
      <span class="views-field views-field-nothing">        <span class="field-content">peaky 2ft ESE,   </span>  </span>  
      <span class="views-field views-field-field-surf-report-wind">        <span class="field-content">Moderate NE,</span>  </span>  
      <span class="views-field views-field-field-surf-report-weather">        <span class="field-content">mostly fine</span>  </span>  
      <span class="views-field views-field-field-surf-report-rating">        <span class="field-content">3/10</span>  </span>  
      <div class="views-field views-field-body">        <div class="field-content"><p>Dawn report: The northern corner away from the winds, picking up the most of the ESE swell there is, are the picks. Some alright little peaks around, perfect for a lazy Sunday. Check back after 8am for the photo update from Darryl. </p></div>  </div>  </div>
        </div>
  
  
  
  
  
  
    </div>    </div>
  
  
  
  
    </div>
  
      </div>
    <div class="panel-pane pane-views-panes pane-surf-reports-photos" >
  
      
  
      <div class="view view-surf-reports view-id-surf_reports view-display-id-photos view-dom-id-529b6bfc1ec3e4469ba01b9fce83c1ef">
                <div class="view-header">
          Daily Photos:    </div>
  
  
  
          <div class="view-content">
            <div class="views-row views-row-1 views-row-odd views-row-first">
      
      <div class="views-field views-field-field-surf-report-images">        <div class="field-content"><div class="field field-name-field-surf-report-images field-type-file field-label-hidden"><div class="field-items"><div class="field-item even"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00014_18.jpg?itok=i3MQHAbE" class="fresco" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00014_18.jpg?itok=3zylw6Xw" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item odd"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00016_9.jpg?itok=XopybQEq" class="fresco" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00016_9.jpg?itok=debBxjIq" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item even"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00019_8.jpg?itok=bV0c6DSt" class="fresco" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00019_8.jpg?itok=k-iA3UsQ" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item odd"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00010_46.jpg?itok=siT-UQAw" class="fresco extra" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00010_46.jpg?itok=cFkW9-2g" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item even"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00009_51.jpg?itok=Xoskg2Uy" class="fresco extra" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00009_51.jpg?itok=OP_3GbTl" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item odd"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00002_58.jpg?itok=1u4ccKYf" class="fresco extra" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00002_58.jpg?itok=zN4RzbRd" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item even"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00003_66.jpg?itok=Pcm3TpqK" class="fresco extra" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00003_66.jpg?itok=W1_Xfv6j" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item odd"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00006_68.jpg?itok=qukpN33M" class="fresco extra" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00006_68.jpg?itok=13KVXozA" width="330" height="218" alt="" /></div>

    </a></div><div class="field-item even"><a href="http://www.swellnet.com/sites/default/files/styles/swellnet_large_1000x667/public/surf-reports/images/dsc00001_64.jpg?itok=9B7ZqS27" class="fresco extra" data-fresco-group="unique_name" data-fresco-group-options="thumbnails:false"><div  class="ds-1col file file-image file-image-jpeg view-mode-gallery_teaser_postcard clearfix">

  
      <img src="http://www.swellnet.com/sites/default/files/styles/swellnet_postcard_330x218/public/surf-reports/images/dsc00001_64.jpg?itok=Z2t_9633" width="330" height="218" alt="" /></div>

    </a></div></div></div></div>  </div>  </div>
      <div class="views-row views-row-2 views-row-even views-row-last">
          </div>
        </div>
  
  
  
  
  
  
    </div>
  
      </div>
    <div class="panel-pane pane-best-board-pane" >
  
      
  
      <span>Want to get more waves? The best board for today is  Modern Blackbird</span><a href="http://www.surfindustries.com/surfboards/modern_blackbird.php?utm_source=swellnet&amp;utm_medium=link&amp;utm_campaign=boardAdvisor" target="_blank"><img src='/profiles/swellnet/modules/custom/swellnet_location/swellnet_location_extras/images/boards/modern_blackbird.jpg' alt=' Modern Blackbird'/><span>More Info...<span></a>
  
      </div>
    <div class="panel-pane pane-views-panes pane-nearest-observed-surf-reports-nearest-observed-surf-reports" >
  
      
  
      <div class="view view-nearest-observed-surf-reports view-id-nearest_observed_surf_reports view-display-id-nearest_observed_surf_reports view-dom-id-a4bca69fe31aa30318dd4b9cbeb5e6d6">
                <div class="view-header">
          Nearest Observed Surf Reports:    </div>
  
  
  
          <div class="view-content">
            <div class="views-row views-row-1 views-row-odd views-row-first">
      
      <div class="views-field views-field-title">        <span class="field-content"><a href="/reports/australia/new-south-wales/eastern-beaches">Eastern Beaches</a></span>  </div>  </div>
      <div class="views-row views-row-2 views-row-even">
      
      <div class="views-field views-field-title">        <span class="field-content"><a href="/reports/australia/new-south-wales/northern-beaches">Northern Beaches</a></span>  </div>  </div>
      <div class="views-row views-row-3 views-row-odd views-row-last">
      
      <div class="views-field views-field-title">        <span class="field-content"><a href="/reports/australia/new-south-wales/wollongong">Wollongong</a></span>  </div>  </div>
        </div>
  
  
  
  
  
  
    </div>
  
      </div>
    <div class="panel-pane pane-views-panes pane-nearest-surfcams-nearest-surfcams" >
  
      
  
      <div class="view view-nearest-surfcams view-id-nearest_surfcams view-display-id-nearest_surfcams view-dom-id-d24fb10c5b0f1f1bff72cde16b77165b">
                <div class="view-header">
          Nearest Surfcams:    </div>
  
  
  
          <div class="view-content">
            <div class="views-row views-row-1 views-row-odd views-row-first">
      
      <div class="views-field views-field-static-image">        <span class="field-content"><a href="/surfcams/cronulla-point"><img src="http://static.swellnet.com.au/images/surfcams/cronulla-point-thumb.jpg" alt="Cronulla Point" title="Cronulla Point" /></a></span>  </div>  
      <div class="views-field views-field-title">        <span class="field-content"><a href="/surfcams/cronulla-point">Cronulla Point</a></span>  </div>  
      <div class="views-field views-field-field-geofield-distance">        <span class="field-content">7.12298467691027</span>  </div>  </div>
      <div class="views-row views-row-2 views-row-even">
      
      <div class="views-field views-field-static-image">        <span class="field-content"><a href="/surfcams/shark-island"><img src="http://static.swellnet.com.au/images/surfcams/shark-island-thumb.jpg" alt="Shark Island" title="Shark Island" /></a></span>  </div>  
      <div class="views-field views-field-title">        <span class="field-content"><a href="/surfcams/shark-island">Shark Island</a></span>  </div>  
      <div class="views-field views-field-field-geofield-distance">        <span class="field-content">7.12298467691027</span>  </div>  </div>
      <div class="views-row views-row-3 views-row-odd">
      
      <div class="views-field views-field-static-image">        <span class="field-content"><a href="/surfcams/cronulla-beaches"><img src="http://static.swellnet.com.au/images/surfcams/cronulla-beaches-thumb.jpg" alt="Cronulla Beaches" title="Cronulla Beaches" /></a></span>  </div>  
      <div class="views-field views-field-title">        <span class="field-content"><a href="/surfcams/cronulla-beaches">Cronulla Beaches</a></span>  </div>  
      <div class="views-field views-field-field-geofield-distance">        <span class="field-content">7.20821918577671</span>  </div>  </div>
      <div class="views-row views-row-4 views-row-even views-row-last">
      
      <div class="views-field views-field-static-image">        <span class="field-content"><a href="/surfcams/maroubra"><img src="http://static.swellnet.com.au/images/surfcams/maroubra-thumb.jpg" alt="Maroubra" title="Maroubra" /></a></span>  </div>  
      <div class="views-field views-field-title">        <span class="field-content"><a href="/surfcams/maroubra">Maroubra</a></span>  </div>  
      <div class="views-field views-field-field-geofield-distance">        <span class="field-content">14.83716042439773</span>  </div>  </div>
        </div>
  
  
  
  
  
  
    </div>
  
      </div>
    <div class="panel-pane pane-swellnet-location-report-pane" >
  
      
  
      <div class="report-summary">
      <div class="report-part report-surf">
        <label>Model Estimated Surf Size (6am)</label>
              <div class="surf">
            <span class="height">2ft</span>
            <span class="description">(Waist-Shoulder High)</span>
                  </div>
          </div>

      <div class="report-part report-fancy report-wind">
        <div class="arrow">
          <span style="font-size: 20px; left: 8px; top: 5px;  color: #ffffff; transform: rotate(210deg); -webkit-transform: rotate(210deg); -ms-transform: rotate(210deg);">A</span>
        </div>
        <div>
          <label>Live Winds (Kurnell 06:30)</label>
          <span>
                      NNE 10kt
                  </span>
        </div>
      </div>

      <div class="report-part report-trains">
        <label>Swell Trains</label>
                          <div>
              <label>Primary:</label>
              <span class="amount">1.1</span>
              <span class="unit">m</span>
              <span>@</span>
              <span class="period">9.3s</span>
              <span class="direction">ESE (119&deg;)</span>
            </div>
                            <div>
              <label>Secondary:</label>
              <span class="amount">0.8</span>
              <span class="unit">m</span>
              <span>@</span>
              <span class="period">5.1s</span>
              <span class="direction">ENE (62&deg;)</span>
            </div>
                            <div>
              <label>Tertiary:</label>
              <span class="amount">0.3</span>
              <span class="unit">m</span>
              <span>@</span>
              <span class="period">8s</span>
              <span class="direction">ENE (72&deg;)</span>
            </div>
                </div>

      <div class="report-part report-fancy report-weather">
        <img src="/profiles/swellnet/themes/swellzen/images/weather/windy.gif" alt="Windy"/>
        <div>
          <label>Weather</label>
          <span class="description">Windy</span>
        </div>
      </div>

      <div class="report-part report-sun">
        <label>Sunset / Sunrise</label>
        <div>
          <span>First light: 5:58am</span>
          <span>Sunrise: 6:24am</span>
          <span>Sunset: 7:55pm</span>
          <span>Last light: 8:21pm</span>
        </div>
      </div>

      <div class="report-part report-fancy report-temperature">
        <div class="current">
          <span>21</span>
        </div>
        <div>
          <label>Air Temperature</label>
          <span>Current: 21.9 (6:30am)</span>
          <span>Min: 20℃ - Max: 31℃</span>
        </div>
      </div>
    </div>

    <div id="wind-station-name" class="element-invisible">Kurnell</div>
    <div class="report-wind-history">
      <label>Wind History</label>
      <div>
        <svg></svg>
      </div>
    </div>

    <div id="weather-station-name" class="element-invisible">Mascot</div>
    <div class="report-tide-prediction">
      <label>Tide Prediction</label>
      <div>
        <svg></svg>
      </div>
    </div>

  
      </div>
    </div>
    <div class="layout-sixthree section panel-right">
      <div class="panel-pane pane-block pane-search-form" >
  
      
  
      <form action="/reports/australia/new-south-wales/cronulla" method="post" id="search-block-form" accept-charset="UTF-8"><div><div class="container-inline">
          <h2 class="element-invisible">Search form</h2>
        <div class="form-item form-type-textfield form-item-search-block-form">
      <label class="element-invisible" for="edit-search-block-form--2">Search </label>
     <input title="Enter the terms you wish to search for." type="text" id="edit-search-block-form--2" name="search_block_form" value="" size="15" maxlength="128" class="form-text" />
    </div>
    <div class="form-actions form-wrapper" id="edit-actions"><input type="submit" id="edit-submit" name="op" value="Go" class="form-submit" /></div><input type="hidden" name="form_build_id" value="form-LvM2oNz1MIJ_HtW0auJhWcY75vJyV2p96A60LyGV0lA" />
    <input type="hidden" name="form_id" value="search_block_form" />
    </div>
    </div></form>
  
      </div>
    <div class="panel-pane pane-block pane-boxes-swellnet-internal-subscribe-cta block-boxes-simple " >
  
      
  
      <div id='boxes-box-swellnet_internal_subscribe_cta' class='boxes-box'><div class="boxes-box-content"><a href="/signup"><img src="//www.swellnet.com/profiles/swellnet/modules/features/swellnet_blocks_and_layout/images/swellnet-pro-cta.jpg" /></a></div></div>
  
      </div>
    <div class="panel-pane pane-block pane-boxes-swellnet-adblock-mrec block-boxes-simple" >
  
      
  
      <div id='boxes-box-swellnet_adblock_mrec' class='boxes-box'><div class="boxes-box-content"><span class="mcnamf" title="300x250"></span></div></div>
  
      </div>
    <div class="panel-pane pane-block pane-boxes-swellnet-adblock-halfisland block-boxes-simple" >
  
      
  
      <div id='boxes-box-swellnet_adblock_halfisland' class='boxes-box'><div class="boxes-box-content"><span class="mcnamf" title="300x125"></span></div></div>
  
      </div>
    <div class="panel-pane pane-block pane-boxes-swellnet-facebook-like-box block-boxes-simple" >
  
      
  
      <div id='boxes-box-swellnet_facebook_like_box' class='boxes-box'><div class="boxes-box-content"><iframe src="//www.facebook.com/plugins/likebox.php?href=http%3A%2F%2Fwww.facebook.com%2Fswellnet&amp;width=300&amp;height=558&amp;show_faces=true&amp;colorscheme=light&amp;stream=true&amp;border_color&amp;header=false" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:300px; height:558px;" allowTransparency="true"></iframe></div></div>
  
      </div>
    </div>      </div> <!-- /#main -->
    
      </div>

        </div> <!-- /#page -->

      <div id="page-footer">&nbsp;</div> <!-- sticky-footer artefact -->
    </div>

    <footer id="footer" role="contentinfo">
      <div id="footer-content">
          <div id="block-menu-block-1" class="block block-menu-block first odd" role="navigation">

            <h2 class="block__title block-title">Main menu</h2>
    
      <div class="menu-block-wrapper menu-block-1 menu-name-main-menu parent-mlid-0 menu-level-1">
      <ul class="menu"><li class="menu__item is-leaf first leaf menu-mlid-3561" id="nav-home"><a href="/welcome" title="Home" class="menu__link">Home</a></li>
    <li class="menu__item is-leaf leaf menu-mlid-420" id="nav-surf-reports-and-forecasts"><a href="/reports" class="menu__link">Surf Reports &amp; Forecasts</a></li>
    <li class="menu__item is-leaf leaf menu-mlid-421" id="nav-surfcams"><a href="/surfcams" class="menu__link">Surfcams</a></li>
    <li class="menu__item is-leaf leaf menu-mlid-417" id="nav-news"><a href="/news" class="menu__link">News</a></li>
    <li class="menu__item is-leaf leaf menu-mlid-418" id="nav-photos"><a href="/photos" class="menu__link">Photos</a></li>
    <li class="menu__item is-leaf leaf menu-mlid-411" id="nav-forum"><a href="/forum" title="" class="menu__link">Forum</a></li>
    <li class="menu__item is-leaf last leaf menu-mlid-3556 new" id="nav-shop"><a href="http://shop.swellnet.com" title="The Swellnet Shop" class="menu__link">Shop</a></li>
    </ul></div>

    </div>
    <div id="block-menu-menu-miscellaneous-pages" class="block block-menu even" role="navigation">

            <h2 class="block__title block-title">Miscellaneous pages</h2>
    
      <ul class="menu"><li class="menu__item is-leaf first leaf" id="nav-privacy-policy"><a href="/privacy" class="menu__link">Privacy policy</a></li>
    <li class="menu__item is-leaf leaf" id="nav-terms-and-conditions"><a href="/terms" class="menu__link">Terms and Conditions</a></li>
    <li class="menu__item is-leaf leaf" id="nav-contact-us"><a href="/contact" title="" class="menu__link">Contact us</a></li>
    <li class="menu__item is-leaf last leaf" id="nav-about-swellnet"><a href="/about" class="menu__link">About Swellnet</a></li>
    </ul>
    </div>
    <div id="block-menu-menu-user-menu-secondary" class="block block-menu odd" role="navigation">

            <h2 class="block__title block-title">User menu secondary</h2>
    
      <ul class="menu"><li class="menu__item is-leaf first leaf" id="nav-my-account"><a href="/user" title="" class="menu__link">My account</a></li>
    <li class="menu__item is-leaf leaf" id="nav-register"><a href="/user/register" title="" class="menu__link">Register</a></li>
    <li class="menu__item is-leaf last leaf" id="nav-forgot-password"><a href="/user/password" title="" class="menu__link">Forgot password</a></li>
    </ul>
    </div>
    <div id="block-boxes-swellnet-footer-colophon" class="block block-boxes block-boxes-simple last even">

      
      <div id='boxes-box-swellnet_footer_colophon' class='boxes-box'><div class="boxes-box-content"><img src="//www.swellnet.com/profiles/swellnet/themes/swellzen/images/swellnet-logo-white.png" title="Swellnet"/>
    <p>&copy; Swellnet 2014. All rights reserved.</p></div></div>
    </div>
      </div>
    </footer>
        <script type="text/javascript">
      var _sf_async_config=Drupal.settings.chartbeat;
      (function(){
        function loadChartbeat() {
          window._sf_endpt=(new Date()).getTime();
          var e = document.createElement('script');
          e.setAttribute('language', 'javascript');
          e.setAttribute('type', 'text/javascript');
          e.setAttribute('src',
             (("https:" == document.location.protocol) ? "https://s3.amazonaws.com/" : "http://") +
             "static.chartbeat.com/js/chartbeat.js");
          document.body.appendChild(e);
        }
        var oldonload = window.onload;
        window.onload = (typeof window.onload != 'function') ?
           loadChartbeat : function() { oldonload(); loadChartbeat(); };
      })();
    </script>
      <!-- Begin MCN ad code -->
        <script type="text/javascript">
        (function(){var e=document.createElement("script"),protocol = "https:" == document.location.protocol ? "https:" : "http:";e.async=true;e.src=protocol+"//medrx.sensis.com.au/mcn/cube.min.js";var n=document.getElementsByTagName("script")[0];n.parentNode.insertBefore(e,n);})();window._mcn={};_mcn.config=[];
        _mcn.config.push(function(){
          _mcn.publisher("swellnet");
          _mcn.amf.postLoadAdCall(true);
          _mcn.amf.targeting("area", "reports/australia/new-south-wales/cronulla");
        });
      </script>
      <!-- End MCN ad code -->
      <!-- BEGIN EFFECTIVE MEASURE CODE -->
      <!-- BEGIN EFFECTIVE MEASURE CODE -->
      <!-- COPYRIGHT EFFECTIVE MEASURE -->
      <script type="text/javascript">
        //<![CDATA[
          (function() {
            var em = document.createElement('script'); em.type = 'text/javascript'; em.async = true;
            em.src = ('https:' == document.location.protocol ? 'https://au-ssl' : 'http://au-cdn') + '.effectivemeasure.net/em.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(em, s);
          })();
        //]]>
      </script>
      <noscript>
      <img alt="" src="//au.effectivemeasure.net/em_image" style="position:absolute; left:-5px;" />
      </noscript>
      <!-- END EFFECTIVE MEASURE CODE -->
      <!-- START Nielsen Online SiteCensus V6.0 -->
      <!-- COPYRIGHT 2012 Nielsen Online -->
      <script src="//secure-au.imrworldwide.com/v60.js" type="text/javascript"></script>
      <script type="text/javascript">
        //<![CDATA[
          //
            var pvar = { cid: "mcn", content: "swellnet", server: "secure-au" };  var trac = nol_t(pvar);  trac.record().post();
          //
        //]]>
      </script>
      <noscript>
      <div>
      <img alt="" height="1" src="//secure-au.imrworldwide.com/cgi-bin/m?ci=mcn&amp;cg=swellnet&amp;cc=1&amp;ts=noscript" width="1" />
      </div>
      </noscript>
      <!-- END Nielsen Online SiteCensus V6.0 -->
      <script src="/profiles/swellnet/themes/zen/../swellzen/js/lib/mustache/mustache.js"></script>
      <script src="/profiles/swellnet/themes/zen/../swellzen/js/dropdown-menu.js"></script>
      <link rel="stylesheet" href="/profiles/swellnet/themes/zen/../swellzen/css/jumboMenu.css">
    </body>
    </html>
      """
   
    //#endregion
   
    [<Fact>] 
    let ``SwellNetJobHandler parses date ok`` () =  ()
//                                                let log = (Foq.Mock<ILogWriter>()).Create()
//                                                let bus = Foq.Mock<IBus>().Create()
//                                                let cj = fixture.Create<CustomJobs.SwellNetRatingJob>()
//                                                let htmlContentWithTodaysDate = htmlContent.Replace("Sun 9 February 6:36am", DateTime.Now.ToLongDateString())
//                                                cj.Payload <- htmlContentWithTodaysDate
//                                                let handler = new CustomJobs.SwellNetRatingHandler(log,bus)
//                                                (handler:>NServiceBus.IHandleMessages<CustomJobs.SwellNetRatingJob>).Handle(cj)
//                                                Mock.Verify(<@ log.Write (LogMessage.Info(cj.ToString())) @>, Foq.Times.AtLeastOnce)
//                                                let jr = new Jobs.JobResult(cj, true, "4/10")
//                                                try
//                                                    Mock.Verify(<@ bus.Reply(jr) @>, Foq.Times.AtMostOnce)
//                                                with | ex -> printfn "%A" ex
//                                                ()
                               


