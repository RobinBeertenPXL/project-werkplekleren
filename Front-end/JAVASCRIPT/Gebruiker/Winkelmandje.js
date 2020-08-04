// Shopping cart variables
let shoppingCart = JSON.parse(sessionStorage.getItem('shoppingCart'));

let cartSection = document.getElementById('winkelwagentje');
let cartRow;

let productName;
let productPrice;
let productAmount;
let productTotal;
let productDelete;
let productNameContent;
let productPriceContent;
let productTotalContent;

let productAmountContent;
let dataClass2;

let productDeleteContent;
let dataClass;

let payButtonContainer;
let payButton;
let totalText;
let totalPrice;

function clearShoppingCart() {
    sessionStorage.setItem('shoppingCart', JSON.stringify({}));

    let cartSection = document.getElementById('winkelwagentje');

    while (cartSection.childElementCount > 1) {
        cartSection.removeChild(cartSection.lastChild);
    }

    payButtonContainer.style.display = 'none';
}

window.onload = function() {
    // Payment
    payButtonContainer = document.getElementById('betaalknop-container');
    payButton = document.createElement('input');
    payButton.type = 'submit';
    payButton.id = 'betalen';
    payButton.value = 'Betalen';
    totalText = document.createElement('p');
    totalText.innerText = 'Subtotaal';
    totalText.id = 'subtotaal';
    totalPrice = document.createElement('p');
    calculateTotalPrice(shoppingCart);
    totalPrice.id = 'totale-prijs';
    payButtonContainer.appendChild(totalText);
    payButtonContainer.appendChild(totalPrice);
    payButtonContainer.appendChild(payButton);

    if (Object.keys(shoppingCart).length === 0) {
        payButtonContainer.style.display = 'none';
    }
    else {
        payButtonContainer.style.display = 'initial';
    }

    payButton.addEventListener('click', (e) => {
        e.preventDefault();

        sessionStorage.setItem('shoppingCart', JSON.stringify(shoppingCart));

        window.location.href = '../Bestelling/Bestelling.html';
        // postRequest();

        // clearShoppingCart();
    });

    console.log(shoppingCart);

    // Fill shopping cart
    for (let key in shoppingCart) {
        let product = shoppingCart[key];
        console.log(product);

        // Creation of row in shopping cart that contains all fields for each product
        cartRow = document.createElement('article');
        cartRow.id = `product${product.Productid}`;
        cartSection.appendChild(cartRow);

        // Creation of all fields in the shopping cart row
        productName = document.createElement('div');
        productPrice = document.createElement('div');
        productAmount = document.createElement('div');
        productTotal = document.createElement('div');
        productDelete = document.createElement('div');
        productNameContent = document.createElement('p');
        productPriceContent = document.createElement('p');
        productAmountContent = document.createElement('input');
        productTotalContent = document.createElement('p');
        productDeleteContent = document.createElement('input');

        // Product name
        productNameContent.innerText = product.Productname;
        productName.appendChild(productNameContent);
        cartRow.appendChild(productName);

        // Product price
        productPriceContent.innerText = `€ ${product.Price}`;
        productPrice.appendChild(productPriceContent);
        productPrice.classList.add('product-prijs');
        cartRow.appendChild(productPrice);

        // Product amount
        productAmountContent.type = 'number';
        productAmountContent.min = '1';
        productAmountContent.value = product.amount <= 0 ? 1 : product.amount;
        productAmountContent.id = `amount${product.Productid}`;

        dataClass2 = document.createAttribute('data-price')
        dataClass2.value = product.Price;
        productAmountContent.setAttributeNode(dataClass2);

        productAmountContent.addEventListener('change', () => {
            let obj = event.currentTarget;

            if (obj.value <= 0) {
               obj.value = 1;
            }

            let id = obj.id.replace('amount', '');
            let amountValue = obj.value;
            let currentPrice = obj.getAttribute('data-price');
            let totalField = document.getElementById('total' + id);
            totalField.innerText = `€ ${String(Math.round(((+amountValue * +currentPrice) + Number.EPSILON) * 100) / 100)}`;
            shoppingCart[id].amount = amountValue;
            //shoppingCart[id].Price = totalField.innerText;
            sessionStorage.setItem('shoppingCart', JSON.stringify(shoppingCart));
            calculateTotalPrice(shoppingCart);
        });

        productAmountContent.addEventListener('keydown', (e) => {
            //e.preventDefault();
        });

        productAmount.appendChild(productAmountContent);
        cartRow.appendChild(productAmount);

        // Product total price
        productTotalContent.innerText = `€ ${String(Math.round(((+product.amount * +product.Price) + Number.EPSILON) * 100) / 100)}`;
        productTotalContent.id = `total${product.Productid}`;
        productTotal.appendChild(productTotalContent);
        cartRow.appendChild(productTotal);

        // Product delete button
        productDeleteContent.type = 'submit';
        productDeleteContent.value = 'X';

        dataClass = document.createAttribute('data-productid');
        dataClass.value = `product${product.Productid}`;
        productDeleteContent.setAttributeNode(dataClass);

        productDeleteContent.addEventListener('click', (e) => {
            e.preventDefault();
            let obj = event.currentTarget;
            let currentId = obj.getAttribute('data-productid');
            let currentProduct = currentId.replace('product', '');
            let tempCartRow = document.getElementById(`${currentId}`);
            tempCartRow.remove();

            for (let i = 0; i < Object.keys(shoppingCart).length; i++) {
                let tempCart = Object.values(shoppingCart)[i];
                if (+tempCart.Productid === +currentProduct) {
                    delete shoppingCart[currentProduct];
                }
            }

            sessionStorage.setItem('shoppingCart', JSON.stringify(shoppingCart));

            if (Object.keys(shoppingCart).length === 0) {
                payButtonContainer.style.display = 'none';
            }
            else {
                payButtonContainer.style.display = 'initial';
            }

            calculateTotalPrice(shoppingCart);
        });

        productDelete.appendChild(productDeleteContent);
        productDelete.classList.add('product-verwijderen');
        cartRow.appendChild(productDelete);
    }
};

function calculateTotalPrice(shoppingCart) {
    let productsInCart = Object.values(shoppingCart);
    let total = 0;

    productsInCart.forEach(product => {
        total += Math.round(((+product.amount * +product.Price) + Number.EPSILON) * 100) / 100;
        //sessionStorage.setItem('totalPrice', total)
    });

    totalPrice.innerText = `€ ${total}`;
}

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
