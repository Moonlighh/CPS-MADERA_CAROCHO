/*

Este c�digo es un script de JavaScript que utiliza la librer�a Bootstrap para aplicar estilos personalizados
de validaci�n a los formularios que tengan la clase needs-validation. Este script se ejecuta autom�ticamente
al cargar la p�gina debido a la sintaxis de la funci�n de flecha que lo envuelve en par�ntesis y lo llama.

El c�digo en s� utiliza dos m�todos de JavaScript:

document.querySelectorAll('.needs-validation'): Este m�todo busca en el documento HTML todos los elementos que
tengan la clase needs-validation. En este caso, est� buscando todos los formularios que tengan esta clase.

Array.from(forms).forEach(form => {...}): Este m�todo convierte el resultado anterior en un array y, luego,
aplica la funci�n de flecha a cada elemento del array. En este caso, la funci�n de flecha agrega un "escucha"
de evento al formulario y previene su env�o si la validaci�n del formulario no es correcta. Tambi�n agrega la
clase was-validated al formulario para que Bootstrap aplique los estilos de validaci�n personalizados.

En resumen, este c�digo es utilizado por Bootstrap para personalizar la validaci�n de los formularios y asegurarse
de que el usuario complete correctamente los campos requeridos. Tambi�n impide que el formulario sea enviado si no
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