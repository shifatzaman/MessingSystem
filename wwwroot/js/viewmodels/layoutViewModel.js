var LayoutViewModel = function () {
    var self = this;

    self.Notifications = ko.observableArray([]);
    self.UnseenCount = ko.observable(0);


    self.GetNotifications = async function () {
        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/notification/';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {

            if (response.data.hasOwnProperty('notifications')) {
                self.Notifications(response.data.notifications);
            }

            if (response.data.hasOwnProperty('unseenCount')) {
                self.UnseenCount(response.data.unseenCount);
            }
        }
    }

    self.NotificationClicked = async function (vm, event) {

        event.stopPropagation();

        if (vm && vm.id) {
            var baseurl = window.location.origin;

            var apiUrl = baseurl + '/notification/status/' + vm.id;

            var response = await getJson(apiUrl, true);

            if (response && response.success) {
                if (vm.notificationUrl) {
                    redirect(getBaseUrl() + vm.notificationUrl);
                }
            }
        }
    }

    self.NotificationBellClicked = function (vm, event) {
        event.stopPropagation();
        $('#notification-dropdown').show();
    }

    self.UpdateLoggedInStatus = async function () {

    }
}