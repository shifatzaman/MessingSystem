var Notice = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.date = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
        self.title = ko.observable();
        self.message = ko.observable();
        self.isVisible = ko.observable(false);
    }
}