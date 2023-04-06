-----------------PROCEDIMIENTOS ALMACENADOS 
USE BD_PRUEBAS_MADERERA
GO

--===== PROCEDIMIENTOS PARA EL SISTEMA =============
----===INICIAR SESION========
CREATE OR ALTER PROCEDURE spIniciarSesion(@dato varchar(40), @contra varchar(200))
AS
BEGIN
	SELECT *FROM 
	CLIENTE c inner join Rol r ON r.idRol = c.idRol
	WHERE (userName = @dato or correo = @dato) and pass = @contra
END
GO

----========ROL======---
CREATE OR ALTER PROCEDURE spListarRol
AS
BEGIN
	SELECT * FROM rol WHERE idRol=1 or idRol=3;
END
GO

----Actualizar Usuario
--CREATE OR ALTER PROCEDURE spActualizarUsuario(
--	@newUsuario varchar(40),
--	@contra varchar(200),
--	@oldUsuario varchar(40)
--)
--AS
--BEGIN
--	update ADMINISTRADOR set usuario = @newUsuario
--	WHERE usuario = @oldUsuario and contra = @contra;
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
--	WHERE usuario = @usuario and contra = @oldContra;
--END
--GO

--CREATE OR ALTER procedure spObtenerUsuario
--AS 
--BEGIN
--	SELECT u.idUsuario, u.userName, u.correo, u.pass FROM USUARIO u
--END
--GO


--CREATE OR ALTER PROCEDURE spBuscarClienteAdmin
--(
--@Campo varchar(20)
--)
--AS
--BEGIN
--SELECT c.idCliente,c.razonSocial, c.dni,c.telefono,c.direccion,u.userName,u.correo,r.descripcion,u.activo FROM cliente c inner join usuario u
--	ON c.idCliente=u.idCliente inner join
--	rol r ON r.idRol=u.idRol WHERE (u.idRol=1 or u.idRol=3 )and c.razonSocial like '%'+@Campo+'%';
--END
--GO

--CREATE OR ALTER PROCEDURE spEliminarUsuario
--(
-- @usuario int 
--)
--AS
--BEGIN
--   delete FROM usuario WHERE idCliente=@usuario; 
--   delete FROM cliente WHERE idCliente=@usuario;
--END
--GO

--------------------------------------UBIGEO-------------------------------------

--LISTA DEPARTAMENTO
CREATE OR ALTER PROCEDURE sp_ListaDepartamento
AS
BEGIN
    Select  distinct (Departamento) FROM Ubigeo;
END
GO

--LISTA PROVINCIA
CREATE OR ALTER PROCEDURE sp_ListaProvincia(@departamento varchar(32))
As
BEGIN
    Select distinct(Provincia) FROM Ubigeo
    WHERE Departamento = @departamento;
END
GO

--LISTA DISTRITO
CREATE OR ALTER PROCEDURE sp_ListaDistrito
As
BEGIN
    Select idUbigeo, distrito FROM Ubigeo WHERE departamento='La Libertad'
	order by distrito;
END
GO

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
	SELECT p.idProveedor, p.razonSocial, p.dni, p.correo, p.telefono, p.descripcion, p.estProveedor,
	u.departamento,u.provincia, u.distrito FROM  PROVEEDOR p 
	inner join ubigeo u ON p.idUbigeo = u.idUbigeo
	WHERE p.estProveedor = 1;
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
	WHERE idProveedor = @idProveedor;
END
GO

--CREATE OR ALTER PROCEDURE spEliminarProveedor(@idProveedor int)
--AS
--BEGIN
--	delete PROVEEDOR WHERE idProveedor = @idProveedor;
--END
--GO

CREATE OR ALTER PROCEDURE spDeshabilitarProveedor(@idProveedor int)
AS
BEGIN
	update PROVEEDOR set estProveedor = 0 WHERE idProveedor = @idProveedor;
END
GO

