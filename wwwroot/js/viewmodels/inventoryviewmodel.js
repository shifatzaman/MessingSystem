function InventoryViewModel() {
    var self = this;
    self.InventoryItems = ko.observableArray([]);
    self.InventoryItemTypes = ko.observableArray([]);
    self.SelInventoryItem = ko.observable(new InventoryItem());
    self.SelInventoryItemId = ko.observable(0);
    self.SelInventoryItemType = ko.observable(new InventoryItemType());
    self.SelInventoryItemTypeId = ko.observable(0);


    self.GetInventoryItems = async function () {

        showSpinner();

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/inventory/history';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            hideSpinner();
            self.InventoryItems(response.data);
        }
        else {
            hideSpinner();
            self.InventoryItems([]);
        }
    };


    self.GetInventoryItem = async function () {

        showSpinner();

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/inventory/' + self.SelInventoryItemId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            hideSpinner();
            self.SelInventoryItem(new InventoryItem(response.data));
        }
        else {
            hideSpinner();
            self.SelInventoryItem(new InventoryItem());
        }
    };

    self.GetInventoryItemTypes = async function () {

        showSpinner();

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/inventory/type/list';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            hideSpinner();
            self.InventoryItemTypes(response.data);
        }
        else {
            hideSpinner();
            self.InventoryItemTypes([]);
        }
    };

    self.GetInventoryItemType = async function () {

        showSpinner();

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/inventory/type/' + self.SelInventoryItemTypeId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            hideSpinner();
            self.SelInventoryItemType(new InventoryItemType(response.data));
        }
        else {
            hideSpinner();
            self.SelInventoryItemType(new InventoryItemType());
        }
    };

    self.AddBtnClicked = function () {
        var baseurl = window.location.origin;

        var redirectUrl = baseurl + '/manager/bazar/add';
        redirect(redirectUrl);
    };

    self.AddInventoryItem = async function (formElement) {
        if (!$(formElement).valid()) return;

        var baseurl = window.location.origin;
        var inventoryData = {
            date: self.SelInventoryItem().date(),
            itemType: self.SelInventoryItem().itemType(),
            quantity: self.SelInventoryItem().quantity(),
            unitPrice: self.SelInventoryItem().unitPrice()
        };


        var apiUrl = baseurl +'/inventory/add';

        var response = await postJson(apiUrl, true, inventoryData);

        if (response && response.success) {
            showNotificationModal(response.message);
            self.SelInventoryItem(new InventoryItem());
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
                self.SelInventoryItem(new InventoryItem());
            }
        }
    };

    self.AddNewInventoryBtnClicked = function () {
        var baseurl = window.location.origin;

        var redirectUrl = baseurl + '/manager/inventoryitem/add';
        redirect(redirectUrl);
    }

    self.AddInventoryItemType = async function (formElement) {
        if (!$(formElement).valid()) return;

        var baseurl = window.location.origin;
        var inventoryData = {
            name: self.SelInventoryItemType().name(),
            unit: self.SelInventoryItemType().unit(),
            unitPrice: self.SelInventoryItemType().unitPrice(),
            quantity: self.SelInventoryItemType().quantity()
        };

        if (self.SelInventoryItemType && self.SelInventoryItemType()
            && self.SelInventoryItemType().itemTypeId
            && self.SelInventoryItemType().itemTypeId()) {
            inventoryData.itemTypeId = self.SelInventoryItemType().itemTypeId();
        }

        var apiUrl = baseurl + '/inventory/type/add';

        var response = await postJson(apiUrl, true, inventoryData);

        if (response && response.success && response.message) {
            showConfirmationDialogue(response.message + " Do you want to add more?",
                function () {
                    self.SelInventoryItemType(new InventoryItemType());
                },
                function () {
                    redirect(getBaseUrl() + '/manager/bazar/');
                }
            );
            
            
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
                self.SelInventoryItemType(new InventoryItemType());
            }
        }
    };

    self.DeleteInventoryItemTypeConfirmed = async function (inventoryId) {

        var apiUrl = getBaseUrl() + '/inventory/type/' + inventoryId;

        var response = await postJson(apiUrl, true, {}, "DELETE");

        if (response && response.success) {
            showNotificationModal(response.message);
            self.GetInventoryItemTypes();
            self.GetInventoryItems();
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };

    self.DeleteInventoryItemConfirmed = async function (inventoryId) {

        var apiUrl = getBaseUrl() + '/inventory/delete/' + inventoryId;

        var response = await postJson(apiUrl, true, {}, "DELETE");

        if (response && response.success) {
            showNotificationModal(response.message);
            self.GetInventoryItems();
            self.GetInventoryItemTypes();
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };


    self.EditInventoryItemType = function (vm) {
        if (vm && vm.itemTypeId) {
            redirect(getBaseUrl() + '/manager/inventoryitem/add?itemtypeid=' + vm.itemTypeId);
        }
    }

    self.EditInventoryItem = function (vm) {
        if (vm && vm.inventoryId) {
            redirect(getBaseUrl() + '/manager/bazar/add?inventoryId=' + vm.inventoryId);
        }
    }

    self.DeleteInventoryItemType = function (vm) {
        if (vm && vm.itemTypeId) {
            showConfirmationDialogue("Are you sure you want to delete this item? Please note that deleting this will cause unexpected behavior in items depending on this",
                function () {
                    self.DeleteInventoryItemTypeConfirmed(vm.itemTypeId);
                },
                function () {
                });
        }
    }

    self.DeleteInventoryItem = function (vm) {
        if (vm && vm.inventoryId) {
            showConfirmationDialogue("Are you sure you want to delete this item? Please note that deleting this will cause unexpected behavior in items depending on this",
                function () {
                    self.DeleteInventoryItemConfirmed(vm.inventoryId);
                },
                function () {
                });
        }
    }

    self.SelInventoryItemTypeId.subscribe(function (newVal) {
        if (newVal) {
            self.GetInventoryItemType();
        }
    });

    self.SelInventoryItemId.subscribe(function (newVal) {
        if (newVal) {
            self.GetInventoryItem();
        }
    });

    $('#inventory-form').validate(inventoryItemValidationRules);
    $('#inventory-type-form').validate(inventoryItemTypeValidationRules);

    //$('#login-form').validate(loginValidationRules);
}

var inventoryItemValidationRules = {
    rules: {
        date: {
            required: true
        },
        type: {
            required: true
        },
        quantity: {
            required: true
        }
    },
    messages: {
        date: {
            required: "Please enter date"
        },
        type: {
            required: "Please enter type of inventory item"
        },
        quantity: {
            required: "Please enter quantity"
        }
    }
};

var inventoryItemTypeValidationRules = {
    rules: {
        name: {
            required: true
        },
        unit: {
            required: true
        },
        unitPrice: {
            required: true
        },
        quantity: {
            required: true
        }
    },
    messages: {
        name: {
            required: "Please enter name of item"
        },
        unit: {
            required: "Please enter unit of measurement"
        },
        unitPrice: {
            required: "Please enter per unit price"
        },
        quantity: {
            required: "Please enter quantity"
        }
    }
};