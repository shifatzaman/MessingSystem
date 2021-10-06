function CafeBillViewModel() {
    var self = this;
    self.SelMessMember = ko.observable(new MessMember());
    self.CafeBills = ko.observableArray([]);
    self.SelMemberId = ko.observable();
    self.SelCafeBill = ko.observable(new CafeBill());


    self.GetMemberInfo = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/member/' + self.SelMemberId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.SelMessMember(new MessMember(response.data));
        }
    };

    self.GetCafeBills = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/cafe/' + self.SelMemberId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            var arr = []
            if (Array.isArray(response.data)) {
                ko.utils.arrayForEach(response.data, function (item) {
                    if (item.date) {
                        item.date = moment(item.date).format('yyyy-MM-DD')
                    }
                    var cafebillItem = new CafeBill(item);
                    arr.push(cafebillItem);
                });
            }
            self.CafeBills(arr);
        }
        else {
            self.CafeBills([]);
        }
    };

    self.AddCafeBilling = function () {
        self.SelCafeBill(new CafeBill());
        $('#cafebilling').modal('show');
    };

    self.EditCafeBilling = function (vm) {
        if (vm && vm.id && vm.id()) {
            self.SelCafeBill(ko.toJS(vm));
            $('#cafebilling').modal('show');
        }
    }

    self.DeleteCafeBill = function (vm) {
        if (vm && vm.id && vm.id()) {
            showConfirmationDialogue("Are you sure you want to delete this item?",
                function () {
                    self.DeleteCafeBillConfirmed(vm.id());
                },
                function () {

                })
        }
    }

    self.DeleteCafeBillConfirmed = async function (id) {
        //if (!$(formElement).valid()) return;

        showSpinner();
        var apiUrl = getBaseUrl() + '/messing/cafe/' + id;

        var response = await postJson(apiUrl, true, {}, "DELETE");
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.GetCafeBills();
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };

    self.SaveCafeBill = async function (formElement) {
        //if (!$(formElement).valid()) return;

        showSpinner();

        var baseurl = window.location.origin;
        var cafeBillData = ko.toJS(self.SelCafeBill());

        if (self.SelMemberId && self.SelMemberId()) {
            cafeBillData.memberId = self.SelMemberId();
        }

        var apiUrl = baseurl + '/messing/cafe/add';

        var response = await postJson(apiUrl, true, cafeBillData);
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.SelCafeBill(new CafeBill());
            self.GetCafeBills();
            $('#cafebilling').modal('hide');
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelCafeBill(new CafeBill());
            }
        }
    };

    self.SelMemberId.subscribe(function (newVal) {
        if (newVal) {
            self.GetCafeBills();
            self.GetMemberInfo();
        }
    });

    //$('#member-form').validate(memberValidationRules);

    //$('#login-form').validate(loginValidationRules);
}

//var memberValidationRules = {
//    rules: {
//        name: {
//            required: true
//        }
//    },
//    messages: {
//        name: {
//            required: "Please enter name"
//        }
//    }
//};

