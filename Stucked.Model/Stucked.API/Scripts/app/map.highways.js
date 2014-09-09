
var segments = [];
var highways = [];
var mapDataSegments = [];

var colorValues = [
    { 'color': 'green', 'status': 'Smooth' },
    { 'color': 'yellow', 'status': 'Slow' },
    { 'color': 'orange', 'status': 'Delayed' },
    { 'color': 'red', 'status': 'Stucked' }
];

var defaultZoom = 12;
var defaultZoomIn = 14;
var map;

var segmentsEndpointUrl = 'API/Segment';
var highwaysEndpointUrl = 'API/Highway';


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
            highways.push(value);
        });
    });
}

function renderHighwayinMap(id) {
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
        
    map.data.addGeoJson(parsed);
}

$(document).ready(function() {
    $("#btn-au1").click(function() {
        console.log('Show AU-1 segments only.');

        renderHighwayinMap(1);
    });

    $("#btn-au6").click(function () {
        console.log('Show AU-6 segments only');

        renderHighwayinMap(2);
    });

    $("#btn-aud").click(function () {
        console.log('Show AU-DELLEPIANE segments only');

        renderHighwayinMap(3);
    });

    $("#btn-aui").click(function () {
        console.log('Show AU-ILLIA segments only');

        renderHighwayinMap(4);
    });

    $("#btn-au9").click(function () {
        console.log('Show AU-9 JULIO segments only');

        renderHighwayinMap(5);
    });

    $("#btn-all").click(function () {
        //map.data.LoadGeoJson(mapDataSegments);
    });

    $("#btn-none").click(function () {
        console.log('Hide all segments.');

        map.data.setMap(null);
    });
});