var InventoryItem = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.date = ko.observable(new Date());
        self.itemType = ko.observable();
        self.quantity = ko.observable();
    }
}