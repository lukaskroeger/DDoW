using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace MAUIApp.Utils;
internal static class TextUtils
{   

    //The set of tags supported by the Windows TextLabel. It seems like this is the smallest set of supported tags of all platforms.
    private static readonly ImmutableHashSet<string> supportedTags = ImmutableHashSet.Create("b", "br", "em","i","p","strong","u", "ul", "li", "div");
    //Remove unsupported Tags from an html string
    public static string GetSupportedHTML(string htmlText)
    {
        var sanitizerOptions = new HtmlSanitizerOptions();
        sanitizerOptions.AllowedTags = supportedTags;
        var sanitizer = new HtmlSanitizer(sanitizerOptions);
        sanitizer.KeepChildNodes = true;
        htmlText = sanitizer.Sanitize(htmlText);
        //Windows label cannot handle HTML Entities
        htmlText = HttpUtility.HtmlDecode(htmlText);
        //Windows label cannot handle empty tags
        htmlText = Regex.Replace(htmlText, @"<(\w+)\b(?:\s+[\w\-.:]+(?:\s*=\s*(?:""[^""]*""|'[^']*'|[\w\-.:]+))?)*\s*/?>\s*</\1\s*>", string.Empty, RegexOptions.Multiline);
        return htmlText;
    }
}
