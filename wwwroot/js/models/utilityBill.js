var UtilityBill = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.date = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
        self.item = ko.observable();
        self.price = ko.observable();
    }
}