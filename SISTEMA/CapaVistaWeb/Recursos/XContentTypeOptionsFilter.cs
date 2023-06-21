using System.Web.Mvc;

public class XContentTypeOptionsFilter : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        filterContext.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        base.OnResultExecuting(filterContext);
    }
}