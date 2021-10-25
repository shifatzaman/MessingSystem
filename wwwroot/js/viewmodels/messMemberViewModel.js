function MessMemberViewModel() {
    var self = this;
    self.MessMembers = ko.observableArray([]);
    self.SelMessMember = ko.observable(new MessMember());
    self.SelMemberId = ko.observable(0);
    self.MemberStatusOptions = ko.observableArray(memberStatusOptions);
    self.MaritalStatusOptions = ko.observableArray(maritalStatusOptions);
    self.UploadedFile = ko.observable();
    self.UserRoleOptions = ko.observable(userRoleOptions);

    self.GetMessMembers = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/member/list';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.MessMembers(response.data);
        }
        else {
            self.MessMembers([]);
        }
    };

    self.ProPicChanged = function (vm, event) {

        if (event && event.target && event.target.files && event.target.files.length > 0)
        {
            var uploadedFile = event.target.files[0];
            var tmppath = URL.createObjectURL(uploadedFile);

            if (vm && vm.imagePath) {
                vm.imagePath(tmppath);
            }

            self.UploadedFile(uploadedFile);
        }
    };

    self.AddNewMemberClicked = function () {
        var baseurl = window.location.origin;

        var redirectUrl = baseurl + '/manager/messmember/add';
        redirect(redirectUrl);
    };

    self.AddMessMember = async function (formElement) {
        if (!$(formElement).valid()) return;

        showSpinner();

        var baseurl = window.location.origin;
        var memberData = ko.toJS(self.SelMessMember());

        var apiUrl = baseurl + '/messing/member/add';

        var formData = new FormData();
        formData.append("messmember", ko.toJSON(memberData));
        formData.append("file", self.UploadedFile());

        var response = await postFormData(apiUrl, true, formData);

        if (response && response.success) {
            hideSpinner();
            showConfirmationDialogue(response.message, function () {
                redirect(getBaseUrl() + '/Manager/MessMember');
                },
                function () {
                    redirect(getBaseUrl() + '/Manager/MessMember');
                })
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelMessMember(new MessMember());
            }
        }
    };

    self.GetMemberInfo = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/member/' + self.SelMemberId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.SelMessMember(new MessMember(response.data));
        }
    };

    self.DeleteMemberConfirmed = async function (memberId) {
        var apiUrl = getBaseUrl() + '/messing/member/' + memberId;

        var response = await postJson(apiUrl, true, {}, "DELETE");

        if (response && response.success) {
            showNotificationModal(response.message);
            self.GetMessMembers();
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    }

    self.GoToExtraMessing = function (vm) {
        if (vm.id) {
            redirect(getBaseUrl() + '/Manager/MessMember/ExtraMessings?memberid=' + vm.id);
        }
    }

    self.GoToCafeteriaBill = function (vm) {
        if (vm.id) {
            redirect(getBaseUrl() + '/Manager/MessMember/CafeterialBills?memberid=' + vm.id);
        }
    }

    self.GenerateMonthlyBill = function (vm) {
        if (vm.id) {
            redirect(getBaseUrl() + '/Manager/MessMember/MonthlyBill?memberid=' + vm.id);
        }
    }

    self.EditMember = function (vm) {
        if (vm.id) {
            redirect(getBaseUrl() + '/Manager/MessMember/Add?memberid=' + vm.id);
        }
    }

    self.DeleteMember = function (vm) {
        if (vm.id) {
            showConfirmationDialogue("Are you sure you want to delete this member permanently?", function () {
                self.DeleteMemberConfirmed(vm.id);
            },
                function () {

                });
        }
    }

    self.SelMemberId.subscribe(function (newVal) {
        if (newVal) {
            self.GetMemberInfo();
        }

    })

    $('#member-form').validate(memberValidationRules);

    //$('#login-form').validate(loginValidationRules);
}

var memberValidationRules = {
    rules: {
        name: {
            required: true
        }
    },
    messages: {
        name: {
            required: "Please enter name"
        }
    }
};