CREATE OR ALTER PROCEDURE spBuscarProveedor(
	@Campo varchar(40)
)
AS
BEGIN
	Select p.idProveedor, p.razonSocial, p.dni, p.correo, p.telefono, u.departamento, u.provincia, u.distrito, p.descripcion, p.estProveedor FROM PROVEEDOR p 
	inner join UBIGEO u ON p.idUbigeo = u.idUbigeo 
	WHERE razonSocial like @Campo+'%'or dni like @Campo+'%'	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdProveedor(
	@idProveedor int
)
AS
BEGIN
	Select p.idProveedor,p.razonSocial,p.dni,p.correo,p.telefono,p.descripcion,p.estProveedor,u.idUbigeo,u.distrito FROM PROVEEDOR p INNER JOIN UBIGEO u
	ON p.idUbigeo=u.idUbigeo WHERE idProveedor= @idProveedor;
	
END
GO
--------------------------------------PROVEEDOR_PRODUCTO
CREATE OR ALTER PROCEDURE spListarProveedorProducto
AS
BEGIN
	Select p.idProvedoor_Producto as id, pro.razonSocial as proveedor, prod.nombre as madera, 
	p.precioCompra as precio, pro.idProveedor, prod.idProducto from PROVEEDOR_PRODUCTO p
	inner join PROVEEDOR pro on p.idProveedor = pro.idProveedor
	inner join PRODUCTO prod on p.idProducto = prod.idProducto
	WHERE pro.estProveedor = 1;
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
	SELECT *FROM tipo_empleado;
END
GO

--CREATE OR ALTER PROCEDURE spActualizarTipoEmpleado(
--	@idTipo_Empleado int,
--	@nombre varchar(30)
--)
--AS
--BEGIN
--	update TIPO_EMPLEADO set nombre = @nombre
--	WHERE idTipo_Empleado = @idTipo_Empleado
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
    inner join Ubigeo u ON e.idUbigeo = u.idUbigeo
    inner join TIPO_EMPLEADO t ON e.idTipo_Empleado = t.idTipo_Empleado
    WHERE estEmpleado = 1
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
	WHERE idEmpleado = @idEmpleado;
END
GO

CREATE OR ALTER PROCEDURE spDeshabilitarEmpleado(@idEmpleado int)
AS
BEGIN
	update EMPLEADO set estEmpleado = 0 WHERE idEmpleado = @idEmpleado;
END
GO

CREATE OR ALTER PROCEDURE spBuscarEmpleado(
	@Campo varchar(40)
)
AS
BEGIN
    SELECT e.idEmpleado, e.nombres, e.dni, e.telefono, e.direccion, e.salario, e.descripcion, t.nombre as tipo, u.distrito as distrito FROM EMPLEADO e 
    inner join Ubigeo u ON e.idUbigeo = u.idUbigeo
    inner join TIPO_EMPLEADO t ON e.idTipo_Empleado = t.idTipo_Empleado
	WHERE NOMBRES like @Campo+'%'
	or dni like @Campo+'%'	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdEmpleado(
    @idEmpleado int
)
AS
BEGIN
    Select e.idEmpleado ,e.nombres ,e.dni,e.telefono,e.direccion,e.f_inicio,e.f_fin, e.salario, e.descripcion, e.estEmpleado,t.nombre as tipo,u.distrito FROM empleado e 
    INNER JOIN UBIGEO u ON e.idUbigeo=u.idUbigeo
    inner join tipo_empleado t ON e.idTipo_Empleado = t.idTipo_Empleado
    WHERE idEmpleado= @IdEmpleado;
END
GO

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
--	update TIPO_PRODUCTO set nombre = @nombre WHERE idTipo_Producto = @idTipo_Producto
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
	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto;
END
GO

CREATE OR ALTER PROCEDURE spBuscarProductoid(
@prmintidProducto int
)
AS
BEGIN
SELECT p.idProducto, p.nombre, p.longitud, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto WHERE p.idProducto=@prmintidProducto;
END
GO

CREATE OR ALTER PROCEDURE spActualizarProducto(
	@idProducto int,
	@nombre varchar(40),
	@longitud int,
	@diametro float,
	@precioVenta float,
	@idTipo_Producto int
)
AS
BEGIN
	update PRODUCTO set nombre = @nombre, longitud = @longitud, precioVenta=@precioVenta,
	idTipo_Producto = @idTipo_Producto
	WHERE idProducto = @idProducto;
