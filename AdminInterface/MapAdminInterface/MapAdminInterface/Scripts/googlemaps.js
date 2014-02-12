function initialize(latitude, longitude, zoomValue) {

    var lat = latitude.replace(',', '.')
    var lng = longitude.replace(',', '.')

    var latLng = new google.maps.LatLng(lat, lng);
    var options = {
        zoom: zoomValue,
        center: latLng,
        scrollwheel: false,
        disableDoubleClickZoom: true,
        mapTypeId: google.maps.MapTypeId.SATELLITE
    }
    var map = new google.maps.Map(document.getElementById('map_canvas'), options);
    var marker = new google.maps.Marker({
        position: latLng,
        map: map,
        draggable: true
    });
    google.maps.event.addListener(marker, 'drag', function (event) {
        var lattitude = document.getElementById('Area_latitude')
        if (lattitude != null) {
            lattitude.value = String(event.latLng.lat()).replace('.', ',')
            var longitude = document.getElementById('Area_longitude')
            longitude.value = String(event.latLng.lng()).replace('.', ',')
        }
        var lattitude = document.getElementById('Location_latitude')
        if (lattitude != null) {
            lattitude.value = String(event.latLng.lat()).replace('.', ',')
            var longitude = document.getElementById('Location_longitude')
            longitude.value = String(event.latLng.lng()).replace('.', ',')
        }
    });
}
