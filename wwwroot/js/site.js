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