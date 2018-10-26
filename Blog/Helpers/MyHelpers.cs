using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Helpers
{
    /// <summary>
    /// Custom Html helpers
    /// </summary>
    public static class MyHelpers
    {
        // Returns ccs id for the navigation item related to the current page based on controller - action match
        public static string IsCurrent(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssId = "current";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            
            if (String.IsNullOrEmpty(controller))
                controller = currentController;
            
            if (String.IsNullOrEmpty(action))
                action = currentAction;
            
            return controller == currentController && action == currentAction ?
                cssId : String.Empty;
        }

        // Shortens a text so that it is below the specified maximum character.
        // Makes sure that the text is truncated at a space and adds ellipsis (...).
         public static string Truncate(this HtmlHelper html, string text, int maxCharacter)
        {
             if (text.Length > maxCharacter)
             {
                 text = text.Substring(0, maxCharacter);
                 int length = text.LastIndexOf(' ');
                 text = text.Substring(0, length);
                 return text + "...";
             }
            return text;
        }
    }
}