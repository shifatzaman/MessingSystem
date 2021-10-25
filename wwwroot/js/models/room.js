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
        self.dateOfEntry = ko.observable(new Date());
    }

    self.DateOfEntryFormatted = ko.computed(function () {
        if (self.dateOfEntry && self.dateOfEntry()) {
            return moment(new Date(self.dateOfEntry())).format('DD/MM/YYYY');
        }
    });
}