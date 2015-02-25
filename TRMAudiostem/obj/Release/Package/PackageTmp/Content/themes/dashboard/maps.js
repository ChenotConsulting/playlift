
var map;

function initialize(mapContainer, zoom, lat, lng) {
    var mapProp = {
        center: new google.maps.LatLng(lat, lng),
        zoom: zoom,
        mapTypeId: google.maps.MapTypeId.HYBRID
    };
    map = new google.maps.Map(document.getElementById(mapContainer), mapProp);
}

function loadScript() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "http://maps.googleapis.com/maps/api/js?key=&sensor=false&callback=initialize";
    document.body.appendChild(script);
}

function Geocode(index, businessName, postcode, address, address2, city, logo, centerMap) {
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ address: postcode + ", UK" }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var result = results[0];
            var latLong = result.geometry.location;

            var markerImage = 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=' + index + '|8bc53f|333333';
            var marker = new google.maps.Marker({
                position: latLong,
                map: map,
                icon: markerImage,
                title: postcode
            });

            google.maps.event.trigger(map, 'resize');

            if (centerMap) {
                map.setCenter(result.geometry.location);
            }

            marker.setMap(map);

            google.maps.event.addListener(marker, 'click', function () {
                showInfo(marker, businessName, address, address2, postcode, city, logo);
            });
        }
        //else {
        //    alert(businessName + " not found");
        //}
    });
}

function showInfo(marker, businessName, address, address2, city, postcode, logo) {
    if (address2 != "") {
        var content = '<img width="80" src="' +
                        logo + '" /><br /><b>' +
                        businessName + '</b><br />' +
                        address + '<br />' +
                        address2 + '<br />' +
                        postcode + '<br />' +
                        city;
    } else {
        var content = '<img width="80" src="' +
                        logo + '" /><br /><b>' +
                        businessName + '</b><br />' +
                        address + '<br />' +
                        postcode + '<br />' +
                        city;
    }
    var infowindow = new google.maps.InfoWindow({
        content: content
    });

    infowindow.open(map, marker);
}

function loadMap(container, url, userId) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');

    $.ajax({
        cache: false,
        type: 'GET',
        async: true,
        dataType: "html",
        data: { userId: userId },
        url: url,
        success: function (html) {
            $container.hide().html(html).slideDown(200);
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });

    return false;
}