//Pages Calendar - Version 1.2.0
(function ($) {
    'use strict';

    $.fn.pagescalendar = function (method) {
        var methods = {

            init: function (options) {
                this.pagescalendar.settings = $.extend({}, this.pagescalendar.defaults, options);
                return this.each(function () {
                    var $element = $(this),
                        element = this;
                });
            },

            today: function () {
                helpers.today();
            },
            next: function () {
                helpers.nextMonth();
            },
            prev: function () {
                helpers.previousMonth();
            },
            setDate: function (date) {
                helpers.setDate(date);
            },
            getDate: function (format) {
                return helpers.getDate(format);
            },
            render: function () {
                helpers.checkOptionsAndBuild();
            },
            destroy: function () {
                //TODO
            },
            setLocale: function (lang) {
                settings.locale = lang;
                helpers.setLocale();
            },
            reloadEvents: function () {
                helpers.loadEvents();
            },
            addEvent: function (event) {
                helpers.addEvent(event);
            },
            removeEvent: function (index) {
                helpers.deleteEvent(index)
            },
            updateEvent: function (index, eventObj) {
                helpers.updateEvent(index, eventObj);
            },
            getEvents: function (option) {
                return helpers.getEventArray(option);
            }
        }
        //Private Vars
        var settings = $.fn.pagescalendar.settings;
        var content;
        var daysOfMonth;
        var setActive;
        var timeFormat = settings.timeFormat;
        var globalEventList;
        var cellHeight;
        var startOfWeekDate;
        var calendar = $.fn.pagescalendar.defaults.calendar;
        var helpers = {
            /**
             * Main Init function
             */
            checkOptionsAndBuild: function () {
                if (settings.ui.year == null || settings.ui.month == null || settings.ui.date == null || settings.ui.week == null || settings.ui.grid == null) {
                    alert("You have not included the proper ui[] settings, please refer docs");
                }
                this.setLocale();
                calendar.monthLong = moment().format(settings.ui.month.format);

                if (settings.now != null) {
                    calendar.month = moment($.fn.pagescalendar.settings.now).month();
                    calendar.year = moment($.fn.pagescalendar.settings.now).year();
                    calendar.date = moment($.fn.pagescalendar.settings.now).format("D");
                    calendar.dayOfWeek = moment($.fn.pagescalendar.settings.now).day();
                    calendar.monthLong = moment($.fn.pagescalendar.settings.now).format('MMM');
                } else {
                    calendar.month = moment().month();
                    calendar.year = moment().year();
                    calendar.date = moment().format("D");
                    calendar.dayOfWeek = moment().day();
                    calendar.monthLong = moment().format('MMM');
                }

                this.buildYears();
                this.buildMonths();
               
                $.fn.pagescalendar.settings.onViewRenderComplete.call(this);
                this.bindEventHanders();
                // this.loadEvents();
                // this.setEventBubbles(settings.events);
                // this.autoFocusActiveElement();
                helpers.scrollToElement('#weeks-wrapper .active')
            },

            /**
             * Render Years
             */
            buildYears: function () {
                var container = "#years";
                $(container).html("");
                content = "";
                var diffYears = settings.ui.year.endYear - settings.ui.year.startYear
                diffYears = (diffYears > 90) ? 90 : diffYears;
                var yearInc = settings.ui.year.startYear;

                for (var i = 1; i <= diffYears; i++) {
                    yearInc = moment(yearInc, settings.ui.year.format).add(1, 'year').format(settings.ui.year.format);
                    var activeClass = (calendar.year == yearInc) ? 'active' : '';
                    content += '<div class="year">';
                    content += '<a href="#" class="year-selector ' + activeClass + '" data-year=' + yearInc + '>' + yearInc + '</a>';
                    // calendar.loadedYears.push(yearInc);
                    content += '</div>';
                }

                $(container).append(content);
                this.dragHandler('years');


            },

            /**
             * Render Months
             */
            buildMonths: function () {
                var container = "#months";
                $(container).html("");
                content = "";
                var monthInc = moment(moment([calendar.year, calendar.month, calendar.date]).startOf('year'), 'YYYY-MM').format(settings.ui.month.format);
                var monthInc1 = moment(moment([calendar.year, calendar.month, calendar.date]).startOf('year'), 'MMMM').format('MMMM');
                for (var i = 1; i <= 12; i++) {
                    var activeClass = (moment([calendar.year, calendar.month, calendar.date]).format(settings.ui.month.format) == monthInc) ? 'active' : '';
                    content += '<div class="month">';
                    content += '<a href="#" class="month-selector ' + activeClass + '" data-month="' + monthInc + '">' + monthInc1 + '</a>';
                    content += '</div>';
                    monthInc = moment(monthInc, settings.ui.month.format).add(1, 'month').format(settings.ui.month.format);
                    monthInc1 = moment(monthInc1, 'MMMM').add(1, 'month').format('MMMM');
                }
                $(container).append(content);

                $('.month-selector').on('click', function () {

                    var month = $(this).attr('data-month');
                    calendar.month = moment(month, settings.ui.month.format).month();
                  //  helpers.renderViewsOnDateChange();
                    helpers.highlightMonth();
                    month = month.split('-');

                    var startDate = moment(month[0] + "-" + month[1] + "-" + 1).format('YYYY-MM-D');
                    var enddate = moment(startDate).endOf('month').format('YYYY-MM-D');


                    generateReport(startDate, enddate);
                    //generateReport();
                });
                this.dragHandler('months');
            },
            /**
             * Higlight Month On Date Change
             */
            highlightMonth: function () {
                var container = "#months";
                $('.month a').removeClass('active');
                $('.month:nth-child(' + (parseInt(calendar.month) + 1) + ') > a').addClass('active');
            },
            /**
             * Higlight Year On Date Change
             */
            highlightYear: function () {
                var diff = calendar.year - settings.ui.year.startYear
                var container = "#years";
                $('.year a').removeClass('active');
                $('.year:nth-child(' + diff + ') > a').addClass('active')
            },
            /**
             * Display Date
             */
            buildCurrentDateHeader: function () {
              //  $("#currentDate").text(moment([calendar.year, calendar.month, calendar.date]).format(settings.ui.date.format));
            },

            /**
             * Touch Drag Handler
             * @param {string} element
             */
            dragHandler: function (element) {
                //Setup Drag Handler
                if ($('body').hasClass('mobile'))
                    return
                if ($('#' + element).length != 1)
                    return

                $('.drager').scrollbar();
                var myElement = document.getElementById(element);
                var hammertime = new Hammer(myElement);
                var element = $('#' + element).parent();
                var lP = element.scrollLeft();
                var inverseX;
                hammertime.on('panmove', function (ev) {
                    inverseX = -(ev.deltaX);
                    element.scrollLeft(inverseX + lP);
                    //lP=0;
                });
                hammertime.on('panend pancancel', function (ev) {
                    lP = element.scrollLeft();
                })
            },
            autoFocusActiveElement: function () {
                var timer;
                $(window).resize(function () {
                    clearTimeout(timer);
                    timer = setTimeout(function () {
                        helpers.scrollToElement('#weeks-wrapper .active');
                    }, 500);
                });
            },

            scrollToElement: function (el) {
                el = $(el);
                if (!el.length != 0)
                    return
                var par = $(el).parent();
                var t = helpers.isElementInViewport(el);
                //console.log(t)
                if (!t) {
                    var elOffset = el.offset().left;
                    var elHeight = par.children().width();
                    var windowHeight = $(window).width();
                    var offset;

                    if (elHeight < windowHeight) {
                        offset = elOffset - ((windowHeight / 2) - (elHeight / 2));
                    } else {
                        offset = elOffset;
                    }
                    $('#weeks-wrapper').parent().animate({
                        scrollLeft: offset
                    }, 10);
                }

            },

            isElementInViewport: function (el) {
                if (typeof jQuery === "function" && el instanceof jQuery) {
                    el = el[0];
                }
                var rect = el.getBoundingClientRect();
                return (
                    rect.top >= 0 &&
                    rect.left >= 0 &&
                    rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) && /*or $(window).height() */
                    rect.right <= (window.innerWidth || document.documentElement.clientWidth) /*or $(window).width() */
                );
            },

            /**
             * Render Week
             */
            buildWeek: function () {
                var container = ".weeks-wrapper";
                $(container).html("");
                daysOfMonth = moment([calendar.year, calendar.month]).daysInMonth();
                content = "";
                $("#popups").html("");
                for (var i = 1; i <= daysOfMonth; i++) {
                    var t = moment([calendar.year, calendar.month, i]).format('ddd');
                    var activeClass = (calendar.date == i) ? 'active current-date' : '';

                    (t == moment(settings.ui.week.startOfTheWeek, 'd').format('ddd') || i == 1) ? content += '<table class="week-date ' + activeClass + '"><tr><td class="week"><div class="dateheader1 dateheader">Sector Name</div><table class="sectorNames"><tbody></tbody></table></td><td class="week ' + activeClass + '"><table class=""><tr> ' : '';
                    content += '<td id="date' + i + '" class=""><div class="dateheader">' + moment([calendar.year, calendar.month, i]).format('dddd D-MMMM ') + '</div><table class="sectorVal"></table></td>';


                    (t == moment(settings.ui.week.endOfTheWeek, 'd').format('ddd')) ? content += '</tr></table></td></tr></table>' : '';

                    var popup = "<div class=\"head hide\">PNR's</div><div class=\"content hide\"></div>";
                    $("#popups").append("<div class='popup" + i + "'>" + popup + "</div>")
                }
                content += '</div>';

                $(container).append(content);
                $('.weeks-wrapper .week .day-wrapper .week-date.active').closest(".week").addClass('active');
                this.dragHandler('weeks-wrapper');
            },

            /**
             * Highlight Selected Week
             * @param {jobject} elem
             */
            highlightWeek: function (elem) {
                $('.week').removeClass('active');
                $(elem).closest('.week').addClass('active');
            },
            /**
             * Show Event Bubble on Top
             * @param {array} eventArray
             */
            setEventBubbles: function (eventArray) {
                $('.has-event').removeClass('has-event');
                $('#weekViewTableHead .event-bubble').remove();
                for (var item in eventArray) {
                    var eventYear = moment(eventArray[item].start).format(settings.ui.year.format);
                    var eventMonth = moment(eventArray[item].start).format(settings.ui.month.format);
                    var eventDate = moment(eventArray[item].start).format('D');
                    $('.year > [data-year="' + eventYear + '"]').addClass('has-event');

                    if (calendar.year == moment(eventArray[item].start).format("YYYY")) {
                        $('.month > [data-month="' + eventMonth + '"]').addClass('has-event');

                        if (calendar.month + 1 == moment(eventArray[item].start).format("M")) {
                            $('.date-selector > .week-date > .day > [data-date="' + eventDate + '"]').addClass('has-event');
                            var toheaderDate = moment(eventArray[item].start).format('YYYY-MM-DD')
                            var headerDate = $("#weekViewTableHead").find('.thead > [data-day="' + toheaderDate + '"]');
                            if (eventArray[item].class != null) {
                                if (headerDate.length != 0) {
                                    if (headerDate.children('.' + eventArray[item].class).length == 0) {
                                        headerDate.append('<div class="event-bubble ' + eventArray[item].class + '"></div>');
                                    }
                                }
                            }
                        }
                    }
                }
            },

            /**
             * Render Week View
             */
          


            /**
             * Higlight Active Week On Weekview/ view
             */
            highlightActiveDay: function () {
                $('#weekViewTableHead').find('.thead .tcell').removeClass('active');
                $('#weekGrid').find('.trow .tcell').removeClass('active');

                var d = moment([calendar.year, calendar.month, calendar.date]).day();
                $('#weekViewTableHead').find('.thead .tcell:nth-child(' + (d + 1) + ')').addClass('active');
                $('#weekGrid').find('.trow .tcell:nth-child(' + (d + 1) + ')').addClass('active');
            },

            /**
             * Util Calendar Functions
             */
            nextMonth: function () {
                currentYear = moment([calendar.year, calendar.month, calendar.date]).add(1, 'months').year();
                currentMonth = moment([calendar.year, calendar.month, calendar.date]).add(1, 'months').month();
                this.setHeaderDate();
                this.buildMiniCalender();
                this.loadDatesToWeekView();
                this.highlightActiveDay();
                this.loadEvents();
            },
            previousMonth: function () {
                currentYear = moment([calendar.year, calendar.month, calendar.date]).subtract(1, 'months').year();
                currentMonth = moment([calendar.year, calendar.month, calendar.date]).subtract(1, 'months').month();
                this.setHeaderDate();
                this.buildMiniCalender();
                this.loadDatesToWeekView();
                this.highlightActiveDay();
                this.loadEvents();
            },
            today: function () {
                calendar.year = moment().year();
                calendar.month = moment().month();
                calendar.date = moment().format("D");
                this.setHeaderDate();
                this.buildMiniCalender();
                this.highlightWeek();
                this.loadDatesToWeekView();
                this.highlightActiveDay();
            },

            /**
             * Add Event To Array
             * @param {object} event
             */
            addEvent: function (event) {
                settings.events.push(event);
                this.loadEvents();
            },

            /**
             * Delete Event From Array
             * @param {id} index
             */
            deleteEvent: function (index) {
                settings.events.splice(parseInt(index), 1);
                this.loadEvents();
            },
            /**
             * Delete Event From Array
             * @param {id} index
             * @param {object} eventObj
             */
            updateEvent: function (index, eventObj) {
                settings.events[index] = eventObj;
                this.loadEvents();
            },
            /**
             * Load Events From Array
             */
            loadEvents: function () {
            },

            /**
             * Render Event To View
             * @param {int} eventStartHours
             * @param {int} eventStartMins
             * @param {int} eventEndHours
             * @param {int} evenEndMins
             * @param {int} cellNo
             * @param {int} arrayIndex
             * @param {int} eventDuration
             */
            drawEvent: function (eventStartHours, eventStartMins, eventEndHours, evenEndMins, cellNo, arrayIndex, eventDuration) {

                var rowMap = -(settings.minTime - eventStartHours);

                if (settings.maxTime < eventEndHours) {
                    eventEndHours = settings.maxTime;
                }
                if (rowMap < 0) {
                    console.log(rowMap)
                    eventStartHours = settings.minTime;
                    rowMap = 0;
                }

                cellHeight = $('.tcell').innerHeight();
                evenEndMins = parseInt(evenEndMins);
                var container = $('#weekGrid');

                var row = container.find('.trow:nth-child(' + (parseInt(rowMap) + 1) + ')');
                var cell;
                var top, height;
                height = cellHeight;
                if (settings.slotDuration == '30')
                    height = height * 2;
                //TODO ADD MORE OPTION FOR settings.slotDuration LIKE 15/30 mins options

                if (settings.slotDuration == '30') {
                    if (parseInt(eventStartMins) >= 30) {

                        eventStartMins = 30;

                        cell = row.find('.tcell:nth-child(' + (parseInt(cellNo) + 1) + ')').children('.cell-inner:nth-child(2)');
                    } else {
                        eventStartMins = 0
                        cell = row.find('.tcell:nth-child(' + (parseInt(cellNo) + 1) + ')').children('.cell-inner:nth-child(1)');
                    }
                    var diff = moment(settings.events[arrayIndex].end).diff(settings.events[arrayIndex].start, 'hours', true)
                    height = height * diff;
                } else {
                    cell = row.find('.tcell:nth-child(' + (parseInt(cellNo) + 1) + ')').children('.cell-inner');
                    //Time gap`
                    var timeGap = parseInt(eventEndHours) - parseInt(eventStartHours);
                    if (timeGap == 0) {
                        timeGap = 1;
                    }
                    height = height * timeGap;
                }

                height = "height:" + height + "px;";
                var id = 'ca_' + moment(settings.events[arrayIndex].start).unix() + arrayIndex;
                var eventContent = "<div class='event-container " + settings.events[arrayIndex].class + "' data-event-duration=" + eventDuration + " data-index=" + arrayIndex + " data-startTime=" + settings.events[arrayIndex].start + " data-endTime=" + settings.events[arrayIndex].end + " id=" + id + " data-id=" + id + " data-row=" + (parseInt(eventStartHours)) + " data-cell=" + cellNo + " style=" + height + ">"
                eventContent += "<div class='event-inner'>";
                eventContent += "<div class='event-title'>" + settings.events[arrayIndex].title + "</div>";
                eventContent += "<div class='time-wrap'><span class='event-start-time'>" + moment(settings.events[arrayIndex].start).format('h:mm') + "</span> - ";
                //console.log(settings.events[arrayIndex].end)

                eventContent += "<span class='event-end-time'>" + moment(settings.events[arrayIndex].end).format('h:mm') + "</span></div>";

                eventContent += "</div>"
                eventContent += "</div>"

                cell.append(eventContent);
                this.bindEventDraggers();
                settings.events[arrayIndex].dataID = id;
                $.fn.pagescalendar.settings.onEventRender.call(this);
            },

            /**
             * Apply Calenda Event Drag and Resize Options
             */
            bindEventDraggers: function () {
                var snapGridHeight = $('.cell-inner').outerHeight() / 2;
                var snapGridWidth = $('.tcell').outerWidth();
                var previousH, stepper;
                settings.slotDuration == '30' ? stepper = 30 : stepper = 60;
                $(".event-container")
                    .resizable({
                        handles: "s",
                        minHeight: 40,
                        grid: [snapGridWidth, 40],
                        start: function (event, ui) {
                            previousH = ui.size.height;
                        },
                        resize: function (event, ui) {
                            if (previousH > ui.size.height) {
                                var et = ui.element.attr('data-endtime');
                                var x = moment(et).subtract(stepper, 'minutes').format();
                                ui.element.attr('data-endtime', x);
                                ui.element.attr('data-event-duration', parseFloat(ui.element.attr('data-event-duration')) - (stepper / 60));
                                var elem = ui.element.children('.event-inner').children('.time-wrap').children('.event-end-time');
                                elem.html(moment(x).format('h:mm'));
                                previousH = ui.size.height;
                            } else {
                                var et = ui.element.attr('data-endtime');
                                var x = moment(et).add(stepper, 'minutes').format();
                                ui.element.attr('data-endtime', x);
                                ui.element.attr('data-event-duration', parseFloat(ui.element.attr('data-event-duration')) + (stepper / 60));
                                var elem = ui.element.children('.event-inner').children('.time-wrap').children('.event-end-time');
                                elem.html(moment(x).format('h:mm'));
                                previousH = ui.size.height;
                            }

                        },
                        stop: function (event, ui) {
                            var elem = ui.element;
                            var i = elem.attr('data-index');
                            settings.events[i].end = elem.attr('data-endtime');
                            var eventO = helpers.constructEventForUser(i);
                            helpers.setEventBubbles(settings.events);
                            $.fn.pagescalendar.settings.onEventResizeComplete(eventO);
                        }
                    });
                $('.cell-inner').sortable({
                    connectWith: '.cell-inner',
                    iframeFix: false,
                    items: '.event-container',
                    opacity: 0.8,
                    helper: 'original',
                    grid: [snapGridWidth, 40],
                    forceHelperSize: true,
                    placeholder: 'sortable-box-placeholder round-all',
                    forcePlaceholderSize: true,
                    tolerance: 'pointer',
                    greedy: true,
                    revert: 300,
                    over: function (event, ui) {
                        var elem = ui.placeholder.parent('.cell-inner');
                        var startTime = elem.attr("data-time-slot");

                        var date = $('#weekViewTableHead .thead .tcell:nth-child(' + (elem.parent().index() + 1) + ')').attr('data-day');
                        var duration = ui.helper.attr("data-event-duration");
                        var endTime = moment(startTime, ["h:mm"]).add(duration, 'hours').format('h:mm');
                        endTime = moment(date + endTime, 'YYYY/MM/D h:mm').format();

                        date = moment(date + startTime, 'YYYY/MM/D h:mm').format();

                        ui.helper.attr('data-starttime', date);
                        ui.helper.attr('data-endtime', endTime);

                        ui.helper.children('.event-inner').children('.time-wrap').children('.event-start-time').text(moment(ui.helper.attr('data-starttime')).format('h:mm'));
                        ui.helper.children('.event-inner').children('.time-wrap').children('.event-end-time').text(moment(ui.helper.attr('data-endtime')).format('h:mm'));

                    },
                    receive: function (event, ui) {
                        var elem = ui.item;
                        var i = elem.attr('data-index');
                        settings.events[i].start = elem.attr('data-starttime');
                        settings.events[i].end = elem.attr('data-endtime');
                        var eventO = helpers.constructEventForUser(i);
                        helpers.setEventBubbles(settings.events);
                        $.fn.pagescalendar.settings.onEventDragComplete(eventO);
                    }
                });
            },

            /**
             * Date Change Event Only for Horizontal Weeks
             */
            weekDateChange: function (day, elem) {
                calendar.date = day;
               // this.buildCurrentDateHeader();
                this.highlightWeek(elem);
                this.loadDatesToWeekView();
                this.highlightActiveDay();
                this.loadEvents();
                this.setEventBubbles(settings.events);
            },

            /**
             * Time Slot Double Click
             * @param {object} obj
             * @return {object} timeSlot
             */
            timeSlotDblClick: function (obj) {
                var elem = $(obj);
                var h = "00";

                if (elem.index() == 1) {
                    h = "30";
                }
                if (window.innerWidth <= 1024 || settings.view == 'day') {
                    var date = moment($('#weekViewTableHead .thead .tcell').attr('data-day')).format('YYYY/MM/D');
                }
                else {
                    var date = moment(calendar.startOfWeekDate).add(elem.parent().index(), 'days').format('YYYY/MM/D');
                }
                console.log(elem.attr('data-time-slot'))
                var time = moment(elem.attr('data-time-slot'), ["H:mm"]).format(' H:mm');
                date = moment(date + time, 'YYYY/MM/D h:mm').format();
                var timeSlot = {
                    date: date,
                }

                $.fn.pagescalendar.settings.onTimeSlotDblClick(timeSlot);
            },

            bindEventHanders: function () {
                $(document).on("click", ".date-selector", function (e) {
                    $(".week-date").removeClass('active')
                    $(this).children('.week-date').addClass('active');
                    helpers.weekDateChange(parseInt($(this).children('.week-date').children('.day').children('a').attr('data-date')), $(this));
                });

                $(document).on("dblclick doubletap", ".cell-inner", function () {
                    helpers.timeSlotDblClick(this);
                });
                $(document).on('click', '.event-container', function () {
                    var eventO = helpers.constructEventForUser($(this).attr('data-index'));
                    $.fn.pagescalendar.settings.onEventClick(eventO);
                });
                $('.year-selector').on('click', function () {
                    var year = $(this).attr('data-year');
                    calendar.year = moment(year, settings.ui.year.format).year();

                    helpers.highlightYear();
                    helpers.renderViewsOnDateChange();
                    stdate = year + stdate.slice(4)
                    etdate = year + etdate.slice(4)
                    console.log(stdate);
                    generateReport(stdate, etdate);


                });


                //$(".event-container").resizable();
            },

            //MISC.
            constructEventForUser: function (i) {
                var eventO = {
                    index: i,
                    title: settings.events[i].title,
                    class: settings.events[i].class,
                    start: settings.events[i].start,
                    end: settings.events[i].end,
                    allDay: settings.events[i].allDay,
                    other: settings.events[i].other
                };
                return eventO
            },
            renderViewsOnDateChange: function () {
                helpers.buildMonths();
             
                helpers.bindEventHanders
                helpers.loadEvents();
            },
            setLocale: function () {
                var currentLang = settings.locale;
                moment.locale(currentLang);
            },
            setDate: function (d) {
                calendar.month = moment(d).month();
                calendar.year = moment(d).year();
                calendar.date = moment(d).format("D");
                calendar.dayOfWeek = moment(d).day();
                calendar.monthLong = moment(d).format('MMM');
                this.checkOptionsAndBuild();
            },
            getDate: function (format) {
                if (format == null) {
                    format = 'MMMM Do YYYY'
                }
                return moment([calendar.year, calendar.month, calendar.date]).format(format);
            },
            getEventArray: function (option) {
                if (option == null || option == 'all')
                    return settings.events;
            },
        }

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method "' + method + '" does not exist in pagescalendar plugin!');
        }

    }

    $.fn.pagescalendar.defaults = {
        ui: {
            year: {
                visible: true,
                format: 'YYYY',
                startYear: '2014',
                endYear: moment().add(10, 'year').format('YYYY'),
                eventBubble: true
            },
            month: {
                visible: true,
                format: 'YYYY-MM',
                eventBubble: true
            },
            date: {
                format: 'MMMM YYYY, D dddd'
            },
            week: {
                day: {
                    format: 'D'
                },
                header: {
                    format: 'dd'
                },
                eventBubble: true,
                startOfTheWeek: '0',
                endOfTheWeek: '6'
            },
            grid: {
                dateFormat: 'D dddd',
                timeFormat: 'h A',
                eventBubble: true,
                slotDuration: '30'
            }
        },
        header: {
            visible: true,
            dateFormat: 'MMM YYYY'
        },
        miniCalendar: {
            visible: true,
            highlightWeek: true,
            showEventBubbles: true
        },
        eventObj: {
            editable: true
        },
        view: 'week',
        now: null,
        locale: 'en',
        timeFormat: 'h a',
        minTime: 0,
        maxTime: 24,
        dateFormat: 'MMMM Do YYYY',
        slotDuration: '30', //In Mins : only supports 30 and 60
        events: [],
        //Event CallBacks
        onViewRenderComplete: function () { },
        onEventDblClick: function () { },
        onEventClick: function (event) { },
        onEventRender: function () { },
        onEventDragComplete: function (event) { },
        onEventResizeComplete: function (event) { },
        onTimeSlotDblClick: function (timeSlot) { }

        /*
        TO DO 
        eventDragStart (callback)
        eventResizeStart (callback)
        eventMouseover (callback)
        eventMouseout (callback)
        */

    }
    $.fn.pagescalendar.defaults.calendar = {
        year: moment().year(),
        date: moment().format("D"),
        month: moment().month(),
        monthLong: '',
        dayOfWeek: moment().day() - 1,
        daysOfMonth: 0,
        startOfWeekDate: null,
        timeFormat: '',
        loadedYears: {}
    }
    $.fn.pagescalendar.settings = {}

})(jQuery);