using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace UmarSeat.Helpers
{
    public static class lowercase
    {
        public static MvcHtmlString LowerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expr, object htmlAttributes)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "text");
            var lowerPropertyName = ExpressionHelper.GetExpressionText(expr).ToLower();
            tag.MergeAttribute("name", lowerPropertyName);
            tag.MergeAttribute("id", lowerPropertyName);
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            result.Append(tag.ToString());
            return new MvcHtmlString(result.ToString());
        }
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }  
    }
}