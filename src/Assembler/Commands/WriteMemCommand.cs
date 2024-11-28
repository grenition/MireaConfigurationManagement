namespace MireaConfigurationManagement.Assembler;

public class WriteMemCommand : AssemblerCommand
{
    public override string Name => "WRITE_MEM";

    public int RegAddressSource { get; private set; }
    public int RegAddressBase { get; private set; }
    public int Offset { get; private set; }

    public override bool TryParse(string[] tokens, out AssemblerCommand command)
    {
        command = null;

        if (tokens.Length != 4 || tokens[0].ToUpper() != Name)
            return false;

        if (int.TryParse(tokens[1], out int regSource) &&
            int.TryParse(tokens[2], out int regBase) &&
            int.TryParse(tokens[3], out int offset))
        {
            command = new WriteMemCommand
            {
                RegAddressSource = regSource,
                RegAddressBase = regBase,
                Offset = offset
            };
            return true;
        }
        return false;
    }

    public override byte[] Assemble()
    {
        uint instructionValue = 0;
        instructionValue |= (uint)(3 & 0x3F);
        instructionValue |= ((uint)(RegAddressSource & 0x7) << 6);
        instructionValue |= ((uint)(RegAddressBase & 0x7) << 9);
        instructionValue |= ((uint)(Offset & 0x7FF) << 12);

        byte[] instructionBytes = new byte[3];
        instructionBytes[0] = (byte)(instructionValue & 0xFF);
        instructionBytes[1] = (byte)((instructionValue >> 8) & 0xFF);
        instructionBytes[2] = (byte)((instructionValue >> 16) & 0xFF);

        return instructionBytes;
    }

    public override string GetLogEntry()
    {
        return $"A=3, B={RegAddressSource}, C={RegAddressBase}, D={Offset}";
    }
}
