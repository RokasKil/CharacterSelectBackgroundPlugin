using System.Numerics;
using System.Runtime.InteropServices;

namespace CharacterSelectBackgroundPlugin.Data.Layout
{

    public enum InstanceType : byte
    {
        BgPart = 1,
        Light = 3,
        Vfx = 4,
        Prefab = 6,
        Sound = 7,
        EnvSpace = 13,
        Prefab2 = 15, //???
        Weapon = 39,
        ColliderLayer2 = 41,
        ColliderLayer3 = 43,
        ColliderLayer4 = 49,
        ColliderGeneric = 57,
        ColliderLayer5 = 71,
        Decal = 83,
        ColliderLayer7 = 86,
        ColliderLayer8 = 87,
        ColliderLayer9 = 88,
        ColliderLayer10 = 89,
    }
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct ILayoutInstanceVTable
    {
        //[VirtualFunction(54)]
        //public partial void SetActiveVF54(bool active);
        [FieldOffset(0x1b0)] public delegate* unmanaged[Stdcall]<ILayoutInstance*, bool, void> setActiveVF54;
        //[VirtualFunction(63)]
        //public partial void SetActive(bool active);
        [FieldOffset(0x1f8)] public delegate* unmanaged[Stdcall]<ILayoutInstance*, bool, void> setActive;
    }

    // instances can have both graphics and collision representation; for ones that have both one is 'primary' and other is 'secondary'
    [StructLayout(LayoutKind.Explicit, Size = 0x30)]
    public unsafe struct ILayoutInstance
    {
        [FieldOffset(0x0)] public ILayoutInstanceVTable* VTable;
        [StructLayout(LayoutKind.Explicit, Size = 0x8)]
        public unsafe struct Identifier
        {
            [FieldOffset(0)] public byte u0; // high 6 bits: index of this instance in prefab, or 'all ones' (0x3F) if top level
            [FieldOffset(1)] public InstanceType Type;
            [FieldOffset(2)] public ushort LayerKey;
            [FieldOffset(4)] public uint InstanceKey;
        }

        [FieldOffset(0x08)] public LayoutLayer* Layer;
        [FieldOffset(0x10)] public LayoutManagerExpanded* Layout;
        [FieldOffset(0x18)] public Identifier Id;
        [FieldOffset(0x20)] public uint SubId; // for instances that are created as part of prefab; high byte is key for nesting level 1 and so on - max 4 nesting levels supported
        [FieldOffset(0x24)] public int IndexInPool;
        [FieldOffset(0x28)] public byte IndexInPrefab;
        [FieldOffset(0x29)] public byte Flags1; // bits0-3: ???, bits4-6: nesting level, bit7: ???
        [FieldOffset(0x2A)] public byte Flags2;
        [FieldOffset(0x2B)] public byte Flags3;

        public int NestingLevel => (Flags1 >> 4) & 7;
        public readonly bool isActive => (Flags3 & 0b10000) != 0;
        public readonly ulong UUID => Id.InstanceKey + ((ulong)SubId << 32);

        public void SetActive(bool active)
        {
            fixed (ILayoutInstance* thisPtr = &this)
            {
                VTable->setActive(thisPtr, active);
            }
        }

        //activates/deactives VFX, not sure what it does on other types but probably safe to use?
        public void SetActiveVF54(bool active)
        {
            fixed (ILayoutInstance* thisPtr = &this)
            {
                VTable->setActiveVF54(thisPtr, active);
            }
        }
        //[VirtualFunction(0)]
        //public partial void Dtor(byte freeFlags);

        //[VirtualFunction(1)]
        //public partial void Init(void* creator, byte* primaryPath);

        //[VirtualFunction(2)]
        //public partial void Deinit();

        //[VirtualFunction(4)]
        //public partial void SetProperties(FileLayerGroupInstance* data);

        //[VirtualFunction(5)]
        //public partial void SetLayer(LayoutLayer* layer);

        //[VirtualFunction(6)]
        //public partial int GetSizeOf();

        //[VirtualFunction(7)]
        //public partial byte* GetPrimaryPath();

        //[VirtualFunction(14)]
        //public partial Vector3* GetTranslation(Vector3* result);

        //[VirtualFunction(15)]
        //public partial Quaternion* GetRotation(Quaternion* result);

        //[VirtualFunction(16)]
        //public partial Vector3* GetScale(Vector3* result);