END
GO

--CREATE OR ALTER PROCEDURE spEliminarProducto(@idProducto int)
--AS
--BEGIN
--	delete PRODUCTO  WHERE idProducto = @idProducto;
--END
--GO

CREATE OR ALTER PROCEDURE spBuscarProducto(
	@Campo varchar(40)
)
AS
BEGIN
SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto WHERE p.nombre LIKE '%'+@campo+'%' OR P.longitud LIKE @Campo;
 
END
GO

CREATE OR ALTER PROCEDURE spOrdenarProducto
(
	@dato int
)
AS
BEGIN

IF(@dato=1)
SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto ORDER BY p.nombre ASC;
else

	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.nombre as tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto ORDER BY p.nombre DESC;
END
GO

--------------------------------------CLIENTE
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

CREATE OR ALTER PROCEDURE spListarCliente
AS
BEGIN
	SELECT c.idCliente,c.razonSocial, c.dni,c.telefono,c.direccion, c.correo, c.userName, c.pass, c.activo, u.departamento, r.descripcion FROM 
	Cliente c inner join UBIGEO u ON c.idUbigeo= u.idUbigeo inner join
	Rol r ON r.idRol = c.idRol WHERE c.idRol = 2
END
GO

CREATE OR ALTER PROCEDURE spListarClienteAdmin
AS
BEGIN
	SELECT c.idCliente,c.razonSocial, c.dni,c.telefono,c.direccion,c.userName,c.correo,r.descripcion,c.activo FROM 
	Cliente c inner join Rol r ON r.idRol = c.idRol WHERE c.idRol=1
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
--	 telefono = @telefono, direccion = @direccion, idUbigeo = @idUbigeo WHERE idCliente = @idCliente;
--END
--GO

CREATE OR ALTER PROCEDURE spEliminarCliente(
	@idCliente int
)
AS
BEGIN
	delete CLIENTE WHERE idCliente = @idCliente;
END
GO

CREATE OR ALTER PROCEDURE spBuscarCliente(
	@Campo varchar(40)
)
AS
BEGIN
	Select *FROM Cliente WHERE razonSocial like @Campo+'%'
	or dni like @Campo+'%'
	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdCliente(
	@IdCliente int
)
AS
BEGIN
	Select *FROM Cliente WHERE idCliente = @IdCliente;	
END
GO

