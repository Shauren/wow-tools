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
        Unk5 = 0x10,
        SkipChangeHandlers = 0x20,
        Unk7 = 0x40,
        Unk8 = 0x80,
    }

    [Flags]
    public enum CustomUpdateFieldFlag
    {
        None = 0,
        ViewerDependent = 0x1,
    }
}
