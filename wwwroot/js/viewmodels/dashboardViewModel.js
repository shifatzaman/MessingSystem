var DashboardViewModel = function () {
    var self = this;

    self.Notices = ko.observableArray();
    self.Members = ko.observableArray();

    self.GetNotices = async function () {

        showSpinner();

        var url = getBaseUrl() + '/notice/list?dashboard=true';

        var response = await getJson(url, true);

        if (response && response.success && response.data) {
            hideSpinner();

            var arr = []
            if (Array.isArray(response.data)) {
                ko.utils.arrayForEach(response.data, function (item) {
                    var notice = new Notice(item);
                    arr.push(notice);
                });
            }
            self.Notices(arr);
        }
        else {
            hideSpinner();
            self.Notices([]);
        }

    };

    self.GetMembers = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/member/list?includeAdminsOnly=true';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.Members(response.data);
        }
        else {
            self.Members([]);
        }
    };
};