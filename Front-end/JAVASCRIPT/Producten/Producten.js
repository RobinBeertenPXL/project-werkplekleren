let productsList = document.querySelector('.tabel');
let products;
let surroundingDiv;
let text;
let productImage;
let productImageContainer;

// Fetch products
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

// Get products
function logProductsData(result) {
    if (result.data) {
        // Alle producten opnemen in sessionStorage
        let obj = JSON.parse(result.data);
        console.log(obj);
        sessionStorage.setItem('products', JSON.stringify(obj));
        products = obj;
    }
}

function getProductsData() {
    fetch('https://localhost:44377/api/Product')
        .then(validateResponse)
        .then(logResult)
        .then(logProductsData)
        .then(makeProductsPage)
        .catch(logError);
}

let productContainers = [];

function makeProductsPage() {
    products.forEach(prod => {
        surroundingDiv = document.createElement('div');
        surroundingDiv.classList.add('lijn');
        surroundingDiv.style.cursor = 'pointer';

        surroundingDiv.addEventListener('click', () => {
            window.location.href = `IndividueelProduct.html?productid=${prod.productid}`;
        });

        productContainers.push(surroundingDiv);
        productsList.appendChild(surroundingDiv);

        text = document.createElement('p');
        productImageContainer = document.createElement('figure');
        productImageContainer.id = 'product-afbeelding';
        productImage = document.createElement('img');



        productImage.src = `../../Assets/Afbeeldingen/Meubels/${prod.category}/${prod.productimage}.jpg`;
        productImage.alt = `${prod.productimage}`;

        productImageContainer.appendChild(productImage.cloneNode(true));
        surroundingDiv.appendChild(productImageContainer.cloneNode(true));
        text.innerText = `${prod.productname}`;
        surroundingDiv.appendChild(text.cloneNode(true));
        text.innerText = `${prod.category}`;
        surroundingDiv.appendChild(text.cloneNode(true));
        text.innerText = `€ ${prod.price}`;
        surroundingDiv.appendChild(text.cloneNode(true));
    });
}

// Product toevoegen
let addProductContainer = document.getElementById('product-toevoegen-container');
let addProductButton = document.createElement('button');
addProductButton.innerText = 'Product toevoegen';
let inputArray = document.getElementsByTagName('input');
let radioButtons = [];

for (let i = 0; i < inputArray.length; i++) {
    if (inputArray[i].type === 'radio') {
        radioButtons.push(inputArray[i]);
    }
}

window.onload = function() {
    getProductsData();

    addProductButton.addEventListener('click', () => {
       window.location.href = 'ProductToevoegen.html';
    });
    addProductContainer.appendChild(addProductButton);

    if (sessionStorage.getItem('email') === 'admin' && sessionStorage.getItem('pwd') === 'admin') {
        addProductContainer.style.display = 'initial';
    } else {
        addProductContainer.style.display = 'none';
    }

    console.log(productContainers);
    console.log(sessionStorage.getItem('shoppingCart'));

    radioButtons[0].checked = true;

    radioButtons.forEach(button => {
       button.addEventListener('change', () => {
           productContainers.forEach(product => {
               if (button.value === 'Alle producten') {
                   product.style.display = 'initial';
               } else {
                   if (button.value === product.childNodes[2].innerText) {
                       product.style.display = 'initial';
                   } else {
                       product.style.display = 'none';
                   }
               }
           });
       })
    });
}
