using Microsoft.AspNetCore.Mvc;
using TestApp.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("/book")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Book> GetBooks()
        {
            return BooksStore.books;
        }
        [HttpGet("id", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Book> GetBook(int id) => BooksStore.books.FirstOrDefault(u => u.Id == id);

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Book> CreateBook([FromBody] Book book)
        {
            if (BooksStore.books.FirstOrDefault(c => c.Title.ToLower() == book.Title.ToLower()) != null)
            {
                ModelState.AddModelError("", "Book already exist!");
                return BadRequest(ModelState);
            }
            if (book == null)
            {
                return BadRequest(book);
            }
            if (book.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            book.Id = BooksStore.books.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            BooksStore.books.Add(book);

            return CreatedAtRoute("GetBook", new { id = book.Id }, book);
        }


        [HttpDelete("id", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Book> DeleteBook(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var book = BooksStore.books.FirstOrDefault(u => u.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            BooksStore.books.Remove(book);
            return NoContent();
        }

        [HttpPut("id", Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || book.Id != id)
            {
                return BadRequest();
            }
            var b = BooksStore.books.FirstOrDefault(u => u.Id == id);
            b.Title = book.Title;

            return NoContent();
        }
    }
}
