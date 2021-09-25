var MessMember = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.name = ko.observable();
        self.bnumb = ko.observable();
        self.rank = ko.observable();
        self.appt = ko.observable();
        self.unit = ko.observable();
        self.contactNo = ko.observable();
        self.roomNo = ko.observable();
        self.maritialStatus = ko.observable();
        self.memberStatus = ko.observable();
        self.dateOfEntry = ko.observable();
    }
}