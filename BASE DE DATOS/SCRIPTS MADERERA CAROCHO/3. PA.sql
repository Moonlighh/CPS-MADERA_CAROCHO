-----------------PROCEDIMIENTOS ALAMCENADOS 
USE BD_PRUEBAS_MADERERA
GO

--====PROCEDIMIENTOS PARA EL SISTEMA =============

----===INICIAR SESION========
CREATE OR ALTER PROCEDURE spIniciarSesion(@dato varchar(40), @contra varchar(200))
AS
BEGIN
	SELECT *from CLIENTE c 
	inner join Rol r on r.idRol = c.idRol
	where (userName = @dato or correo = @dato) and pass = @contra
END
GO

---===== PROCEDIMINETOS PARA CLIENTE===========

--====CREAL CLIENTE===========
CREATE OR ALTER PROCEDURE spCrearCliente(
    @razonSocial varchar(40),
    @dni varchar(8),
    @telefono varchar(9),
    @direccion varchar(60),
    @idUbigeo VARCHAR(6),
	@correo varchar(40),
	@userName varchar(20),
	@pass varchar(200),
	@idRol int

)
AS
BEGIN
    INSERT INTO CLIENTE (razonSocial,dni,telefono,direccion,idUbigeo,correo,userName,pass, idRol) values (@razonSocial, @dni, @telefono, @direccion, @idUbigeo, @correo, @userName, @pass, @idRol);
END
GO

--===PROCEDIMIENTOS PARA UBIGEO======

---===LISTAR DISTRITOS====

CREATE OR ALTER PROCEDURE sp_ListaDistrito
As
BEGIN
    Select idUbigeo, distrito from Ubigeo where departamento='La Libertad'
	order by distrito;
END
GO
----========ROL======---
--CREATE OR ALTER PROCEDURE spListarRol
--AS
--BEGIN
--select * from rol where idRol=1 or idRol=3;
--END
--GO
----Actualizar Usuario
--CREATE OR ALTER PROCEDURE spActualizarUsuario(
--	@newUsuario varchar(40),
--	@contra varchar(200),
--	@oldUsuario varchar(40)
--)
--AS
--BEGIN
--	update ADMINISTRADOR set usuario = @newUsuario
--	where usuario = @oldUsuario and contra = @contra;
--END
--GO
----Actualizar Contraseña
--CREATE OR ALTER PROCEDURE spActualizarContra(
--	@usuario varchar(40),
--	@oldContra varchar(200),
--	@newContra varchar(200)
--)
--AS
--BEGIN
--	update ADMINISTRADOR set contra = @newContra
--	where usuario = @usuario and contra = @oldContra;
--END
--GO

--CREATE OR ALTER procedure spObtenerUsuario
--AS 
--BEGIN
--	SELECT u.idUsuario, u.userName, u.correo, u.pass from USUARIO u
--END
--GO
---------USUARIO CON ROL ADMINISTRADOR OR EMPLEADO------
--CREATE OR ALTER PROCEDURE spListarClienteAdmin
--AS
--BEGIN
--	select c.idCliente,c.razonSocial, c.dni,c.telefono,c.direccion,u.userName,u.correo,r.descripcion,u.activo from cliente c inner join usuario u
--	on c.idCliente=u.idCliente inner join
--	rol r on r.idRol=u.idRol where u.idRol=1 or u.idRol=3
--END
--GO

--CREATE OR ALTER PROCEDURE spBuscarClienteAdmin
--(
--@Campo varchar(20)
--)
--AS
--BEGIN
--select c.idCliente,c.razonSocial, c.dni,c.telefono,c.direccion,u.userName,u.correo,r.descripcion,u.activo from cliente c inner join usuario u
--	on c.idCliente=u.idCliente inner join
--	rol r on r.idRol=u.idRol where (u.idRol=1 or u.idRol=3 )and c.razonSocial like '%'+@Campo+'%';
--END
--GO

--CREATE OR ALTER PROCEDURE spEliminarUsuario
--(
-- @usuario int 
--)
--AS
--BEGIN
--   delete from usuario where idCliente=@usuario; 
--   delete from cliente where idCliente=@usuario;
--END
--GO

