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

// Get products
function logProductsData(result) {
    if (result.data) {
        // Alle producten opnemen in sessionStorage
        let obj = JSON.parse(result.data);
        console.log(obj);
        sessionStorage.setItem('products', JSON.stringify(obj));
    }
}

function getProductsData() {
    fetch('https://localhost:44377/api/Product')
        .then(validateResponse)
        .then(logResult)
        .then(logProductsData)
        .catch(logError);
}

window.onload = function() {
    // Producten ophalen
    getProductsData();
};
