
var segments = [];
var highways = [, ];
var signs = [];

var areSignsVisible = false;
var pins = [];

var mapDataSegments = [];
var maps = new Array();

var map;

var colorValues = [
    { 'color': 'green', 'status': 'Smooth' },
    { 'color': 'yellow', 'status': 'Slow' },
    { 'color': 'orange', 'status': 'Delayed' },
    { 'color': 'red', 'status': 'Stucked' }
];

var defaultZoom = 12;
var defaultZoomIn = 14;

var segmentsEndpointUrl = 'API/Segment';
var highwaysEndpointUrl = 'API/Highway';
var signsEndopintUrl    = "API/HighwaySign";


function createBAMap() {
    var buenosAires = new google.maps.LatLng(-34.619980, -58.4450);

    // Create a simple map
    map = new google.maps.Map(document.getElementById('map-canvas'), {
        zoom: defaultZoom,
        center: buenosAires
    });
}


function initializeHighwaysMap() {

    this.showLoading();

    this.createBAMap();
    this.loadHighways();

    $.getJSON(segmentsEndpointUrl, function (data) {

        // Load segments style.
        map.data.setStyle(function (feature) {
            return /** @type {google.maps.Data.StyleOptions} */({
                strokeColor: feature.getProperty('color'),
                strokeWeight: 3
            });
        });

        // loop into each segment
        $.each(data, function (key, value) {
            var json = JSON.parse(value.GeoJson);

            var mapData = [];
            mapData.HighwayId = value.HighwayId;
            mapData.Key = key;
            mapData.Value = value.GeoJson;

            mapDataSegments.push(mapData);

            //map.data.addGeoJson(json, value.Name);
            segments.push(value);
        });

        // pre-set the infoWindow
        var infoWindow = new google.maps.InfoWindow(
            {
                content: '',
                size: new google.maps.Size(10, 30)
            });

        //get the legend container, create a legend, add a legend renderer fn
        var $legendContainer = $('#legend-container'),
            $legend = $('<div id="legend">').appendTo($legendContainer),
            renderLegend = function (colorValuesArray) {
                $legend.empty();
                $.each(colorValuesArray, function (index, val) {
                    var $div = $('<div style="height:25px;">').append($('<div class="legend-color-box">').css({
                        backgroundColor: val.color,
                    })).append($("<span>").css("lineHeight", "23px").html(val.status));

                    $legend.append($div);
                });
            }

        //make a legend for the first time
        renderLegend(colorValues);

        //add the legend to the map
        map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push($legendContainer[0]);

        // add onlick handle to generate and show the infoWindow (current transit status)
        map.data.addListener('click', function (event) {

            // look for the status that correspond to the given color
            var statusValue = colorValues.filter(function (value) {
                return (value.color == event.feature.getProperty("color"))
            });

            // look for the current item (highway point of measure) for showing its info in the box
            var item = segments.filter(function (value) {
                return (value.Name == event.feature.getProperty("name"))
            });

            // concatenate the message and show it in the infowindow  
            var itemDescription = '<strong>From:</strong> ' + item[0].NameStart + '<br /><strong>To:</strong> ' + item[0].NameEnd;
            itemDescription += (item[0].Detail) ? '<br />(' + item[0].Detail + ')' : ' ';

            infoWindow.setContent('<div id="dialog-box-title">Current Transit Status</div><div style="line-height:1.35;overflow:hidden;white-space:nowrap;">' +
                                       itemDescription + '<br/><strong>Status:</strong> <span style="background-color: ' + event.feature.getProperty("color") + '">&nbsp' + statusValue[0].status + '&nbsp</span></div>');

            // position the anchor
            var anchor = new google.maps.MVCObject();
            anchor.set("position", event.latLng);

            // zoom in & center the map
            var newZoom = map.getZoom() <= defaultZoom ? defaultZoomIn : map.getZoom();
            map.setZoom(newZoom);
            map.panTo(event.latLng);

            // open the infoWindow (current transit status)
            infoWindow.open(map, anchor);
        });

        hideLoading();
    });
}


