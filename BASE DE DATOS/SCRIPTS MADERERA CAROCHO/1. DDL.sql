USE MASTER
AlTER DATABASE [BD_PRUEBAS_MADERERA]  SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE [BD_PRUEBAS_MADERERA]
CREATE DATABASE [BD_PRUEBAS_MADERERA]
USE [BD_PRUEBAS_MADERERA]
GO

CREATE TABLE UBIGEO(
	idUbigeo VARCHAR(6) UNIQUE not null,
	departamento VARCHAR(32) DEFAULT null,
	provincia VARCHAR(32) DEFAULT null,
	distrito VARCHAR(32) DEFAULT null
)
GO

CREATE TABLE PROVEEDOR(
	idProveedor INT PRIMARY KEY IDENTITY,
	razonSocial VARCHAR(40) not null,
	dni VARCHAR(8) not null,
	correo VARCHAR(40),
	telefono VARCHAR(9) DEFAULT null,
	descripcion VARCHAR (80),
	estProveedor BIT DEFAULT 1,
	idUbigeo VARCHAR(6) null

	CONSTRAINT fk_Proveedor_Ubigeo FOREIGN KEY (idUbigeo) REFERENCES Ubigeo (idUbigeo)
)
GO

CREATE TABLE TIPO_PRODUCTO(
	idTipo_Producto INT PRIMARY KEY IDENTITY,
	tipo VARCHAR(30) not null,
)
GO

CREATE TABLE PRODUCTO(
	idProducto INT PRIMARY KEY IDENTITY,
	nombre VARCHAR(40) not null,
	longitud FLOAT not null,
	diametro FLOAT not null,
	precioVenta FLOAT not null,
	stock INT DEFAULT 0,
	idTipo_Producto INT not null

	CONSTRAINT fk_Producto_Tipo FOREIGN KEY (idTipo_Producto) REFERENCES TIPO_PRODUCTO (idTipo_Producto)
)
GO

CREATE TABLE PROVEEDOR_PRODUCTO
(
  idProveedor_Producto INT PRIMARY KEY IDENTITY,
  idProveedor INT not null,
  idProducto INT not null,
  precioCompra FLOAT not null,
 
  CONSTRAINT fk_proveedor_producto_producto FOREIGN KEY (idProducto) REFERENCES PRODUCTO (idProducto),
  CONSTRAINT fk_proveedor_producto_proveedor FOREIGN KEY (idProveedor) REFERENCES PROVEEDOR (idProveedor)
)
GO

CREATE TABLE TIPO_EMPLEADO(
	idTipo_Empleado INT PRIMARY KEY IDENTITY,
	nombre VARCHAR(30) not null
)
GO

CREATE TABLE EMPLEADO(
    idEmpleado INT PRIMARY KEY IDENTITY,
    nombres VARCHAR(50) not null,
    dni VARCHAR(8) not null,
    telefono VARCHAR(9) DEFAULT null,
    direccion VARCHAR(60) null,
    f_inicio DATETIME DEFAULT GETDATE(),
    f_fin DATE DEFAULT GETDATE(),
    salario FLOAT,
    descripcion VARCHAR(50),
    estEmpleado BIT DEFAULT 1,
    idTipo_Empleado INT null,
    idUbigeo VARCHAR(6) null


    CONSTRAINT fk_EMPLEADO_TIPO FOREIGN KEY (idTipo_Empleado) REFERENCES TIPO_EMPLEADO (idTipo_Empleado),
    CONSTRAINT fk_EMPLEADO_UBIGEO FOREIGN KEY (idUbigeo) REFERENCES Ubigeo (idUbigeo),
)
GO

CREATE TABLE ROL(
	idRol INT PRIMARY KEY IDENTITY,
	descripcion VARCHAR(50)
)
GO

CREATE TABLE USUARIO(
	idUsuario INT PRIMARY KEY IDENTITY,
	razonSocial VARCHAR(40) not null,
	dni VARCHAR(8) not null,
	telefono VARCHAR(9),
	direccion VARCHAR(60),
	idUbigeo VARCHAR(6),
	fecCreacion DATETIME DEFAULT GETDATE(),
	correo VARCHAR(40),
	userName VARCHAR (20) not null,
	pass VARCHAR(200) null,
	idRol INT,
	activo BIT DEFAULT 1,

	CONSTRAINT fk_Usuario_Ubigeo FOREIGN KEY (idUbigeo) REFERENCES Ubigeo (idUbigeo),
	CONSTRAINT fk_Usuario_rol FOREIGN KEY(idRol) REFERENCES Rol (idRol)
)
GO

