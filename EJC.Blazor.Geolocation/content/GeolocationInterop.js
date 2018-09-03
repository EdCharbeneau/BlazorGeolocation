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
    hasGeolocaitonFeature: function () {
        console.log("has geo");
        return navigator.geolocation ? true : false;
    },
    getCurrentPosition: function (geolocationRef, options) {
        const success = (result) => {
            console.log("success");
            geolocationRef.invokeMethodAsync('RaiseOnGetPosition', ejcGeolocation.toSerializeable(result));
        };
        const error = (er) =>
            geolocationRef.invokeMethodAsync('RaiseOnGetPositionError', er.code);
        if (ejcGeolocation.hasGeolocaitonFeature()) {
            console.log("getGeo");
            navigator.geolocation.getCurrentPosition(success, error, options);
        }
    },
    watchPosition: function (geolocationRef, options) {
        const success = (result) =>
            geolocationRef.invokeMethodAsync('RaiseOnWatchPosition', ejcGeolocation.toSerializeable(result));
        const error = (er) =>
            geolocationRef.invokeMethodAsync('RaiseOnWatchPositionError', er.code);
        if (ejcGeolocation.hasGeolocaitonFeature()) {
            return navigator.geolocation.watchPosition(success, error, options);
        }
        return 0;
    },
    clearWatch: function (watchId) {
        if (ejcGeolocation.hasGeolocaitonFeature()) {
            navigator.geolocation.clearWatch(watchId);
        }
    }
};