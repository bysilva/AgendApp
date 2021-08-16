using AgendApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace AgendApp.Services
{
    public class ContactoService : IContactos
    {
        public ContactoService(string connection)
        {
            connectionString = connection;
        }
        string connectionString;
        public int AddContacto(ContactoModel contacto)
        {
            if (string.IsNullOrEmpty(contacto.Nombre))
                throw new ArgumentException("Ingresa el nombre del contacto, por favor.");
            int idContacto = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string insertContacto = "insert into Contacto (Nombre, Telefono) " +
                                                "output INSERTED.IDCONTACTO" +
                                                " values (@Nombre, @Telefono)";
                        using (var commandContacto = new SqlCommand(insertContacto, conn))
                        {
                            commandContacto.Parameters.Add(new SqlParameter("@Nombre", contacto.Nombre));
                            commandContacto.Parameters.Add(new SqlParameter("@Telefono", contacto.Telefono));

                            var valorRetornadoContacto = commandContacto.ExecuteScalar();
                            if (valorRetornadoContacto != null)
                                int.TryParse(valorRetornadoContacto.ToString(), out idContacto);

                            if (idContacto > 0)
                            {
                                scope.Complete();
                            }
                            else
                            {
                                scope.Dispose();
                            }
                        }
                    }
                }
                return idContacto;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public List<ContactoModel> GetContactos()
        {
            List<ContactoModel> ListaContactos = new List<ContactoModel>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand("select IdContacto, Nombre, Telefono from Contacto", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ContactoModel contacto = new ContactoModel()
                            {
                                IdContacto = (int)reader["IdContacto"],
                                Nombre = (string)reader["Nombre"],
                                Telefono = (string)reader["Telefono"]
                            };

                            ListaContactos.Add(contacto);
                        }
                    }
                }
            }
            return ListaContactos;
        }

        //public List<ContactoModel> GetContactos()
        //{
        //    List<ContactoModel> lista = new List<ContactoModel>();
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        using (var command = new SqlCommand("select IdContacto,Nombre,Telefono", conn))
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    ContactoModel contact = new ContactoModel()
        //                    {
        //                        IdContacto = (int)reader["IdContacto"],
        //                        Nombre = (string)reader["Nombre"],
        //                        Telefono = (string)reader["Telefono"]
        //                    };
        //                    lista.Add(contact);
        //                }
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return lista;
        //}

        public bool UpdateContacto(ContactoModel contacto)
        {
            //Solo Cabecera

            bool isSuccess = false;

            try
            {
                //Declaramos la transaccion
                using (TransactionScope scope = new TransactionScope())
                {
                    //Establecemos la conexion
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string actualizaContacto = "update Contacto " +
                                                " set Nombre = @Nombre, " +
                                                "Telefono = @Telefono, " +
                                                "where" +
                                                " id = @Id ";

                        using (var commandAlumno = new SqlCommand(actualizaContacto, conn))
                        {
                            commandAlumno.Parameters.Add(new SqlParameter("@Nombre", contacto.Nombre));
                            commandAlumno.Parameters.Add(new SqlParameter("@Telefono", contacto.Telefono));

                            var filasActualizadas = commandAlumno.ExecuteNonQuery();

                            if (filasActualizadas > 0)
                            {
                                isSuccess = true;
                                scope.Complete();
                            }
                            else
                            {
                                scope.Dispose();
                            }
                        }

                    }

                }

                return isSuccess;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public bool RemoveContacto(ContactoModel contacto)
        {
            bool isSuccess = false;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string eliminaContacto = "remove from contacto where IdContacto=@IdContacto";

                        using (var command = new SqlCommand(eliminaContacto, conn))
                        {
                            command.Parameters.Add(new SqlParameter("@IdContacto", contacto.IdContacto));
                            var updatedRows = command.ExecuteNonQuery();
                            if (updatedRows > 0)
                            {
                                isSuccess = true;
                                scope.Complete();
                            }
                            else
                                scope.Dispose();
                        }
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
