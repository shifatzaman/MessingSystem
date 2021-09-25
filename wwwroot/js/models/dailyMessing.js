var DailyMessing = function (data) {
    var self = this;
    if (data) {
        ko.mapping.fromJS(data, {}, self);
    }
    else {
        self.date = ko.observable(new Date());
        self.mealType = ko.observable();
        self.dailyMessingItems = ko.observableArray([]);
        self.dailyMessingItems.push(new DailyMessingItem());
    }
}