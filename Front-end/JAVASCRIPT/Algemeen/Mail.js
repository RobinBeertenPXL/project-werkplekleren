// General functions
function validateResponse(response) {
    if (!response.ok)
        throw Error(response.statusText);
    return response;
}

function logResult(result) {
    return result.json();
}

let emailSend = document.getElementById('mail-verstuurd');

function clearMailFields(result) {
    if (result.data) {
        emailSend.innerText = 'Email succesvol verzonden.';
        subject.value = '';
        mailText.value = '';
    }
}

function logError(error) {
    console.log('Looks like there was a problem:', error);
}

// Send email
let submitButton = document.getElementById('send-email');
let recipient = document.getElementById('ontvanger');
let subject = document.getElementById('onderwerp');
let mailText = document.getElementById('mail-body');

window.onload = function() {
    submitButton.addEventListener('click', (e) => {
        e.preventDefault();

        if (subject.value === '' || mailText.value === '') {
            emailSend.innerText = 'Gelieve alle velden eerst in te vullen.';
        } else {
            const data = {
                mailto: recipient.value,
                mailsubject: subject.value,
                mailbody: mailText.value
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
                .then(clearMailFields)
                .catch(logError);
        }
    });
}
