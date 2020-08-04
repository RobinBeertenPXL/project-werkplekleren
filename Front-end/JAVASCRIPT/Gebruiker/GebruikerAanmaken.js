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

function emailExists(result) {
    if (!result.data) {
        messageResult.innerText = 'Dit e-mailadres bestaat al! Gelieve een ander e-mailadres te gebruiken.';
    }
    else {
        messageResult.innerText = 'Gebruiker succesvol aangemaakt. U wordt binnen enkele seconden naar de inlogpagina geleid.';
        setTimeout(function() { window.location.href = 'GebruikerInloggen.html'; }, 5000);
    }
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

// Post user
let firstName = document.getElementById('voorn');
let lastName = document.getElementById('naam');
let dateOfBirth = document.getElementById('datum');
let emailAddress = document.getElementById('email');
let passWord = document.getElementById('paswoord');

function postRequest() {
    const email = emailAddress.value;
    const pwd = passWord.value;
    const firstname = firstName.value;
    const lastname = lastName.value;
    const birthdate = dateOfBirth.value;
    const data = {
        firstName: firstname,
        lastName: lastname,
        birthdate: birthdate,
        email: email,
        password: pwd
    };

    fetch('https://localhost:44377/api/Account/', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    })
        .then(validateResponse)
        .then(logResult)
        .then(emailExists)
        .catch(logError);
}

// Check user fields validity
let firstNameErrorMessage = document.getElementById('firstname-error');
let lastNameErrorMessage = document.getElementById('lastname-error');
let dateOfBirthErrorMessage = document.getElementById('date-error');
let emailErrorMessage = document.getElementById('email-error');
let passWordErrorMessage = document.getElementById('password-error');

firstName.addEventListener('input', () => {
    firstName.value = firstName.value.charAt(0).toUpperCase() + firstName.value.slice(1);
});

lastName.addEventListener('input', () => {
    lastName.value = lastName.value.charAt(0).toUpperCase() + lastName.value.slice(1);
});

function checkInputFields() {
    let firstNameIsValid = false;
    let lastNameIsValid = false;
    let dateOfBirthIsValid = false;
    let emailIsValid = false;
    let passwordIsValid = false;

    firstNameIsValid = ValidateName(firstName.value);
    checkFirstNameValidity(firstNameIsValid);

    lastNameIsValid = ValidateName(lastName.value);
    checkLastNameValidity(lastNameIsValid);

    dateOfBirthIsValid = ValidateDate(dateOfBirth.value);
    checkDateValidity(dateOfBirthIsValid);

    emailIsValid = ValidateEmail(emailAddress.value);
    checkEmailValidity(emailIsValid);

    passwordIsValid = ValidatePassword(passWord.value);
    checkPasswordValidity(passwordIsValid);

    return firstNameIsValid && lastNameIsValid && dateOfBirthIsValid && emailIsValid && passwordIsValid;
}

function ValidateName(name) {
    const expression = /^[A-Za-z]+$/; // Letters
    return expression.test(String(name));
}

function checkFirstNameValidity(firstNameIsValid) {
    if (firstNameIsValid) {
        firstNameErrorMessage.innerText = '';
    }
    else if (firstName.value === '') {
        firstNameErrorMessage.innerText = 'Voornaam is verplicht';
    }
    else {
        firstNameErrorMessage.innerText = 'Enkel letters toegestaan';
    }
}

function checkLastNameValidity(lastNameIsValid) {
    if (lastNameIsValid) {
        lastNameErrorMessage.innerText = '';
    }
    else if (lastName.value === '') {
        lastNameErrorMessage.innerText = 'Achternaam is verplicht';
    }
    else {
        lastNameErrorMessage.innerText = 'Enkel letters toegestaan';
    }
}

function ValidateDate(date) {
    return (date < '2001-12-31' && date > '1930-01-01'); // Data tussen 1930 en 2001
}

function checkDateValidity(dateOfBirthIsValid) {
    if (dateOfBirthIsValid) {
        dateOfBirthErrorMessage.innerText = '';
    }
    else if (dateOfBirth.value === '') {
        dateOfBirthErrorMessage.innerText = 'Geboortedatum is verplicht';
    }
    else {
        dateOfBirthErrorMessage.innerText = 'Enkel data tussen 1930 en 2001 toegestaan';
    }
}

function ValidateEmail(email)
{
    const expression = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Geldig e-mail formaat
    return expression.test(String(email).toLowerCase());
}

function checkEmailValidity(emailIsValid) {
    if (emailIsValid) {
        emailErrorMessage.innerText = '';
    }
    else if (emailAddress.value === '') {
        emailErrorMessage.innerText = 'E-mailadres is verplicht';
    }
    else {
        emailErrorMessage.innerText = 'Geen geldig e-mailadresformaat';
    }
}

function ValidatePassword(password)
{
    const expression = /^[A-Za-z]\w{7,14}$/; // Beginnen met letter, tussen de 7 en 14 tekens
    return expression.test(String(password));
}

function checkPasswordValidity(passwordIsValid) {
    if (passwordIsValid) {
        passWordErrorMessage.innerText = '';
    }
    else if (passWord.value === '') {
        passWordErrorMessage.innerText = 'Paswoord is verplicht';
    }
    else {
        passWordErrorMessage.innerText = 'Paswoord moet tussen 7 en 16 tekens zijn, met als eerste teken een letter';
    }
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
                postRequest();
                firstNameErrorMessage.innerText = '';
                lastNameErrorMessage.innerText = '';
                dateOfBirthErrorMessage.innerText = '';
                emailErrorMessage.innerText = '';
                passWordErrorMessage.innerText = '';
                //window.location.href = 'GebruikerInloggen.html';
            }
        })

        resetButton.addEventListener('click', () => {
            messageResult.innerText = '';
            firstNameErrorMessage.innerText = '';
            lastNameErrorMessage.innerText = '';
            dateOfBirthErrorMessage.innerText = '';
            emailErrorMessage.innerText = '';
            passWordErrorMessage.innerText = '';
        });
    } else {
        submitButton.disabled = true;
        resetButton.disabled = true;
        messageResult.innerText = 'U bent reeds ingelogd.';
    }
};
