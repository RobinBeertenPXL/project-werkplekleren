// General functions
function setButtonVisibility(buttonId, display) {
    let profileButton = document.getElementById(buttonId);
    profileButton.style.display = display;
}

function userLoggedIn() {
    if (sessionStorage.getItem('email') === null || sessionStorage.getItem('pwd') === null || sessionStorage.getItem('email') === '' || sessionStorage.getItem('pwd') === '') {
        setButtonVisibility('inloggen', 'initial');
        setButtonVisibility('profielinfo', 'none');
        setButtonVisibility('aanmaken', 'initial');
        setButtonVisibility('uitloggen', 'none');
    }
    else {
        setButtonVisibility('inloggen', 'none');
        setButtonVisibility('profielinfo', 'initial');
        setButtonVisibility('aanmaken', 'none');
        setButtonVisibility('uitloggen', 'initial');
    }
}

// Switch header user buttons display
let logout = document.getElementById('uitloggen');

setButtonVisibility('profielinfo', 'none');
setButtonVisibility('uitloggen', 'none');

logout.addEventListener('click', () => {
    sessionStorage.setItem('email', '');
    sessionStorage.setItem('pwd', '');
    sessionStorage.setItem('userid', '');
    sessionStorage.setItem('shoppingCart', JSON.stringify({}));
    userLoggedIn();
});

userLoggedIn();
