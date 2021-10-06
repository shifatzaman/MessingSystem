function LoginViewModel() {
    var self = this;
    self.Email = ko.observable();
    self.Password = ko.observable();

    self.Login = async function (formElement) {
        if (!$(formElement).valid()) return;

        var baseurl = window.location.origin;
        var loginData = {
            email: self.Email(),
            password: self.Password()

        };

        var apiUrl = 'https://localhost:44388/account/login';


        var response = await postJson(apiUrl, false, loginData);

        if (response && response.success && response.data) {
            storeTokenData(response.data);
            if (response.data.redirectUrl)
                redirect(getBaseUrl() + response.data.redirectUrl);
        }
        else {
            if (response.message) {
                alert(response.message);
            }
        }
    };

    $('#login-form').validate(loginValidationRules);
}

var loginValidationRules = {
    rules: {
        email: {
            required: true
        },
        password: {
            required: true
        }
    },
    messages: {
        email: {
            required: "Please enter your email"
        },
        password: {
            required: "Please enter password"
        }
    }
};