        //[VirtualFunction(17)]
        //public partial Transform* GetTransform(Transform* result);

        //[VirtualFunction(18)]
        //public partial void SetTransform(Transform* transform);

        //[VirtualFunction(21)]
        //public partial bool HavePrimary();

        //[VirtualFunction(23)]
        //public partial Graphics.Scene.Object* GetGraphics();

        //[VirtualFunction(24)]
        //public partial Graphics.Scene.Object* GetGraphics2();

        //[VirtualFunction(25)]
        //public partial void SetGraphics(Graphics.Scene.Object* obj, Transform* transform);

        //// arg can be either byte** path or int* Type
        //[VirtualFunction(27)]
        //public partial void CreatePrimary(Transform* transform, void* pathOrType);

        //[VirtualFunction(28)]
        //public partial void DestroyPrimary();

        //[VirtualFunction(29)]
        //public partial bool HaveSecondary();

        //[VirtualFunction(30)]
        //public partial bool IsColliderLoaded();

        //[VirtualFunction(31)]
        //public partial byte* GetSecondaryPath();

        //[VirtualFunction(32)]
        //public partial void CreateSecondary();

        //[VirtualFunction(33)]
        //public partial void DestroySecondary();

        //[VirtualFunction(34)]
        //public partial Collider* GetCollider();

        //[VirtualFunction(35)]
        //public partial Collider* GetCollider2();

        //[VirtualFunction(36)]
        //public partial bool IsColliderActive();

        //[VirtualFunction(37)]
        //public partial void SetColliderActive(bool active);

        //[VirtualFunction(38)]
        //public partial void UpdateCollider();

        //[VirtualFunction(55)]
        //public partial bool WantToBeActive();

        //[VirtualFunction(63)]
        //public partial void SetActive(bool active);

        //[VirtualFunction(64)]
        //public partial Vector3* GetTranslationImpl();

        //[VirtualFunction(65)]
        //public partial void SetTranslationImpl(Vector3* Value);

        //[VirtualFunction(66)]
        //public partial Quaternion* GetRotationImpl();

        //[VirtualFunction(67)]
        //public partial void SetRotationImpl(Quaternion* Value);

        //[VirtualFunction(68)]
        //public partial Vector3* GetScaleImpl();

        //[VirtualFunction(69)]
        //public partial void SetScaleImpl(Vector3* Value);

        //[VirtualFunction(70)]
        //public partial Transform* GetTransformImpl();

        //[VirtualFunction(71)]
        //public partial void SetTransformImpl(Transform* Value);

        //[VirtualFunction(72)]
        //public partial Vector4* GetBoundingSphereImpl(Vector4* result);

        // vf73: getWorldBB, uses AABB with padded vec3's
        // vf74: get transform split into RT matrix + scale

        // [VirtualFunction(77)] ...
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x2C)]
    public unsafe partial struct Transform
    {
        [FieldOffset(0x00)] public Vector3 Translation;
        [FieldOffset(0x0C)] public int Type; // note: this is a padding field that in some contexts is used to store collider Type
        [FieldOffset(0x10)] public Quaternion Rotation;
        [FieldOffset(0x20)] public Vector3 Scale;

        public Matrix4x4 Compose()
        {
            var res = Matrix4x4.CreateScale(Scale);
            res = Matrix4x4.Transform(res, Rotation);
            res.Translation = Translation;
            return res;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x68)]
    public unsafe partial struct AnalyticShapeData
    {
        [FieldOffset(0x00)] public int NumRefs;
        [FieldOffset(0x04)] public uint Crc;
        //[FieldOffset(0x08)] public uint u8;
        //[FieldOffset(0x0C)] public uint uC;
        [FieldOffset(0x10)] public Transform Transform;
        //[FieldOffset(0x3C)] public uint u3C;
        [FieldOffset(0x40)] public Vector3 BoundsMin;
        [FieldOffset(0x4C)] public uint MaterialId;
        [FieldOffset(0x50)] public Vector3 BoundsMax;
        [FieldOffset(0x5C)] public uint MaterialMask;
        //[FieldOffset(0x60)] public int u60;
        //[FieldOffset(0x64)] public int u64;

        // yes, they really store the collider Type in the padding of SRT structure...
        //public FileLayerGroupAnalyticCollider.Type Type => (FileLayerGroupAnalyticCollider.Type)Transform.Type;
    }
}
