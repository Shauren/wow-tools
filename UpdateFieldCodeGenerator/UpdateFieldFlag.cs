namespace UpdateFieldCodeGenerator
{
    [Flags]
    public enum UpdateFieldFlag : byte
    {
        None = 0,
        Owner = 0x01,
        PartyMember = 0x02,
        UnitAll = 0x04,
        Empath = 0x08,
        ViewerDependent = 0x10,
        NewObject = 0x20,
    }

    [Flags]
    public enum CustomUpdateFieldFlag
    {
        None = 0,
        ForceNewBlockBit = 0x1,
        NoArrayElementBits = 0x2,
    }
}
