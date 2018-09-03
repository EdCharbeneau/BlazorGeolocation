using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace EJC.Blazor.Geolocation
{
    public class Geolocation
    {
        private Action<Position> OnWatchPosition;
        private Action<Position> OnGetPosition;
        private Action<PositionError> OnWatchPositionError;
        private Action<PositionError> OnGetPositionError;

        public async Task GetCurrentPosition(Action<Position> onSuccess,
                                             Action<PositionError> onError,
                                             PositionOptions options = null)
        {
            OnGetPosition = onSuccess;
            OnGetPositionError = onError;
            await JSRuntime.Current.InvokeAsync<bool>("ejcGeolocation.getCurrentPosition", new DotNetObjectRef(this), options);
        }
        public static async Task<bool> HasGeolocationFeature() =>
            await JSRuntime.Current.InvokeAsync<bool>("ejcGeolocation.hasGeolocaitonFeature");

        public async Task<int> WatchPosition(Action<Position> onSuccess,
                                             Action<PositionError> onError,
                                             PositionOptions options = null)
        {
            OnWatchPosition = onSuccess;
            OnWatchPositionError = onError;
            return await JSRuntime.Current.InvokeAsync<int>("ejcGeolocation.watchPosition", new DotNetObjectRef(this), options);
        }
        public async Task ClearWatch(int watchId, Action onPositionReported) =>
            await JSRuntime.Current.InvokeAsync<int>("ejcGeolocation.clearWatch", watchId);

        [JSInvokable]
        public void RaiseOnGetPosition(Position p) => OnGetPosition?.Invoke(p);
        [JSInvokable]
        public void RaiseOnGetPositionError(PositionError err) => OnGetPositionError?.Invoke(err);

        [JSInvokable]
        public void RaiseOnWatchPosition(Position p) => OnWatchPosition?.Invoke(p);

        [JSInvokable]
        public void RaiseOnWatchPositionError(PositionError err) => OnWatchPositionError?.Invoke(err);
    }
}
