using MadWin.Infrastructure.Context;

public  class Tools
{
    private  readonly MadWinDBContext _context;
    public Tools(MadWinDBContext context)
    {
        _context = context;
    }
    public MadWin.Core.Entities.Settings.Setting GetTools()
    {
        var q = _context.Setting.FirstOrDefault();
        return q;
    }
}

