﻿using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCompra
    {
        private static readonly logCompra _instancia = new logCompra();

        public static logCompra Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public bool CrearCompra(entCompra comp, out int idGenerado)
        {
            bool isValid = ValidationHelper.TryValidateEntity(comp);
            if (!isValid || comp.Total <=0 || (comp.Usuario.Activo == false))
            {
                idGenerado = -1; //Asegurarnos que idGenerado conserve su valor
                return false;
            }
            return datCompra.Instancia.CrearCompra(comp, out idGenerado);
        }
        public List<entCompra> ListarCompra()
        {
            return datCompra.Instancia.ListarCompra();
        }
        public bool EliminarCompra(int comp)
        {
            return datCompra.Instancia.EliminarCompra(comp);
        }
        #endregion

        #region Otros
        public int DevolverID(string tipo)
        {
            return datCompra.Instancia.GenerarID(tipo);
        }
        public List<entCompra> BuscarCompra(string busqueda)
        {
            return datCompra.Instancia.BuscarCompra(busqueda);
        }
        #endregion
    }
}
