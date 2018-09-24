window.ejcGeolocation = {
    toSerializeable: function (e) {
        return {
            "coords": {
                "latitude": e.coords.latitude,
                "longitude": e.coords.longitude,
                "accuracy": e.coords.accuracy,
                "altitude": e.coords.altitude,
                "altitudeAccuracy": e.coords.altitudeAccuracy,
                "heading": e.coords.heading,
                "speed": e.coords.speed
            },
            "timestamp": new Date(e.timestamp)
        };
    },
    hasGeolocationFeature: function () {
        return navigator.geolocation ? true : false;
    },
    getCurrentPosition: function (geolocationRef, options) {
        const success = (result) => {
            geolocationRef.invokeMethodAsync('RaiseOnGetPosition', ejcGeolocation.toSerializeable(result));
        };
        const error = (er) =>
            geolocationRef.invokeMethodAsync('RaiseOnGetPositionError', er.code);
        if (ejcGeolocation.hasGeolocationFeature()) {
            navigator.geolocation.getCurrentPosition(success, error, options);
        }
    },
    watchPosition: function (geolocationRef, options) {
        const success = (result) =>
            geolocationRef.invokeMethodAsync('RaiseOnWatchPosition', ejcGeolocation.toSerializeable(result));
        const error = (er) =>
            geolocationRef.invokeMethodAsync('RaiseOnWatchPositionError', er.code);
        if (ejcGeolocation.hasGeolocationFeature()) {
            return navigator.geolocation.watchPosition(success, error, options);
        }
        return 0;
    },
    clearWatch: function (watchId) {
        if (ejcGeolocation.hasGeolocationFeature()) {
            navigator.geolocation.clearWatch(watchId);
        }
    }
};
