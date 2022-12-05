var startDate;
var endDate;
var myVar;
var course = "CS 1400";
var diffDate;
var date1;
var date2;
var queryDate = [];
var queryEnrollment = [];
const map = new Map();

$(function () {
    $("#datepicker1").datepicker({ minDate: "11/15/2022", maxDate: 'today' });
    $("#datepicker1").on("change", function () {
        startDate = $(this).val();
        var d1 = $('#datepicker1').datepicker('getDate');
        date1 = d1.getMonth() + "/" + d1.getDate();
        
    });
    $("#datepicker2").datepicker({ minDate: "11/15/2022", maxDate: 'today' });
    $("#datepicker2").on("change", function () {
        endDate = $(this).val();
        var d2 = $('#datepicker2').datepicker('getDate');
        date2 = d2.getMonth() + "/" + d2.getDate();
    });
});

function myNewFunction(sel) {
    course = sel.options[sel.selectedIndex].text;
}


function getData() {
    if (startDate == null) {
        alert("Please choose a start date")
    }
    else if (endDate == null) {
        alert("Please choose a end date")
    }
    else if (startDate > endDate) {
        alert("Invaild chosen date")
    } else {
        if (startDate + endDate != map.get(course)) {
            const elements1 = document.getElementsByClassName('loader');
            const elements2 = document.getElementById('container');
            elements1[0].style.display = 'block';
            elements2.style.display = 'none';
            myVar = setTimeout(showPage, 2000);
        }
    }

}

function showPage() {
    const elements1 = document.getElementsByClassName('loader');
    const elements2 = document.getElementById('container');
    var d1 = $('#datepicker1').datepicker('getDate');
    var d2 = $('#datepicker2').datepicker('getDate');

    diffDate = Math.floor((d2.getTime() - d1.getTime()) / 86400000);

    var c = course.split(" ");

    var enrollment = {
        startDate: startDate,
        endDate: endDate,
        dept: c[0],
        courseNumber: c[1]
    }

    $.ajax({
        type: "post",
        url: "/Enrollments/GetEnrollmentData",
        data: enrollment,
        content: "application/json;",
        dataType: "json",
        success: function (data) {
            var d = data.date[0].split(" ");
            var month = 0;
            var date_number = parseInt(d[1]);
            if (d[0] == "Nov") {
                month = 10;
            }
            $("#container").highcharts().addSeries(
                {
                    name: course,
                    data: data.enrollment,
                    pointStart: Date.UTC(2022, month, date_number),
                    pointInterval: 86400000
                });
        }
    });

    elements1[0].style.display = 'none';
    elements2.style.display = 'block';

    map.set(course, startDate + endDate);
}


Highcharts.chart('container', {
    chart: {
        type: 'spline'
    },
    title: {
        text: 'Enrollments Over Time'
    },
    subtitle: {
        text: 'Enrollments Over Time'
    },
    xAxis: {
        title: {
            text: 'Dates'
        },
        type: "datetime",
        labels: {
            formatter: function () {
                return Highcharts.dateFormat('%b/%e', this.value);
            }
        }
    },
    yAxis: {
        title: {
            text: 'Students Enrollments'
        },
        labels: {
            formatter: function () {
                return this.value;
            }
        }
    },
    tooltip: {
        crosshairs: true,
        shared: true
    },
    plotOptions: {
        spline: {
            marker: {
                radius: 4,
                lineColor: '#666666',
                lineWidth: 1
            }
        }
    }
});

function darkMode() {
    var element = document.body;
    element.classList.toggle("dark-mode");
}

