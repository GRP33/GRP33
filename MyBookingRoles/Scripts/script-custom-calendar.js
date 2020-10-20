import { timeFormat } from "d3";

$(document).ready(function () {
    $('#calendar').fullCalendar({
        header:
        {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },

        events: function (start, end, timezone, callback) {
            $.ajax({
                url: '/ArtistCalendar/GetEvents',
                type: "GET",
                dataType: "JSON",

                success: function (result) {
                    var events = [];

                    $.each(result, function (i, data) {
                        events.push(
                            {
                                title: data.Artist.ArtistName,
                                description: data.AplicationUser.Username,
                                start: moment(data.Start_Date).format('YYYY-MM-DD'),
                                time: moment(data.TimeBlockHelper),
                                end: moment(data.Duration).format('HH-mm'),
                                backgroundColor: "#9501fc",
                                borderColor: "#fc0101"
                            });
                    });

                    callback(events);
                }
            });
        },

        eventRender: function (event, element) {
            element.qtip(
                {
                    content: event.description
                });
        },

        editable: false
    });
}); 