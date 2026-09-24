window.farmMap = (() => {
    let map;
    let marker;

    function setMarker(lat, lng) {
        if (marker) marker.setLatLng([lat, lng]);
        else marker = L.marker([lat, lng]).addTo(map);
        map.setView([lat, lng], Math.max(map.getZoom(), 15));
    }

    async function selectLocation(dotnet, lat, lng) {
        setMarker(lat, lng);
        let address = `${lat.toFixed(6)}, ${lng.toFixed(6)}`;
        try {
            const response = await fetch(`https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}&accept-language=vi`);
            if (response.ok) {
                const result = await response.json();
                address = result.display_name || address;
            }
        } catch { }
        await dotnet.invokeMethodAsync("SetMapLocation", lat, lng, address);
    }

    return {
        init: (dotnet, latitude, longitude) => {
            if (map) map.remove();
            const hasPosition = latitude != null && longitude != null;
            const center = hasPosition ? [latitude, longitude] : [14.0583, 108.2772];
            map = L.map("farm-location-map").setView(center, hasPosition ? 15 : 6);
            L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
                maxZoom: 19,
                attribution: "&copy; OpenStreetMap contributors"
            }).addTo(map);
            if (hasPosition) setMarker(latitude, longitude);
            map.on("click", event => selectLocation(dotnet, event.latlng.lat, event.latlng.lng));
            setTimeout(() => map.invalidateSize(), 0);
        },
        destroy: () => {
            if (map) map.remove();
            map = null;
            marker = null;
        }
    };
})();
