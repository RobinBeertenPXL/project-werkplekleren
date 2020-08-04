// General functions individual product
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

let cartString = sessionStorage.getItem('shoppingCart');
let cart = cartString === null ? {} : JSON.parse(cartString);
let inCartMessage = document.getElementById('in-mandje-boodschap');

function logProductData(result) {
    let obj = JSON.parse(result.data);
    console.log(obj);
    obj.amount = 1;
    productImage.src = `../../Assets/Afbeeldingen/Meubels/${obj.category}/${obj.productImage}.jpg`;
    productImage.alt = `${obj.productImage}`;
    productName.innerText = obj.Productname;
    productPrice.innerText = `${obj.Price} euro`;
    productDescription.innerText = obj.Description;
    shoppingCart.addEventListener('click', () => {
        if (sessionStorage.getItem('email') === '' || sessionStorage.getItem('email') === null || sessionStorage.getItem('pwd') === '' || sessionStorage.getItem('pwd') === null) {
            inCartMessage.innerText = 'Gelieve in te loggen als u een product aan het winkelmandje wenst toe te voegen.';
        } else {
            cart[obj.Productid] = obj;
            sessionStorage.setItem(`shoppingCart`, JSON.stringify(cart));
            inCartMessage.innerText = 'Het product is in uw winkelmandje geplaatst.';
        }
    });
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

// Get individual product
let productName = document.querySelector('.naam-artikel');
let productPrice = document.querySelector('.prijs-artikel');
let productDescription = document.getElementById('omschrijving-artikel');
let productImage = document.getElementById('afbeelding1');
let shoppingCart = document.getElementById('in-winkelmandje');

function getProductsData(productid) {
    fetch(`https://localhost:44377/api/Product?productID=${productid}`)
        .then(validateResponse)
        .then(logResult)
        .then(logProductData)
        .catch(logError);
}

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
const productid = urlParams.get('productid');

let articleInfo = document.getElementById('artikel-kopen');
let changeProductButton = document.createElement('button');
changeProductButton.innerText = 'Product wijzigen';
articleInfo.appendChild(changeProductButton);
let currentUserEmail = sessionStorage.getItem('email');
let currentUserPassword = sessionStorage.getItem('pwd');

window.onload = function () {
    if (currentUserEmail === 'admin' && currentUserPassword === 'admin') {
        changeProductButton.style.display = 'initial';
    }
    else {
        changeProductButton.style.display = 'none';
    }

    changeProductButton.addEventListener('click', () => {
        window.location.href = `ProductWijzigen.html?productid=${productid}`;
    });

    getProductsData(productid);
};
