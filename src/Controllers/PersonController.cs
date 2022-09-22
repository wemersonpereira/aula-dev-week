using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

using src.Models;
using src.Persistence;




namespace aula_dev_week.src.Controllers



{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private DatabaseContext _context { get; set; }

        public PersonController(DatabaseContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public ActionResult<List<Pessoa>> Get()
        {           
            var result = _context.pessoas.Include(c => c.contratos).ToList();
            if(!result.Any()) return NoContent();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Pessoa> Post(Pessoa pessoa)
        {
            try
            {
                _context.pessoas.Add(pessoa);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                
                return BadRequest(new {
                    msg = "Registro não Cadastrado",
                    Status = HttpStatusCode.NotFound
                });
            }
            
            return Created("Criado", pessoa);
        }

        [HttpPut("{id}")]
        public ActionResult<Object> Update(
                [FromRoute]int id,
                [FromBody]Pessoa pessoa
             )
        {
             var result = _context.pessoas.AsNoTracking().SingleOrDefault(p => p.Id == id);

            if(result is null ){
                return NotFound(new {
                    msg = "Registro não encontrado",
                    Status = HttpStatusCode.NotFound
                });
            }

            try
            {    
                _context.pessoas.Update(pessoa);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {                
                return BadRequest(new {
                    msg = "Houve erro ao enviar solicitação de atualização do id " + id,
                    Status = HttpStatusCode.OK
                });
            }

            return  Ok( new {                
                msg = $"Dados do id {id} atualizados",
                Status = HttpStatusCode.OK
            });
            
        }

        [HttpDelete("{id}")]
        public ActionResult<Object> Delete([FromRoute]int id)
        {
            var result = _context.pessoas.SingleOrDefault(p => p.Id == id);

                if(result is null){
                    return BadRequest(new {
                        msg = "Código Pessoa não existe, solicitação negada!",
                        Status = HttpStatusCode.BadRequest
                    });
                }

            _context.pessoas.Remove(result);
            _context.SaveChanges();
            return Ok(new {
                msg = $"deletado pessoa com id: {id}",
                Status = HttpStatusCode.OK
            });
        }
    }

    
}