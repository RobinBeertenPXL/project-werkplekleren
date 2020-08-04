// General functions
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

let messageResult = document.getElementById('error');

function userLogin(result) {
    if (!result.data) {
        messageResult.innerText = 'Het opgegeven e-mailadres of paswoord is onjuist.'
    } else {
        messageResult.innerText = '';
        // Gevonden account opnemen in sessionStorage
        let obj = JSON.parse(result.data);
        console.log(obj);
        sessionStorage.setItem('userid', obj[0].userid);
        sessionStorage.setItem('fname', obj[0].firstname);
        sessionStorage.setItem('lname', obj[0].name);
        sessionStorage.setItem('email', obj[0].email);
        sessionStorage.setItem('pwd', obj[0].password);
        sessionStorage.setItem('payment', obj[0].paymentmethod);
        // Na login doorverwijzen naar startpagina
        window.location.href = "../Homepage/HomePage.html";
    }
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

// Check user fields validity
let emailAddress = document.getElementById('email');
let passWord = document.getElementById('paswoord');
let emailErrorMessage = document.getElementById('email-error');
let passWordErrorMessage = document.getElementById('password-error');

function checkInputFields() {
    let emailIsValid = false;
    let passwordIsValid = false;

    if (checkInputFieldValidity(emailAddress)) {
        emailErrorMessage.innerText = '';
        emailIsValid = true;
    } else {
        emailErrorMessage.innerText = 'Gelieve een e-mailadres op te geven';
    }

    if (checkInputFieldValidity(passWord)) {
        passWordErrorMessage.innerText = '';
        passwordIsValid = true;
    } else {
        passWordErrorMessage.innerText = 'Gelieve een paswoord op te geven';
    }

    return emailIsValid && passwordIsValid;
}

function checkInputFieldValidity(inputField) {
    return inputField.value !== '';
}

// Get user
function getUserData() {
    const email = emailAddress.value;
    const pwd = passWord.value;

    fetch(`https://localhost:44377/api/Account?email=${email}&pwd=${pwd}`)
        .then(validateResponse)
        .then(logResult)
        .then(userLogin)
        .catch(logError);
}

let submitButton = document.getElementById('verzenden');
let resetButton = document.getElementById('leegmaken');

window.onload = function () {
    if (sessionStorage.getItem('email') === null || sessionStorage.getItem('pwd') === null || sessionStorage.getItem('email') === '' || sessionStorage.getItem('pwd') === '') {
        submitButton.disabled = false;
        resetButton.disabled = false;

        submitButton.addEventListener('click', (e) => {
            e.preventDefault();

            if (checkInputFields()) {
                getUserData();
            }
        })

        resetButton.addEventListener('click', () => {
            messageResult.innerText = '';
        });
    } else {
        submitButton.disabled = true;
        resetButton.disabled = true;
        messageResult.innerText = 'U bent reeds ingelogd.';
    }
};
