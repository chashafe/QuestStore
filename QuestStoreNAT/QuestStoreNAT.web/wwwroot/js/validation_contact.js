const nameField = document.getElementById('name');
const emailField = document.getElementById('email');
const subjectField = document.getElementById('subject');
const messageField = document.getElementById('message');
const sendButton = document.getElementById('send');

const checkName = () => {
    const nameComponents = nameField.value.split(" ");

    if (nameComponents[0] === ''){
        return false;
    }

    for (const component of nameComponents) {
        if (component[0] !== component[0].toUpperCase()){
            return false;
        }

    }
    return true;
}

const addNameAlert = () => {
    const newAlert = document.createElement('div');
    newAlert.className = "alert";
    newAlert.id = "name-alert";
    newAlert.innerHTML = 'Name must begin with a capital letter!';
    const nameLabel = nameField.nextSibling;
    const parent = nameField.parentElement;
    parent.insertBefore(newAlert, nameLabel);
}

const removeNameAlert = () => {
    const currentAlert = document.getElementById("name-alert");
    if (currentAlert !== null) {
        currentAlert.remove();
    }
}

const validateEmail = () => {
    const expression = /\S+@\S+\.\S+/;
    const email = emailField.value;
    return expression.test(email);
}

const addEmailAlert = () => {
    const newAlert = document.createElement('div');
    newAlert.className = "alert";
    newAlert.id = "email-alert";
    newAlert.innerHTML = `Invalid email address!`;
    const emailLabel = emailField.nextSibling;
    const parent = emailField.parentElement;
    parent.insertBefore(newAlert, emailLabel);
}

const removeEmailAlert = () => {
    const currentAlert = document.getElementById("email-alert");
    if (currentAlert !== null) {
        currentAlert.remove();
    }
}

const addConfirtmation = () => {
    const newAlert = document.createElement('div');
    newAlert.className = "confirmation";
    newAlert.id = "confirmation";
    newAlert.innerHTML = `The email has been sent!`;
    const nameLabel = nameField.previousSibling.previousSibling;
    const parent = nameField.parentElement;
    parent.insertBefore(newAlert, nameLabel);
}

const removeConfirmation = () => {
    const currentAlert = document.getElementById("confirmation");
    if (currentAlert !== null) {
        currentAlert.remove();
    }
}

const clearInput = () => {
    nameField.value = '';
    emailField.value = '';
    subjectField.value = '';
    messageField.value = '';
}


const sendHandler = () => {
    let valid = true;

    if (checkName() === true) {
        removeNameAlert();
    }

    else {
        removeNameAlert();
        addNameAlert();
        valid = false;
    }

    if (validateEmail() === true) {
        removeEmailAlert();
    }

    else {
        removeEmailAlert();
        addEmailAlert();
        valid = false;
    }

    if (valid) {
        removeConfirmation();
        addConfirtmation();
        clearInput();
    }
}

sendButton.addEventListener('click', sendHandler);