namespace MireaConfigurationManagement.Assembler;

public class BitrevCommand : AssemblerCommand
{
    public override string Name => "BITREV";

    private const uint A = 58;

    private uint B; 
    private uint C;

    public override bool TryParse(string[] tokens)
    {
        if (tokens.Length != 3 || tokens[0] != Name)
            return false;

        string reg = tokens[1];
        string memoryAddressStr = tokens[2];
        
        if (!uint.TryParse(reg, out B))
            throw new Exception("Invalid register number in BITREV");

        if (!uint.TryParse(memoryAddressStr, out C))
            throw new Exception("Invalid memory address in BITREV");

        return true;
    }

    public override byte[] Assemble()
    {
        ulong code = 0;
        
        code |= ((ulong)(A & 0x3F)) << 0;         
        code |= ((ulong)(B & 0x7)) << 6;        
        code |= ((ulong)(C & 0x3FFFFF)) << 9; 

        byte[] bytes = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)((code >> (i * 8)) & 0xFF);
        }

        return bytes;
    }

    public override string GetLogEntry()
    {
        return $"BITREV: A={A}, B={B}, C={C}";
    }
}