--------------------------------------UBIGEO
-------------------------------------

--LISTA DEPARTAMENTO
--CREATE OR ALTER PROCEDURE sp_ListaDepartamento
--AS
--BEGIN
--    Select  distinct (Departamento) from Ubigeo;
--END
--GO
-------------------------------------
--LISTA PROVINCIA
--CREATE OR ALTER PROCEDURE sp_ListaProvincia(@departamento varchar(32))
--As
--BEGIN
--    Select distinct(Provincia) from Ubigeo
--    where Departamento = @departamento;
--END
--GO
-------------------------------------


--------------------------------------PROVEEDOR
CREATE OR ALTER PROCEDURE spCrearProveedor
(
	@razonSocial varchar(40),
	@dni varchar(8),
	@correo varchar(40),
	@telefono varchar(9),
	@descripcion varchar (80),
	@estProveedor bit,
	@idUbigeo VARCHAR(6) null
)
AS
BEGIN
	INSERT INTO PROVEEDOR values (@razonSocial, @dni, @correo, @telefono, @descripcion, @estProveedor, @idUbigeo);
END
GO

CREATE OR ALTER PROCEDURE spListarProveedor
AS
BEGIN
	select p.idProveedor, p.razonSocial, p.dni, p.correo, p.telefono, p.descripcion, p.estProveedor,
	u.departamento,u.provincia, u.distrito from  PROVEEDOR p 
	inner join ubigeo u on p.idUbigeo = u.idUbigeo
	where p.estProveedor = 1;
END
GO

CREATE OR ALTER PROCEDURE spActualizarProveedor(
	@idProveedor int,	
	@razonSocial varchar(40),
	@dni varchar(8),
	@correo varchar(40),
	@telefono varchar(9),
	@descripcion varchar (80),
	@estProveedor bit,
	@idUbigeo VARCHAR(6) null
)
AS
BEGIN
	update PROVEEDOR set razonSocial = @razonSocial, dni = @dni, correo = @correo,
	telefono = @telefono, descripcion = @descripcion, estProveedor = @estProveedor, idUbigeo = @idUbigeo
	where idProveedor = @idProveedor;
END
GO

--CREATE OR ALTER PROCEDURE spEliminarProveedor(@idProveedor int)
--AS
--BEGIN
--	delete PROVEEDOR where idProveedor = @idProveedor;
--END
--GO

CREATE OR ALTER PROCEDURE spDeshabilitarProveedor(@idProveedor int)
AS
BEGIN
	update PROVEEDOR set estProveedor = 0 where idProveedor = @idProveedor;
END
GO

CREATE OR ALTER PROCEDURE spBuscarProveedor(
	@Campo varchar(40)
)
AS
BEGIN
	Select p.idProveedor, p.razonSocial, p.dni, p.correo, p.telefono, u.departamento, u.provincia, u.distrito, p.descripcion, p.estProveedor from PROVEEDOR p 
	inner join UBIGEO u on p.idUbigeo = u.idUbigeo 
	where razonSocial like @Campo+'%'or dni like @Campo+'%'	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdProveedor(
	@idProveedor int
)
AS
BEGIN
	Select p.idProveedor,p.razonSocial,p.dni,p.correo,p.telefono,p.descripcion,p.estProveedor,u.idUbigeo,u.distrito from PROVEEDOR p INNER JOIN UBIGEO u
	ON p.idUbigeo=u.idUbigeo where idProveedor= @idProveedor;
	
END
GO

--------------------------------------TIPO_EMPLEADO
--CREATE OR ALTER PROCEDURE spCrearTipoEmpleado(
--	@nombre varchar(30)
--)
--AS
--BEGIN
--	Insert into TIPO_EMPLEADO VALUES(@nombre);
--END
--GO

CREATE OR ALTER PROCEDURE spListarTipoEmpleado
AS
BEGIN
	select *from tipo_empleado;
END
GO

--CREATE OR ALTER PROCEDURE spActualizarTipoEmpleado(
--	@idTipo_Empleado int,
--	@nombre varchar(30)
--)
--AS
--BEGIN
--	update TIPO_EMPLEADO set nombre = @nombre
--	where idTipo_Empleado = @idTipo_Empleado
--END
--GO

