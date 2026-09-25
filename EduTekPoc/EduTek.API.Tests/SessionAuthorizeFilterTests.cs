using EduTek.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace EduTek.API.Tests;

public class SessionAuthorizeFilterTests
{
    [Fact]
    public void UnauthenticatedUser_IsRedirectedToLogin()
    {
        var filter = new SessionAuthorizeAttribute("Admin");
        var context = CreateActionExecutingContext();

        filter.OnActionExecuting(context);

        var redirect = Assert.IsType<RedirectToActionResult>(context.Result);
        Assert.Equal("Login", redirect.ActionName);
        Assert.Equal("Auth", redirect.ControllerName);
    }

    [Fact]
    public void AuthenticatedUser_WithUnauthorizedRole_Returns403AccessDenied()
    {
        var filter = new SessionAuthorizeAttribute("Admin");
        var context = CreateActionExecutingContext();
        context.HttpContext.Session.SetString("AccessToken", "fake-jwt-token");
        context.HttpContext.Session.SetString("Role", "Student");

        filter.OnActionExecuting(context);

        var viewResult = Assert.IsType<ViewResult>(context.Result);
        Assert.Equal(StatusCodes.Status403Forbidden, viewResult.StatusCode);
        Assert.Equal("~/Views/Shared/AccessDenied.cshtml", viewResult.ViewName);
    }

    [Fact]
    public void AuthenticatedUser_WithAuthorizedRole_IsAllowedThrough()
    {
        var filter = new SessionAuthorizeAttribute("Admin");
        var context = CreateActionExecutingContext();
        context.HttpContext.Session.SetString("AccessToken", "fake-jwt-token");
        context.HttpContext.Session.SetString("Role", "Admin");

        filter.OnActionExecuting(context);

        Assert.Null(context.Result);
    }

    [Fact]
    public void AuthenticatedUser_WithoutRoleRestriction_IsAllowedThrough()
    {
        var filter = new SessionAuthorizeAttribute();
        var context = CreateActionExecutingContext();
        context.HttpContext.Session.SetString("AccessToken", "fake-jwt-token");
        context.HttpContext.Session.SetString("Role", "Student");

        filter.OnActionExecuting(context);

        Assert.Null(context.Result);
    }

    private static ActionExecutingContext CreateActionExecutingContext()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());
    }

    private class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _store.Remove(key);
        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) =>
            _store.TryGetValue(key, out value!);
    }
}
