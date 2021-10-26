function MealsViewModel() {
    var self = this;
    self.MealsByDate = ko.observableArray([]);
    self.SelDate = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
    self.MemberStatusOptions = ko.observableArray(memberStatusOptions);
    self.MemberMealsLoaded = ko.observable(false);

    self.GetMeals = async function () {

        showSpinner();

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/member/meals/' + self.SelDate();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {

            var arr = [];

            if (Array.isArray(response.data)) {

                ko.utils.arrayForEach(response.data, function (item) {
                    arr.push(new Meals(item));
                });
            }

            self.MealsByDate(arr);
            self.MemberMealsLoaded(true);
            hideSpinner();
        }
        else {
            self.MealsByDate([]);
            self.MemberMealsLoaded(true);
            hideSpinner();
        }
    };

    self.UpdateMeals = async function () {
        var baseurl = window.location.origin;
        var mealsArr = [];
        showSpinner();
        ko.utils.arrayForEach(self.MealsByDate(), function (item) {
            mealsArr.push(ko.toJS(item))
        });

        var mealData = {
            date: self.SelDate(),
            memberMeals: mealsArr
        };
        var apiUrl = baseurl + '/messing/member/meals';

        var response = await postJson(apiUrl, true, mealData);

        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
        }
        else {
            if (response.message) {
                hideSpinner();
                showNotificationModal(response.message);
            }
        }
    }

    self.GenerateDailyMeals = function () {
        self.GetMeals();
    }
}

