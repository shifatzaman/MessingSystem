var Room = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.type = ko.observable();
        self.roomNo = ko.observable();
        self.allocatedTo = ko.observable();
        self.isAllocated = ko.observable(false);
    }
}