------------------------------------EMPLEADO
CREATE OR ALTER PROCEDURE spCrearEmpleado
(
	@nombres varchar(40),
	@dni varchar(8),
	@telefono varchar(9),
	@direccion varchar(60),
	@salario float,
	@descripcion varchar(50),
	@idTipo_Empleado int,
	@idUbigeo VARCHAR(6)
)
AS
BEGIN
	insert empleado (nombres,dni, telefono, direccion, salario, descripcion, idTipo_Empleado, idUbigeo) 
	values          (@nombres, @dni, @telefono, @direccion, @salario, @descripcion, @idTipo_Empleado, @idUbigeo);
END
GO

CREATE OR ALTER PROCEDURE spListarEmpleado
AS
BEGIN
    SELECT e.idEmpleado, e.nombres, e.dni, e.telefono, e.direccion, e.salario, e.descripcion, t.nombre as tipo, u.distrito as distrito FROM EMPLEADO e 
    inner join Ubigeo u on e.idUbigeo = u.idUbigeo
    inner join TIPO_EMPLEADO t on e.idTipo_Empleado = t.idTipo_Empleado
    where estEmpleado = 1
    order by nombres;
END
GO

CREATE OR ALTER PROCEDURE spActualizarEmpleado(
	@idEmpleado int,
	@nombres varchar(40),
	@dni varchar(8),
	@telefono varchar(9),
	@direccion varchar(60),
	@f_inicio date,
	@f_fin date,
	@salario float,
	@descripcion varchar(50),
	@estEmpleado bit,
	@idTipo_Empleado int,
	@idUbigeo VARCHAR(6)
)
AS
BEGIN
	update EMPLEADO set nombres = @nombres, dni = @dni, telefono = @telefono,
	direccion = @direccion, f_inicio = @f_inicio, f_fin = @f_fin, salario = @salario, descripcion = @descripcion,
	estEmpleado = @estEmpleado, idTipo_Empleado = @idTipo_Empleado, idUbigeo = @idUbigeo
	where idEmpleado = @idEmpleado;
END
GO

CREATE OR ALTER PROCEDURE spDeshabilitarEmpleado(@idEmpleado int)
AS
BEGIN
	update EMPLEADO set estEmpleado = 0 where idEmpleado = @idEmpleado;
END
GO

CREATE OR ALTER PROCEDURE spBuscarEmpleado(
	@Campo varchar(40)
)
AS
BEGIN
    SELECT e.idEmpleado, e.nombres, e.dni, e.telefono, e.direccion, e.salario, e.descripcion, t.nombre as tipo, u.distrito as distrito FROM EMPLEADO e 
    inner join Ubigeo u on e.idUbigeo = u.idUbigeo
    inner join TIPO_EMPLEADO t on e.idTipo_Empleado = t.idTipo_Empleado
	where NOMBRES like @Campo+'%'
	or dni like @Campo+'%'	
END
GO

--CREATE OR ALTER PROCEDURE spBuscarIdEmpleado(
--    @idEmpleado int
--)
--AS
--BEGIN
--    Select e.idEmpleado ,e.nombres ,e.dni,e.telefono,e.direccion,e.f_inicio,e.f_fin, e.salario, e.descripcion, e.estEmpleado,t.nombre as tipo,u.distrito from empleado e 
--    INNER JOIN UBIGEO u ON e.idUbigeo=u.idUbigeo
--    inner join tipo_empleado t on e.idTipo_Empleado = t.idTipo_Empleado
--    where idEmpleado= @IdEmpleado;
--END
--GO
--------------------------------------TIPO_PRODUCTO
--CREATE OR ALTER PROCEDURE spCrearTipoProducto(
--	@nombre varchar(30)
--)
--AS
--BEGIN
--	INSERT INTO TIPO_PRODUCTO VALUES(@nombre);
--END
--GO

CREATE OR ALTER PROCEDURE spListarTipoProducto
AS
BEGIN
	SELECT *FROM TIPO_PRODUCTO;
END
GO

