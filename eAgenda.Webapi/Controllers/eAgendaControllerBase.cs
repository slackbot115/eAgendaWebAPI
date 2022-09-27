using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eAgenda.Webapi.Controllers
{
    [ApiController]
    public class eAgendaControllerBase : ControllerBase
    {
        protected ActionResult InternalError<T>(Result<T> tarefaResult)
        {
            return StatusCode(500, new
            {
                sucesso = false,
                erros = tarefaResult.Errors.Select(x => x.Message)
            });
        }

        protected ActionResult BadRequest<T>(Result<T> registroResult)
        {
            return StatusCode(300, new
            {
                sucesso = false,
                erros = registroResult.Errors.Select(x => x.Message)
            });
        }

        protected ActionResult NotFound<T>(Result<T> tarefaResult)
        {
            return StatusCode(404, new
            {
                sucesso = false,
                erros = tarefaResult.Errors.Select(x => x.Message)
            });
        }

        protected static bool RegistroNaoEncontrado<T>(Result<T> tarefaResult)
        {
            return tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada"));
        }
    }
}
