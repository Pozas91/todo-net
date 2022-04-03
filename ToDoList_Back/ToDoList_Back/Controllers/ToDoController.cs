using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ToDoList_Back.Models;

namespace ToDoList_Back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        // Create a fake database in memory to keep user's tasks until reset of application (key id to fast search)
        private static readonly Dictionary<int, Task> Database = new();

        [HttpGet("/tasks")]
        [EnableCors]
        public IEnumerable<Task> GetAll()
        {
            // Return all values in database memory
            return Database.Values;
        }

        [HttpPost("/tasks")]
        [EnableCors]
        public ActionResult<Task> Create([FromBody] string text)
        {
            // Append that fake id to the new task and by default
            var task = new Task
            {
                // As id I decide use fake database length
                // (tasks cannot be deleted, for this reason is "secure" use this method)
                Id = Database.Count, Completed = false, Text = text
            };

            // Add task given into database memory
            Database.Add(task.Id, task);

            // Return accepted response to notice client
            return Accepted(task);
        }

        [HttpPut("/tasks/{id:int}")]
        [EnableCors]
        public ActionResult<Task> Toggle(int id)
        {
            if (Database.ContainsKey(id))
            {
                // Filter task by id (like database)
                var task = Database[id];

                // Toggle status
                task.Completed = !task.Completed;

                // Return new task information
                return Accepted(task);
            }
            else
            {
                return NotFound();
            }
        }
    }
}