async function getJson(url, creadentialsneeded) {

    var token = getTokenData()

    const response = await fetch(url, {
        method: 'GET', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: creadentialsneeded ? 'include' : 'same-origin', // include, *same-origin, omit
        headers: {
            'Authorization': creadentialsneeded ? `Bearer ${token}` : '',
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
    });

    if (response.status == 401) {
        var responseData = {
            success: false,
            message: 'unauthorized'
        }

        return responseData;
    }

    return response.json(); // parses JSON response into native JavaScript objects
}

async function postJson(url, creadentialsneeded, data = {}, method = 'POST') {

    var token = getTokenData();
    url = url;


    const response = await fetch(url, {
        method: method, // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: creadentialsneeded ? 'include' : 'same-origin', // include, *same-origin, omit
        headers: {
            'Authorization': creadentialsneeded ? `Bearer ${token}` : '',
            'Content-Type': 'application/json'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data)
    });

    if (response.status == 401) {
        var responseData = {
            success: false,
            message: 'unauthorized'
        }

        return responseData;
    }

    return response.json(); // parses JSON response into native JavaScript objects
}

async function postFormData(url, creadentialsneeded, data = {}) {

    var token = getTokenData();
    url = url;


    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: creadentialsneeded ? 'include' : 'same-origin',
        headers: {
            'Authorization': creadentialsneeded ? `Bearer ${token}` : ''
        },// include, *same-origin, omit
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data
    });

    if (response.status == 401) {
        var responseData = {
            success: false,
            message: 'unauthorized'
        }

        return responseData;
    }

    return response.json(); // parses JSON response into native JavaScript objects
}

