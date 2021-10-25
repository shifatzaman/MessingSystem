var DailyMessingTemplateData = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.templateName = ko.observable();
        self.mealType = ko.observable();
        self.dailyMessingTemplateItems = ko.observableArray([]);
    }
}