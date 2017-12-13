using XamCnblogs.Portable.Helpers;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class TabPageViewModel : ViewModelBase
    {
        ArticlesDetailsModel articlesDetailsModel;
        public ArticlesDetailsModel ArticlesDetails
        {
            get { return articlesDetailsModel; }
            set { SetProperty(ref articlesDetailsModel, value); }
        }
        public TabPageViewModel()
        {
            ArticlesDetails = new ArticlesDetailsModel()
            {
                HasContent = false
            };
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand ?? (refreshCommand = new Command(async () =>
             {
                 try
                 {
                     IsBusy = false;
                     await Task.Run(() =>
                     {
                         ArticlesDetails.Content = @"<p>Author: Hoyho Luo</p>
<p>Email: luohaihao@gmail.com</p>
<p>Source Url:<a href=""http://here2say.me/11/"" data-cke-saved-href=""http://here2say.me/11/"">http://here2say.me/11/</a></p>
<p>转载请保留此出处</p>
<p>&nbsp;</p>
<p>&nbsp; &nbsp;本文介绍基于搜狗的微信公众号定向爬虫，使用C#实现，故取名WeGouSharp。本文中的项目托管在Github上，你可以戳<a href=""https://github.com/hoyho/WeGouSharp"" target=""_blank"" data-cke-saved-href=""https://github.com/hoyho/WeGouSharp"">WeGouSharp</a>获取源码，欢迎点星。关于微信公共号爬虫的项目网上已经不少，然而基本大多数的都是使用Python实现 鉴于鄙人是名.NET开发人员，于是又为广大微软系同胞创建了这个轮子，使用C#实现的微信爬虫 蓝本为<a href=""https://github.com/Chyroc/WechatSogou"" data-cke-saved-href=""https://github.com/Chyroc/WechatSogou"">Chyroc/WechatSogou</a>， 在此还请各位大佬指教。</p>
<p>&nbsp;</p>
<address><big>目录</big></address><address><big>1.项目结构</big></address><address><big>2.数据结构</big></address><address><big>3.xpath介绍</big>&nbsp;</address><address><big>4.使用HtmlAgilityPack解析网页内容</big></address><address><big>5.验证码处理以及文件缓存</big></address><address>&nbsp;</address><address>&nbsp;</address><address>&nbsp;</address><address>&nbsp;</address>
<h2><strong>一、&nbsp;项目结构</strong></h2>
<p>如下图</p>
<p><img src=""http://images2017.cnblogs.com/blog/896762/201710/896762-20171003162219365-908555720.png"" alt=""""></p>
<p><img alt="""" data-cke-saved-src=""file:///C:UsersHoyhoDesktopstruct.png""><img src=""/media/article_images/2017/09/20/struct.png"" alt="""" data-cke-saved-src=""/media/article_images/2017/09/20/struct.png""></p>
<p>API类：</p>
<p>所有直接的操作封装好在API类中,直接使用里面的方法</p>
<div>Basic类:</div>
<div>主要处理逻辑</div>
<div>&nbsp;</div>
<div>FileCache：</div>
<div>主要出现验证码的时候需要使用Ccokie验证身份，此类可以加密后序列化保存UIN,BIZ,COOKIE等内容以供后续使用</div>
<div>&nbsp;</div>
<div>HttpHelper类：</div>
<div>网络请求，包括图片</div>
<div>&nbsp;</div>
<div>Tools类：</div>
<div>图片处理，cookie加载等</div>
<p>&nbsp;</p>
<div>依赖包可以直接使用package文件夹的版本</div>
<div>也可以自行在NuGet添加 如(visual studio--&gt;tools--&gt;Nuget Package Manager--&gt;Package Manager Console)：</div>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""21"">
<div class=""cnblogs_Highlighter"">
<pre class=""brush:bash;gutter:true;"">Install-Package HtmlAgilityPack
</pre>
</div>
<p>　　</p>
<img class=""cke_reset cke_widget_mask"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt=""""><span class=""cke_reset cke_widget_drag_handler_container""><img class=""cke_reset cke_widget_drag_handler"" title=""Click and drag to move"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""" width=""15"" height=""15"" data-cke-widget-drag-handler=""1""></span></div>
<p>&nbsp;</p>
<h2><strong>二、&nbsp;数据结构</strong></h2>
<p>本项目根据微信公账号以及搜狗搜索定义了多个结构，可以查看模型类，主要包括以下：</p>
<p>公众号结构：</p>
<div class=""cnblogs_code"">
<pre><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">struct</span><span style=""color: #000000""> OfficialAccount
    {

        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> AccountPageurl;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> WeChatId;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Name;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Introduction;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">bool</span><span style=""color: #000000""> IsAuth; 
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> QrCode;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span> ProfilePicture;<span style=""color: #008000"">//</span><span style=""color: #008000"">public string RecentArticleUrl;</span>
    } </pre>
</div>
<p><img class=""cke_reset cke_widget_mask"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt=""""><span class=""cke_reset cke_widget_drag_handler_container""><img class=""cke_reset cke_widget_drag_handler"" title=""Click and drag to move"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""" width=""15"" height=""15"" data-cke-widget-drag-handler=""1""></span></p>
<div>
<p>字段含义</p>
</div>
<table border=""1"" cellspacing=""0"">
<thead>
<tr><th style=""text-align: left"">字段</th><th>含义</th></tr>
</thead>
<tbody>
<tr>
<td>AccountPageurl</td>
<td>微信公众号页</td>
</tr>
<tr>
<td>WeChatId</td>
<td>公号ID（唯一)</td>
</tr>
<tr>
<td>Name</td>
<td>名称</td>
</tr>
<tr>
<td>Introduction</td>
<td>介绍</td>
</tr>
<tr>
<td>IsAuth</td>
<td>是否官方认证</td>
</tr>
<tr>
<td>QrCode</td>
<td>二维码链接</td>
</tr>
<tr>
<td>ProfilePicture</td>
<td>头像链接</td>
</tr>
</tbody>
</table>
<h3>&nbsp;</h3>
<div>&nbsp;</div>
<h3>公号群发消息结构(含图文推送)</h3>
<div>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""19"">
<div class=""cnblogs_code"">
<pre><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">struct</span><span style=""color: #000000""> BatchMessage
    {
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">int</span><span style=""color: #000000""> Meaasgeid;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000"">  SendDate;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span> Type; <span style=""color: #008000"">//</span><span style=""color: #008000"">49:图文，1:文字，3:图片，34:音频，62:视频public string Content; </span>

        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> ImageUrl; 

        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> PlayLength;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">int</span><span style=""color: #000000""> FileId;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> AudioSrc;

        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">for type 图文public string ContentUrl;</span>
        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">int</span><span style=""color: #000000""> Main;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Title;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Digest;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> SourceUrl;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Cover;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Author;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> CopyrightStat;

        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">for type 视频public string CdnVideoId;</span>
        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Thumb;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> VideoSrc;

        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">others</span>
    }</pre>
</div>
<p>&nbsp;</p>
<img class=""cke_reset cke_widget_drag_handler"" style=""font-family: &quot;PingFang SC&quot;, &quot;Helvetica Neue&quot;, Helvetica, Arial, sans-serif; font-size: 14px"" title=""Click and drag to move"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""" width=""15"" height=""15"" data-cke-widget-drag-handler=""1""></div>
<p>字段含义</p>
</div>
<table border=""1"" cellspacing=""0"">
<thead>
<tr><th>字段</th><th>含义</th></tr>
</thead>
<tbody>
<tr>
<td>Meaasgeid</td>
<td>消息号</td>
</tr>
<tr>
<td>SendDate</td>
<td>发出时间（unix时间戳）</td>
</tr>
<tr>
<td>Type</td>
<td>消息类型:49:图文， 1:文字， 3:图片， 34:音频， 62:视频</td>
</tr>
<tr>
<td>Content</td>
<td>文本内容（针对类型1即文字）</td>
</tr>
<tr>
<td>ImageUrl</td>
<td>图片（针对类型3，即图片）</td>
</tr>
<tr>
<td>PlayLength</td>
<td>播放长度（针对类型34，即音频，下同）</td>
</tr>
<tr>
<td>FileId</td>
<td>音频文件id</td>
</tr>
<tr>
<td>AudioSrc</td>
<td>音频源</td>
</tr>
<tr>
<td>ContentUrl</td>
<td>文章来源（针对类型49，即图文，下同）</td>
</tr>
<tr>
<td>Main</td>
<td>不明确</td>
</tr>
<tr>
<td>Title</td>
<td>文章标题</td>
</tr>
<tr>
<td>Digest</td>
<td>不明确</td>
</tr>
<tr>
<td>SourceUrl</td>
<td>可能是阅读原文</td>
</tr>
<tr>
<td>Cover</td>
<td>封面图</td>
</tr>
<tr>
<td>Author</td>
<td>作者</td>
</tr>
<tr>
<td>CopyrightStat</td>
<td>可能是否原创？</td>
</tr>
<tr>
<td>CdnVideoId</td>
<td>视频id（针对类型62，即视频，下同）</td>
</tr>
<tr>
<td>Thumb</td>
<td>视频缩略图</td>
</tr>
<tr>
<td>VideoSrc</td>
<td>视频链接</td>
</tr>
</tbody>
</table>
<h3>&nbsp;</h3>
<h3>&nbsp;</h3>
<h3>文章结构</h3>
<div>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""18"">
<div class=""cnblogs_code"">
<pre>  <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">struct</span><span style=""color: #000000""> Article
    {
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Url;
        </span><span style=""color: #0000ff"">public</span> List&lt;<span style=""color: #0000ff"">string</span>&gt;<span style=""color: #000000"">Imgs;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Title;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Brief;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Time;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> ArticleListUrl;
        </span><span style=""color: #0000ff"">public</span><span style=""color: #000000""> OfficialAccount officialAccount;
    }</span></pre>
</div>
<p>&nbsp;</p>
<img class=""cke_reset cke_widget_drag_handler"" style=""font-family: &quot;PingFang SC&quot;, &quot;Helvetica Neue&quot;, Helvetica, Arial, sans-serif; font-size: 14px"" title=""Click and drag to move"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""" width=""15"" height=""15"" data-cke-widget-drag-handler=""1""></div>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""18"">
<div>
<pre>字段含义</pre>
</div>
<table border=""1"" cellspacing=""0"">
<thead>
<tr><th>字段</th><th>含义</th></tr>
</thead>
<tbody>
<tr>
<td>Url</td>
<td>文章链接</td>
</tr>
<tr>
<td>Imgs</td>
<td>封面图（可能多个）</td>
</tr>
<tr>
<td>Title</td>
<td>文章标题</td>
</tr>
<tr>
<td>Brief</td>
<td>简介</td>
</tr>
<tr>
<td>Time</td>
<td>发表日期（unix时间戳）</td>
</tr>
<tr>
<td>OfficialAccount</td>
<td>关联的公众号（信息不全，仅供参考）</td>
</tr>
</tbody>
</table>
</div>
<pre></pre>
<h3>&nbsp;</h3>
<h3>搜索榜结构</h3>
<div class=""cnblogs_code"">
<pre><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">struct</span><span style=""color: #000000""> HotWord
    {
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">int</span> Rank;<span style=""color: #008000"">//</span><span style=""color: #008000"">排行</span>
        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span><span style=""color: #000000""> Word;
        </span><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span> JumpLink; <span style=""color: #008000"">//</span><span style=""color: #008000"">相关链接</span>
        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">int</span> HotDegree; <span style=""color: #008000"">//</span><span style=""color: #008000"">热度</span>
    }</pre>
</div>
<p>&nbsp;</p>
<p>&nbsp;</p>
</div>
<p>&nbsp;</p>
<h2><strong>三 、xpath介绍</strong></h2>
<h3>&nbsp;什么是 XPath?</h3>
<ul>
<li>XPath 使用路径表达式在 XML 文档中进行导航</li>
<li>XPath 包含一个标准函数库</li>
<li>XPath 是 XSLT 中的主要元素</li>
<li>XPath 是一个 W3C 标准</li>
</ul>
<p>简而言之，Xpath是XML元素的位置,下面是W3C教程时间，老鸟直接跳过</p>
<div>
<h2>&nbsp;</h2>
<div>
<h3>XML 实例文档</h3>
<p>我们将在下面的例子中使用这个 XML 文档。</p>
<div class=""cnblogs_code"">
<pre><span style=""color: #0000ff"">&lt;?</span><span style=""color: #ff00ff"">xml version=""1.0"" encoding=""ISO-8859-1""</span><span style=""color: #0000ff"">?&gt;</span>

<span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">bookstore</span><span style=""color: #0000ff"">&gt;</span>

<span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">book</span><span style=""color: #0000ff"">&gt;</span>
  <span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">title </span><span style=""color: #ff0000"">lang</span><span style=""color: #0000ff"">=""eng""</span><span style=""color: #0000ff"">&gt;</span>Harry Potter<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">title</span><span style=""color: #0000ff"">&gt;</span>
  <span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">price</span><span style=""color: #0000ff"">&gt;</span>29.99<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">price</span><span style=""color: #0000ff"">&gt;</span>
<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">book</span><span style=""color: #0000ff"">&gt;</span>

<span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">book</span><span style=""color: #0000ff"">&gt;</span>
  <span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">title </span><span style=""color: #ff0000"">lang</span><span style=""color: #0000ff"">=""eng""</span><span style=""color: #0000ff"">&gt;</span>Learning XML<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">title</span><span style=""color: #0000ff"">&gt;</span>
  <span style=""color: #0000ff"">&lt;</span><span style=""color: #800000"">price</span><span style=""color: #0000ff"">&gt;</span>39.95<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">price</span><span style=""color: #0000ff"">&gt;</span>
<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">book</span><span style=""color: #0000ff"">&gt;</span>

<span style=""color: #0000ff"">&lt;/</span><span style=""color: #800000"">bookstore</span><span style=""color: #0000ff"">&gt;</span>
 </pre>
</div>
<p>&nbsp;</p>
</div>
<div>
<h3>选取节点</h3>
<p>XPath 使用路径表达式在 XML 文档中选取节点。节点是通过沿着路径或者 step 来选取的。</p>
<h3>下面列出了最有用的路径表达式：</h3>
<table class=""dataintable"" border=""1"" cellspacing=""0"">
<tbody>
<tr><th>表达式</th><th>描述</th></tr>
<tr>
<td>nodename</td>
<td>选取此节点的所有子节点。</td>
</tr>
<tr>
<td>/</td>
<td>从根节点选取。</td>
</tr>
<tr>
<td>//</td>
<td>从匹配选择的当前节点选择文档中的节点，而不考虑它们的位置。</td>
</tr>
<tr>
<td>.</td>
<td>选取当前节点。</td>
</tr>
<tr>
<td>..</td>
<td>选取当前节点的父节点。</td>
</tr>
<tr>
<td>@</td>
<td>选取属性。</td>
</tr>
</tbody>
</table>
<h3>实例</h3>
<p>在下面的表格中，我们已列出了一些路径表达式以及表达式的结果：</p>
<table class=""dataintable"" border=""1"" cellspacing=""0"">
<tbody>
<tr><th>路径表达式</th><th>结果</th></tr>
<tr>
<td>bookstore</td>
<td>选取 bookstore 元素的所有子节点。</td>
</tr>
<tr>
<td>/bookstore</td>
<td>
<p>选取根元素 bookstore。</p>
<p>注释：假如路径起始于正斜杠( / )，则此路径始终代表到某元素的绝对路径！</p>
</td>
</tr>
<tr>
<td>bookstore/book</td>
<td>选取属于 bookstore 的子元素的所有 book 元素。</td>
</tr>
<tr>
<td>//book</td>
<td>选取所有 book 子元素，而不管它们在文档中的位置。</td>
</tr>
<tr>
<td>bookstore//book</td>
<td>选择属于 bookstore 元素的后代的所有 book 元素，而不管它们位于 bookstore 之下的什么位置。</td>
</tr>
<tr>
<td>//@lang</td>
<td>选取名为 lang 的所有属性。</td>
</tr>
</tbody>
</table>
</div>
<div>
<h3>谓语（Predicates）</h3>
<p>谓语用来查找某个特定的节点或者包含某个指定的值的节点。</p>
<p>谓语被嵌在方括号中。</p>
<h3>实例</h3>
<p>在下面的表格中，我们列出了带有谓语的一些路径表达式，以及表达式的结果：</p>
<table class=""dataintable"" border=""1"" cellspacing=""0"">
<tbody>
<tr><th>路径表达式</th><th>结果</th></tr>
<tr>
<td>/bookstore/book[1]</td>
<td>选取属于 bookstore 子元素的第一个 book 元素。</td>
</tr>
<tr>
<td>/bookstore/book[last()]</td>
<td>选取属于 bookstore 子元素的最后一个 book 元素。</td>
</tr>
<tr>
<td>/bookstore/book[last()-1]</td>
<td>选取属于 bookstore 子元素的倒数第二个 book 元素。</td>
</tr>
<tr>
<td>/bookstore/book[position()&lt;3]</td>
<td>选取最前面的两个属于 bookstore 元素的子元素的 book 元素。</td>
</tr>
<tr>
<td>//title[@lang]</td>
<td>选取所有拥有名为 lang 的属性的 title 元素。</td>
</tr>
<tr>
<td>//title[@lang='eng']</td>
<td>选取所有 title 元素，且这些元素拥有值为 eng 的 lang 属性。</td>
</tr>
<tr>
<td>/bookstore/book[price&gt;35.00]</td>
<td>选取 bookstore 元素的所有 book 元素，且其中的 price 元素的值须大于 35.00。</td>
</tr>
<tr>
<td>/bookstore/book[price&gt;35.00]/title</td>
<td>选取 bookstore 元素中的 book 元素的所有 title 元素，且其中的 price 元素的值须大于 35.00。</td>
</tr>
</tbody>
</table>
</div>
<div>
<h3>选取未知节点</h3>
<p>XPath 通配符可用来选取未知的 XML 元素。</p>
<table class=""dataintable"" border=""1"" cellspacing=""0"">
<tbody>
<tr><th>通配符</th><th>描述</th></tr>
<tr>
<td>*</td>
<td>匹配任何元素节点。</td>
</tr>
<tr>
<td>@*</td>
<td>匹配任何属性节点。</td>
</tr>
<tr>
<td>node()</td>
<td>匹配任何类型的节点。</td>
</tr>
</tbody>
</table>
<h3>实例</h3>
<p>在下面的表格中，我们列出了一些路径表达式，以及这些表达式的结果：</p>
<table class=""dataintable"" border=""1"" cellspacing=""0"">
<tbody>
<tr><th>路径表达式</th><th>结果</th></tr>
<tr>
<td>/bookstore/*</td>
<td>选取 bookstore 元素的所有子元素。</td>
</tr>
<tr>
<td>//*</td>
<td>选取文档中的所有元素。</td>
</tr>
<tr>
<td>//title[@*]</td>
<td>选取所有带有属性的 title 元素。
<div><small>来源：&nbsp;<a href=""http://www.w3school.com.cn/xpath/xpath_syntax.asp"">http://www.w3school.com.cn/xpath/xpath_syntax.asp</a></small></div>
</td>
</tr>
</tbody>
</table>
</div>
<p>&nbsp;</p>
<h2><span><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-attribute""><span class=""hljs-value""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-attribute""><span class=""hljs-value""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title""><span class=""hljs-tag""><span class=""hljs-title"">&nbsp;</span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></h2>
</div>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>如图，假设我要抓取首页一个banner图，可以在chrome按下F12参考该元素的Xpath，</p>
<p><img src=""http://images2017.cnblogs.com/blog/896762/201710/896762-20171003162311927-673828729.jpg"" alt=""""></p>
<p>&nbsp;</p>
<p><img src=""/media/article_images/2017/09/20/select_xpath.jpg"" alt="""" data-cke-saved-src=""/media/article_images/2017/09/20/select_xpath.jpg""></p>
<p>即该图片对应的Xpth为: //*[@id=""loginWrap""]/div[4]/div[1]/div[1]/div/a[4]/img</p>
<div>解读：该图片位于ID=&nbsp;loginWrap下面的第4个div下的...的img标签内</div>
<p>&nbsp;</p>
<div><big>为什么这里介绍Xpath，是因为我们网页分析是使用HtmlAgilityPack来解析,</big><big>他可以把根据Xpath解析我们所需的元素。</big></div>
<div><big>比如我们调试确定一个文章页面的特定位置为标题，图片，作者，内容，链接的Xpath即可完全批量化且准确地解析以上信息</big></div>
<div>&nbsp;</div>
<h2>&nbsp;</h2>
<h2><strong>四、 使用HtmlAgilityPack解析网页内容</strong></h2>
<p>HttpTool类里封装了一个较多参数的HTTP GET操作,用于获取搜狗的页面：</p>
<p>因为搜狗本身是做搜索引擎的缘故，所以反爬虫是非常严厉的，因此HTTP GET的方法要注意携带很多参数，且不同页面要求不一样.一般地，要带上默认的</p>
<p>referer和host&nbsp;然后请求头的UserAgent&nbsp;要伪造，常用的useragent有</p>
<div class=""cnblogs_code"">
<pre><span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">static</span> List&lt;<span style=""color: #0000ff"">string</span>&gt; _agent = <span style=""color: #0000ff"">new</span> List&lt;<span style=""color: #0000ff"">string</span>&gt;<span style=""color: #000000"">
{
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; AcooBrowser; .NET CLR 1.1.4322; .NET CLR 2.0.50727)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Acoo Browser; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.0.04506)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/4.0 (compatible; MSIE 7.0; AOL 9.5; AOLBuild 4337.35; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 2.0.50727; Media Center PC 6.0)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.0.04506.30)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/523.15 (KHTML, like Gecko, Safari/419.3) Arora/0.3 (Change: 287 c9dfb30)</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (X11; U; Linux; en-US) AppleWebKit/527+ (KHTML, like Gecko, Safari/419.3) Arora/0.6</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.2pre) Gecko/20070215 K-Ninja/2.1.1</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9) Gecko/20080705 Firefox/3.0 Kapiko/3.0</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (X11; Linux i686; U;) Gecko/20070322 Kazehakase/0.4.5</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.8) Gecko Fedora/1.9.0.8-1.fc10 Kazehakase/0.5.6</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.20 (KHTML, like Gecko) Chrome/19.0.1036.7 Safari/535.20</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; fr) Presto/2.9.168 Version/11.52</span><span style=""color: #800000"">""</span><span style=""color: #000000"">,
};</span></pre>
</div>
<p>&nbsp;</p>
<pre>&nbsp;</pre>
<p>&nbsp;</p>
<p>自定义的GET 方法</p>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""14""><img class=""cke_reset cke_widget_drag_handler"" style=""font-family: &quot;PingFang SC&quot;, &quot;Helvetica Neue&quot;, Helvetica, Arial, sans-serif; font-size: 14px"" title=""Click and drag to move"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""" width=""15"" height=""15"" data-cke-widget-drag-handler=""1"">
<div class=""cnblogs_code"">
<pre>  <span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;summary&gt;</span>
        <span style=""color: #808080"">///</span><span style=""color: #008000""> 指定header参数的HTTP Get方法
        </span><span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;/summary&gt;</span>
        <span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;param name=""headers""&gt;&lt;/param&gt;</span>
        <span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;param name=""url""&gt;&lt;/param&gt;</span>
        <span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;returns&gt;</span><span style=""color: #008000"">respondse</span><span style=""color: #808080"">&lt;/returns&gt;</span>
        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">string</span> Get(WebHeaderCollection headers, <span style=""color: #0000ff"">string</span> url ,<span style=""color: #0000ff"">string</span> responseEncoding=<span style=""color: #800000"">""</span><span style=""color: #800000"">UTF-8</span><span style=""color: #800000"">""</span>,<span style=""color: #0000ff"">bool</span> isUseCookie = <span style=""color: #0000ff"">false</span><span style=""color: #000000"">)
        {
            </span><span style=""color: #0000ff"">string</span> responseText = <span style=""color: #800000"">""""</span><span style=""color: #000000"">;
            </span><span style=""color: #0000ff"">try</span><span style=""color: #000000"">
            {
                </span><span style=""color: #0000ff"">var</span> request =<span style=""color: #000000""> (HttpWebRequest)WebRequest.Create(url);
                    request.Method </span>= <span style=""color: #800000"">""</span><span style=""color: #800000"">GET</span><span style=""color: #800000"">""</span><span style=""color: #000000"">;
                </span><span style=""color: #0000ff"">foreach</span> (<span style=""color: #0000ff"">string</span> key <span style=""color: #0000ff"">in</span><span style=""color: #000000""> headers.Keys)
                {
                    </span><span style=""color: #0000ff"">switch</span><span style=""color: #000000""> (key.ToLower())
                    {
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">user-agent</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            request.UserAgent </span>=<span style=""color: #000000""> headers[key];
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">referer</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            request.Referer </span>=<span style=""color: #000000""> headers[key];
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">host</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            request.Host </span>=<span style=""color: #000000""> headers[key];
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">contenttype</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            request.ContentType </span>=<span style=""color: #000000""> headers[key];
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">accept</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            request.Accept </span>=<span style=""color: #000000""> headers[key];
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">default</span><span style=""color: #000000"">:
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                    }
               }
                </span><span style=""color: #0000ff"">if</span> (<span style=""color: #0000ff"">string</span><span style=""color: #000000"">.IsNullOrEmpty(request.Referer))
                {
                    request.Referer </span>= <span style=""color: #800000"">""</span><span style=""color: #800000"">http://weixin.sogou.com/</span><span style=""color: #800000"">""</span><span style=""color: #000000"">;
                };
                </span><span style=""color: #0000ff"">if</span> (<span style=""color: #0000ff"">string</span><span style=""color: #000000"">.IsNullOrEmpty(request.Host))
                {
                    request.Host </span>= <span style=""color: #800000"">""</span><span style=""color: #800000"">weixin.sogou.com</span><span style=""color: #800000"">""</span><span style=""color: #000000"">;
                };
                </span><span style=""color: #0000ff"">if</span> (<span style=""color: #0000ff"">string</span><span style=""color: #000000"">.IsNullOrEmpty(request.UserAgent))
                {
                    Random r </span>= <span style=""color: #0000ff"">new</span><span style=""color: #000000""> Random();
                    </span><span style=""color: #0000ff"">int</span> index = r.Next(WechatSogouBasic._agent.Count - <span style=""color: #800080"">1</span><span style=""color: #000000"">);
                    request.UserAgent </span>=<span style=""color: #000000""> WechatSogouBasic._agent[index];
                }
                </span><span style=""color: #0000ff"">if</span><span style=""color: #000000""> (isUseCookie)
                {
                    CookieCollection cc </span>=<span style=""color: #000000""> Tools.LoadCookieFromCache();
                    request.CookieContainer </span>= <span style=""color: #0000ff"">new</span><span style=""color: #000000""> CookieContainer();
                    request.CookieContainer.Add(cc);
                }
                HttpWebResponse response </span>=<span style=""color: #000000""> (HttpWebResponse)request.GetResponse();
                </span><span style=""color: #0000ff"">if</span> (isUseCookie &amp;&amp; response.Cookies.Count &gt;<span style=""color: #800080"">0</span><span style=""color: #000000"">)
                {
                    </span><span style=""color: #0000ff"">var</span> cookieCollection =<span style=""color: #000000""> response.Cookies;
                    WechatCache cache </span>= <span style=""color: #0000ff"">new</span> WechatCache(Config.CacheDir, <span style=""color: #800080"">3000</span><span style=""color: #000000"">);
                    </span><span style=""color: #0000ff"">if</span> (!cache.Add(<span style=""color: #800000"">""</span><span style=""color: #800000"">cookieCollection</span><span style=""color: #800000"">""</span>, cookieCollection, <span style=""color: #800080"">3000</span>)) { cache.Update(<span style=""color: #800000"">""</span><span style=""color: #800000"">cookieCollection</span><span style=""color: #800000"">""</span>, cookieCollection, <span style=""color: #800080"">3000</span><span style=""color: #000000"">); };
                }
                </span><span style=""color: #008000"">//</span><span style=""color: #008000""> Get the stream containing content returned by the server.</span>
                Stream dataStream =<span style=""color: #000000""> response.GetResponseStream();
                </span><span style=""color: #008000"">//</span><span style=""color: #008000"">如果response是图片，则返回以base64方式返回图片内容，否则返回html内容</span>
                <span style=""color: #0000ff"">if</span> (response.Headers.Get(<span style=""color: #800000"">""</span><span style=""color: #800000"">Content-Type</span><span style=""color: #800000"">""</span>) == <span style=""color: #800000"">""</span><span style=""color: #800000"">image/jpeg</span><span style=""color: #800000"">""</span> || response.Headers.Get(<span style=""color: #800000"">""</span><span style=""color: #800000"">Content-Type</span><span style=""color: #800000"">""</span>) == <span style=""color: #800000"">""</span><span style=""color: #800000"">image/jpg</span><span style=""color: #800000"">""</span><span style=""color: #000000"">)
                {
                    Image img </span>= Image.FromStream(dataStream, <span style=""color: #0000ff"">true</span><span style=""color: #000000"">);
                    </span><span style=""color: #0000ff"">using</span> (MemoryStream ms = <span style=""color: #0000ff"">new</span><span style=""color: #000000""> MemoryStream())
                    {
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000""> Convert Image to byte[]
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">img.Save(""myfile.jpg"");</span>
<span style=""color: #000000"">                        img.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
                        </span><span style=""color: #0000ff"">byte</span>[] imageBytes =<span style=""color: #000000""> ms.ToArray();
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000""> Convert byte[] to Base64 String</span>
                        <span style=""color: #0000ff"">string</span> base64String =<span style=""color: #000000""> Convert.ToBase64String(imageBytes);
                        responseText </span>=<span style=""color: #000000""> base64String;
                    }
                }
                </span><span style=""color: #0000ff"">else</span> <span style=""color: #008000"">//</span><span style=""color: #008000"">read response string</span>
<span style=""color: #000000"">                {
                    </span><span style=""color: #008000"">//</span><span style=""color: #008000""> Open the stream using a StreamReader for easy access.</span>
<span style=""color: #000000"">                    Encoding encoding;
                    </span><span style=""color: #0000ff"">switch</span><span style=""color: #000000""> (responseEncoding.ToLower())
                    {
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">utf-8</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            encoding </span>=<span style=""color: #000000""> Encoding.UTF8;
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">unicode</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            encoding </span>=<span style=""color: #000000""> Encoding.Unicode;
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">case</span> <span style=""color: #800000"">""</span><span style=""color: #800000"">ascii</span><span style=""color: #800000"">""</span><span style=""color: #000000"">:
                            encoding </span>=<span style=""color: #000000""> Encoding.ASCII;
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                        </span><span style=""color: #0000ff"">default</span><span style=""color: #000000"">:
                            encoding </span>=<span style=""color: #000000""> Encoding.Default;
                            </span><span style=""color: #0000ff"">break</span><span style=""color: #000000"">;
                               
                    }
                    StreamReader reader </span>= <span style=""color: #0000ff"">new</span> StreamReader(dataStream, encoding);<span style=""color: #008000"">//</span><span style=""color: #008000"">System.Text.Encoding.Default
                    </span><span style=""color: #008000"">//</span><span style=""color: #008000""> Read the content.</span>
                    <span style=""color: #0000ff"">if</span> (response.StatusCode ==<span style=""color: #000000""> HttpStatusCode.OK)
                    {
                        responseText </span>=<span style=""color: #000000""> reader.ReadToEnd();
                        </span><span style=""color: #0000ff"">if</span> (responseText.Contains(<span style=""color: #800000"">""</span><span style=""color: #800000"">用户您好，您的访问过于频繁，为确认本次访问为正常用户行为，需要您协助验证</span><span style=""color: #800000"">""</span><span style=""color: #000000"">))
                        {
                            _vcode_url </span>=<span style=""color: #000000""> url;
                            </span><span style=""color: #0000ff"">throw</span> <span style=""color: #0000ff"">new</span> Exception(<span style=""color: #800000"">""</span><span style=""color: #800000"">weixin.sogou.com verification code</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
                        }
                    }
                    </span><span style=""color: #0000ff"">else</span><span style=""color: #000000"">
                    {
                        logger.Error(</span><span style=""color: #800000"">""</span><span style=""color: #800000"">requests status_code error</span><span style=""color: #800000"">""</span> +<span style=""color: #000000""> response.StatusCode);
                        </span><span style=""color: #0000ff"">throw</span> <span style=""color: #0000ff"">new</span> Exception(<span style=""color: #800000"">""</span><span style=""color: #800000"">requests status_code error</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
                    }
                    reader.Close();
                }

                dataStream.Close();
                response.Close();
            }
            </span><span style=""color: #0000ff"">catch</span><span style=""color: #000000""> (Exception e)
            {
                logger.Error(e);
            }
            </span><span style=""color: #0000ff"">return</span><span style=""color: #000000""> responseText;
        }</span></pre>
</div>
<p>&nbsp;</p>
</div>
<p>&nbsp;</p>
<p>前面关于Xpath废话太多，直接上一个案例，解析公众号页面：</p>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""13"">
<div class=""cnblogs_code"">
<pre><span style=""color: #0000ff"">public</span> List&lt;OfficialAccount&gt; SearchOfficialAccount(<span style=""color: #0000ff"">string</span> keyword, <span style=""color: #0000ff"">int</span> page = <span style=""color: #800080"">1</span><span style=""color: #000000"">)
        {
            List</span>&lt;OfficialAccount&gt; accountList = <span style=""color: #0000ff"">new</span> List&lt;OfficialAccount&gt;<span style=""color: #000000"">();
            </span><span style=""color: #0000ff"">string</span> text = <span style=""color: #0000ff"">this</span>._SearchAccount_Html(keyword, page);<span style=""color: #008000"">//</span><span style=""color: #008000"">返回了一个搜索页面的html代码</span>
            HtmlDocument pageDoc = <span style=""color: #0000ff"">new</span><span style=""color: #000000""> HtmlDocument();
            pageDoc.LoadHtml(text);
            HtmlNodeCollection targetArea </span>= pageDoc.DocumentNode.SelectNodes(<span style=""color: #800000"">""</span><span style=""color: #800000"">//ul[@class='news-list2']/li</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
            </span><span style=""color: #0000ff"">if</span> (targetArea != <span style=""color: #0000ff"">null</span><span style=""color: #000000"">)
            {
                </span><span style=""color: #0000ff"">foreach</span> (HtmlNode node <span style=""color: #0000ff"">in</span><span style=""color: #000000""> targetArea)
                {
                    </span><span style=""color: #0000ff"">try</span><span style=""color: #000000"">
                    {
                        OfficialAccount accountInfo </span>= <span style=""color: #0000ff"">new</span><span style=""color: #000000""> OfficialAccount();
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">链接中包含了&amp;amp; html编码符，要用htmdecode，不是urldecode</span>
                        accountInfo.AccountPageurl = WebUtility.HtmlDecode(node.SelectSingleNode(<span style=""color: #800000"">""</span><span style=""color: #800000"">div/div[@class='img-box']/a</span><span style=""color: #800000"">""</span>).GetAttributeValue(<span style=""color: #800000"">""</span><span style=""color: #800000"">href</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""""</span><span style=""color: #000000"">));
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">accountInfo.ProfilePicture = node.SelectSingleNode(""div/div[1]/a/img"").InnerHtml;</span>
                        accountInfo.ProfilePicture = WebUtility.HtmlDecode(node.SelectSingleNode(<span style=""color: #800000"">""</span><span style=""color: #800000"">div/div[@class='img-box']/a/img</span><span style=""color: #800000"">""</span>).GetAttributeValue(<span style=""color: #800000"">""</span><span style=""color: #800000"">src</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""""</span><span style=""color: #000000"">));
                        accountInfo.Name </span>= node.SelectSingleNode(<span style=""color: #800000"">""</span><span style=""color: #800000"">div/div[2]/p[1]</span><span style=""color: #800000"">""</span>).InnerText.Trim().Replace(<span style=""color: #800000"">""</span><span style=""color: #800000"">&lt;!--red_beg--&gt;</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""""</span>).Replace(<span style=""color: #800000"">""</span><span style=""color: #800000"">&lt;!--red_end--&gt;</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""""</span><span style=""color: #000000"">);
                        accountInfo.WeChatId </span>= node.SelectSingleNode(<span style=""color: #800000"">""</span><span style=""color: #800000"">div/div[2]/p[2]/label</span><span style=""color: #800000"">""</span><span style=""color: #000000"">).InnerText.Trim();
                        accountInfo.QrCode </span>= WebUtility.HtmlDecode(node.SelectSingleNode(<span style=""color: #800000"">""</span><span style=""color: #800000"">div/div[3]/span/img</span><span style=""color: #800000"">""</span>).GetAttributeValue(<span style=""color: #800000"">""</span><span style=""color: #800000"">src</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""""</span><span style=""color: #000000"">));
                        accountInfo.Introduction </span>= node.SelectSingleNode(<span style=""color: #800000"">""</span><span style=""color: #800000"">dl[1]/dd</span><span style=""color: #800000"">""</span>).InnerText.Trim().Replace(<span style=""color: #800000"">""</span><span style=""color: #800000"">&lt;!--red_beg--&gt;</span><span style=""color: #800000"">""</span>,<span style=""color: #800000"">""""</span>).Replace(<span style=""color: #800000"">""</span><span style=""color: #800000"">&lt;!--red_end--&gt;</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""""</span><span style=""color: #000000"">);
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">早期的账号认证和后期的认证显示不一样？，对比 bitsea 和 NUAA_1952 两个账号
                        </span><span style=""color: #008000"">//</span><span style=""color: #008000"">现在改为包含该script的即认证了</span>
                        <span style=""color: #0000ff"">if</span> (node.InnerText.Contains(<span style=""color: #800000"">""</span><span style=""color: #800000"">document.write(authname('2'))</span><span style=""color: #800000"">""</span><span style=""color: #000000"">))
                        {
                            accountInfo.IsAuth </span>= <span style=""color: #0000ff"">true</span><span style=""color: #000000"">;
                        }
                        </span><span style=""color: #0000ff"">else</span><span style=""color: #000000"">
                        {
                            accountInfo.IsAuth </span>= <span style=""color: #0000ff"">false</span><span style=""color: #000000"">;
                        }
                        accountList.Add(accountInfo);
                    }
                    </span><span style=""color: #0000ff"">catch</span><span style=""color: #000000""> (Exception e)
                    {
                        logger.Warn(e);
                    }
                }
            }
            
          
            </span><span style=""color: #0000ff"">return</span><span style=""color: #000000""> accountList; 
        }</span></pre>
</div>
<p>&nbsp;</p>
<pre class=""cke_widget_element"" data-cke-widget-data=""%7B%22lang%22%3A%22cs%22%2C%22code%22%3A%22%20public%20List%3COfficialAccount%3E%20SearchOfficialAccount(string%20keyword%2C%20int%20page%20%3D%201)%5Cn%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20List%3COfficialAccount%3E%20accountList%20%3D%20new%20List%3COfficialAccount%3E()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20string%20text%20%3D%20this._SearchAccount_Html(keyword%2C%20page)%3B%2F%2F%E8%BF%94%E5%9B%9E%E4%BA%86%E4%B8%80%E4%B8%AA%E6%90%9C%E7%B4%A2%E9%A1%B5%E9%9D%A2%E7%9A%84html%E4%BB%A3%E7%A0%81%5Cn%20%20%20%20%20%20%20%20%20%20%20%20HtmlDocument%20pageDoc%20%3D%20new%20HtmlDocument()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20pageDoc.LoadHtml(text)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20HtmlNodeCollection%20targetArea%20%3D%20pageDoc.DocumentNode.SelectNodes(%5C%22%2F%2Ful%5B%40class%3D'news-list2'%5D%2Fli%5C%22)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20if%20(targetArea%20!%3D%20null)%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20foreach%20(HtmlNode%20node%20in%20targetArea)%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20try%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20OfficialAccount%20accountInfo%20%3D%20new%20OfficialAccount()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2F%E9%93%BE%E6%8E%A5%E4%B8%AD%E5%8C%85%E5%90%AB%E4%BA%86%26amp%3B%20html%E7%BC%96%E7%A0%81%E7%AC%A6%EF%BC%8C%E8%A6%81%E7%94%A8htmdecode%EF%BC%8C%E4%B8%8D%E6%98%AFurldecode%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.AccountPageurl%20%3D%20WebUtility.HtmlDecode(node.SelectSingleNode(%5C%22div%2Fdiv%5B%40class%3D'img-box'%5D%2Fa%5C%22).GetAttributeValue(%5C%22href%5C%22%2C%20%5C%22%5C%22))%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2FaccountInfo.ProfilePicture%20%3D%20node.SelectSingleNode(%5C%22div%2Fdiv%5B1%5D%2Fa%2Fimg%5C%22).InnerHtml%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.ProfilePicture%20%3D%20WebUtility.HtmlDecode(node.SelectSingleNode(%5C%22div%2Fdiv%5B%40class%3D'img-box'%5D%2Fa%2Fimg%5C%22).GetAttributeValue(%5C%22src%5C%22%2C%20%5C%22%5C%22))%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.Name%20%3D%20node.SelectSingleNode(%5C%22div%2Fdiv%5B2%5D%2Fp%5B1%5D%5C%22).InnerText.Trim().Replace(%5C%22%3C!--red_beg--%3E%5C%22%2C%20%5C%22%5C%22).Replace(%5C%22%3C!--red_end--%3E%5C%22%2C%20%5C%22%5C%22)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.WeChatId%20%3D%20node.SelectSingleNode(%5C%22div%2Fdiv%5B2%5D%2Fp%5B2%5D%2Flabel%5C%22).InnerText.Trim()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.QrCode%20%3D%20WebUtility.HtmlDecode(node.SelectSingleNode(%5C%22div%2Fdiv%5B3%5D%2Fspan%2Fimg%5C%22).GetAttributeValue(%5C%22src%5C%22%2C%20%5C%22%5C%22))%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.Introduction%20%3D%20node.SelectSingleNode(%5C%22dl%5B1%5D%2Fdd%5C%22).InnerText.Trim().Replace(%5C%22%3C!--red_beg--%3E%5C%22%2C%5C%22%5C%22).Replace(%5C%22%3C!--red_end--%3E%5C%22%2C%20%5C%22%5C%22)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2F%E6%97%A9%E6%9C%9F%E7%9A%84%E8%B4%A6%E5%8F%B7%E8%AE%A4%E8%AF%81%E5%92%8C%E5%90%8E%E6%9C%9F%E7%9A%84%E8%AE%A4%E8%AF%81%E6%98%BE%E7%A4%BA%E4%B8%8D%E4%B8%80%E6%A0%B7%EF%BC%9F%EF%BC%8C%E5%AF%B9%E6%AF%94%20bitsea%20%E5%92%8C%20NUAA_1952%20%E4%B8%A4%E4%B8%AA%E8%B4%A6%E5%8F%B7%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2F%E7%8E%B0%E5%9C%A8%E6%94%B9%E4%B8%BA%E5%8C%85%E5%90%AB%E8%AF%A5script%E7%9A%84%E5%8D%B3%E8%AE%A4%E8%AF%81%E4%BA%86%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20if%20(node.InnerText.Contains(%5C%22document.write(authname('2'))%5C%22))%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.IsAuth%20%3D%20true%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20else%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.IsAuth%20%3D%20false%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountList.Add(accountInfo)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20catch%20(Exception%20e)%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20logger.Warn(e)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%5Cn%20%20%20%20%20%20%20%20%20%20%5Cn%20%20%20%20%20%20%20%20%20%20%20%20return%20accountList%3B%20%5Cn%20%20%20%20%20%20%20%20%7D%22%2C%22classes%22%3Anull%7D"" data-cke-widget-upcasted=""1"" data-cke-widget-keep-attr=""0"" data-widget=""codeSnippet""><code class=""language-cs hljs""> <br></code></pre>
<div>以上，说白了，解析就是Xpath调试，关键是看目标内容是是元素标签内容，还是标签属性，</div>
<div>如果是标签内容即形式为 &lt;h&gt;<span style=""background-color: #ffffff; color: #ff0000"">我是内容</span>&lt;/h&gt;</div>
<div>则： node.SelectSingleNode(""<span style=""color: #ff0000"">div/div[2]/p[2]/label</span>"").InnerText.Trim();</div>
<div>&nbsp;</div>
<div>如果要提取的目标内容是标签属性，如&nbsp;&lt;a&nbsp;<span style=""color: #ff0000"">href=""/im_target_url.htm""&nbsp;</span>&gt;点击链接&lt;/a&gt;</div>
<div>则node.SelectSingleNode(""<span style=""color: #ff0000"">div/div[@class='img-box']/a</span>"").GetAttributeValue(""href"",&nbsp;"""")</div>
<div>&nbsp;</div>
<pre class=""cke_widget_element"" data-cke-widget-data=""%7B%22lang%22%3A%22cs%22%2C%22code%22%3A%22%20public%20List%3COfficialAccount%3E%20SearchOfficialAccount(string%20keyword%2C%20int%20page%20%3D%201)%5Cn%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20List%3COfficialAccount%3E%20accountList%20%3D%20new%20List%3COfficialAccount%3E()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20string%20text%20%3D%20this._SearchAccount_Html(keyword%2C%20page)%3B%2F%2F%E8%BF%94%E5%9B%9E%E4%BA%86%E4%B8%80%E4%B8%AA%E6%90%9C%E7%B4%A2%E9%A1%B5%E9%9D%A2%E7%9A%84html%E4%BB%A3%E7%A0%81%5Cn%20%20%20%20%20%20%20%20%20%20%20%20HtmlDocument%20pageDoc%20%3D%20new%20HtmlDocument()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20pageDoc.LoadHtml(text)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20HtmlNodeCollection%20targetArea%20%3D%20pageDoc.DocumentNode.SelectNodes(%5C%22%2F%2Ful%5B%40class%3D'news-list2'%5D%2Fli%5C%22)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20if%20(targetArea%20!%3D%20null)%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20foreach%20(HtmlNode%20node%20in%20targetArea)%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20try%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20OfficialAccount%20accountInfo%20%3D%20new%20OfficialAccount()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2F%E9%93%BE%E6%8E%A5%E4%B8%AD%E5%8C%85%E5%90%AB%E4%BA%86%26amp%3B%20html%E7%BC%96%E7%A0%81%E7%AC%A6%EF%BC%8C%E8%A6%81%E7%94%A8htmdecode%EF%BC%8C%E4%B8%8D%E6%98%AFurldecode%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.AccountPageurl%20%3D%20WebUtility.HtmlDecode(node.SelectSingleNode(%5C%22div%2Fdiv%5B%40class%3D'img-box'%5D%2Fa%5C%22).GetAttributeValue(%5C%22href%5C%22%2C%20%5C%22%5C%22))%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2FaccountInfo.ProfilePicture%20%3D%20node.SelectSingleNode(%5C%22div%2Fdiv%5B1%5D%2Fa%2Fimg%5C%22).InnerHtml%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.ProfilePicture%20%3D%20WebUtility.HtmlDecode(node.SelectSingleNode(%5C%22div%2Fdiv%5B%40class%3D'img-box'%5D%2Fa%2Fimg%5C%22).GetAttributeValue(%5C%22src%5C%22%2C%20%5C%22%5C%22))%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.Name%20%3D%20node.SelectSingleNode(%5C%22div%2Fdiv%5B2%5D%2Fp%5B1%5D%5C%22).InnerText.Trim().Replace(%5C%22%3C!--red_beg--%3E%5C%22%2C%20%5C%22%5C%22).Replace(%5C%22%3C!--red_end--%3E%5C%22%2C%20%5C%22%5C%22)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.WeChatId%20%3D%20node.SelectSingleNode(%5C%22div%2Fdiv%5B2%5D%2Fp%5B2%5D%2Flabel%5C%22).InnerText.Trim()%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.QrCode%20%3D%20WebUtility.HtmlDecode(node.SelectSingleNode(%5C%22div%2Fdiv%5B3%5D%2Fspan%2Fimg%5C%22).GetAttributeValue(%5C%22src%5C%22%2C%20%5C%22%5C%22))%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.Introduction%20%3D%20node.SelectSingleNode(%5C%22dl%5B1%5D%2Fdd%5C%22).InnerText.Trim().Replace(%5C%22%3C!--red_beg--%3E%5C%22%2C%5C%22%5C%22).Replace(%5C%22%3C!--red_end--%3E%5C%22%2C%20%5C%22%5C%22)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2F%E6%97%A9%E6%9C%9F%E7%9A%84%E8%B4%A6%E5%8F%B7%E8%AE%A4%E8%AF%81%E5%92%8C%E5%90%8E%E6%9C%9F%E7%9A%84%E8%AE%A4%E8%AF%81%E6%98%BE%E7%A4%BA%E4%B8%8D%E4%B8%80%E6%A0%B7%EF%BC%9F%EF%BC%8C%E5%AF%B9%E6%AF%94%20bitsea%20%E5%92%8C%20NUAA_1952%20%E4%B8%A4%E4%B8%AA%E8%B4%A6%E5%8F%B7%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%2F%2F%E7%8E%B0%E5%9C%A8%E6%94%B9%E4%B8%BA%E5%8C%85%E5%90%AB%E8%AF%A5script%E7%9A%84%E5%8D%B3%E8%AE%A4%E8%AF%81%E4%BA%86%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20if%20(node.InnerText.Contains(%5C%22document.write(authname('2'))%5C%22))%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.IsAuth%20%3D%20true%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20else%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountInfo.IsAuth%20%3D%20false%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20accountList.Add(accountInfo)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20catch%20(Exception%20e)%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20logger.Warn(e)%3B%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%7D%5Cn%20%20%20%20%20%20%20%20%20%20%20%20%5Cn%20%20%20%20%20%20%20%20%20%20%5Cn%20%20%20%20%20%20%20%20%20%20%20%20return%20accountList%3B%20%5Cn%20%20%20%20%20%20%20%20%7D%22%2C%22classes%22%3Anull%7D"" data-cke-widget-upcasted=""1"" data-cke-widget-keep-attr=""0"" data-widget=""codeSnippet""><code class=""language-cs hljs"">&nbsp;</code></pre>
</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<h2><strong>五 、验证码处理以及文件缓存</strong></h2>
<div>&nbsp; 公众号的主页(示例广州大学公众号<a href=""https://mp.weixin.qq.com/profile?src=3&amp;timestamp=1505923231&amp;ver=1&amp;signature=gWXdb*Jzt1oByDAzW5aTzEWnXo6mkUwg3Ynjm3CYvKV0kdCLxALBR7JJ-EheLBI-v6UcocJqGmPbUY2KMXuSsg=="" data-cke-saved-href=""https://mp.weixin.qq.com/profile?src=3&amp;timestamp=1505923231&amp;ver=1&amp;signature=gWXdb*Jzt1oByDAzW5aTzEWnXo6mkUwg3Ynjm3CYvKV0kdCLxALBR7JJ-EheLBI-v6UcocJqGmPbUY2KMXuSsg=="">https://mp.weixin.qq.com/profile?src=3×tamp=1505923231&amp;ver=1&amp;signature=gWXdb*Jzt1oByDAzW5aTzEWnXo6mkUwg3Ynjm3CYvKV0kdCLxALBR7JJ-EheLBI-v6UcocJqGmPbUY2KMXuSsg==</a>)因为页面是属于微信的，反爬虫非常严格，因此多次刷新容易产生要输入验证码的页面<img alt="""" data-cke-saved-src=""file:///C:/Users/Hoyho/Documents/My%20Knowledge/temp/76176e6b-49dc-4d87-9f9b-bab8e5f84ace.jpg""><img src=""/media/article_images/2017/09/20/76176e6b-49dc-4d87-9f9b-bab8e5f84ace.jpg"" alt="""" data-cke-saved-src=""/media/article_images/2017/09/20/76176e6b-49dc-4d87-9f9b-bab8e5f84ace.jpg""></div>
<div>&nbsp;<img src=""http://images2017.cnblogs.com/blog/896762/201710/896762-20171003162411630-1627685040.png"" alt=""""></div>
<div>比如公号主页多次刷新会出现验证码</div>
<div><img src=""/media/article_images/2017/09/20/qq20170921005138.png"" alt="""" data-cke-saved-src=""/media/article_images/2017/09/20/qq20170921005138.png""></div>
<div>&nbsp;</div>
<div>此时要向一个网址post验证码才可以解封</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>解封操作如下</div>
<div>
<div class=""cnblogs_code"">
<pre><span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;summary&gt;</span>
        <span style=""color: #808080"">///</span><span style=""color: #008000""> 页面出现验证码，输入才能继续,此验证依赖cookie, 获取验证码的requset有个cookie，每次不同，需要在post验证码的时候带上
        </span><span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;/summary&gt;</span>
        <span style=""color: #808080"">///</span> <span style=""color: #808080"">&lt;returns&gt;&lt;/returns&gt;</span>
        <span style=""color: #0000ff"">public</span> <span style=""color: #0000ff"">bool</span> VerifyCodeForContinute(<span style=""color: #0000ff"">string</span> url ,<span style=""color: #0000ff"">bool</span><span style=""color: #000000""> isUseOCR)
        {
            </span><span style=""color: #0000ff"">bool</span> isSuccess = <span style=""color: #0000ff"">false</span><span style=""color: #000000"">;
            logger.Debug(</span><span style=""color: #800000"">""</span><span style=""color: #800000"">vcode appear, use VerifyCodeForContinute()</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
            DateTime Epoch </span>= <span style=""color: #0000ff"">new</span> DateTime(<span style=""color: #800080"">1970</span>, <span style=""color: #800080"">1</span>, <span style=""color: #800080"">1</span>,<span style=""color: #800080"">0</span>,<span style=""color: #800080"">0</span>,<span style=""color: #800080"">0</span>,<span style=""color: #800080"">0</span><span style=""color: #000000"">);
            </span><span style=""color: #0000ff"">var</span> timeStamp17 = (DateTime.UtcNow - Epoch).TotalMilliseconds.ToString(<span style=""color: #800000"">""</span><span style=""color: #800000"">R</span><span style=""color: #800000"">""</span>); <span style=""color: #008000"">//</span><span style=""color: #008000"">get timestamp with 17 bit</span>
            <span style=""color: #0000ff"">string</span> codeurl = <span style=""color: #800000"">""</span><span style=""color: #800000"">https://mp.weixin.qq.com/mp/verifycode?cert=</span><span style=""color: #800000"">""</span> +<span style=""color: #000000""> timeStamp17;
            WebHeaderCollection headers </span>= <span style=""color: #0000ff"">new</span><span style=""color: #000000""> WebHeaderCollection();
            </span><span style=""color: #0000ff"">var</span> content = <span style=""color: #0000ff"">this</span>.Get(headers, codeurl,<span style=""color: #800000"">""</span><span style=""color: #800000"">UTF-8</span><span style=""color: #800000"">""</span>,<span style=""color: #0000ff"">true</span><span style=""color: #000000"">);
            ShowImageHandle showImageHandle </span>= <span style=""color: #0000ff"">new</span><span style=""color: #000000""> ShowImageHandle(DisplayImageFromBase64);
            showImageHandle.BeginInvoke(content, </span><span style=""color: #0000ff"">null</span>, <span style=""color: #0000ff"">null</span><span style=""color: #000000"">);
            Console.WriteLine(</span><span style=""color: #800000"">""</span><span style=""color: #800000"">请输入验证码：</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
            </span><span style=""color: #0000ff"">string</span> verifyCode =<span style=""color: #000000""> Console.ReadLine();
            </span><span style=""color: #0000ff"">string</span> postURL = <span style=""color: #800000"">""</span><span style=""color: #800000"">https://mp.weixin.qq.com/mp/verifycode</span><span style=""color: #800000"">""</span><span style=""color: #000000"">;
            timeStamp17 </span>= (DateTime.UtcNow - Epoch).TotalMilliseconds.ToString(<span style=""color: #800000"">""</span><span style=""color: #800000"">R</span><span style=""color: #800000"">""</span>); <span style=""color: #008000"">//</span><span style=""color: #008000"">get timestamp with 17 bit</span>
            <span style=""color: #0000ff"">string</span> postData = <span style=""color: #0000ff"">string</span>.Format(<span style=""color: #800000"">""</span><span style=""color: #800000"">cert={0}&amp;input={1}</span><span style=""color: #800000"">""</span>,timeStamp17,verifyCode );<span style=""color: #008000"">//</span><span style=""color: #008000""> ""{"" + string.Format(@""'cert':'{0}','input':'{1}'"", timeStamp17, verifyCode) + ""}"";</span>
            headers.Add(<span style=""color: #800000"">""</span><span style=""color: #800000"">Host</span><span style=""color: #800000"">""</span>, <span style=""color: #800000"">""</span><span style=""color: #800000"">mp.weixin.qq.com</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
            headers.Add(</span><span style=""color: #800000"">""</span><span style=""color: #800000"">Referer</span><span style=""color: #800000"">""</span><span style=""color: #000000"">, url);
            </span><span style=""color: #0000ff"">string</span> remsg = <span style=""color: #0000ff"">this</span>.Post(postURL, headers, postData,<span style=""color: #0000ff"">true</span><span style=""color: #000000"">);
            </span><span style=""color: #0000ff"">try</span><span style=""color: #000000"">
            {
                JObject jo </span>= JObject.Parse(remsg);<span style=""color: #008000"">//</span><span style=""color: #008000"">把json字符串转化为json对象  </span>
                <span style=""color: #0000ff"">int</span> statusCode = (<span style=""color: #0000ff"">int</span>)jo.GetValue(<span style=""color: #800000"">""</span><span style=""color: #800000"">ret</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
                </span><span style=""color: #0000ff"">if</span> (statusCode == <span style=""color: #800080"">0</span><span style=""color: #000000"">)
                {
                    isSuccess </span>= <span style=""color: #0000ff"">true</span><span style=""color: #000000"">;
                }
                </span><span style=""color: #0000ff"">else</span><span style=""color: #000000"">
                {
                    logger.Error(</span><span style=""color: #800000"">""</span><span style=""color: #800000"">cannot unblock because </span><span style=""color: #800000"">""</span> + jo.GetValue(<span style=""color: #800000"">""</span><span style=""color: #800000"">msg</span><span style=""color: #800000"">""</span><span style=""color: #000000"">));
                    </span><span style=""color: #0000ff"">var</span> vcodeException = <span style=""color: #0000ff"">new</span><span style=""color: #000000""> WechatSogouVcodeException();
                    vcodeException.MoreInfo </span>= <span style=""color: #800000"">""</span><span style=""color: #800000"">cannot jiefeng because </span><span style=""color: #800000"">""</span> + jo.GetValue(<span style=""color: #800000"">""</span><span style=""color: #800000"">msg</span><span style=""color: #800000"">""</span><span style=""color: #000000"">);
                    </span><span style=""color: #0000ff"">throw</span><span style=""color: #000000""> vcodeException;
                }
            }</span><span style=""color: #0000ff"">catch</span><span style=""color: #000000"">(Exception e)
            {
                logger.Error(e);
            }
            </span><span style=""color: #0000ff"">return</span><span style=""color: #000000""> isSuccess;
        }</span></pre>
</div>
<p>&nbsp;</p>
</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>解释下：</div>
<div>先访问一个验证码产生页面，带17位时间戳</div>
<div>
<div><code class=""language-cs"">var timeStamp17 = (DateTime.UtcNow - Epoch).TotalMilliseconds.ToString(""R""); //get timestamp with 17 bit</code></div>
</div>
<div><span>再向这个url query post你的验证码:</span></div>
<div><span><span>因此这里记得要启用<span>如果启用了cookie，会通过FileCache类将cookie保存在缓存文件，下次请求如果开启cookie container的话就会带上此cookie</span></span></span></div>
<div>
<div class=""cke_widget_wrapper cke_widget_block cke_widget_selected"" data-cke-widget-wrapper=""1"" data-cke-filter=""off"" data-cke-display-name=""code snippet"" data-cke-widget-id=""11"">
<div class=""cnblogs_code"">
<pre>CookieCollection cc =<span style=""color: #000000""> Tools.LoadCookieFromCache();
request.CookieContainer </span>= <span style=""color: #0000ff"">new</span><span style=""color: #000000""> CookieContainer();
request.CookieContainer.Add(cc);</span></pre>
</div>
<p>&nbsp;</p>
<img class=""cke_reset cke_widget_mask"" style=""font-family: &quot;PingFang SC&quot;, &quot;Helvetica Neue&quot;, Helvetica, Arial, sans-serif; font-size: 14px"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""">
<pre class=""cke_widget_element"" data-cke-widget-data=""%7B%22lang%22%3A%22cs%22%2C%22code%22%3A%22CookieCollection%20cc%20%3D%20Tools.LoadCookieFromCache()%3B%5Cnrequest.CookieContainer%20%3D%20new%20CookieContainer()%3B%5Cnrequest.CookieContainer.Add(cc)%3B%22%2C%22classes%22%3Anull%7D"" data-cke-widget-upcasted=""1"" data-cke-widget-keep-attr=""0"" data-widget=""codeSnippet""><span class=""cke_reset cke_widget_drag_handler_container"" style=""font-family: &quot;PingFang SC&quot;, &quot;Helvetica Neue&quot;, Helvetica, Arial, sans-serif; font-size: 14px""><img class=""cke_reset cke_widget_drag_handler"" title=""Click and drag to move"" src=""data:image/gif;base64,R0lGODlhAQABAPABAP///wAAACH5BAEKAAAALAAAAAABAAEAAAICRAEAOw=="" alt="""" width=""15"" height=""15"" data-cke-widget-drag-handler=""1""></span></pre>
</div>
<p>&nbsp;</p>
</div>
<div>&nbsp;</div>
<h2><strong>六、后话</strong></h2>
<div>&nbsp; &nbsp;</div>
<div>&nbsp; &nbsp; 上面只是一部分，刚开始写的时候也没想到会有这么多坑，但是没办法，坑再多只能自己慢慢填了，比如OCR，第三方打码接入，多线程等等后期再实现。一个人的精力毕竟有限，相对满大街的Python爬虫，C#的爬虫性质的项目本来就不多，尽管代码写得非常粗糙，但是我选择了开放源码希望更多人参与，欢迎各位看官收藏，可以的话给个星或者提交代码</div>
<div><a href=""https://github.com/hoyho/WeGouSharp"" target=""_blank"" data-cke-saved-href=""https://github.com/hoyho/WeGouSharp"">项目地址</a></div>
<div>&nbsp;</div>
<div>&nbsp;</div><img src=""http://counter.cnblogs.com//blog/post/7623903"" width=""1"" height=""1"" style=""border:0px;visibility:hidden""/>";
                         ArticlesDetails.HasError = false;
                         ArticlesDetails.HasContent = true;
                     });
                     IsBusy = false;
                 }
                 catch (Exception)
                 {
                 }
             }
         ));

        public class ArticlesDetailsModel : BaseViewModel
        {
            string author;
            public string Author
            {
                get { return author; }
                set { SetProperty(ref author, value); }
            }
            string avatar;
            public string Avatar
            {
                get { return avatar; }
                set { SetProperty(ref avatar, value); }
            }
            string diggDisplay;
            public string DiggDisplay
            {
                get { return diggDisplay; }
                set { SetProperty(ref diggDisplay, value); }
            }
            string commentDisplay;
            public string CommentDisplay
            {
                get { return commentDisplay; }
                set { SetProperty(ref commentDisplay, value); }
            }
            string viewDisplay;
            public string ViewDisplay
            {
                get { return viewDisplay; }
                set { SetProperty(ref viewDisplay, value); }
            }
            string content;
            public string Content
            {
                get { return content; }
                set { SetProperty(ref content, value); }
            }
            string dateDisplay;
            public string DateDisplay
            {
                get { return dateDisplay; }
                set { SetProperty(ref dateDisplay, value); }
            }
            bool hasError;
            public bool HasError
            {
                get { return hasError; }
                set { SetProperty(ref hasError, value); }
            }
            bool hasContent;
            public bool HasContent
            {
                get { return hasContent; }
                set { SetProperty(ref hasContent, value); }
            }
        }

    }
}
