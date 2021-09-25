function DailyMessingViewModel() {
    var self = this;
    //Get
    self.SelDate = ko.observable(new Date());
    self.DailyMessings = ko.observableArray([]);

    //Add
    self.SelDailyMessing = ko.observable(new DailyMessing());
    self.MealOptions = ko.observableArray(mealOptions);
    self.InventoryItemTypes = ko.observableArray([]);

    self.GetInventoryItemTypes = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/inventory/type/list';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.InventoryItemTypes(response.data);
        }
        else {
            self.InventoryItemTypes([]);
        }
    };

    self.AddDailyMessing = async function (formElement) {
        if (!$(formElement).valid()) return;

        var baseurl = window.location.origin;
        var messingData = ko.toJS(self.SelDailyMessing());

        var apiUrl = baseurl + '/messing/daily/add';

        var response = await postJson(apiUrl, true, messingData);

        if (response && response.success) {
            showNotificationModal(response.message);
            self.SelDailyMessing(new MessMember());
            hideSpinner();
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
                self.SelDailyMessing(new MessMember());
            }
            hideSpinner();
        }
    };



    self.AddMoreItemClicked = function () {
        if (self.SelDailyMessing && self.SelDailyMessing() && self.SelDailyMessing().dailyMessingItems) {
            self.SelDailyMessing().dailyMessingItems.push(new DailyMessingItem());
        }
    }

    self.GetDailyMessings = async function () {

        showSpinner();

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/daily/' + self.SelDate();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {

            var arr = [];

            if (Array.isArray(response.data)) {

                ko.utils.arrayForEach(response.data, function (item) {
                    arr.push(new DailyMessing(item));
                });
            }

            self.DailyMessings(arr);
            hideSpinner();
        }
        else {
            self.DailyMessings([]);
            hideSpinner();
        }
    };

    self.AddDailyMessingBtnClicked = function () {
        var baseurl = window.location.origin;

        var redirectUrl = baseurl + '/manager/dailymessing/add';
        redirect(redirectUrl);
    }
}