--------------------------------------VENTA
CREATE OR ALTER PROCEDURE spCrearVenta(
	@idventa int out,
	@total float,
	@idCliente int
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	INSERT INTO VENTA (total,idCliente)values (@total,@idCliente);
	Set @idventa=@@identity;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	Set @idventa=-1;
END CATCH
GO

CREATE OR ALTER PROCEDURE spListarVenta
(
	@id int
)
AS
BEGIN
	 SELECT  v.idVenta,v.fecha,v.total,v.estado,c.idCliente,c.razonSocial FROM Venta v inner join cliente c
	 ON v.idCliente=c.idCliente WHERE v.idCliente=@id;	
END
GO

--CREATE OR ALTER PROCEDURE spListarVentaPagada
--(
--@fecha date
--)
--AS
--BEGIN
--	 SELECT *FROM Venta v
--	WHERE MONTH(fecha) = MONTH (@fecha) and YEAR(fecha) = YEAR (@fecha) and v.estado=1
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
--	WHERE MONTH(fecha) = MONTH (@fecha) and YEAR(fecha) = YEAR (@fecha) and v.estado=0
--	order by idVenta desc
--END
--GO

--CREATE OR ALTER PROCEDURE spActualizarVenta(
--	@idVenta int,
--	@estado bit
--)
--AS
--BEGIN
--	update VENTA set estado = @estado WHERE idVenta = @idVenta;
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
	Select c.idCompra, c.fecha, c.total, c.idProveedor, p.razonSocial FROM COMPRA c 
	inner join PROVEEDOR p ON p.idProveedor = c.idProveedor
END
GO

--/*CREATE OR ALTER PROCEDURE spReporteCompra
--AS
--BEGIN
--    SELECT c.idCompra, c.fecha, p.nombre, p.longitud, det.cantidad, det.preUnitario, det.subTotal FROM COMPRA c 
--	inner join DETALLE_COMPRA det ON c.idCompra = det.idCompra
--	inner join PRODUCTO p ON det.idProducto = p.idProducto
--END
--GO*/

--CREATE OR ALTER PROCEDURE spEliminarCompra(
--	@idCompra int
--)
--AS
--BEGIN
--    delete DETALLE_COMPRA WHERE idCompra= @idCompra;
--	delete COMPRA WHERE idCompra = @idCompra;
--END
--GO

--CREATE OR ALTER PROCEDURE spBuscarCompra(
--	@Campo varchar(40)
--)
--AS
--BEGIN
--	Select c.idCompra, c.fecha, c.total, c.IdProveedor, p.descripcion FROM COMPRA c inner join PROVEEDOR p
--	ON p.idProveedor = c.idProveedor
--	WHERE p.descripcion like @Campo+'%'
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
--	WHERE idProducto = @idProducto 
--END
--GO

--/*CREATE OR ALTER PROCEDURE spMostrarReporteCompra(
--	@idCompra int	
--)
--AS
--BEGIN
--	Select det.idCompra as CODIGO, p.razonSocial as PROVEEDOR , c.fecha as FECHA, pro.nombre as DESCRIPCIÓN, 
--	concat (pro.longitud, ' ','MTS') as LONGITUD, det.cantidad as CANTIDAD,det.preUnitario as PRECIO_UNITARIO, 
--	det.subTotal as SUBTOTAL FROM DETALLE_COMPRA det
	
--	inner join COMPRA c ON det.idCompra = c.idCompra
--	inner join PROVEEDOR p ON c.idProveedor = p.idProveedor
--	inner join PRODUCTO pro ON det.idProducto = pro.idProducto
--	WHERE c.idCompra = @idCompra;
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
--	WHERE idProducto = @idProducto;
--END
--GO

--CREATE OR ALTER PROCEDURE spMostrarReporteVentas(
--	@idVenta int	
--)
--AS
--BEGIN
--	Select det.idventa as CODIGO, cli.razonSocial as CLIENTE , v.fecha as FECHA, pro.nombre as DESCRIPCIÓN, 
--	concat (pro.longitud, ' ','MTS') as LONGITUD, det.cantidad as CANTIDAD, 
--	det.subTotal as SUBTOTAL FROM DETALLE_VENTA det
	
--	inner join VENTA v ON det.idVenta = v.idVenta
--	inner join CLIENTE cli ON v.idCliente = cli.idCliente
--	inner join PRODUCTO pro ON det.idProducto = pro.idProducto
--	WHERE v.idVenta = @idVenta;
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
--	set @totVentas = (SELECT sum(total) as TotalVentas FROM VENTA); --set permite establecer un valor para los parametros establecidos
--	set @totCompras = (SELECT sum(total) as TotalCompra FROM COMPRA);
--	set @cantVentas = (SELECT count(idVenta) as CantidadVentas FROM VENTA);
--	set @cantCompras = (SELECT count(idCompra) as CantidadCompras FROM COMPRA);
--	set @cantProveedor = (SELECT count(idProveedor) as CantidadProveedores FROM PROVEEDOR
--	WHERE estProveedor = 0);
--	set @cantCliente = (SELECT count(idCliente) as CantidadClientes FROM CLIENTE);

--END
--GO

--Productos preferidos
--CREATE OR ALTER PROCEDURE spProductosPreferidos
--AS
--BEGIN
--	Select top 3 p.nombre as NombreMadera, count(dv.idProducto) as CantidadSalidas FROM DETALLE_VENTA dv
--	inner join PRODUCTO p ON p.idProducto = dv.idProducto
--	group by dv.idProducto, p.nombre
--	order by count(2) desc; 
--END
--GO