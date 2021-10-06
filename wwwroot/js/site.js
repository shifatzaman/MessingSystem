// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function redirect(url) {
    window.location.href = url;
};

function getBaseUrl() {
    return window.location.origin;
}

var memberStatusOptions = [
    {
        key: "Dining",
        value: 1
    },
    {
        key: "Non-Dining",
        value: 2
    }
];

var mealOptions = [
    {
        key: "BreakFast",
        value: 1
    },
    {
        key: "Lunch",
        value: 2
    },
    {
        key: "Dinner",
        value: 3
    },
    {
        key: "TeaBreak",
        value: 4
    }
];


var roomOptions = [
    {
        key: "VIP Room",
        value: 1
    },
    {
        key: "Guest Room",
        value: 2
    },
    {
        key: "Regular Room",
        value: 3
    }
];

function showSpinner() {
    $('#loaderSpinner').show();
}

function hideSpinner() {
    $('#loaderSpinner').hide();
}

function showNotificationModal(message) {
    try {
        $('#noti-modal-text').text(message);
        $('#notificationModal').modal('show');
    } catch (e) {

    }
    
}

function showConfirmationDialogue(message, success_callback, error_callback) {
    try {
        $('#conf-modal-text').text(message);

        //Ok
        $("#conf-modal-ok").click(function (e) {
            if (typeof success_callback == "function")
                success_callback();
        })

        //Cancel
        $("#conf-modal-cancel").click(function (e) {
            if (typeof error_callback == "function")
                error_callback();
        })

        $('#confirmation-modal').modal('show');
    } catch (e) {

    }

}

function getParamValueFromUrl(paramName) {
    var searchParams = new URLSearchParams(window.location.search);

    if (searchParams && searchParams.has(paramName)) {
        return searchParams.get(paramName);
    }
}

ko.bindingHandlers.datePicker = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        // Register change callbacks to update the model
        // if the control changes.       
        ko.utils.registerEventHandler(element, "change", function () {
            var value = valueAccessor();
            value(new Date(element.value));
        });
    },
    // Update the control whenever the view model changes
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        element.value = value().toISOString();
    }
};


function printContent(content, header, title, cssTitles = []) {
    title = title ? title : "Print preview";
    var w = window.open('', 'Print Preview', 'fullscreen="yes"');
    if (w) {
        w.document.write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        w.document.write("<html>");
        w.document.write("<head>");
        w.document.write('<title>' + title + '</title>');
        w.document.write("<meta charset='utf-8'>");

        var headChlidElements = $('head').find('link[title!=bootstrap]');
        var headElement = document.createElement("head");

        for (var i = 0; i < headChlidElements.length; i++) {
            $(headChlidElements[i]).clone().appendTo($(headElement));
        }

        w.document.write($(headElement).html());

        w.document.write("<style>");
        for (var i = 0; i < document.styleSheets.length; i++) {
            var sheet = document.styleSheets[i];
            if (sheet && sheet.title && (sheet.title === 'bootstrap' || cssTitles.includes(sheet.title))) {
                if (typeof sheet.cssRules !== 'undefined') {
                    for (var j = 0; j < sheet.cssRules.length; j++) {
                        var rule = sheet.cssRules[j];
                        if (rule && rule.cssText) {
                            w.document.write(rule.cssText);
                        }
                    }
                }
            }
        }
        w.document.write("</style>");

        w.document.write('</head>');
        w.document.write('<body>');
        w.document.write('<div style="background: white;">');
        w.document.write(header);
        w.document.write(content);
        w.document.write('</div>');
        w.document.close();

        var is_chrome = Boolean(w.chrome);
        if (is_chrome) {
            w.onload = function () { // wait until all resources loaded 
                w.focus(); // necessary for IE >= 10
                w.print();
                setTimeout(function () {
                    w.close();
                }, 1000);
                // change window to mywindow
            };
        }
        else {
            setTimeout(function () {
                w.focus(); // necessary for IE >= 10
                w.print();  // change window to mywindow
                w.close();// change window to mywindow
            }, 500);
        }
    }
}

function redirectToMontlyBillForMember() {
    var user = getUserData();

    if (user && user.memberId) {
        redirect(getBaseUrl() + '/Member/Meals/MonthlyBill?memberid=' + user.memberId);
    }
}