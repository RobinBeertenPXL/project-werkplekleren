// General functions
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

let orderHistoryContainer = document.getElementById('product-geschiedenis');
let collapseButton, collapseDiv, reorderButton, noOrderHistory;

let cart = {};

function showOrderHistory(result) {
    if (!result.data) {
        noOrderHistory = document.createElement('p');
        noOrderHistory.id = 'geen-bestelgeschiedenis';
        noOrderHistory.innerText = 'U heeft nog niets bij ons besteld.';
        orderHistoryContainer.appendChild(noOrderHistory);
        console.log('404 not found');
    } else {
        let obj = JSON.parse(result.data);
        console.log(obj);
        let tempOrderId = 0;
        obj.forEach(order => {
            if (order.orderid !== tempOrderId) { // Nieuw order wanneer je in if komt, else nog steeds in zelfde order en orderline creëren
                tempOrderId = order.orderid;
                collapseButton = document.createElement('button');
                collapseDiv = document.createElement('div');
                reorderButton = document.createElement('button');
                reorderButton.type = 'button';
                reorderButton.id = `order${order.orderid}`;
                reorderButton.innerText = 'Opnieuw bestellen';

                reorderButton.addEventListener('click', () => {
                    for (let i = 0; i < obj.length; i++) {
                        if (obj[i].orderid === +event.currentTarget.id.replace('order', '')) {
                            cart[obj[i].productid] = {
                                Productname: `${obj[i].productname}`,
                                Price: `${obj[i].price}`,
                                amount: `${obj[i].amount}`,
                                Productid: `${obj[i].productid}`
                            }
                            sessionStorage.setItem(`shoppingCart`, JSON.stringify(cart));
                        }
                    }
                    window.location.href = 'Winkelmandje.html';
                });

                collapseButton.type = 'button';
                collapseButton.classList.add('collapsible');
                collapseButton.innerText = `Bestellingsnummer: ${order.orderid}` + '\n' + `Datum: ${order.creationdate.substr(0, order.creationdate.indexOf('T'))}`;
                collapseButton.appendChild(reorderButton);
                collapseDiv.classList.add('content');
                collapseDiv.id = `${order.orderid}`;

                orderHistoryContainer.appendChild(collapseButton);
                orderHistoryContainer.appendChild(collapseDiv);
            }

            CreateOrderLine(order);
        });

        MakeCollapsible();
    }
}

let collapseDivParagraph, currentDiv;

function CreateOrderLine(order) {
    currentDiv = document.getElementById(`${order.orderid}`);
    collapseDivParagraph = document.createElement('p');
    collapseDivParagraph.innerText = `Productnaam: ${order.productname}` + '\n' + `Prijs: ${order.price}` + '\n' + `Hoeveelheid: ${order.amount}`;
    currentDiv.appendChild(collapseDivParagraph);
}

function MakeCollapsible() {
    let coll = document.getElementsByClassName("collapsible");
    let i;

    for (i = 0; i < coll.length; i++) {
        coll[i].addEventListener("click", function() {
            this.classList.toggle("active");
            let content = this.nextElementSibling;
            if (content.style.display === "block") {
                content.style.display = "none";
            } else {
                content.style.display = "block";
            }
        });
    }
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

function getOrderData() {
    const userID = sessionStorage.getItem('userid');

    fetch(`https://localhost:44377/api/Order?userId=${userID}`)
        .then(validateResponse)
        .then(logResult)
        .then(showOrderHistory)
        .catch(logError);
}

window.onload = function () {
    getOrderData();
};
