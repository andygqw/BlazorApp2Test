namespace BlazorApp2Test.Data;

using BlazorApp2Test.Models;

public class LogData
{
    private readonly UserDbContext _context;
    
    public LogData(UserDbContext context)
    {
        _context = context;
    }

    public async Task Log (String operation, String description, int? id)
    {
        try
        {
            Log log = new Log();
            log.operation = operation;
            log.description = description;
            if (id.HasValue) log.user_id = id.Value;
            log.create_time = DateTime.Now;
            
            if (_context.Log != null)
            {
                _context.Log.Add(log);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Log database error: " + ex.Message);
        }
    }
}