function loadHighways() {

    $.getJSON(highwaysEndpointUrl, function (data) {
        // loop into each highway
        $.each(data, function (key, value) {
            highways.push(value.HighwayId, value);
        });
    });
}

function renderHighwayinMap(id) {
    var item = maps.filter(function (map) {
        return (map.highwayId == id)
    });

    // if segments of this highway are not added to the collection of maps, I do that 
    if (item == "") {
        var result = '{"type": "FeatureCollection","features": [';
        for (var i = 0, len = mapDataSegments.length; i < len; i++) {
            var item = mapDataSegments[i];
            if (item.HighwayId === id) {
                result += item.Value + ',';
            }
        }
        var result2 = result.substring(0, result.length - 1);
        result2 += ']}';

        var parsed = JSON.parse(result2);

        // add all the segments from a given Highway to a map collection
        var features = map.data.addGeoJson(parsed);

        maps.push({ 'highwayId': id, 'map': features });
    }
    else
    {
        console.log('Already invited to the party bro!');
    }
}

$(document).ready(function() {
    $("#btn-au1").click(function() {
        renderHighwayinMap(1);
    });

    $("#btn-au6").click(function () {
        renderHighwayinMap(2);
    });

    $("#btn-aud").click(function () {
        renderHighwayinMap(3);
    });

    $("#btn-aui").click(function () {
        renderHighwayinMap(4);
    });

    $("#btn-au9").click(function () {
        renderHighwayinMap(5);
    });

    $("#btn-all").click(function () {
        renderHighwayinMap(1);
        renderHighwayinMap(2);
        renderHighwayinMap(3);
        renderHighwayinMap(4);
        renderHighwayinMap(5);
    });

    $("#btn-none").click(function () {
        maps = new Array(); // remove all the maps (of highways) in the collection

        // remove all the features in the whole map
        map.data.forEach( function(feature) {
            map.data.remove(feature);
        });
    });

    $("#btn-show-signs").click(function () {
        if (areSignsVisible == false) {
            loadSigns();

            changeSignsButtonText('Hide signs');
            areSignsVisible = true;
        }
        else {
            hideSigns();

            changeSignsButtonText('Show signs');
            areSignsVisible = false;
        }
    });
});

function loadSigns() {

    this.showLoading();

    $.getJSON(signsEndopintUrl, function (data) {
        var buenosAires = new google.maps.LatLng(-34.61530, -58.51550);
        
        // pre-set the infoWindow
        var infoWindow = new google.maps.InfoWindow(
            {
                content: '',
                size: new google.maps.Size(10, 30)
            });

        // loop into each sign
        $.each(data, function (key, value) {

            var json = JSON.parse(value.HighwaySign.GeoJson);

            var lat = json.coordinates[1];
            var long = json.coordinates[0];
            var currentLatlng = new google.maps.LatLng(lat, long);

            // create a marker for the current sign
            var marker = new google.maps.Marker({
                position: currentLatlng,
                map: map,
                title: value.HighwaySign.Name
            });

            pins.push(marker);

            // add onlick handle to generate and show the infoWindow (current sign status)
            google.maps.event.addListener(marker, 'click', function (event) {

                var description = value.HighwaySign.Description + ' Sentido: ' + value.HighwaySign.Direction + ' (' + value.HighwaySign.Location + ')';

                infoWindow.setContent('<div id="dialog-box-title">Current Sign Status</div><div style="line-height:1.35;overflow:hidden;white-space:nowrap;">' +
                                           description + '<br/><strong>Status:</strong> <span style="background-color: white">&nbsp' + value.Status + '&nbsp</span></div>');

                // zoom in & center the map
                var newZoom = map.getZoom() <= defaultZoom ? defaultZoomIn : map.getZoom();
                map.setZoom(newZoom);
                map.panTo(event.latLng);

                // open the infoWindow (current sign status)
                infoWindow.open(map, marker);
            });

            signs.push(value);
        });

        hideLoading();
    });
}

function hideSigns() {
    for (var i = 0; i < pins.length; i++) {
        pins[i].setMap(null);
    }
    pins = [];
}


function changeSignsButtonText(text) {
    $("#btn-show-signs").text(text);
}