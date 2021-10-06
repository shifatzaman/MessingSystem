function MessBillViewModel() {
    var self = this;
    //Get
    self.StartDate = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
    self.EndDate = ko.observable(moment(new Date()).format('yyyy-MM-DD'));
    self.SelMemberId = ko.observable();
    self.SelMessMember = ko.observable(new MessMember());
    self.SelMessBill = ko.observable();
    self.BillGenarated = ko.observable(false);

    self.GenerateBill = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/bill/' + self.SelMemberId() + '/' + self.StartDate() + '/' + self.EndDate();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.SelMessBill(response.data);
            self.BillGenarated(true);
        }
        else {
            self.SelMessBill([]);
        }
    };

    
    self.GetMemberInfo = async function () {

        var baseurl = window.location.origin;

        var apiUrl = baseurl + '/messing/member/' + self.SelMemberId();

        var response = await getJson(apiUrl, true);

        if (response && response.success && response.data) {
            self.SelMessMember(new MessMember(response.data));
        }
    };

    self.PrintBill = function () {
        var content = '<div>' + $("#billing-report").html() + '</div>';
        var header = 'Mess Bill';
        var preview = 'Mess Bill';
        printContent(content, header, preview);
    }

    self.SelMemberId.subscribe(function (newVal) {
        if (newVal) {
            self.GetMemberInfo();
        }
    });
}