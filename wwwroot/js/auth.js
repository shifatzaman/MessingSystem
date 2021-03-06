function getStoredData() {
    let storedData = localStorage.getItem("mess-magement");
    return JSON.parse(storedData);
}

function storeTokenData(data) {
    if (data) {
        localStorage.setItem("mess-magement", JSON.stringify(data));
    }
}

function getTokenData() {
    let storedData = getStoredData();

    if (storedData && storedData.tokenData && storedData.tokenData.token
        && storedData.tokenData.validity) {
        if (moment().utc().format() <= storedData.tokenData.validity) {
            return storedData.tokenData.token;
        }
        else
            return false;
    }

    return false;;
}

function getUserData() {
    let storedData = getStoredData();

    if (storedData && storedData.userdata) {
        return storedData.userdata;
    }

    return null;
}

function logout() {
    localStorage.setItem("mess-magement", null);
    redirect(getBaseUrl());
}

function loggedInUserGuard() {
    if (!getTokenData()) {
        logout();
    }
}

function nonloggedInUserGuard() {
    if (getTokenData()) {
        var baseurl = window.location.origin;
        var user = getUserData();

        if (user.memberId > 0) {
            var redirectUrl = baseurl + '/Member/Dashboard';
            redirect(redirectUrl);
        }
        else {
            var redirectUrl = baseurl + '/Manager/Dashboard';
            redirect(redirectUrl);
        }
    }
}