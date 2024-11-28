namespace MireaConfigurationManagement.Assembler;

public class LoadConstCommand : AssemblerCommand
{
    public override string Name => "LOAD_CONST";

    private const uint A = 28;

    private uint B;
    private uint C;

    public override bool TryParse(string[] tokens)
    {
        if (tokens.Length != 3 || tokens[0] != Name)
            return false;

        string operand = tokens[1];
        string reg = tokens[2];

        if (!uint.TryParse(operand, out B))
            throw new Exception("Invalid constant value in LOAD_CONST");

        if (!uint.TryParse(reg, out C))
            throw new Exception("Invalid register number in LOAD_CONST");

        return true;
    }

    public override byte[] Assemble()
    {
        ulong code = 0;

        code |= (A & 0x3F) << 0;
        code |= (B & 0x3FFF) << 6;
        code |= (C & 0x7) << 20;

        byte[] bytes = new byte[3];
        bytes[0] = (byte)((code >> 0) & 0xFF);
        bytes[1] = (byte)((code >> 8) & 0xFF);
        bytes[2] = (byte)((code >> 16) & 0xFF);

        return bytes;
    }

    public override string GetLogEntry()
    {
        return $"LOAD_CONST: A={A}, B={B}, C={C}";
    }
}
