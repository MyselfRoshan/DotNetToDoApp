using System.ComponentModel.DataAnnotations;

namespace Todo.Models;
public class TodoItem
{
    public TodoItem()
    {
        Title = string.Empty;
    }
    [Key]
    public int Id { set; get; }
    public string Title { set; get; }
    public DateTime CreatedDate { set; get; }
    // public DateTime ScheduledDate {set;get;}
    public DateTime? CompletedDate { set; get; }

}