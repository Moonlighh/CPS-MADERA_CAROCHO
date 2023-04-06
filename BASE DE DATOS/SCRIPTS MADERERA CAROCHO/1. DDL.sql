USE MASTER 
GO
alter database [BD_PRUEBAS_MADERERA] set single_user with rollback immediate
DROP DATABASE [BD_PRUEBAS_MADERERA]
CREATE DATABASE [BD_PRUEBAS_MADERERA]
USE [BD_PRUEBAS_MADERERA]
GO

CREATE TABLE UBIGEO(
	idUbigeo VARCHAR(6) UNIQUE NOT NULL,
	departamento varchar(32) DEFAULT NULL,
	provincia varchar(32) DEFAULT NULL,
	distrito varchar(32) DEFAULT NULL
)
GO

CREATE TABLE PROVEEDOR(
	idProveedor int primary key identity,
	razonSocial varchar(40) not null,
	dni varchar(8) not null,
	correo varchar(40),
	telefono varchar(9) default null,
	descripcion varchar (80),
	estProveedor bit default 1,
	idUbigeo VARCHAR(6) null

	constraint fk_Proveedor_Ubigeo foreign key (idUbigeo) references Ubigeo (idUbigeo)
)
GO

CREATE TABLE TIPO_PRODUCTO(
	idTipo_Producto int primary key identity,
	nombre varchar(30) not null,
)
GO

CREATE TABLE PRODUCTO(
	idProducto int primary key identity,
	nombre varchar(40) not null,
	longitud float not null,
	diametro float not null,
	precioVenta float not null,
	stock int default 0,
	idTipo_Producto int not null

	constraint fk_Producto_Tipo foreign key (idTipo_Producto) references TIPO_PRODUCTO (idTipo_Producto)
)
GO

CREATE TABLE PROVEEDOR_PRODUCTO
(
  idProvedoor_Producto int primary key identity,
  idProveedor int not null,
  idProducto int not null,
  precioCompra float not null,
 

  constraint fk_proveedor_producto_producto foreign key (idProducto) references PRODUCTO (idProducto),
  constraint fk_proveedor_producto_proveedor foreign key (idProveedor) references PROVEEDOR (idProveedor)
)
GO

CREATE TABLE ROL(
	idRol int primary key identity,
	descripcion varchar(50)
)
GO

CREATE TABLE TIPO_EMPLEADO(
	idTipo_Empleado int primary key identity,
	nombre varchar(30) not null
)
GO

CREATE TABLE EMPLEADO(
    idEmpleado int primary key identity,
    nombres varchar(50) not null,
    dni varchar(8) not null,
    telefono varchar(9) default null,
    direccion varchar(60) null,
    f_inicio date default getdate(),
    f_fin date default getdate(),
    salario float,
    descripcion varchar(50),
    estEmpleado bit default 1,
    idTipo_Empleado int null,
    idUbigeo VARCHAR(6) null


    constraint fk_EMPLEADO_TIPO foreign key (idTipo_Empleado) references TIPO_EMPLEADO (idTipo_Empleado),
    constraint fk_EMPLEADO_UBIGEO foreign key (idUbigeo) references Ubigeo (idUbigeo),
)
GO

CREATE TABLE CLIENTE(
	idCliente int primary key identity,
	razonSocial varchar(40) not null,
	dni varchar(8) not null,
	telefono varchar(9),
	direccion varchar(60),
	idUbigeo VARCHAR(6),
	fecCreacion datetime default getdate(),
	correo varchar(40),
	userName varchar (20) not null,
	pass varchar(200) null,
	idRol int,
	activo bit default 1,

	constraint fk_Cliente_Ubigeo foreign key (idUbigeo) references Ubigeo (idUbigeo),
	constraint fk_usuario_rol foreign key(idRol) references Rol (idRol)
)
GO

CREATE TABLE VENTA(
	idVenta int primary key identity,
	fecha datetime default getdate(),
	total float not null,
	estado bit default 1,--Pagado -En espera
	idCliente int not null

	constraint fk_Venta_Cliente foreign key (idCliente) references CLIENTE (idCliente)
)
GO

CREATE TABLE DETALLE_VENTA(
	idDetVenta int identity,
	idVenta int not null,
	idProducto int not null,
	cantidad int not null,
	subTotal float not null
	
	constraint pk_detVenta primary key (idDetVenta), 
	constraint fk_detVenta_Venta foreign key (idVenta) references VENTA (idVenta),
	constraint fk_detVenta_Producto foreign key (idProducto) references PRODUCTO (idProducto)
)
GO

CREATE TABLE COMPRA(
	idCompra int primary key identity,
	fecha date default getdate(),
	total float not null,
	idProveedor int not null

	constraint fk_Compra_Proveedor foreign key (idProveedor) references PROVEEDOR (idProveedor)
)
GO

CREATE TABLE DETALLE_COMPRA(
	idDetCompra int identity,
	idCompra int not null,
	idProducto int not null,
	cantidad int not null,
	subTotal float not null

	constraint pk_detCompra primary key (idDetCompra),
	constraint fk_detCompra_Compra foreign key (idCompra) references COMPRA (idCompra),
	constraint fk_detCompra_Producto foreign key (idProducto) references PRODUCTO (idProducto)
)
GO

--------------------------------------------RESTRICCIONES---------------------------------------------
--CLIENTE
ALTER TABLE CLIENTE ADD CONSTRAINT UQ_CLIENTE_dni UNIQUE(dni);
ALTER TABLE CLIENTE ADD	CONSTRAINT CHK_CLIENTE_telefono CHECK(telefono LIKE '9[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or telefono = '' or telefono LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE CLIENTE ADD	CONSTRAINT CHK_CLIENTE_dni CHECK(dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE CLIENTE ADD CONSTRAINT uq_Cliente_userName UNIQUE(userName);
ALTER TABLE CLIENTE ADD CONSTRAINT uq_Cliente_correo UNIQUE(correo);
--PROVEEDOR
ALTER TABLE PROVEEDOR ADD CONSTRAINT  UQ_PROVEEDOR_dni UNIQUE(dni);
ALTER TABLE PROVEEDOR ADD CONSTRAINT CHK_PROVEEDOR_telefono CHECK(telefono LIKE '9[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or telefono = '' or telefono LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE PROVEEDOR ADD CONSTRAINT CHK_PROVEEDOR_dni CHECK(dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE PROVEEDOR ADD CONSTRAINT CHK_estProveedor CHECK(estProveedor LIKE '[0-2]') 
--EMPLEADO
ALTER TABLE EMPLEADO ADD CONSTRAINT UQ_EMPLEADO_dni UNIQUE(dni);
ALTER TABLE EMPLEADO ADD CONSTRAINT CHK_EMPLEADO_dni CHECK(dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
ALTER TABLE EMPLEADO ADD CONSTRAINT CHK_EMPLEADO_telefono CHECK(telefono LIKE '9[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or telefono = '' or telefono LIKE '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
GO

