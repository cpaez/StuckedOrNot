﻿
var segments = [];
var highways = new Array();
var signs = [];

var stuckedSegments = new Array();
var delayedSegments = new Array();

var areSignsVisible = false;
var pins = [];

var mapDataSegments = [];
var maps = new Array();

var map;
var infoWindow;

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


function createMap() {
    var buenosAires = new google.maps.LatLng(-34.619980, -58.4450);

    // Create a simple map
    map = new google.maps.Map(document.getElementById('map-canvas'), {
        zoom: defaultZoom,
        center: buenosAires
    });

    // Load segments style.
    map.data.setStyle(function (feature) {
        return /** @type {google.maps.Data.StyleOptions} */({
            strokeColor: feature.getProperty('color'),
            strokeWeight: 3
        });
    });

    // pre-set the infoWindow
    infoWindow = new google.maps.InfoWindow(
    {
        content: '',
        size: new google.maps.Size(10, 30)
    });

    // create map legend (colors per status)
    this.createMapLegend();
}

function createMapLegend() {
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
}

function updateHighwaysMap() {
    segments = [];
    mapDataSegments = [];

    stuckedSegments = new Array();
    delayedSegments = new Array();

    $.blockUI({ message: '<h3><img src="Images/busy.gif" /> Loading...</h3><br />' });

    // call API service endpoint to get the whole list of segments
    $.getJSON(segmentsEndpointUrl, function (data) {

        // loop into each segment
        $.each(data, function (key, value) {
            var item = value.Segment;

            var json = JSON.parse(item.GeoJson);

            var mapData = [];
            mapData.HighwayId = item.HighwayId;
            mapData.Key = key;
            mapData.Value = item.GeoJson;

            mapDataSegments.push(mapData);

            // select stucked and delayed segments
            if (value.Status == 'Stucked')
                stuckedSegments.push({ 'highwayId': item.HighwayId, 'Segment': item });

            if (value.Status == 'Delayed')
                delayedSegments.push({ 'highwayId': item.HighwayId, 'Segment': item });

            //map.data.addGeoJson(json, value.Name);
            segments.push(item);
        });

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

        // render Detailed Info
        showDetailedInfo();

        $.unblockUI;
    });
}

function initializeHighwaysMap() {
    this.createMap();

    // load highways calling the service API
    this.loadHighways();

    // set up timer to automatically update the map info
    this.setUpdateTimer();

    // get the map info calling the service API
    this.updateHighwaysMap();
}

function setUpdateTimer() {
    var interval = 60000 * 5; // 5 minutes

    var updateTimer = window.setInterval(function () {
        updateHighwaysMap();
    }, interval);
}

function loadHighways() {

    $.getJSON(highwaysEndpointUrl, function (data) {
        // loop into each highway
        $.each(data, function (key, value) {
            highways.push({ 'Id': value.HighwayId, 'Highway': value });
        });

        showHighwayButtons();
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

$(document).ready(function () {

    $("#view-initial-map").click(function () {
        setMapInitialPositionAndZoom();
    });

    $("#refresh-status").click(function () {
        updateHighwaysMap();
    });

    $("#btn-all").click(function () {
        for (var i = 0; i < highways.length; i++) {
            var highway = highways[i].Highway;
            renderHighwayinMap(highway.HighwayId);
        }
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

    $.blockUI({ message: '<h3><img src="Images/busy.gif" /> Loading...</h3><br />' });

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

        $.unblockUI;
    });
}

function hideSigns() {
    for (var i = 0; i < pins.length; i++) {
        pins[i].setMap(null);
    }
    pins = [];
}

function showHighwayButtons() {
    for (var i = 0; i < highways.length; i++) {
        var highway = highways[i].Highway;
        var buttonId = "btn-au" + highway.HighwayId;
        
        // create and add highways buttons
        var button = $("<input/>", { type: "button", id: buttonId, value: highway.Code });
        button.click(getEventHandlerFunction(highway.HighwayId));
        $('#buttons').append(button);
    }
}

function getEventHandlerFunction(id) {
    return function () {
        renderHighwayinMap(id);
    };
}

function showDetailedInfo() {
    // stucked segments
    this.setStuckedMessage();

    // delayed segments
    this.setDelayedMessage();

    // most stucked highway
    this.setMostStuckedMessage();
}

function changeSignsButtonText(text) {
    $("#btn-show-signs").text(text);
}

function setStuckedMessage() {
    if (stuckedSegments.length > 0) {
        $("#btn-show-stucked").hide(); // change to show when the functionallity of displaying independent segments is implemented
        $("#msg-stucked").text("- There are " + stuckedSegments.length + " stucked segments.");
    }
    else {
        $("#btn-show-stucked").hide();
        $("#msg-stucked").text("- There are no stucked segments.");
    }
}

function setDelayedMessage() {
    if (delayedSegments.length > 0) {
        $("#btn-show-delayed").hide(); // change to show when the functionallity of displaying independent segments is implemented
        $("#msg-delayed").text("- There are " + delayedSegments.length + " delayed segments.");
    }
    else {
        $("#btn-show-delayed").hide();
        $("#msg-delayed").text("- There are no delayed segments.");
    }
}

function setMostStuckedMessage() {
    if (stuckedSegments.length == 0 && delayedSegments.length == 0) {
        $("#btn-most-stucked").hide();
        $("#msg-most-stucked").text("- Huray! No highways stucked nor delayed. It seems you'll have a nice trip.");
    }
    else {
        var stuckedId = 1;

        var modeStucked = mode(stuckedSegments);
        var modeDelayed = mode(delayedSegments);

        if (stuckedSegments.length == 0) {
            stuckedId = modeDelayed.highwayId;
        }
        else if (delayedSegments.length == 0) {
            stuckedId = modeStucked.highwayId;
        }
        else if (stuckedSegments.length > delayedSegments.length) {
            stuckedId = modeStucked.highwayId;
        }
        else {
            stuckedId = modeStucked.highwayId;
        }

        // get the name
        var stuckedHighway = highways.filter(function (highway) {
            return (highway.Id == stuckedId)
        });

        $("#btn-most-stucked").show();
        $("#btn-most-stucked").click(function () {
            renderHighwayinMap(stuckedId);
        });

        if (stuckedHighway.length > 0) {
            var mostStuckedMessage = "(" + stuckedHighway[0].Highway.Code + ") " + stuckedHighway[0].Highway.Name;
            $("#msg-most-stucked").text("- The most stucked highway is: " + mostStuckedMessage + ". Please avoid to use it. If you still have to go through it, take your precautions.");
        }
        else {
            $("#msg-most-stucked").text("- Huray! No highways stucked nor delayed. It seems you'll have a nice trip.");
        }
    }
}

function setMapInitialPositionAndZoom()
{
    map.setZoom(defaultZoom);
    map.setCenter(new google.maps.LatLng(-34.619980, -58.4450));
}

function blockElement(element)
{
    console.log(element);
    $(element).block({
        message: '<h5><img src="Images/busy.gif" /> Loading...</h5>'
    });
}

function unblockElement(element) {
    $(element).unblock();
}


function mode(array) {
    if (array.length == 0)
        return null;
    var modeMap = {};
    var maxEl = array[0], maxCount = 1;
    for (var i = 0; i < array.length; i++) {
        var el = array[i];
        if (modeMap[el] == null)
            modeMap[el] = 1;
        else
            modeMap[el]++;
        if (modeMap[el] > maxCount) {
            maxEl = el;
            maxCount = modeMap[el];
        }
    }
    return maxEl;
}