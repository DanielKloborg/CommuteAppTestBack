using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ToDoController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDo>>> GetToDoList()
        {
            return Ok(await _dataContext.ToDoList.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<ToDo>>> CreateToDo(ToDo todo)
        {
            _dataContext.ToDoList.Add(todo);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.ToDoList.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<ToDo>>> UpdateToDo(ToDo todo)
        {
            var dbtodo = await _dataContext.ToDoList.FindAsync(todo.Id);
            if (dbtodo == null) return BadRequest("ToDo punkt ikke fundet.");
            dbtodo.Note = todo.Note;
            dbtodo.CheckMark = todo.CheckMark;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.ToDoList.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ToDo>>> DeleteToDo(ToDo todo)
        {
            var dbtodo = await _dataContext.ToDoList.FindAsync(todo.Id);
            if (dbtodo == null) return BadRequest("ToDo punkt ikke fundet.");

            _dataContext.ToDoList.Remove(dbtodo);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.ToDoList.ToListAsync());
        }
    }
}
