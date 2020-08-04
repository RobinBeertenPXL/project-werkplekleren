// General functions
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

// Get logged in user
let firstName = document.getElementById('voorn');
let lastName = document.getElementById('naam');
let dateOfBirth = document.getElementById('datum');
let emailAddress = document.getElementById('email');
let passWord = document.getElementById('paswoord');
let methodOfPayment = document.getElementById('betaalmethode');
let saveButton = document.getElementById('opslaan');

firstName.addEventListener('input', () => {
    firstName.value = firstName.value.charAt(0).toUpperCase() + firstName.value.slice(1);
});

lastName.addEventListener('input', () => {
    lastName.value = lastName.value.charAt(0).toUpperCase() + lastName.value.slice(1);
});

function logUserData(result) {
    if (result.data) {
        let obj = result.data;
        console.log(obj);
        sessionStorage.setItem('personid', obj.personid);

        fillInfoFields(result, firstName, lastName, dateOfBirth, emailAddress, passWord, methodOfPayment);

        let profileInfo = [firstName, lastName, dateOfBirth, emailAddress, passWord, methodOfPayment];

        changeButton.addEventListener('click', (e) => {
            e.preventDefault();

            profileInfo.forEach(field => {
                field.disabled = false;
            });

            changeButton.style.display = 'none';
            saveButton.style.display = 'initial';
        });

        saveButton.addEventListener('click', (e) => {
            e.preventDefault();

            profileInfo.forEach(field => {
                field.disabled = true;
            });

            if (checkInputFields()) {
                saveData(profileInfo);
            }

            changeButton.style.display = 'initial';
            saveButton.style.display = 'none';
        });
    }
}

function fillInfoFields(result, firstname, name, birthdate, email, password, methodOfPayment) {
    let obj = result.data;
    let dateOfBirth = obj.birthDate;
    firstname.value = `${obj.firstName}`;
    name.value = `${obj.name}`;
    birthdate.value = `${dateOfBirth.substr(0, dateOfBirth.indexOf('T'))}`;
    email.value = `${sessionStorage.getItem('email')}`;
    password.value = `${sessionStorage.getItem('pwd')}`;
    methodOfPayment.value = `${obj.paymentMethod}`;
    methodOfPayment.options = `${obj.paymentMethod}`;
}

// Check user input validity
let firstNameErrorMessage = document.getElementById('firstname-error');
let lastNameErrorMessage = document.getElementById('lastname-error');
let dateOfBirthErrorMessage = document.getElementById('date-error');
let emailErrorMessage = document.getElementById('email-error');
let passWordErrorMessage = document.getElementById('password-error');

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

// Update user
function saveData(profileInfo) {
    let userID = sessionStorage.getItem('userid');
    let personID = sessionStorage.getItem('personid');
    let firstname = profileInfo[0].value;
    let name = profileInfo[1].value;
    let birthdate = profileInfo[2].value;
    let email = profileInfo[3].value;
    let password = profileInfo[4].value;
    let payment = profileInfo[5].value;

    const data = {
        firstName: firstname,
        lastName: name,
        birthdate: birthdate,
        email: email,
        password: password,
        paymentMethod: payment
    };

    fetch(`https://localhost:44377/api/Account?userID=${userID}&personID=${personID}`, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    })
        .then(validateResponse)
        .then(logResult)
        .then(userExists)
        .catch(logError);
}

// Check if user exists and update session storage
let messageResult = document.getElementById('error');

function userExists(result) {
    if (!result.data) {
        messageResult.innerText = 'Dit e-mailadres bestaat al! Gelieve een ander e-mailadres te gebruiken.'
    }
    else {
        messageResult.innerText = 'Gebruikergegevens succesvol gewijzigd.';
        let oldPassword = sessionStorage.getItem('pwd');

        if (oldPassword !== passWord.value) {
            const data = {
                mailto: emailAddress.value,
                mailsubject: 'Paswoordwijziging',
                mailbody: 'Beste ' +  `${firstName.value}` + ' ' + `${lastName.value}` + '\n\nU heeft uw paswoord gewijzigd.\n\nMet vriendelijke groeten\n\nHet Roomy Team\nCorda Campus\nKempische Steenweg 293\n3500 Hasselt\nBelgië'
            };

            fetch('https://localhost:44377/api/Mail/', {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data),
            })
                .then(validateResponse)
                .then(logResult)
                .catch(logError);
        }

        sessionStorage.setItem('fname', firstName.value);
        sessionStorage.setItem('lname', lastName.value);
        sessionStorage.setItem('email', emailAddress.value);
        sessionStorage.setItem('pwd', passWord.value);
        sessionStorage.setItem('payment', methodOfPayment.value);
    }
}

// Get logged in user
let changeButton = document.getElementById('wijzigen');
let orderHistory = document.getElementById('aankoop-geschiedenis');

window.onload = function getUserData() {
    if (sessionStorage.getItem('email') === null || sessionStorage.getItem('pwd') === null || sessionStorage.getItem('email') === '' || sessionStorage.getItem('pwd') === '') {
        changeButton.disabled = true;
        orderHistory.disabled = true;
        messageResult.innerText = 'U moet ingelogd zijn om uw profielgegevens aan te passen.';
    } else {
        changeButton.disabled = false;
        orderHistory.disabled = false;

        orderHistory.addEventListener('click', (e) => {
            e.preventDefault();

            window.location.href = 'AankoopGeschiedenis.html';
        });

        fetch(`https://localhost:44377/api/Person?userID=${sessionStorage.getItem('userid')}`)
            .then(validateResponse)
            .then(logResult)
            .then(logUserData)
            .catch(logError);
    }
};
