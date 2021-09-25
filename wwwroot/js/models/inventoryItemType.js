var InventoryItemType = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.name = ko.observable();
        self.unit = ko.observable();
        self.unitPrice = ko.observable();
        self.quantity = ko.observable();
    }
}