--CREATE OR ALTER PROCEDURE spActualizarTipoProducto
--	@idTipo_Producto int,
--	@nombre varchar(30)
--AS
--BEGIN
--	update TIPO_PRODUCTO set nombre = @nombre where idTipo_Producto = @idTipo_Producto
--END
--GO
--------------------------------------PRODUCTO
CREATE OR ALTER PROCEDURE spCrearProducto(
	@nombre varchar(40),
	@longitud int,
	@diametro float,
	@precioVenta float,
	@stock int,
	@idTipo_Producto int
)
AS
BEGIN
	INSERT INTO PRODUCTO values (@nombre, @longitud, @diametro, @precioVenta, @stock, @idTipo_Producto);
END
GO

CREATE OR ALTER PROCEDURE spListarProducto
AS
BEGIN
	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo from PRODUCTO p
	inner join TIPO_PRODUCTO t on p.idTipo_Producto = t.idTipo_Producto;
END
GO

--CREATE OR ALTER PROCEDURE spBuscarProductoid(
--@prmintidProducto int
--)
--AS
--BEGIN
--SELECT p.idProducto, p.nombre, p.longitud, p.precioCompra, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo from PRODUCTO p
--	inner join TIPO_PRODUCTO t on p.idTipo_Producto = t.idTipo_Producto where p.idProducto=@prmintidProducto;
--END
--GO
--CREATE OR ALTER PROCEDURE spActualizarProducto(
--	@idProducto int,
--	@nombre varchar(40),
--	@longitud int,
--	@precioCompra float,
--	@precioVenta float,
--	@idTipo_Producto int
--)
--AS
--BEGIN
--	update PRODUCTO set nombre = @nombre, longitud = @longitud, precioCompra = @precioCompra,precioVenta=@precioVenta,
--	idTipo_Producto = @idTipo_Producto
--	where idProducto = @idProducto;
--END
--GO

--CREATE OR ALTER PROCEDURE spEliminarProducto(@idProducto int)
--AS
--BEGIN
--	delete PRODUCTO  where idProducto = @idProducto;
--END
--GO

CREATE OR ALTER PROCEDURE spBuscarProducto(
	@Campo varchar(40)
)
AS
BEGIN
SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo from PRODUCTO p
	inner join TIPO_PRODUCTO t on p.idTipo_Producto = t.idTipo_Producto where p.nombre LIKE '%'+@campo+'%' OR P.longitud LIKE @Campo;
 
END
GO

CREATE OR ALTER PROCEDURE spOrdenarProducto
(
	@dato int
)
AS
BEGIN

IF(@dato=1)
SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo from PRODUCTO p
	inner join TIPO_PRODUCTO t on p.idTipo_Producto = t.idTipo_Producto ORDER BY p.nombre ASC;
else

	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo from PRODUCTO p
	inner join TIPO_PRODUCTO t on p.idTipo_Producto = t.idTipo_Producto ORDER BY p.nombre DESC;
END
GO
--------------------------------------CLIENTE


CREATE OR ALTER PROCEDURE spListarCliente
AS
BEGIN
	select c.idCliente,c.razonSocial, c.dni,c.telefono,c.direccion, c.correo, c.userName, c.pass, c.activo, u.departamento, r.descripcion from 
	Cliente c inner join UBIGEO u on c.idUbigeo= u.idUbigeo inner join
	Rol r on r.idRol = c.idRol where c.idRol = 2
END
GO


--CREATE OR ALTER PROCEDURE spActualizarCliente(
--	@idCliente int,
--	@razonSocial varchar(40),
--	@dni varchar(8),
--	@telefono varchar(9),
--	@direccion varchar(60),
--	@idUbigeo VARCHAR(6)
--)
--AS
--BEGIN
--	update CLIENTE set razonSocial = @razonSocial, dni = @dni,
--	 telefono = @telefono, direccion = @direccion, idUbigeo = @idUbigeo where idCliente = @idCliente;
--END
--GO

CREATE OR ALTER PROCEDURE spEliminarCliente(
	@idCliente int
)
AS
BEGIN
	delete CLIENTE where idCliente = @idCliente;
END
GO

CREATE OR ALTER PROCEDURE spBuscarCliente(
	@Campo varchar(40)
)
AS
BEGIN
	Select *from Cliente where razonSocial like @Campo+'%'
	or dni like @Campo+'%'
	
