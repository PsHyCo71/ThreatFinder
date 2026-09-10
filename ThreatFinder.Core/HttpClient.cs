using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

public class RateLimitingHandler : DelegatingHandler
{
    readonly RateLimiter rateLimiter;

    public RateLimitingHandler(RateLimiterInfo info, HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
        this.rateLimiter = new RateLimiter(info);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.rateLimiter.SyncAcquire();
        return base.Send(request, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await this.rateLimiter.Acquire();
        return await base.SendAsync(request, cancellationToken);
    }
}

public struct RateLimiterInfo
{
    public RateLimiterInfo(double rate_per_sec, ulong burst)
    {
        // assert that they're valid values
        if (!(double.IsFinite(rate_per_sec) && double.IsPositive(rate_per_sec) && rate_per_sec != 0)) {
            throw new ArgumentException("rate_per_sec must not be infinite, negative or zero.");
        }

        if (burst == 0) {
            throw new ArgumentException("burst must not be zero");
        }

        this.nanos_per_token = (ulong) double.Ceiling(1_000_000_000.0 / rate_per_sec);
        this.rate_per_sec = rate_per_sec;
        this.burst_nanos = this.nanos_per_token * burst;
    }

    public ulong nanos_per_token { get; }
    public double rate_per_sec { get; }
    public ulong burst_nanos { get; }
}

static class AtomicHelpers
{
    public static bool FetchUpdate(this ref ulong num, Func<ulong, ulong?> updateFn, out ulong result)
    {
        ulong current = Volatile.Read(ref num);
        while (true)
        {

            ulong? next = updateFn(current);
            // lambda refused update
            if (next is null)
            {
                result = current;
                return false; 
            }

            ulong previous = Interlocked.CompareExchange(ref num, next.Value, current);
            if (previous == current)
            {
                // happy path
                result = next.Value;
                return true;
            }

            current = previous;
        }
    }
}

public class RateLimiter
{
    private ulong next_at;
    private readonly Stopwatch start;
    private readonly RateLimiterInfo info;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TimeSpan? GetWaiting()
    {
        var now = (ulong) this.start.Elapsed.Nanoseconds;
        var earliest = now > this.info.burst_nanos ? now - this.info.burst_nanos : 0; // saturating sub
        this.next_at.FetchUpdate(next => ulong.Max(next, earliest) + this.info.nanos_per_token, out var slot);
        
        return slot > now ? new TimeSpan((long) (slot - now) / 100) : null;
    }

    public void SyncAcquire()
    {
        var span = GetWaiting();

        if (span is not null)
        {
            Thread.Sleep(span.Value);            
        }
    }

    public async Task Acquire()
    {
        var span = GetWaiting();

        if (span is not null)
        {
            await Task.Delay(span.Value);            
        }
    }

    public RateLimiter(RateLimiterInfo info) 
    {
        this.start = Stopwatch.StartNew();
        this.next_at = 0;
        this.info = info;
    }
}