using FFXIVClientStructs.FFXIV.Client.Game;

namespace TitleEdit.Data.Persistence;

public struct Festival
{
    public ushort Id;
    public ushort Phase;

    public Festival(GameMain.Festival festival)
    {
        Id = festival.Id;
        Phase = festival.Phase;
    }
}
