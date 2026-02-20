using System.Runtime.InteropServices;

namespace TitleEdit.Data.Character
{
    [StructLayout(LayoutKind.Explicit, Size = 0x2370)]
    public unsafe struct CharacterExpanded
    {
        [FieldOffset(0x0)]
        public GameObjectVTable* VTable;
        [FieldOffset(0x0)]
        public FFXIVClientStructs.FFXIV.Client.Game.Character.Character Character;
        [FieldOffset(0x600)]
        public MovementMode MovementMode;

        public void SetScale(float scale)
        {
            fixed (CharacterExpanded* thisPtr = &this)
            {
                VTable->setScale(thisPtr, scale);
            }
        }
    }

    public enum MovementMode : byte
    {
        Normal = 0,
        Flying = 1,
        Swiming = 2
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct GameObjectVTable
    {
        //[VirtualFunction(25)]
        //public partial void setScale(float);
        [FieldOffset(0xc8)]
        public delegate* unmanaged[Stdcall]<void*, float, void> setScale;
    }
}
