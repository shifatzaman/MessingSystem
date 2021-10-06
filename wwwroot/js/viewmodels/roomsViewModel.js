function RoomsViewModel() {
    var self = this;
    self.SelRoom = ko.observable(new Room());
    self.Rooms = ko.observableArray([]);
    self.RoomTypes = ko.observableArray(roomOptions);
    self.VacantRoomsOnly = ko.observable(false);

    self.GetRooms = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/rooms?includeVacantRoomsOnly=' + self.VacantRoomsOnly();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            var arr = []
            if (Array.isArray(response.data)) {
                ko.utils.arrayForEach(response.data, function (item) {
                    var roomItem = new Room(item);
                    arr.push(roomItem);
                });
            }
            self.Rooms(arr);
        }
        else {
            self.Rooms([]);
        }
    };

    self.AddRoom = function () {
        self.SelRoom(new Room());
        $('#roomentry').modal('show');
    };

    self.EditRoom = function (vm) {
        if (vm && vm.id && vm.id()) {
            self.SelRoom(new Room(ko.toJS(vm)));
            $('#roomentry').modal('show');
        }
    }

    self.DeleteRoom = function (vm) {
        if (vm && vm.id && vm.id()) {
            showConfirmationDialogue("Are you sure you want to delete this?",
                function () {
                    self.DeleteRoomConfirmed(vm.id())
                },
                function () {

                });
        }
    }

    self.DeleteRoomConfirmed = async function (roomId) {
        //if (!$(formElement).valid()) return;

        showSpinner();

        var baseurl = window.location.origin;
        var apiUrl = baseurl + '/messing/rooms/' + roomId;

        var response = await postJson(apiUrl, true, {}, "DELETE");
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.GetRooms();
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
            }
        }
    };


    self.SaveRoom = async function (formElement) {
        //if (!$(formElement).valid()) return;

        showSpinner();

        var baseurl = window.location.origin;
        var roomData = ko.toJS(self.SelRoom());

        var apiUrl = baseurl + '/messing/rooms';

        var response = await postJson(apiUrl, true, roomData);
        if (response && response.success) {
            hideSpinner();
            showNotificationModal(response.message);
            self.SelRoom(new Room());
            self.GetRooms();
            $('#roomentry').modal('hide');
        }
        else {
            hideSpinner();
            if (response.message) {
                showNotificationModal(response.message);
                self.SelRoom(new Room());
            }
        }
    };

    self.VacantRoomsOnly.subscribe(function () {
        self.GetRooms();
    })

    //$('#member-form').validate(memberValidationRules);

    //$('#login-form').validate(loginValidationRules);
}

//var memberValidationRules = {
//    rules: {
//        name: {
//            required: true
//        }
//    },
//    messages: {
//        name: {
//            required: "Please enter name"
//        }
//    }
//};

