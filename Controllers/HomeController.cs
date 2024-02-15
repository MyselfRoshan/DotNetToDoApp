using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using Npgsql;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly string _connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=root;Database=todo";

    public DateTime? CompletedDate { get; private set; }

    internal TodoViewModel GetAllTodos()
    {
        List<TodoItem> todoItems = [];
        var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand("SELECT * FROM todoitem", con);
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var todoItem = new TodoItem
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                CreatedDate = reader.GetDateTime(2)
            };

            // Check if the CompletedDate column is null
            if (!reader.IsDBNull(3))
            {
                CompletedDate = reader.GetFieldValue<DateTime?>(3);
            }

            todoItems.Add(todoItem);
        }

        return new TodoViewModel
        {
            TodoItems = todoItems
        };
    }
    internal TodoItem UpdateById(int id, string title)
    {
        TodoItem todo = new();
        var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand("UPDATE todoitem SET title = @title WHERE id = @id", con);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("title", title);

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        cmd = new NpgsqlCommand("SELECT * FROM todoitem WHERE id =@id", con);
        cmd.Parameters.AddWithValue("id", id);
        var reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            reader.Read();
            todo.Id = reader.GetInt32(0);
            todo.Title = reader.GetString(1);
            todo.CreatedDate = reader.GetDateTime(2);
            if (!reader.IsDBNull(3))
            {
                todo.CompletedDate = reader.GetFieldValue<DateTime?>(3);
            }

        }
        return todo;
    }

    public IActionResult Index()
    {
        var todoListViwModel = GetAllTodos();
        return View(todoListViwModel);
    }

    [HttpPost]
    public JsonResult Create(string title)
    {
        TodoItem todoItem = new();
        var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand("INSERT INTO todoitem (title, createddate) VALUES (@title, @createddate) RETURNING *", con);
        cmd.Parameters.AddWithValue("title", title);
        cmd.Parameters.AddWithValue("createddate", DateTime.Now);

        try
        {
            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }
        var reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            todoItem.Id = reader.GetInt32(0);
            todoItem.Title = reader.GetString(1);
            todoItem.CreatedDate = reader.GetDateTime(2);
            if (!reader.IsDBNull(3))
            {
                todoItem.CompletedDate = reader.GetFieldValue<DateTime?>(3);
            }

        }
        return Json(todoItem);
    }
    // [HttpPost]
    // public RedirectResult Create(TodoItem todoItem)
    // {
    //     var con = new NpgsqlConnection(_connectionString);
    //     con.Open();

    //     var cmd = new NpgsqlCommand("INSERT INTO todoitem (title, createddate) VALUES (@title, @createddate)", con);
    //     cmd.Parameters.AddWithValue("title", todoItem.Title);
    //     cmd.Parameters.AddWithValue("createddate", DateTime.Now);

    //     try
    //     {
    //         cmd.ExecuteNonQuery();

    //     }
    //     catch (Exception ex)
    //     {

    //         Console.WriteLine(ex);
    //     }
    //     return Redirect("http://localhost:5241");
    // }
    [HttpDelete]
    public JsonResult Delete(int id)
    {
        var con = new NpgsqlConnection(_connectionString);
        con.Open();

        var cmd = new NpgsqlCommand("DELETE FROM todoitem WHERE id = @id", con);
        cmd.Parameters.AddWithValue("id", id);
        try
        {
            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
        }

        return Json(new { id });
    }

    [HttpPatch]
    public JsonResult Update(int id, string title)
    {
        Console.Write(id + title);
        var todo = UpdateById(id, title);
        return Json(new { title });
    }

}
