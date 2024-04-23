
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBook.Data.Contexts;
using WebApiBook.Models;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookContext _context;

    public BooksController(BookContext context)
    {
        _context = context;
    }

    // GET: api/books
    [HttpGet("GetBooks")]
    public async Task<ActionResult<IEnumerable<BookModel>>> GetBooks()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (token == "")
        {
            return Unauthorized();
        }


        var books = await _context.TbBooks.ToListAsync();
        var bookModels = books.Select(book => new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre
        }).ToList();

        return bookModels;
    }

    // GET: api/books/{id}
    [HttpGet("GetBook/{id}")]
    public async Task<ActionResult<BookModel>> GetBook(int id)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (token == "")
        {
            return Unauthorized();
        }

        var book = await _context.TbBooks.FirstOrDefaultAsync(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        var bookModel = new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre
        };

        return bookModel;
    }


    // POST: api/books
    [HttpPost("PostBook")]
    public async Task<ActionResult<BookModel>> PostBook(BookModel bookModel)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (token == "")
        {
            return Unauthorized();
        }

        var tbBook = new TbBook
        {
            Title = bookModel.Title,
            Author = bookModel.Author,
            Genre = bookModel.Genre
        };

        _context.TbBooks.Add(tbBook);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { id = tbBook.Id }, bookModel);
    }

    // PUT: api/books/{id}
    [HttpPut("PutBook/{id}")]
    public async Task<IActionResult> PutBook(int id, BookModel bookModel)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (token == "")
        {
            return Unauthorized();
        }

        if (id != bookModel.Id)
        {
            return BadRequest();
        }

        var tbBook = await _context.TbBooks.FindAsync(id);
        if (tbBook == null)
        {
            return NotFound();
        }

        tbBook.Title = bookModel.Title;
        tbBook.Author = bookModel.Author;
        tbBook.Genre = bookModel.Genre;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    // DELETE: api/books/{id}
    [HttpDelete("DeleteBook/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (token == "")
        {
            return Unauthorized();
        }

        var book = await _context.TbBooks.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.TbBooks.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    // GET: api/books/GetBooksByGenre/{genre}
    [HttpGet("GetBooksByGenre/{genre}")]
    public async Task<ActionResult<IEnumerable<BookModel>>> GetBooksByGenre(string genre)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        if (token == "")
        {
            return Unauthorized();
        }

        var books = await _context.TbBooks.Where(b => b.Genre == genre).ToListAsync();
        var bookModels = books.Select(book => new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre
        }).ToList();

        return bookModels;
    }

    private bool BookExists(int id)
    {
        return _context.TbBooks.Any(e => e.Id == id);
    }
}
