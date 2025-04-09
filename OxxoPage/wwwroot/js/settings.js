document.addEventListener("DOMContentLoaded", function () {
    const changeButton = document.querySelector(".password-container button");

    changeButton.addEventListener("click", function () {
        const username = document.querySelector('input[placeholder="Tu usuario"]').value;
        const password = document.querySelector('input[placeholder="Tu contraseña"]').value;

        if (username && password) {
            alert(`Usuario: ${username}\nNueva contraseña: ${password}`);
            // Aquí podrías hacer un fetch o redirigir a otro proceso
        } else {
            alert("Por favor, llena todos los campos.");
        }
    });
});
