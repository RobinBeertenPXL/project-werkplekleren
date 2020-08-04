let productSection = document.getElementById('producten');
let firstName = document.getElementById('voornaam');
let lastName = document.getElementById('naam');
let emailAddress = document.getElementById('email');
let totalPrice = sessionStorage.getItem('totalPrice');
let totalPriceField = document.getElementById('totale-productprijs');

let deliveryOptions = document.bezorgopties.bezorging;
let deliveryCost = document.getElementById('verzendkosten');

let finalTotal = document.getElementById('eindtotaal');

let confirmOrder = document.getElementById('bestellen');
let cancelOrder = document.getElementById('annuleren');
let orderMessage = document.getElementById('bevestiging-bestelling');

let shoppingCart = JSON.parse(sessionStorage.getItem('shoppingCart'));

let productName, productAmount, productPrice, breakLine;

let allInputs = document.querySelectorAll('input');
let paymentOptions = [];
let paymentMethod = sessionStorage.getItem('payment');
console.log(paymentMethod);

allInputs.forEach(input => {
    if (input.id.substring(0, 5) === 'optie') {
        paymentOptions.push(input);
    }
});

// Orderlines
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

let messageResult = document.getElementById('error');

function orderAdded(result) {
    if (!result.data)
        messageResult.innerText = 'Niet gelukt';
    else
        messageResult.innerText = 'Gelukt';
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

let userId = sessionStorage.getItem('userid');

function postRequest() {
    const data = shoppingCart;

    fetch(`https://localhost:44377/api/Order?userID=${userId}`, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    })
        .then(validateResponse)
        .then(logResult)
        .then(orderAdded)
        .catch(logError);
}

function clearShoppingCart() {
    sessionStorage.setItem('shoppingCart', JSON.stringify({}));

    window.location.href = '../Homepage/Homepage.html';
}

let currentFirstName = sessionStorage.getItem('fname');
let currentLastName = sessionStorage.getItem('lname');
let currentEmailAddress = sessionStorage.getItem('email');
let orderPlaced = 'Beste ' + `${currentFirstName} ${currentLastName}` + '\n\nHartelijk bedankt voor uw bestelling! Wij bezorgen ze zo snel mogelijk naar het opgegeven adres.\n\nMet vriendelijke groeten\n\nHet Roomy Team\nCorda Campus\nKempische Steenweg 293\n3500 Hasselt\nBelgië';
let orderCancelled = 'Beste ' + `${currentFirstName} ${currentLastName}` + '\n\nUw bestelling is geannuleerd.\n\nMet vriendelijke groeten\n\nHet Roomy Team\nCorda Campus\nKempische Steenweg 293\n3500 Hasselt\nBelgië';

function SendEmail(orderMessage) {
    const data = {
        mailto: `${currentEmailAddress}`,
        mailsubject: 'Status van uw bestelling bij roomy.com',
        mailbody: `${orderMessage}`
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

// Check user fields validity
let postalNumber = document.getElementById('postcode');
let streetName = document.getElementById('straat');
let houseNumber = document.getElementById('nummer');
let town = document.getElementById('plaats');
let phoneNumber = document.getElementById('telefoon');

function checkInputFields() {
    let postalIsValid = false;
    let streetIsValid = false;
    let houseNumberIsValid = false;
    let townIsValid = false;
    let phoneIsValid = false;

    if (checkInputFieldValidity(postalNumber)) {
        postalIsValid = true;
    }

    if (checkInputFieldValidity(streetName)) {
        streetIsValid = true;
    }

    if (checkInputFieldValidity(houseNumber)) {
        houseNumberIsValid = true;
    }

    if (checkInputFieldValidity(town)) {
        townIsValid = true;
    }

    if (checkInputFieldValidity(phoneNumber)) {
        phoneIsValid = true;
    }

    return postalIsValid && streetIsValid && houseNumberIsValid && townIsValid && phoneIsValid;
}

function checkInputFieldValidity(inputField) {
    return inputField.value !== '';
}

// Fill page fields onload
window.onload = function() {
    firstName.value = sessionStorage.getItem('fname');
    lastName.value = sessionStorage.getItem('lname');
    emailAddress.value = sessionStorage.getItem('email');
    let total = 0;
    console.log(totalPrice);
    console.log(shoppingCart);

    if (Object.keys(shoppingCart).length === 0) {
        confirmOrder.disabled = true;
        cancelOrder.disabled = true;
        orderMessage.innerText = 'Gelieve in te loggen en een product aan het winkelmandje toe te voegen om een bestelling te kunnen plaatsen.';
    } else {
        paymentOptions.forEach(option => {
            if (option.value === paymentMethod) {
                option.checked = true;
            } else if (paymentMethod === 'Geen betaalmethode gekozen') {
                paymentOptions[0].checked = true;
            }
        });

        confirmOrder.addEventListener('click', (e) => {
            e.preventDefault();

            if (checkInputFields()) {
                postRequest();

                orderMessage.innerText = 'Bedankt voor uw bestelling! Wij hebben ze goed ontvangen. U wordt binnen enkele seconden terug naar de Homepagina gebracht.';
                setTimeout(clearShoppingCart, 7000);

                SendEmail(orderPlaced);
            }
            else {
                orderMessage.innerText = 'Gelieve alle verplichte velden, aangeduid met een "*", in te vullen.'
            }
        });

        cancelOrder.addEventListener('click', (e) => {
            e.preventDefault();

            orderMessage.innerText = 'U bestelling is geannuleerd. U ontvangt hier een mail over. U wordt binnen enkele seconden terug naar de vorige pagina geleid.';
            setTimeout(function() { window.location.href = '../Gebruiker/Winkelmandje.html' }, 7000);

            SendEmail(orderCancelled);
        });

        for (let key in shoppingCart) {
            let product = shoppingCart[key];

            productName = document.createElement('p');
            productAmount = document.createElement('p');
            productPrice = document.createElement('p');
            breakLine = document.createElement('hr');

            productName.classList.add('kol1');
            productAmount.classList.add('kol2');
            productPrice.classList.add('kol3');

            productName.innerText = product.Productname;
            productAmount.innerText = product.amount;
            total += Math.round(((+product.amount * +product.Price) + Number.EPSILON) * 100) / 100;
            productPrice.innerText = `€ ${Math.round(((+product.amount * +product.Price) + Number.EPSILON) * 100) / 100}`;

            productSection.appendChild(productName);
            productSection.appendChild(productAmount);
            productSection.appendChild(productPrice);
            productSection.appendChild(breakLine);
        }

        totalPriceField.innerText = `€ ${total}`;

        deliveryCost.innerText = `€ ${0}`;
        finalTotal.innerText = `€ ${Math.round(((+deliveryCost.innerText.substring(2) + +totalPriceField.innerText.substring(2)) + Number.EPSILON) * 100) / 100}`;

        for (let i = 0; i < deliveryOptions.length; i++) {
            deliveryOptions[i].addEventListener('change', function() {
                deliveryCost.innerText = `€ ${deliveryOptions[i].value}`;
                finalTotal.innerText =  `€ ${Math.round(((+deliveryCost.innerText.substring(2) + +totalPriceField.innerText.substring(2)) + Number.EPSILON) * 100) / 100}`;
            });
        }
    }
}
