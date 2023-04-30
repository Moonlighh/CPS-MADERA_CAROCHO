-----------------------------------------PROCEDIMIENTOS ALMACENADOS ----------------------------------------
USE BD_PRUEBAS_MADERERA
GO


-- PA - UBIGEO
-- *************************************
--LISTA DEPARTAMENTO
CREATE OR ALTER PROCEDURE sp_ListaDepartamento
AS
BEGIN
    SELECT DISTINCT (Departamento) FROM Ubigeo;
END
GO

--LISTA PROVINCIA
CREATE OR ALTER PROCEDURE sp_ListaProvincia(@departamento VARCHAR(32))
AS
BEGIN
    SELECT DISTINCT(Provincia) FROM Ubigeo
    WHERE Departamento = @departamento;
END
GO

--LISTA DISTRITO
CREATE OR ALTER PROCEDURE sp_ListaDistrito
AS
BEGIN
    SELECT idUbigeo, distrito FROM Ubigeo WHERE departamento='La Libertad'
	ORDER BY distrito;
END
GO


-- PA - PROVEEDOR
-- *************************************
CREATE OR ALTER PROCEDURE spCrearProveedor
(
	@razonSocial VARCHAR(40),
	@dni VARCHAR(8),
	@correo VARCHAR(40),
	@telefono VARCHAR(9),
	@descripcion VARCHAR (80),
	@estProveedor BIT,
	@idUbigeo VARCHAR(6) null
)
AS
BEGIN
	INSERT INTO PROVEEDOR VALUES (@razonSocial, @dni, @correo, @telefono, @descripcion, @estProveedor, @idUbigeo);
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
	@idProveedor INT,	
	@razonSocial VARCHAR(40),
	@dni VARCHAR(8),
	@correo VARCHAR(40),
	@telefono VARCHAR(9),
	@descripcion VARCHAR (80),
	@estProveedor BIT,
	@idUbigeo VARCHAR(6) null
)
AS
BEGIN
	UPDATE PROVEEDOR SET razonSocial = @razonSocial, dni = @dni, correo = @correo,
	telefono = @telefono, descripcion = @descripcion, estProveedor = @estProveedor, idUbigeo = @idUbigeo
	WHERE idProveedor = @idProveedor;
END
GO

CREATE OR ALTER PROCEDURE spEliminarProveedor(@idProveedor INT)
AS
BEGIN
	DELETE PROVEEDOR WHERE idProveedor = @idProveedor;
END
GO

CREATE OR ALTER PROCEDURE spDeshabilitarProveedor(@idProveedor INT)
AS
BEGIN
	UPDATE PROVEEDOR SET estProveedor = 0 WHERE idProveedor = @idProveedor;
END
GO

--PROVEEDOR - OTROS
CREATE OR ALTER PROCEDURE spBuscarProveedor(
	@Campo VARCHAR(40)
)
AS
BEGIN
	SELECT p.idProveedor, p.razonSocial, p.dni, p.correo, p.telefono, u.departamento, u.provincia, u.distrito, p.descripcion, p.estProveedor FROM PROVEEDOR p 
	inner join UBIGEO u ON p.idUbigeo = u.idUbigeo 
	WHERE razonSocial like @Campo+'%'or dni like @Campo+'%'	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdProveedor(
	@idProveedor INT
)
AS
BEGIN
	SELECT p.idProveedor,p.razonSocial,p.dni,p.correo,p.telefono,p.descripcion,p.estProveedor,u.idUbigeo,u.distrito FROM PROVEEDOR p inner join UBIGEO u
	ON p.idUbigeo=u.idUbigeo WHERE idProveedor= @idProveedor;
	
END
GO


-- PA - TIPO_PRODUCTO
-- *************************************

