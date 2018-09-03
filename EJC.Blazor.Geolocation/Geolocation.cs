using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace EJC.Blazor.Geolocation
{
    public class Geolocation
    {
        public async Task GetCurrentPosition(PositionOptions options = null) => 
            await JSRuntime.Current.InvokeAsync<bool>("ejcGeolocation.getCurrentPosition", new DotNetObjectRef(this), options);

        public static async Task<bool> HasGeolocationFeature() => 
            await JSRuntime.Current.InvokeAsync<bool>("ejcGeolocation.hasGeolocaitonFeature");

        public async Task<int> WatchPosition(PositionOptions options = null) => 
            await JSRuntime.Current.InvokeAsync<int>("ejcGeolocation.watchPosition", new DotNetObjectRef(this), options);

        public async Task ClearWatch(int watchId, Action onPositionReported) => 
            await JSRuntime.Current.InvokeAsync<int>("ejcGeolocation.clearWatch", watchId);

        [JSInvokable]
        public void RaiseOnGetPosition(Position p) => Invoke(OnGetPosition, p);
        [JSInvokable]
        public void RaiseOnGetPositionError(PositionError err) => Invoke(OnGetPositionError, err);

        [JSInvokable]
        public void RaiseOnWatchPosition(Position p) => Invoke(OnWatchPosition, p);

        [JSInvokable]
        public void RaiseOnWatchPositionError(Position p) => Invoke(OnWatchPositionError, p);

        public MulticastDelegate OnWatchPosition { get; set; }
        public MulticastDelegate OnGetPosition { get; set; }
        public MulticastDelegate OnWatchPositionError { get; set; }
        public MulticastDelegate OnGetPositionError { get; set; }
        public Task Invoke<T>(MulticastDelegate del, T e)
        {
            switch (del)
            {
                case Action action:
                    action.Invoke();
                    return Task.CompletedTask;

                case Action<T> actionEventArgs:
                    actionEventArgs.Invoke(e);
                    return Task.CompletedTask;

                case Func<Task> func:
                    return func.Invoke();

                case Func<T, Task> funcEventArgs:
                    return funcEventArgs.Invoke(e);

                case MulticastDelegate @delegate:
                    return @delegate.DynamicInvoke(e) as Task ?? Task.CompletedTask;

                case null:
                    return Task.CompletedTask;
            }
        }
    }
}
