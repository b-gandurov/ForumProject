using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

public static class StringHelpers
{
    public static string TruncateContent(string content, int maxLength, int postId, IUrlHelper urlHelper)
    {
        if (content.Length <= maxLength)
        {
            return content;
        }
        var truncatedContent = Regex.Replace(content.Substring(0, maxLength), @"\s+\S*$", "") + "...";
        return truncatedContent + $" <a href=\"{urlHelper.Action("Details", "Posts", new { id = postId })}\">Read more</a>";
    }
}
