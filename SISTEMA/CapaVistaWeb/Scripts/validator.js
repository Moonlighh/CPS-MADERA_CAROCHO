/*

Este código es un script de JavaScript que utiliza la librería Bootstrap para aplicar estilos personalizados
de validación a los formularios que tengan la clase needs-validation. Este script se ejecuta automáticamente
al cargar la página debido a la sintaxis de la función de flecha que lo envuelve en paréntesis y lo llama.

El código en sí utiliza dos métodos de JavaScript:

document.querySelectorAll('.needs-validation'): Este método busca en el documento HTML todos los elementos que
tengan la clase needs-validation. En este caso, está buscando todos los formularios que tengan esta clase.

Array.from(forms).forEach(form => {...}): Este método convierte el resultado anterior en un array y, luego,
aplica la función de flecha a cada elemento del array. En este caso, la función de flecha agrega un "escucha"
de evento al formulario y previene su envío si la validación del formulario no es correcta. También agrega la
clase was-validated al formulario para que Bootstrap aplique los estilos de validación personalizados.

En resumen, este código es utilizado por Bootstrap para personalizar la validación de los formularios y asegurarse
de que el usuario complete correctamente los campos requeridos. También impide que el formulario sea enviado si no
se ha completado correctamente.

*/

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