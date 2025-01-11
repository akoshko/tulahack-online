using IdentityModel.OidcClient.Browser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace Tulahack.Desktop.Auth;

public class SystemBrowser : IBrowser
{
    public int Port { get; }
    private readonly string? _path;

    public SystemBrowser(int? port = null, string? path = null)
    {
        _path = path;
        Port = port ?? GetRandomUnusedPort();
    }

    private int GetRandomUnusedPort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options,
        CancellationToken cancellationToken = default)
    {
        using var listener = new LoopbackHttpListener(Port, _path);

        OpenBrowser(new Uri(options.StartUrl));

        try
        {
            var result = await listener.WaitForCallbackAsync();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = "Empty response." };
            }

            return new BrowserResult { Response = result, ResultType = BrowserResultType.Success };
        }
        catch (TaskCanceledException ex)
        {
            return new BrowserResult { ResultType = BrowserResultType.Timeout, Error = ex.Message };
        }
        catch (Exception ex)
        {
            return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = ex.Message };
        }
    }

    private static void OpenBrowser(Uri url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            _ = Process.Start(new ProcessStartInfo
            {
                FileName = url.ToString(),
                UseShellExecute = true,
            });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            _ = Process.Start("xdg-open", url.ToString());
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            _ = Process.Start("open", url.ToString());
        }
    }
}

public class LoopbackHttpListener : IDisposable
{
    const int PcDefaultTimeout = 60 * 5; // 5 mins (in seconds)

    readonly IWebHost _host;
    readonly TaskCompletionSource<string> _source = new();


    public LoopbackHttpListener(int port, string? path = null)
    {
        path ??= string.Empty;
        if (path.StartsWith('/'))
        {
            path = path[1..];
        }

        var url = $"http://127.0.0.1:{port}/{path}";

        _host = new WebHostBuilder()
            .UseKestrel()
            .UseUrls(url)
            .Configure(Configure)
            .Build();
        _host.Start();
    }

    public void Dispose() =>
        Task.Run(async () =>
        {
            await Task.Delay(500);
            _host.Dispose();
        });

    private void Configure(IApplicationBuilder app) =>
        app.Run(async ctx =>
        {
            switch (ctx.Request.Method)
            {
                case "GET":
                    await SetResultAsync(ctx.Request.QueryString.Value, ctx);
                    break;
                case "POST" when !ctx.Request.ContentType.Equals("application/x-www-form-urlencoded",
                    StringComparison.OrdinalIgnoreCase):
                    ctx.Response.StatusCode = 415;
                    break;
                case "POST":
                    {
                        using var sr = new StreamReader(ctx.Request.Body, Encoding.UTF8);
                        var body = await sr.ReadToEndAsync();
                        await SetResultAsync(body, ctx);

                        break;
                    }
                default:
                    ctx.Response.StatusCode = 405;
                    break;
            }
        });

    private async Task SetResultAsync(string value, HttpContext ctx)
    {
        try
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync("<h1>You can now return to the application.</h1>");
            await ctx.Response.Body.FlushAsync();
            _ = _source.TrySetResult(value);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            ctx.Response.StatusCode = 400;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync("<h1>Invalid request.</h1>");
            await ctx.Response.Body.FlushAsync();
        }
    }

    public Task<string> WaitForCallbackAsync(int timeoutInSeconds = PcDefaultTimeout)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(timeoutInSeconds * 1000);
            _ = _source.TrySetCanceled();
        });

        return _source.Task;
    }
}