--CREATE OR ALTER PROCEDURE spCrearTipoProducto(
--	@nombre VARCHAR(30)
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


-- PA - PRODUCTO
-- *************************************
CREATE OR ALTER PROCEDURE spCrearProducto(
	@nombre VARCHAR(40),
	@longitud INT,
	@diametro FLOAT,
	@precioVenta FLOAT,
	@stock INT,
	@idTipo_Producto INT
)
AS
BEGIN
	INSERT INTO PRODUCTO VALUES (@nombre, @longitud, @diametro, @precioVenta, @stock, @idTipo_Producto);
END
GO

CREATE OR ALTER PROCEDURE spListarProductoAdmin
AS
BEGIN
	SELECT pro.idProvedoor_Producto, p.idProducto, p.nombre, p.longitud, p.diametro, pro.precioCompra, p.precioVenta, p.stock, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto
	inner join PROVEEDOR_PRODUCTO pro on p.idProducto = pro.idProducto
	ORDER BY p.idProducto;
END
GO

CREATE OR ALTER PROCEDURE spListarProducto
AS
BEGIN
	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto
END
GO

CREATE OR ALTER PROCEDURE spActualizarProducto(
	@idProducto INT,
	@nombre VARCHAR(40),
	@longitud INT,
	@diametro FLOAT,
	@precioVenta FLOAT,
	@idTipo_Producto INT
)
AS
BEGIN
	UPDATE PRODUCTO SET nombre = @nombre, longitud = @longitud, precioVenta=@precioVenta,
	idTipo_Producto = @idTipo_Producto
	WHERE idProducto = @idProducto;
END
GO

CREATE OR ALTER PROCEDURE spEliminarProducto(@idProducto INT)
AS
BEGIN
	DELETE PRODUCTO  WHERE idProducto = @idProducto;
END
GO

--PRODUCTO - OTROS
CREATE OR ALTER PROCEDURE spBuscarProductoAdmin(
	@Campo VARCHAR(40)
)
AS
BEGIN
	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, pro.precioCompra, p.precioVenta, p.stock, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto
	inner join PROVEEDOR_PRODUCTO pro on pro.idProducto = p.idProducto
	WHERE p.nombre like '%'+@campo+'%' or P.longitud like @Campo;
END
GO

CREATE OR ALTER PROCEDURE spBuscarProducto(
	@Campo VARCHAR(40)
)
AS
BEGIN
	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto WHERE p.nombre LIKE '%'+@campo+'%' OR P.longitud LIKE @Campo;
 
END
GO

CREATE OR ALTER PROCEDURE spBuscarProductoid(
	@idProducto INT
)
AS
BEGIN
SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto WHERE p.idProducto = @idProducto;
END
GO

CREATE OR ALTER PROCEDURE spOrdenarProducto(@dato INT)
AS
BEGIN

IF(@dato=1)
SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto ORDER BY p.nombre ASC;
else

	SELECT p.idProducto, p.nombre, p.longitud, p.diametro, p.precioVenta, p.stock, t.idTipo_Producto, t.tipo FROM PRODUCTO p
	inner join TIPO_PRODUCTO t ON p.idTipo_Producto = t.idTipo_Producto ORDER BY p.nombre DESC;
END
GO

-- PA - PROVEEDOR_PRODUCTO
-- *************************************
CREATE OR ALTER PROCEDURE spListarProveedorProducto
AS
BEGIN
	SELECT p.idProvedoor_Producto AS id, pro.razonSocial AS proveedor, prod.nombre AS madera, prod.longitud, prod.diametro,
	prod.stock, p.precioCompra AS precio, pro.idProveedor, prod.idProducto FROM PROVEEDOR_PRODUCTO p
	inner join PROVEEDOR pro ON p.idProveedor = pro.idProveedor
	inner join PRODUCTO prod ON p.idProducto = prod.idProducto
	WHERE pro.estProveedor = 1;
END
GO


-- PA - TIPO_EMPLEADO
-- *************************************
CREATE OR ALTER PROCEDURE spListarTipoEmpleado
AS
BEGIN
	SELECT *FROM tipo_empleado;
END
GO

--TIPO EMPLEADO
--CREATE OR ALTER PROCEDURE spActualizarTipoEmpleado(
--	@idTipo_Empleado INT,
--	@nombre VARCHAR(30)
--)
--AS
--BEGIN
--	UPDATE TIPO_EMPLEADO SET nombre = @nombre
--	WHERE idTipo_Empleado = @idTipo_Empleado
--END
--GO

-- PA - EMPLEADO
-- *************************************
CREATE OR ALTER PROCEDURE spCrearEmpleado
(
	@nombres VARCHAR(40),
	@dni VARCHAR(8),
	@telefono VARCHAR(9),
	@direccion VARCHAR(60),
	@salario FLOAT,
	@descripcion VARCHAR(50),
	@idTipo_Empleado INT,
	@idUbigeo VARCHAR(6)
)
AS
BEGIN
	INSERT empleado (nombres,dni, telefono, direccion, salario, descripcion, idTipo_Empleado, idUbigeo) 
	VALUES          (@nombres, @dni, @telefono, @direccion, @salario, @descripcion, @idTipo_Empleado, @idUbigeo);
END
GO

CREATE OR ALTER PROCEDURE spListarEmpleado
AS
BEGIN
    SELECT e.idEmpleado, e.nombres, e.dni, e.telefono, e.direccion, e.salario, e.descripcion, t.nombre AS tipo, u.distrito AS distrito, e.f_inicio, e.f_fin FROM EMPLEADO e 
    inner join Ubigeo u ON e.idUbigeo = u.idUbigeo
    inner join TIPO_EMPLEADO t ON e.idTipo_Empleado = t.idTipo_Empleado
    WHERE estEmpleado = 1
    ORDER BY nombres;
END
GO

CREATE OR ALTER PROCEDURE spActualizarEmpleado(
	@idEmpleado INT,
	@nombres VARCHAR(40),
	@dni VARCHAR(8),
	@telefono VARCHAR(9),
	@direccion VARCHAR(60),
	@f_fin DATE,
	@salario FLOAT,
	@descripcion VARCHAR(50),
	@estEmpleado BIT,
	@idTipo_Empleado INT,
	@idUbigeo VARCHAR(6)
)
AS
BEGIN
	update EMPLEADO set nombres = @nombres, dni = @dni, telefono = @telefono,
	direccion = @direccion,f_fin = @f_fin, salario = @salario, descripcion = @descripcion,
	estEmpleado = @estEmpleado, idTipo_Empleado = @idTipo_Empleado, idUbigeo = @idUbigeo
	WHERE idEmpleado = @idEmpleado;
END
GO

CREATE OR ALTER PROCEDURE spDeshabilitarEmpleado(@idEmpleado INT)
AS
BEGIN
	UPDATE EMPLEADO SET estEmpleado = 0 WHERE idEmpleado = @idEmpleado;
END
GO

-- EMPLEADO - OTROS
CREATE OR ALTER PROCEDURE spBuscarEmpleado(
	@Campo VARCHAR(40)
)
AS
BEGIN
    SELECT e.idEmpleado, e.nombres, e.dni, e.telefono, e.direccion, e.salario, e.descripcion, t.nombre AS tipo, u.distrito AS distrito FROM EMPLEADO e 
    inner join Ubigeo u ON e.idUbigeo = u.idUbigeo
    inner join TIPO_EMPLEADO t ON e.idTipo_Empleado = t.idTipo_Empleado
	WHERE NOMBRES like @Campo+'%'
	or dni like @Campo+'%'	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdEmpleado(
    @idEmpleado INT
)
AS
BEGIN
    SELECT e.idEmpleado ,e.nombres ,e.dni,e.telefono,e.direccion,e.f_inicio,e.f_fin, e.salario, e.descripcion, e.estEmpleado,t.nombre AS tipo,u.distrito FROM empleado e 
    inner join UBIGEO u ON e.idUbigeo=u.idUbigeo
    inner join tipo_empleado t ON e.idTipo_Empleado = t.idTipo_Empleado
    WHERE idEmpleado= @IdEmpleado;
END
GO


-- PA - ROL
-- *************************************
CREATE OR ALTER PROCEDURE spListarRol
AS
BEGIN
	SELECT * FROM rol WHERE idRol=1 or idRol=3;
END
GO


-- PA - USUARIO
-- *************************************
CREATE OR ALTER PROCEDURE spCrearUsuario(
    @razonSocial VARCHAR(40),
    @dni VARCHAR(8),
    @telefono VARCHAR(9),
    @direccion VARCHAR(60),
    @idUbigeo VARCHAR(6),
	@correo VARCHAR(40),
	@userName VARCHAR(20),
	@pass VARCHAR(200),
	@idRol INT
)
AS
BEGIN
    INSERT INTO USUARIO (razonSocial,dni,telefono,direccion,idUbigeo,correo,userName,pass, idRol) VALUES (@razonSocial, @dni, @telefono, @direccion, @idUbigeo, @correo, @userName, @pass, @idRol);
END
GO

CREATE OR ALTER PROCEDURE spListarUsuarios
AS
BEGIN
	SELECT c.idUsuario,c.razonSocial, c.dni,c.telefono,c.direccion, c.correo,
	c.activo, c.fecCreacion, u.distrito, r.descripcion FROM 
	Usuario c inner join UBIGEO u ON c.idUbigeo= u.idUbigeo inner join
	Rol r ON r.idRol = c.idRol
END
GO

CREATE OR ALTER PROCEDURE spListarAdministradores
AS
BEGIN
	SELECT a.idUsuario,a.razonSocial, a.dni,a.telefono,a.direccion,a.userName,a.correo,r.descripcion,a.activo FROM 
	Usuario a inner join Rol r ON r.idRol = a.idRol 
	WHERE a.idRol=1 and a.activo = 1
END
GO

CREATE OR ALTER PROCEDURE spListarCliente
AS
BEGIN
	SELECT c.idUsuario,c.razonSocial, c.dni,c.telefono,c.direccion, c.correo, c.userName, c.activo, u.distrito, r.descripcion FROM 
	Usuario c inner join UBIGEO u ON c.idUbigeo= u.idUbigeo inner join
	Rol r ON r.idRol = c.idRol 
	WHERE c.idRol = 2 and c.activo = 1
END
GO

----Actualizar Usuario
--CREATE OR ALTER PROCEDURE spActualizarUsuario(
--	@newUsuario VARCHAR(40),
--	@contra VARCHAR(200),
--	@oldUsuario VARCHAR(40)
--)
--AS
--BEGIN
--	UPDATE ADMINISTRADOR SET usuario = @newUsuario
--	WHERE usuario = @oldUsuario and contra = @contra;
--END
--GO
----Actualizar ContrASeņa
--CREATE OR ALTER PROCEDURE spActualizarContra(
--	@usuario VARCHAR(40),
--	@oldContra VARCHAR(200),
--	@newContra VARCHAR(200)
--)
--AS
--BEGIN
--	UPDATE ADMINISTRADOR SET contra = @newContra
--	WHERE usuario = @usuario and contra = @oldContra;
--END
--GO

CREATE OR ALTER PROCEDURE spDeshabilitarUsuario(
	@idUsuario INT
)
AS
BEGIN
	UPDATE USUARIO SET activo = 0 WHERE idUsuario = @idUsuario;
END
GO

-- USUARIO - OTROS
CREATE OR ALTER PROCEDURE spIniciarSesion(@dato VARCHAR(40), @contra VARCHAR(200))
AS
BEGIN
	SELECT *FROM 
	USUARIO c inner join Rol r ON r.idRol = c.idRol
	WHERE (userName = @dato or correo = @dato) and pass = @contra
END
GO

CREATE OR ALTER PROCEDURE spHabilitarUsuario(
	@idUsuario INT
)
AS
BEGIN
	UPDATE USUARIO SET activo = 1 WHERE idUsuario = @idUsuario;
END
GO

CREATE OR ALTER PROCEDURE spBuscarUsuario(
	@Campo VARCHAR(40)
)
AS
BEGIN
	SELECT *FROM Usuario a
	inner join ROL r ON r.idRol = a.idRol
	inner join UBIGEO u ON u.idUbigeo = a.idUbigeo
	WHERE razonSocial like @Campo+'%'
	or dni like @Campo+'%';
END
GO

CREATE OR ALTER PROCEDURE spBuscarAdministrador(
	@Campo VARCHAR(40)
)
AS
BEGIN
	SELECT *FROM Usuario a
	inner join ROL r ON r.idRol = a.idRol
	inner join UBIGEO u ON u.idUbigeo = a.idUbigeo
	WHERE razonSocial like @Campo+'%'
	or dni like @Campo+'%' and a.activo = 1 and a.idRol = 1;
END
GO

CREATE OR ALTER PROCEDURE spBuscarCliente(
	@Campo VARCHAR(40)
)
AS
BEGIN
	SELECT *FROM Usuario c
	inner join ROL r on r.idRol = c.idRol
	inner join UBIGEO u on u.idUbigeo = c.idUbigeo
	WHERE razonSocial like @Campo+'%'
	or dni like @Campo+'%' and c.activo = 1 and c.idRol = 2 ;
	
END
GO

CREATE OR ALTER PROCEDURE spBuscarIdCliente(
	@IdCliente INT
)
AS
BEGIN
	SELECT *FROM Usuario WHERE idUsuario = @IdCliente;	
END
GO


-- PA - CARRITO
-- *************************************
CREATE OR ALTER PROCEDURE spAgregarProductoCarrito(
	@idCliente INT,
	@idProducto INT,
	@cantidad INT,
	@subTotal FLOAT
)
AS
BEGIN
	INSERT INTO CARRITO VALUES (@idCliente, @idProducto, @cantidad, @subTotal);
END
GO

CREATE OR ALTER PROCEDURE spMostrarCarrito(
	@idUsuario INT
)
AS
BEGIN
	SELECT pro.idProvedoor_Producto, pro.precioCompra, p.idProducto, car.idCarrito, p.nombre, p.longitud, p.diametro, tip.tipo, car.cantidad, car.subtotal FROM CARRITO car
	inner join USUARIO u ON car.idCliente = u.idUsuario
	inner join PROVEEDOR_PRODUCTO pro ON pro.idProvedoor_Producto = car.idProveedor_Producto
	inner join PRODUCTO P ON P.idProducto = pro.idProducto
	inner join TIPO_PRODUCTO tip ON tip.idTipo_Producto = p.idTipo_Producto
	WHERE car.idCliente = 1 and car.idProveedor_Producto = p.idProducto;
END
GO

CREATE OR ALTER PROCEDURE spEditarProductoCarrito(
	@idCarrito INT,
	@cantidad INT,
	@subtotal INT
)
AS
BEGIN
	UPDATE CARRITO SET CANTIDAD = @cantidad, subtotal = @subtotal
	WHERE idCarrito = @idCarrito;
END
GO

CREATE OR ALTER PROCEDURE spEliminarProductoCarrito(
	@idProveedor_Producto INT,
	@idCliente INT
)
AS
BEGIN
	DELETE CARRITO WHERE @idProveedor_Producto = idProveedor_Producto and @idCliente = idCliente;
END
GO

-- PA - COMPRA
-- *************************************
CREATE OR ALTER PROCEDURE spCrearCompra(
	@id INT OUT,
	@total FLOAT,
	@estado BIT,
	@idUsuario INT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
		INSERT INTO COMPRA (total, estado, idUsuario)VALUES (@total,@estado, @idUsuario);
		SET @id=@@identity;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
		SET @id=-1;
END CATCH
GO

CREATE OR ALTER PROCEDURE spListarCompra
AS
BEGIN
	SELECT c.idCompra, c.fecha, c.estado, c.total, u.idUsuario, u.razonSocial AS comprador FROM COMPRA c 
	inner join USUARIO u ON u.idUsuario = c.idUsuario
END
GO

CREATE OR ALTER PROCEDURE spEliminarCompra(
	@idCompra INT
)
AS
BEGIN
    DELETE DETALLE_COMPRA WHERE idCompra= @idCompra;
	DELETE COMPRA WHERE idCompra = @idCompra;
END
GO

-- COMPRA - OTROS
--CREATE OR ALTER PROCEDURE spBuscarCompra(
--	@Campo VARCHAR(40)
--)
--AS
--BEGIN
--	SELECT c.idCompra, c.fecha, c.total, c.IdProveedor, p.descripcion FROM COMPRA c inner join PROVEEDOR p
--	ON p.idProveedor = c.idProveedor
--	WHERE p.descripcion like @Campo+'%'
--	or dni like @Campo+'%'	
--END
--GO

CREATE OR ALTER TRIGGER tgINSERTarCompra
	ON Compra
AFTER INSERT
AS
BEGIN
	DECLARE @idUsuario INT;
	SELECT @idUsuario = INSERTed.idUsuario FROM INSERTed
	inner join compra ON INSERTed.idCompra = compra.idCompra;
	DELETE FROM CARRITO WHERE CARRITO.idCliente = @idUsuario;
END
GO


-- PA DETALLE_COMPRA
-- *************************************
CREATE OR ALTER PROCEDURE spCrearDetCompra(
	@idCompra INT,
	@idProducto INT,
	@cantidad INT,
	@subTotal FLOAT
	
)
AS
BEGIN
	INSERT INTO DETALLE_COMPRA VALUES (@idCompra, @idProducto, @cantidad,@subTotal)
	UPDATE Producto SET stock += @cantidad 
	WHERE idProducto = @idProducto 
END
GO

CREATE OR ALTER PROCEDURE spMostrarDetalleCompra(
	@idCompra INT	
)
AS
BEGIN
	SELECT dtC.idCompra, p.nombre, p.longitud, p.diametro, dtC.cantidad, dtC.subTotal FROM DETALLE_COMPRA dtC
	inner join PRODUCTO p ON p.idProducto = dtC.idProducto
	WHERE dtC.idCompra = @idCompra;
END
GO


















--------------------------------------VENTA
CREATE OR ALTER PROCEDURE spCrearVenta(
	@idventa INT OUT,
	@total FLOAT,
	@idUsuario INT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	INSERT INTO VENTA (total,idUsuario) VALUES (@total,@idUsuario);
	SET @idventa=@@identity;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	SET @idventa=-1;
END CATCH
GO

CREATE OR ALTER PROCEDURE spListarVenta
(
	@id INT
)
AS
BEGIN
	 SELECT  v.idVenta,v.fecha,v.total,v.estado,c.idUsuario,c.razonSocial FROM Venta v inner join USUARIO c
	 ON v.idUsuario=c.idUsuario WHERE v.idUsuario=@id;	
END
GO



CREATE OR ALTER TRIGGER tgUPDATECompra
ON compra
AFTER UPDATE
AS
BEGIN
	DECLARE @estado INT
	DECLARE @idcompra INT
	DECLARE @idc INT
	DECLARE @idp INT
    DECLARE @stock_origen INT
    DECLARE @stock_destino INT

	SELECT @estado=INSERTed.estado,@idcompra=INSERTed.idCompra FROM INSERTed
	
	IF @estado = 1
	BEGIN
		DECLARE cursor_actualizar CURSOR FOR
		SELECT idProducto,cantidad
		FROM DETALLE_COMPRA
		WHERE idCompra=@idcompra

		OPEN cursor_actualizar

		FETCH NEXT FROM cursor_actualizar INTO @idp, @stock_origen

		WHILE @@FETCH_STATUS = 0
		BEGIN
		   UPDATE PRODUCTO
		   SET stock +=@stock_origen
		   WHERE idProducto = @idp
		   FETCH NEXT FROM cursor_actualizar INTO @idp, @stock_origen
		END
		CLOSE cursor_actualizar
		DEALLOCATE cursor_actualizar
	 END
END
GO















--CREATE OR ALTER PROCEDURE spActualizarTipoProducto
--	@idTipo_Producto INT,
--	@nombre VARCHAR(30)
--AS
--BEGIN
--	UPDATE TIPO_PRODUCTO SET nombre = @nombre WHERE idTipo_Producto = @idTipo_Producto
--END
--GO

--CLIENTE
--CREATE OR ALTER PROCEDURE spActualizarCliente(
--	@idUsuario INT,
--	@razonSocial VARCHAR(40),
--	@dni VARCHAR(8),
--	@telefono VARCHAR(9),
--	@direccion VARCHAR(60),
--	@idUbigeo VARCHAR(6)
--)
--AS
--BEGIN
--	UPDATE USUARIO SET razonSocial = @razonSocial, dni = @dni,
--	 telefono = @telefono, direccion = @direccion, idUbigeo = @idUbigeo WHERE idUsuario = @idUsuario;
--END
--GO

--VENTA
--CREATE OR ALTER PROCEDURE spListarVentaPagada
--(
--@fecha DATE
--)
--AS
--BEGIN
--	 SELECT *FROM Venta v
--	WHERE MONTH(fecha) = MONTH (@fecha) and YEAR(fecha) = YEAR (@fecha) and v.estado=1
--	ORDER BY idVenta desc
--END
--GO

--CREATE OR ALTER PROCEDURE spListarVentaNoPagada
--(
--@fecha DATE
--)
--AS
--BEGIN
--	 SELECT *FROM Venta v
--	WHERE MONTH(fecha) = MONTH (@fecha) and YEAR(fecha) = YEAR (@fecha) and v.estado=0
--	ORDER BY idVenta desc
--END
--GO

--CREATE OR ALTER PROCEDURE spActualizarVenta(
--	@idVenta INT,
--	@estado BIT
--)
--AS
--BEGIN
--	UPDATE VENTA SET estado = @estado WHERE idVenta = @idVenta;
--END
--GO


--DETALLE_VENTA
--CREATE OR ALTER PROCEDURE spCrearDetVenta(
--	@idVenta INT,
--	@idProducto INT,
--	@cantidad INT,
--	@subTotal FLOAT
--)
--AS
--BEGIN
--	INSERT INTO DETALLE_VENTA VALUES (@idVenta, @idProducto, @cantidad,@subTotal);
--	UPDATE Producto SET stock -= @cantidad 
--	WHERE idProducto = @idProducto;
--END
--GO