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
