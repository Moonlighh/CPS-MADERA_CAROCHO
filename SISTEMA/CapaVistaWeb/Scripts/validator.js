/*function validateLogin(idLogin) {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    const dUsername = document.getElementById("username_alert");
    const dPassword = document.getElementById("password_alert");
    dUsername.style.display = "none";
    dPassword.style.display = "none";
    debugger
    if (username == "") {
        dUsername.innerText = "Username is required"
        dUsername.style.display = "block";
    }
    if (password == "") {
        dPassword.innerText = "Password is required"
        dPassword.style.display = "block";
    }
    if (username != "" && password != "") {
        const formulario = document.getElementById(idLogin);
        formulario.submit();
    }
}*/

(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()