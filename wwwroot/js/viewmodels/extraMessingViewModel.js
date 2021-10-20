function ExtraMessingViewModel() {
    var self = this;
    self.SelMessMember = ko.observable(new MessMember());
    self.ExtraMessings = ko.observableArray([]);
    self.SelMemberId = ko.observable();
    self.SelExtraMessing = ko.observable(new ExtraMessing());

    self.InventoryItemTypes = ko.observableArray([]);

    self.GetInventoryItemTypes = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/inventory/type/list';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.InventoryItemTypes(response.data);
        }
        else {
            self.InventoryItemTypes([]);
        }
    };

    self.InventoryTypeChanged = function (vm) {
        if (vm && vm.itemType && vm.itemType()) {
            var matchedItem = ko.utils.arrayFirst(self.InventoryItemTypes(), function (item) {
                return item.itemTypeId == vm.itemType();
            });

            if (matchedItem && matchedItem.unit) {
                vm.unit(matchedItem.unit);
            }
            else {
                vm.unit('');
            }
        }
    }


    self.GetExtraMessings = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/extra/' + self.SelMemberId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            var arr = []
            if (Array.isArray(response.data)) {
                ko.utils.arrayForEach(response.data, function (item) {
                    if (item.date) {
                        item.date = moment(item.date).format('yyyy-MM-DD')
                    }
                    var extraItem = new ExtraMessing(item);
                    arr.push(extraItem);
                });
            }
            self.ExtraMessings(arr);
        }
        else {
            self.ExtraMessings([]);
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



    self.AddExtraMessing = function () {
        self.SelExtraMessing(new ExtraMessing());
        $('#extramessing').modal('show');
    };

    self.EditExtraMessing = function (vm) {
        if (vm && vm.id && vm.id()) {
            self.SelExtraMessing(ko.toJS(vm));
            $('#extramessing').modal('show');
        }
    }

    self.DeleteExtraMessing = function (vm) {
        if (vm && vm.id && vm.id()) {
            showConfirmationDialogue("Are you sure you want to delete this item?",
                function () {
                    self.DeleteExtraMessingConfirmed(vm.id());
                },
                function () {

                })
        }
    }

    self.DeleteExtraMessingConfirmed = async function (id) {
        //if (!$(formElement).valid()) return;

        showSpinner();
        var apiUrl = getBaseUrl() + '/messing/extra/' + id;

        var response = await postJson(apiUrl, true, {}, "DELETE");
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.GetExtraMessings();
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };

    self.SaveExtraMessing = async function (formElement) {
        //if (!$(formElement).valid()) return;

        showSpinner();

        var baseurl = window.location.origin;
        var extraMessingData = ko.toJS(self.SelExtraMessing());

        if (self.SelMemberId && self.SelMemberId()) {
            extraMessingData.memberId = self.SelMemberId();
        }

        var apiUrl = baseurl + '/messing/extra/add';

        var response = await postJson(apiUrl, true, extraMessingData);
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.SelExtraMessing(new ExtraMessing());
            self.GetExtraMessings();
            $('#extramessing').modal('hide');
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelExtraMessing(new ExtraMessing());
            }
        }
    };

    self.SelMemberId.subscribe(function (newVal) {
        if (newVal) {
            self.GetExtraMessings();
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

