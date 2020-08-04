// General functions add product
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

let messageResult = document.getElementById('error');

function productExists(result) {
    if (result.data) {
        messageResult.innerText = 'Product succesvol toegevoegd.';
    }
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

// Post product
let productNameField = document.getElementById('productnaam');
let productPriceField = document.getElementById('productprijs');
let productDescriptionField = document.getElementById('productomschrijving');
let productCategoryField = document.getElementById('productcategorie');
let productImageField = document.getElementById('productafbeelding');

function postRequest() {
    const productName = productNameField.value;
    const productPrice = productPriceField.value;
    const productDescription = productDescriptionField.value;
    const productCategory = productCategoryField.value;
    const productImage = productImageField.value;
    const data = {
        productName: productName,
        price: productPrice,
        productimage: productImage,
        description: productDescription,
        category: productCategory
    };

    fetch('https://localhost:44377/api/Product/', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    })
        .then(validateResponse)
        .then(logResult)
        .then(productExists)
        .catch(logError);
}

// Check product input validity
let productNameFieldError = document.getElementById('name-error');
let productPriceFieldError = document.getElementById('price-error');
let productImageFieldError = document.getElementById('image-error');
let productDescriptionFieldError = document.getElementById('description-error');

function checkInputFields() {
    let nameIsValid = false;
    let priceIsValid = false;
    let imageIsValid = false;
    let descriptionIsValid = false;

    if (checkInputFieldValidity(productNameField)) {
        productNameFieldError.innerText = '';
        nameIsValid = true;
    } else {
        productNameFieldError.innerText = 'Gelieve een productnaam op te geven';
    }

    if (checkPriceValidity()) {
        priceIsValid = true;
    }

    if (checkInputFieldValidity(productImageField)) {
        productImageFieldError.innerText = '';
        imageIsValid = true;
    } else {
        productImageFieldError.innerText = 'Gelieve de naam van een bestaande afbeelding op te geven';
    }

    if (checkInputFieldValidity(productDescriptionField)) {
        productDescriptionFieldError.innerText = '';
        descriptionIsValid = true;
    } else {
        productDescriptionFieldError.innerText = 'Gelieve een productomschrijving op te geven';
    }

    return nameIsValid && priceIsValid && imageIsValid && descriptionIsValid;
}

function checkInputFieldValidity(inputField) {
    return inputField.value !== '';
}

function checkPriceValidity() {
    productPriceFieldError.innerText = '';
    let priceString = +productPriceField.value;

    if (productPriceField.value === '') {
        productPriceFieldError.innerText = 'Gelieve een prijs op te geven';
    }
    else if (isNaN(priceString)) {
        productPriceFieldError.innerText = 'Gelieve een geldig getal als prijs op te geven';
    }
    else if (parseFloat(productPriceField.value) <= 0) {
        productPriceFieldError.innerText = 'Gelieve een getal groter dan 0 op te geven';
    }

    return productPriceFieldError.innerText === '';
}

// Add product to page
let addProductButton = document.getElementById('wijzigen');

window.onload = function () {
    addProductButton.addEventListener('click', (e) => {
        e.preventDefault();

        if (checkInputFields()) {
            postRequest();

            productNameField.value = '';
            //productPriceField.value = '';
            productImageField.value = '';
            productDescriptionField.value = '';
            //productCategoryField.value = '';
        }
    })
}
