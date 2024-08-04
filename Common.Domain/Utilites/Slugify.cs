
using System.Text;

namespace Common.Domain.Utilites
{
    public static class Slugify
    {
        public static string ToSlug(this string content)
        {
            content.Replace(" ", "-");
            content.Replace("$", "");
            content.Replace("+", "");
            content.Replace("?", "");
            content.Replace("*", "");
            content.Replace("!", "");
            content.Replace("@", "");
            content.Replace("#", "");
            content.Replace("&", "");
            content.Replace("(", "");
            content.Replace(")", "");
            content.Replace("=", "");
            content.Replace("/", "");
            content.Replace("\\", "");
            content.Replace(".", "");
            content.Replace("~", "");
            
            content.Trim();

            return content;

        }
    }
}
