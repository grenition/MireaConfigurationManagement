namespace MireaConfigurationManagement.Assembler;

public class ReadMemCommand : AssemblerCommand
{
    public override string Name => "READ_MEM";

    public int RegAddressBase { get; private set; }
    public int RegAddressTarget { get; private set; }
    public int Offset { get; private set; }

    public override bool TryParse(string[] tokens, out AssemblerCommand command)
    {
        command = null;

        if (tokens.Length != 4 || tokens[0].ToUpper() != Name)
            return false;

        if (int.TryParse(tokens[1], out int regBase) &&
            int.TryParse(tokens[2], out int regTarget) &&
            int.TryParse(tokens[3], out int offset))
        {
            command = new ReadMemCommand
            {
                RegAddressBase = regBase,
                RegAddressTarget = regTarget,
                Offset = offset
            };
            return true;
        }
        return false;
    }

    public override byte[] Assemble()
    {
        uint instructionValue = 0;
        instructionValue |= (uint)(62 & 0x3F);
        instructionValue |= ((uint)(RegAddressBase & 0x7) << 6);
        instructionValue |= ((uint)(RegAddressTarget & 0x7) << 9);
        instructionValue |= ((uint)(Offset & 0x7FF) << 12);

        byte[] instructionBytes = new byte[3];
        instructionBytes[0] = (byte)(instructionValue & 0xFF);
        instructionBytes[1] = (byte)((instructionValue >> 8) & 0xFF);
        instructionBytes[2] = (byte)((instructionValue >> 16) & 0xFF);

        return instructionBytes;
    }

    public override string GetLogEntry()
    {
        return $"A=62, B={RegAddressBase}, C={RegAddressTarget}, D={Offset}";
    }
}
