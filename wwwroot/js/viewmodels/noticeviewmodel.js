var NoticeViewModel = function () {
    var self = this;

    self.Notices = ko.observableArray();
    self.SelNotice = ko.observable(new Notice());


    self.GetNotices = async function () {

        showSpinner();

        var url = getBaseUrl() + '/notice/list';

        var response = await getJson(url, true);

        if (response && response.success && response.data) {
            hideSpinner();

            var arr = []
            if (Array.isArray(response.data)) {
                ko.utils.arrayForEach(response.data, function (item) {
                    if (item.date) {
                        item.date = moment(item.date).format('yyyy-MM-DD')
                    }
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

    self.AddNotice = function () {
        self.SelNotice(new Notice());
        $('#notice-add').modal('show');
    };

    self.EditNotice = function (vm) {
        if (vm && vm.id && vm.id()) {
            self.SelNotice(ko.toJS(vm));
            $('#notice-add').modal('show');
        }
    }

    self.DeleteNotice = function (vm) {
        if (vm && vm.id && vm.id()) {
            showConfirmationDialogue("Are you sure you want to delete this item?",
                function () {
                    self.DeleteNoticeConfirmed(vm.id());
                },
                function () {

                })
        }
    }

    self.DeleteNoticeConfirmed = async function (id) {
        //if (!$(formElement).valid()) return;

        showSpinner();
        var apiUrl = getBaseUrl() + '/notice/delete/' + id;

        var response = await postJson(apiUrl, true, {}, "DELETE");
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.GetNotices();
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };

    self.SaveNotice = async function (formElement) {
        if (!$(formElement).valid()) return;

        showSpinner();

        var noticedata = ko.toJS(self.SelNotice());

        var apiUrl = getBaseUrl() + '/notice/new';

        var response = await postJson(apiUrl, true, noticedata);
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.SelNotice(new Notice());
            self.GetNotices();
            $('#notice-add').modal('hide');
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelNotice(new Notice());
            }
        }
    };

    $('#notice-form').validate(noticeValidationRules);
};

var noticeValidationRules = {
    rules: {
        date: {
            required: true
        },
        title: {
            required: true
        },
        message: {
            required: true
        }
    },
    messages: {
        date: {
            required: "Please enter date"
        },
        title: {
            required: "Please enter title"
        },
        message: {
            required: "Please enter message"
        }
    }
};
