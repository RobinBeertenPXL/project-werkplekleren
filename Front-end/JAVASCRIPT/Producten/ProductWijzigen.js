const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const productid = urlParams.get('productid');

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

let productDescriptionField = document.getElementById('productomschrijving');
let productNameField = document.getElementById('productnaam');
let productPriceField = document.getElementById('productprijs');
let productImageField = document.getElementById('productafbeelding');
let productCategoryField = document.getElementById('productcategorie');
let changeButton = document.getElementById('wijzigen');
let saveButton = document.getElementById('opslaan');

function logProductData(result) {
    if (result.data) {
        fillInfoFields(result, productDescriptionField, productNameField, productPriceField, productImageField, productCategoryField);

        let productInfo = [productDescriptionField, productNameField, productPriceField, productImageField, productCategoryField];

        changeButton.addEventListener('click', (e) => {
            e.preventDefault();

            productInfo.forEach(field => {
                field.disabled = false;
            });

            changeButton.style.display = 'none';
            saveButton.style.display = 'initial';
        });

        saveButton.addEventListener('click', (e) => {
            e.preventDefault();

            console.log('save button pressed');

            productInfo.forEach(field => {
                field.disabled = true;
            });

            if (checkInputFields()) {
                saveData(productInfo);
            }

            changeButton.style.display = 'initial';
            saveButton.style.display = 'none';
        });
    }
}

function fillInfoFields(result, productDescriptionField, productNameField, productPriceField, productImageField, productCategoryField) {
    let obj = JSON.parse(result.data);
    productDescriptionField.value = `${obj.Description}`;
    productNameField.value = `${obj.Productname}`;
    productPriceField.value = `${obj.Price}`;
    productImageField.value = `${obj.productImage}`;
    productCategoryField.value = `${obj.category}`;
    productCategoryField.options = `${obj.category}`;
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
        productImageField.innerText = '';
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

// Update product
function saveData(productInfo) {
    let prodname = productInfo[1].value;
    let prodprice = productInfo[2].value;
    let prodimage = productInfo[3].value;
    let proddescription = productInfo[0].value;
    let prodcategory = productInfo[4].value;
    const data = {
        productName: prodname,
        price: prodprice,
        productimage: prodimage,
        description: proddescription,
        category: prodcategory
    };

    fetch(`https://localhost:44377/api/Product?productID=${productid}`, {
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

// Check if product exists
let messageResult = document.getElementById('error');

function productExists(result) {
    if (result.data) {
        messageResult.innerText = 'Productgegevens succesvol gewijzigd.';
    }
}

// Get product data
window.onload = function() {
    fetch(`https://localhost:44377/api/Product?productId=${productid}`)
        .then(validateResponse)
        .then(logResult)
        .then(logProductData)
        .catch(logError);
};
