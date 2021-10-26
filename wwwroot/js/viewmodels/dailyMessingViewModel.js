function DailyMessingViewModel() {
    var self = this;
    //Get
    self.SelDate = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
    self.DailyMessings = ko.observableArray([]);
    self.MessingTemplates = ko.observableArray([]);
    self.SelMessingTemplateId = ko.observable('');
    self.DailyMessingLoaded = ko.observable(false);

    //Add
    self.SelDailyMessing = ko.observable(new DailyMessing());
    self.MealOptions = ko.observableArray(mealOptions);
    self.InventoryItemTypes = ko.observableArray([]);

    self.TemplateName = ko.observable('');

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

    self.GetDailyMessingTemplates = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/template';

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.MessingTemplates(response.data);
        }
        else {
            self.MessingTemplates([]);
        }
    };

    self.AddDailyMessing = function (formElement) {
        if (!$(formElement).valid())
            return;

        var missingItem = false, missingQty = false;

        if (self.SelDailyMessing && self.SelDailyMessing()
            && self.SelDailyMessing().dailyMessingItems && self.SelDailyMessing().dailyMessingItems()) {
            ko.utils.arrayForEach(self.SelDailyMessing().dailyMessingItems(), function (item) {
                if (item.itemType && (!item.itemType() || isNaN(item.itemType()))) {
                    missingItem = true;
                }

                if (item.quantity && (!item.quantity() || isNaN(item.quantity()))) {
                    missingQty = true;
                }
            });

            if (missingItem || missingQty) {
                showNotificationModal("All messing items must have valid item and quantity");
                return;
            }

        }


        showConfirmationDialogue("Are you sure you want to add these items to daily messing? Please note, added items can't be removed.",
            function () {
                self.AddDailyMessingConfirmed();
            },
            function () {

            }
        );
    };

    self.AddDailyMessingConfirmed = async  function () {
        var baseurl = window.location.origin;
        var messingData = ko.toJS(self.SelDailyMessing());

        var apiUrl = baseurl + '/messing/daily/add';

        var response = await postJson(apiUrl, true, messingData);

        if (response && response.success) {
            showNotificationModal(response.message);
            self.SelDailyMessing(new DailyMessing());
            hideSpinner();
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
            }
            hideSpinner();
        }
    }

    self.SaveTemplateClicked = function () {
        $('#template').modal('show');
    }


    self.CopyDailyMessingToTemplate = function () {
        var messingData = ko.toJS(self.SelDailyMessing());

        if (messingData) {
            var templateData = {};
            templateData.mealType = messingData.mealType;
            templateData.templateName = self.TemplateName();

            if (messingData.dailyMessingItems.length > 0) {
                var templateItems = [];
                ko.utils.arrayForEach(messingData.dailyMessingItems, function (item) {
                    var templateItem = {
                        itemType : item.itemType,
                        quantity : item.quantity
                    };

                    templateItems.push(templateItem);
                });

                templateData.dailyMessingTemplateItems = templateItems;
            }

            return templateData;
        }

        return null;

    }

    self.SaveTemplateConfirmed = async function (formElement) {

        if (!$(formElement).valid())
            return;

        showSpinner();
        var messingTemplateData = self.CopyDailyMessingToTemplate();

        var apiUrl = getBaseUrl() + '/messing/template';

        var response = await postJson(apiUrl, true, messingTemplateData);

        if (response && response.success) {
            self.TemplateName('');
            self.GetDailyMessingTemplates();
            showNotificationModal(response.message);
            hideSpinner();
        }
        else {
            if (response.message) {
                showNotificationModal(response.message);
            }
            hideSpinner();
        }
    }

    self.SetTempplateAsDailyMessing = function (templateId) {
        var matchedTemplate = ko.utils.arrayFirst(self.MessingTemplates(), function (item) {
            return item.id == templateId;
        });

        if (matchedTemplate) {
            var messingData = new DailyMessing();
            messingData.mealType(matchedTemplate.mealType);

            if (matchedTemplate.dailyMessingTemplateItems.length > 0) {
                var dailyMessingItems = [];
                ko.utils.arrayForEach(matchedTemplate.dailyMessingTemplateItems, function (item) {
                    var messingItem = new DailyMessingItem();
                    messingItem.itemType(item.itemType);
                    messingItem.quantity(item.quantity);
                    self.InventoryTypeChanged(messingItem);
                    dailyMessingItems.push(messingItem);
                });

                messingData.dailyMessingItems(dailyMessingItems);
            }

            self.SelDailyMessing(messingData);
        }
    };

    self.SelMessingTemplateId.subscribe(function (templateId) {
        if (templateId) {
            self.SetTempplateAsDailyMessing(templateId);
        }
    });

    self.AddMoreItemClicked = function () {
        if (self.SelDailyMessing && self.SelDailyMessing() && self.SelDailyMessing().dailyMessingItems) {
            self.SelDailyMessing().dailyMessingItems.push(new DailyMessingItem());
        }
    }

    self.RemoveItemClicked = function (vm) {
        if (self.SelDailyMessing && self.SelDailyMessing() && self.SelDailyMessing().dailyMessingItems) {
            self.SelDailyMessing().dailyMessingItems.remove(vm);
        }
    }

    self.InventoryTypeChanged = function (vm) {
        if (vm && vm.itemType && vm.itemType()) {
            var matchedItem = ko.utils.arrayFirst(self.InventoryItemTypes(), function (item) {
                return item.itemTypeId == vm.itemType();
            });

            if (matchedItem && matchedItem.unit) {
                vm.unit(matchedItem.unit);
            }
            else {
                vm.unit('');
            }
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
            self.DailyMessingLoaded(true);
            hideSpinner();
        }
        else {
            self.DailyMessings([]);
            self.DailyMessingLoaded(true);
            hideSpinner();
        }
    };

    self.AddDailyMessingBtnClicked = function () {
        var baseurl = window.location.origin;

        var redirectUrl = baseurl + '/manager/dailymessing/add';
        redirect(redirectUrl);
    }

    $('#dailymessing-form').validate(dailyMessingValidationRules);
    $('#template-form').validate(templateValidationRules);
}

var dailyMessingValidationRules = {
    rules: {
        date: {
            required: true
        },
        mealType: {
            required: true
        }
    },
    messages: {
        date: {
            required: "Please enter date"
        },
        mealType: {
            required: "Please select type of meal"
        }
    }
};

var templateValidationRules = {
    rules: {
        templateName: {
            required: true
        }
    },
    messages: {
        templateName: {
            required: "Please enter template name"
        }
    }
};