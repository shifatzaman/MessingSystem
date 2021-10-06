function UtilityBillViewModel() {
    var self = this;
    self.UtilityBills = ko.observableArray([]);
    self.SelUtilityBill = ko.observable(new UtilityBill());


    self.GetUtilityBills = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/utility/list';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            var arr = []
            if (Array.isArray(response.data)) {
                ko.utils.arrayForEach(response.data, function (item) {
                    if (item.date) {
                        item.date = moment(item.date).format('yyyy-MM-DD')
                    }
                    var utilItem = new UtilityBill(item);
                    arr.push(utilItem);
                });
            }
            self.UtilityBills(arr);
        }
        else {
            self.UtilityBills([]);
        }
    };



    self.AddUtilityBill = function () {
        self.SelUtilityBill(new UtilityBill());
        $('#utilityBill').modal('show');
    };

    self.EditUtilityBill = function (vm) {
        if (vm && vm.id && vm.id()) {
            self.SelUtilityBill(ko.toJS(vm));
            $('#utilityBill').modal('show');
        }
    }

    self.DeleteUtilityBill = function (vm) {
        if (vm && vm.id && vm.id()) {
            showConfirmationDialogue("Are you sure you want to delete this item?",
                function () {
                    self.DeleteUtilityBillConfirmed(vm.id());
                },
                function () {

                })
        }
    }

    self.DeleteUtilityBillConfirmed = async function (id) {
        //if (!$(formElement).valid()) return;

        showSpinner();
        var apiUrl = getBaseUrl() + '/messing/utility/' + id;

        var response = await postJson(apiUrl, true, {}, "DELETE");
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.GetUtilityBills();
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };

    self.SaveUtilityBill = async function (formElement) {
        //if (!$(formElement).valid()) return;

        showSpinner();

        var baseurl = window.location.origin;
        var utilityBillData = ko.toJS(self.SelUtilityBill());

        var apiUrl = baseurl + '/messing/utility/add';

        var response = await postJson(apiUrl, true, utilityBillData);
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.SelUtilityBill(new UtilityBill());
            self.GetUtilityBills();
            $('#utilityBill').modal('hide');
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelUtilityBill(new UtilityBill());
            }
        }
    };

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

