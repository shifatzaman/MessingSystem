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

    if (storedData && storedData.token) {
        return storedData.token;
    }

    return null;
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
        var baseurl = window.location.origin;
        var redirectUrl = baseurl;
        redirect(redirectUrl);
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