function AgregarProductoCarrito(url) {
    Swal.fire({
        title: "¿Estás seguro de que deseas agregar este producto al carrito?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Agregar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            // Redirigir a la URL especificada
            window.location.href = url;
        }
    });
}

// Crea la función que se llamará cuando se haga clic en el botón "Cerrar sesión"
function cerrarSesion(url) {
    Swal.fire({
        title: "¿Está seguro que desea cerrar sesión?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Aceptar",
        cancelButtonText: "Cancelar",
        customClass: {
            confirmButton: "btn btn-primary mx-2",
            cancelButton: "btn btn-danger mx-2"
        }
    }).then((result) => {
        if (result.isConfirmed) {
            // Redirigir a la URL especificada
            window.location.href = url;
        }
    });
}