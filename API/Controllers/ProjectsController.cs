using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/Projects")] // prefix pre API
    public class ProjectsController : ApiController
    {
        // GET: api/Projects/
        /// <summary>
        /// Zobrazenie zoznamu všetkých projektov
        /// </summary>
        /// <returns>Zoznam projektov vo formáte JSON</returns>
        public async Task<List<Projects>> Get()
        {
            var r = Projects.GetAll();
            return await Task.FromResult(r);
        }

        // GET: api/Projects/prjX
        /// <summary>
        /// Zobrazenie konkrétneho projektu
        /// </summary>
        /// <param name="id">ID projektu</param>
        /// <returns>Projekt vo formáte JSON</returns>
        [Route("{id}")]
        public async Task<Projects> Get(string id)
        {
            var r = Projects.GetOne(id);
            return await Task.FromResult(r);
        }

        // POST: api/Projects
        /// <summary>
        /// Pridanie nového projektu
        /// </summary>
        /// <param name="p">Údaje nového projektu vo formáte JSON</param>
        /// <returns>Správa o vykonaní operácie (úspešné alebo chybová hláška)</returns>
        public string Post([FromBody] Projects p)
        {
            return Projects.Add(p);
        }

        // PUT: api/Projects
        /// <summary>
        /// Editácia existujúceho projektu
        /// </summary>
        /// <param name="p">Údaje upraveného projektu vo formáte JSON</param>
        /// <returns>Správa o vykonaní operácie (úspešné alebo chybová hláška)</returns>
        public string Put([FromBody] Projects p)
        {
            return Projects.Edit(p);
        }

        // DELETE: api/Projects/prjX
        /// <summary>
        /// Zmazanie existujúceho projektu
        /// </summary>
        /// <param name="id">ID projektu, ktorý sa má zmazať</param>
        /// <returns>Správa o vykonaní operácie (úspešné alebo chybová hláška)</returns>
        [Route("{id}")]
        public string Delete(string id)
        {
            return Projects.Del(id);
        }
    }
}
