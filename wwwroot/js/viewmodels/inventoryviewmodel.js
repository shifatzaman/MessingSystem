function InventoryViewModel() {
    var self = this;
    self.InventoryItems = ko.observableArray([]);
    self.InventoryItemTypes = ko.observableArray([]);
    self.SelInventoryItem = ko.observable(new InventoryItem());
    self.SelInventoryItemType = ko.observable(new InventoryItemType());


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

        var apiUrl = baseurl + '/inventory/type/add';

        var response = await postJson(apiUrl, true, inventoryData);

        if (response && response.success) {
            showNotificationModal(response.message);
            self.SelInventoryItemType(new InventoryItemType());
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
                self.SelInventoryItemType(new InventoryItemType());
            }
        }
    };

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