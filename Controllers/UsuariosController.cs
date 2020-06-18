using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PLANETAVERDE_API.Models;

namespace PLANETAVERDE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly PLANETAVERDEWEBContext _context=new PLANETAVERDEWEBContext();

        public UsuariosController()
        {
            
        }


        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public dynamic PostUsuarios([FromBody]JObject user)
        {
            dynamic jsonResponse = new JObject();
            try
            {
                var vl_user = user.Value<string>("user");
                var vl_pass = user.Value<string>("pass");
                JArray Jarray = new JArray();

                var usuarios = _context.Usuarios.FromSqlRaw($"select * from Usuarios where NB_USUARIO='{vl_user}' AND VL_CONTRASEÑA='{vl_pass}'").ToList();

                if (usuarios.Count==0)
                {
                    jsonResponse.code = 400;
                    jsonResponse.msj = "No se encontro el usuario";
                    jsonResponse.data = null;
                    return jsonResponse;
                }

                for (int i = 0; i < usuarios.Count; i++)
                {
                    Jarray.Add(JToken.FromObject(
            new Usuarios
            {
                IdUsuario=usuarios[i].IdUsuario,
                NbUsuario=usuarios[i].NbUsuario,
                VlContraseña = usuarios[i].VlContraseña,
                FhRegistro = usuarios[i].FhRegistro,
            }));
                }

                jsonResponse.code = 200;
                jsonResponse.msj = "";
                jsonResponse.data = Jarray;
                return jsonResponse;
            }
            catch (Exception e)
            {
                jsonResponse.code = 500;
                jsonResponse.data = null;
                jsonResponse.msj = e.Message;
                return jsonResponse;
            }
        }

        
    }
}
