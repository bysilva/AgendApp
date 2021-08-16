using AgendApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendApp.Services
{
    public interface IContactos
    {
        List<ContactoModel> GetContactos();
        int AddContacto(ContactoModel contacto);
        bool UpdateContacto(ContactoModel contacto);

        bool RemoveContacto(ContactoModel contacto);
    }
}
