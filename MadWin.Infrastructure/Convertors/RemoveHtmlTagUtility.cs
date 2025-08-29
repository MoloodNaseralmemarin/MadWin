using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shop2City.Core.Convertors
{
    public class RemoveHtmlTagUtility
    {
        public string RemoveHtmlTag(string HtmlCode)
        {
            Regex regex = new Regex("<(.*?)>");
            var maches = regex.Matches(HtmlCode);

            foreach (Match item in maches)
            {
                HtmlCode = HtmlCode.Replace(item.Value, "");
            }

            return HtmlCode;
        }
        public string GetSafeHtml(string Html)
        {
            Regex reg = new Regex(@"<[\s]*script(.*?)>(.*?)<(.*?)script>", RegexOptions.Multiline);
            var q = reg.Matches(Html);

            foreach (Match item in q)
            {
                Html = Html.Replace(item.Value, "");
            }
            return Html;
        }
    }
}
