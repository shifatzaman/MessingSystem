function MessMemberViewModel() {
    var self = this;
    self.MessMembers = ko.observableArray([]);
    self.SelMessMember = ko.observable(new MessMember());
    self.MemberStatusOptions = ko.observableArray(memberStatusOptions);


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

        var response = await postJson(apiUrl, true, memberData);

        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.SelMessMember(new MessMember());
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelMessMember(new MessMember());
            }
        }
    };

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