END
GO

--CREATE OR ALTER PROCEDURE spBuscarIdCliente(
--	@IdCliente int
--)
--AS
--BEGIN
--	Select *from Cliente where idCliente = @IdCliente;
	
--END
--GO
--------------------------------------VENTA
--CREATE OR ALTER PROCEDURE spCrearVenta(
--	@idventa int out,
--	@total float,
--	@idCliente int
--)
--AS
--BEGIN TRY
--	BEGIN TRANSACTION
--	INSERT INTO VENTA (total,idCliente)values (@total,@idCliente);
--	Set @idventa=@@identity;
--	COMMIT TRANSACTION
--END TRY
--BEGIN CATCH
--	ROLLBACK TRANSACTION
--	Set @idventa=-1;
--END CATCH
--GO

--CREATE OR ALTER PROCEDURE spListarVenta
--(
--	@id int
--)
--AS
--BEGIN
--	 SELECT  v.idVenta,v.fecha,v.total,v.estado,c.idCliente,c.razonSocial FROM Venta v inner join cliente c
--	 on v.idCliente=c.idCliente where v.idCliente=@id;	
--END
--GO

--CREATE OR ALTER PROCEDURE spListarVentaPagada
--(
--@fecha date
--)
--AS
--BEGIN
--	 SELECT *FROM Venta v
--	where MONTH(fecha) = MONTH (@fecha) and YEAR(fecha) = YEAR (@fecha) and v.estado=1
--	order by idVenta desc
--END
--GO

--CREATE OR ALTER PROCEDURE spListarVentaNoPagada
--(
--@fecha date
--)
--AS
--BEGIN
--	 SELECT *FROM Venta v
--	where MONTH(fecha) = MONTH (@fecha) and YEAR(fecha) = YEAR (@fecha) and v.estado=0
--	order by idVenta desc
--END
--GO

--CREATE OR ALTER PROCEDURE spActualizarVenta(
--	@idVenta int,
--	@estado bit
--)
--AS
--BEGIN
--	update VENTA set estado = @estado where idVenta = @idVenta;
--END
--GO
--------------------------------------COMPRA
--CREATE OR ALTER PROCEDURE spCrearCompra(
--	@id int out,
--	@total float,
--	@idProveedor int
--)
--AS
--BEGIN TRY
--	BEGIN TRANSACTION
--		INSERT INTO COMPRA (total,idProveedor)values (@total,@idProveedor);
--		Set @id=@@identity;
--	COMMIT TRANSACTION
--END TRY
--BEGIN CATCH
--	ROLLBACK TRANSACTION
--		Set @id=-1;
--END CATCH
--GO

CREATE OR ALTER PROCEDURE spListarCompra
AS
BEGIN
	Select c.idCompra, c.fecha, c.total, c.idProveedor, p.razonSocial from COMPRA c 
	inner join PROVEEDOR p on p.idProveedor = c.idProveedor
END
GO

--/*CREATE OR ALTER PROCEDURE spReporteCompra
--AS
--BEGIN
--    select c.idCompra, c.fecha, p.nombre, p.longitud, det.cantidad, det.preUnitario, det.subTotal from COMPRA c 
--	inner join DETALLE_COMPRA det on c.idCompra = det.idCompra
--	inner join PRODUCTO p on det.idProducto = p.idProducto
--END
--GO*/

--CREATE OR ALTER PROCEDURE spEliminarCompra(
--	@idCompra int
--)
--AS
--BEGIN
--    delete DETALLE_COMPRA where idCompra= @idCompra;
--	delete COMPRA where idCompra = @idCompra;
--END
--GO

--CREATE OR ALTER PROCEDURE spBuscarCompra(
--	@Campo varchar(40)
--)
--AS
--BEGIN
--	Select c.idCompra, c.fecha, c.total, c.IdProveedor, p.descripcion from COMPRA c inner join PROVEEDOR p
--	on p.idProveedor = c.idProveedor
--	where p.descripcion like @Campo+'%'
--	or dni like @Campo+'%'	
--END
--GO

