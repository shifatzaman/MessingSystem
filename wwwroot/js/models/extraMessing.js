var ExtraMessing = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.date = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
        self.memberId = ko.observable();
        self.itemType = ko.observable();
        self.quantity = ko.observable();
    }

    self.unit = ko.observable('');
}