namespace ConcessionariaAPP.Models;

public class TableViewModel<R, F> where R : class where F : class
{
    public List<string> Columns { get; set; } = [];
    public List<string> Keys { get; set; } = [];
    public List<R> Rows { get; set; } = [];
    public F Filters { get; set; } = default!;
}