--------------------------------------DETALLE_COMPRA
--CREATE OR ALTER PROCEDURE spCrearDetCompra(
--	@idCompra int,
--	@idProducto int,
--	@cantidad int,
--	@subTotal float
	
--)
--AS
--BEGIN
--	INSERT INTO DETALLE_COMPRA values (@idCompra, @idProducto, @cantidad,@subTotal)
--	update Producto set stock += @cantidad 
--	where idProducto = @idProducto 
--END
--GO

--/*CREATE OR ALTER PROCEDURE spMostrarReporteCompra(
--	@idCompra int	
--)
--AS
--BEGIN
--	Select det.idCompra as CODIGO, p.razonSocial as PROVEEDOR , c.fecha as FECHA, pro.nombre as DESCRIPCIÓN, 
--	concat (pro.longitud, ' ','MTS') as LONGITUD, det.cantidad as CANTIDAD,det.preUnitario as PRECIO_UNITARIO, 
--	det.subTotal as SUBTOTAL from DETALLE_COMPRA det
	
--	inner join COMPRA c on det.idCompra = c.idCompra
--	inner join PROVEEDOR p on c.idProveedor = p.idProveedor
--	inner join PRODUCTO pro on det.idProducto = pro.idProducto
--	where c.idCompra = @idCompra;
--END
--GO*/
--------------------------------------DETALLE_VENTA
--CREATE OR ALTER PROCEDURE spCrearDetVenta(
--	@idVenta int,
--	@idProducto int,
--	@cantidad int,
--	@subTotal float
	
--)
--AS
--BEGIN
--	INSERT INTO DETALLE_VENTA values (@idVenta, @idProducto, @cantidad,@subTotal);
--	update Producto set stock -= @cantidad 
--	where idProducto = @idProducto;
--END
--GO

--CREATE OR ALTER PROCEDURE spMostrarReporteVentas(
--	@idVenta int	
--)
--AS
--BEGIN
--	Select det.idventa as CODIGO, cli.razonSocial as CLIENTE , v.fecha as FECHA, pro.nombre as DESCRIPCIÓN, 
--	concat (pro.longitud, ' ','MTS') as LONGITUD, det.cantidad as CANTIDAD, 
--	det.subTotal as SUBTOTAL from DETALLE_VENTA det
	
--	inner join VENTA v on det.idVenta = v.idVenta
--	inner join CLIENTE cli on v.idCliente = cli.idCliente
--	inner join PRODUCTO pro on det.idProducto = pro.idProducto
--	where v.idVenta = @idVenta;
--END
--go

--CREATE OR ALTER PROCEDURE spReturnID(
--    @tipo varchar(10)
--)
--AS
--BEGIN
--    SELECT IDENT_CURRENT(@tipo);
--END
--GO

------------------------------------------CONSULTAS
--CREATE OR ALTER PROCEDURE spDashboardDatos
--	@totVentas float out,--Establecer parametros de salida para mostrarlo posteriormente
--	@totCompras float out,
--	@cantVentas int out,
--	@cantCompras int out,
--	@cantProveedor int out,
--	@cantCliente int out
--AS
--BEGIN
--	set @totVentas = (select sum(total) as TotalVentas from VENTA); --set permite establecer un valor para los parametros establecidos
--	set @totCompras = (select sum(total) as TotalCompra from COMPRA);
--	set @cantVentas = (select count(idVenta) as CantidadVentas from VENTA);
--	set @cantCompras = (select count(idCompra) as CantidadCompras from COMPRA);
--	set @cantProveedor = (select count(idProveedor) as CantidadProveedores from PROVEEDOR
--	where estProveedor = 0);
--	set @cantCliente = (select count(idCliente) as CantidadClientes from CLIENTE);

--END
--GO

--Productos preferidos
--CREATE OR ALTER PROCEDURE spProductosPreferidos
--AS
--BEGIN
--	Select top 3 p.nombre as NombreMadera, count(dv.idProducto) as CantidadSalidas from DETALLE_VENTA dv
--	inner join PRODUCTO p on p.idProducto = dv.idProducto
--	group by dv.idProducto, p.nombre
--	order by count(2) desc; 
--END
--GO