CREATE TABLE CARRITO(
	idCarrito INT PRIMARY KEY IDENTITY,
	idCliente INT,
	idProveedor_Producto INT,
	cantidad INT,
	subtotal FLOAT,
	CONSTRAINT fk_Carrito_Cliente FOREIGN KEY (idCliente) REFERENCES Usuario (idUsuario),
	CONSTRAINT fk_Carrito_ProveedorProducto FOREIGN KEY (idProveedor_Producto) REFERENCES PROVEEDOR_PRODUCTO (idProveedor_Producto)
)
GO

CREATE TABLE COMPRA(
	idCompra INT PRIMARY KEY IDENTITY,
	fecha DATETIME DEFAULT GETDATE(),
	total FLOAT not null,
	estado BIT DEFAULT 0,-- 0 En espera 1 Pagado
	idUsuario INT not null

	CONSTRAINT fk_Compra_Usuario FOREIGN KEY (idUsuario) REFERENCES USUARIO (idUsuario)
)
GO

CREATE TABLE DETALLE_COMPRA(
	idDetCompra INT IDENTITY,
	idCompra INT not null,
	idProducto INT not null,
	cantidad INT not null,
	subTotal FLOAT not null

	CONSTRAINT pk_detCompra PRIMARY KEY (idDetCompra),
	CONSTRAINT fk_detCompra_Compra FOREIGN KEY (idCompra) REFERENCES COMPRA (idCompra),
	CONSTRAINT fk_detCompra_Producto FOREIGN KEY (idProducto) REFERENCES PRODUCTO (idProducto)
)
GO

CREATE TABLE VENTA(
	idVenta INT PRIMARY KEY IDENTITY,
	fecha DATETIME DEFAULT GETDATE(),
	total FLOAT not null,
	estado BIT DEFAULT 1,-- 0 En espera 1 Pagado
	idUsuario INT not null

	CONSTRAINT fk_Venta_Cliente FOREIGN KEY (idUsuario) REFERENCES USUARIO (idUsuario)
)
GO

CREATE TABLE DETALLE_VENTA(
	idDetVenta INT IDENTITY,
	idVenta INT not null,
	idProducto INT not null,
	cantidad INT not null,
	subTotal FLOAT not null
	
	CONSTRAINT pk_detVenta PRIMARY KEY (idDetVenta), 
	CONSTRAINT fk_detVenta_Venta FOREIGN KEY (idVenta) REFERENCES VENTA (idVenta),
	CONSTRAINT fk_detVenta_Producto FOREIGN KEY (idProducto) REFERENCES PRODUCTO (idProducto)
)
GO

--------------------------------------------RESTRICCIONES---------------------------------------------
--USUARIO
ALTER TABLE USUARIO ADD CONSTRAINT UQ_USUARIO_dni UNIQUE(dni);
ALTER TABLE USUARIO ADD	CONSTRAINT CHK_USUARIO_telefono CHECK(telefono LIKE '9[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or telefono = '' or telefono LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE USUARIO ADD	CONSTRAINT CHK_USUARIO_dni CHECK(dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]');
ALTER TABLE USUARIO ADD CONSTRAINT uq_USUARIO_userName UNIQUE(userName);
ALTER TABLE USUARIO ADD CONSTRAINT uq_USUARIO_correo UNIQUE(correo);
--PROVEEDOR
ALTER TABLE PROVEEDOR ADD CONSTRAINT UQ_PROVEEDOR_dni UNIQUE(dni);
ALTER TABLE PROVEEDOR ADD CONSTRAINT CHK_PROVEEDOR_telefono CHECK(telefono LIKE '9[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or telefono = '' or telefono LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE PROVEEDOR ADD CONSTRAINT CHK_PROVEEDOR_dni CHECK(dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE PROVEEDOR ADD CONSTRAINT CHK_PROVEEDOR_estProveedor CHECK(estProveedor LIKE '[0-2]') 
--EMPLEADO
ALTER TABLE EMPLEADO ADD CONSTRAINT UQ_EMPLEADO_dni UNIQUE(dni);
ALTER TABLE EMPLEADO ADD CONSTRAINT CHK_EMPLEADO_dni CHECK(dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE EMPLEADO ADD CONSTRAINT CHK_EMPLEADO_telefono CHECK(telefono LIKE '9[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or telefono = '' or telefono LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
GO

