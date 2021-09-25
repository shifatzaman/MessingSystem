var DailyMessingItem = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.itemType = ko.observable(0);
        self.quantity = ko.observable(0);
    }
}