namespace MireaConfigurationManagement.Assembler;

public class WriteMemCommand : AssemblerCommand
{
    public override string Name => "WRITE_MEM";

    private const uint A = 3;

    private uint B;
    private uint C;
    private uint D;

    public override bool TryParse(string[] tokens)
    {
        if (tokens.Length != 4 || tokens[0] != Name)
            return false;

        string reg = tokens[1];
        string outReg = tokens[2];
        string offset = tokens[3];
        
        if (!uint.TryParse(reg, out B))
            throw new Exception("Invalid register number in READ_MEM");

        if (!uint.TryParse(outReg, out C))
            throw new Exception("Invalid output register number in READ_MEM");

        if (!uint.TryParse(offset, out D))
            throw new Exception("Invalid offset in READ_MEM");

        return true;
    }

    public override byte[] Assemble()
    {
        ulong code = 0;
        
        code |= ((ulong)(A & 0x3F)) << 0;         
        code |= ((ulong)(B & 0x7)) << 6;        
        code |= ((ulong)(C & 0x7)) << 9; 
        code |= ((ulong)(D & 0x7FF)) << 12; 
        
        byte[] bytes = new byte[3];
        bytes[0] = (byte)((code >> 0) & 0xFF);
        bytes[1] = (byte)((code >> 8) & 0xFF);
        bytes[2] = (byte)((code >> 16) & 0xFF); 

        return bytes;
    }

    public override string GetLogEntry()
    {
        return $"WRITE_MEM: A={A}, B={B}, C={C}, D={D}";
    }
}
