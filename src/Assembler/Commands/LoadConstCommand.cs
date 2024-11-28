namespace MireaConfigurationManagement.Assembler;
public class LoadConstCommand : AssemblerCommand
{
    public override string Name => "LOAD_CONST";

    public int ConstValue { get; private set; }
    public int RegAddress { get; private set; }

    public override bool TryParse(string[] tokens, out AssemblerCommand command)
    {
        command = null;

        if (tokens.Length != 3 || tokens[0].ToUpper() != Name)
            return false;

        if (int.TryParse(tokens[1], out int constValue) && int.TryParse(tokens[2], out int regAddress))
        {
            command = new LoadConstCommand
            {
                ConstValue = constValue,
                RegAddress = regAddress
            };
            return true;
        }
        return false;
    }

    public override byte[] Assemble()
    {
        uint instructionValue = 0;
        instructionValue |= (uint)(28 & 0x3F);
        instructionValue |= ((uint)(ConstValue & 0x3FFF) << 6);
        instructionValue |= ((uint)(RegAddress & 0x7) << 20);

        byte[] instructionBytes = new byte[3];
        instructionBytes[0] = (byte)(instructionValue & 0xFF);
        instructionBytes[1] = (byte)((instructionValue >> 8) & 0xFF);
        instructionBytes[2] = (byte)((instructionValue >> 16) & 0xFF);

        return instructionBytes;
    }

    public override string GetLogEntry()
    {
        return $"A=28, B={ConstValue}, C={RegAddress}";
    }
}
