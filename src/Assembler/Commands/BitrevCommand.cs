namespace MireaConfigurationManagement.Assembler;

public class BitrevCommand : AssemblerCommand
{
    public override string Name => "BITREV";

    public int RegAddressTarget { get; private set; }
    public int MemoryAddress { get; private set; }

    public override bool TryParse(string[] tokens, out AssemblerCommand command)
    {
        command = null;

        if (tokens.Length != 3 || tokens[0].ToUpper() != Name)
            return false;

        if (int.TryParse(tokens[1], out int regTarget) && int.TryParse(tokens[2], out int memAddress))
        {
            command = new BitrevCommand
            {
                RegAddressTarget = regTarget,
                MemoryAddress = memAddress
            };
            return true;
        }
        return false;
    }

    public override byte[] Assemble()
    {
        uint instructionValue = 0;
        instructionValue |= (uint)(58 & 0x3F);
        instructionValue |= ((uint)(RegAddressTarget & 0x7) << 6);
        instructionValue |= ((uint)(MemoryAddress & 0xFFFFF) << 9);

        byte[] instructionBytes = new byte[4];
        instructionBytes[0] = (byte)(instructionValue & 0xFF);
        instructionBytes[1] = (byte)((instructionValue >> 8) & 0xFF);
        instructionBytes[2] = (byte)((instructionValue >> 16) & 0xFF);
        instructionBytes[3] = (byte)((instructionValue >> 24) & 0xFF);

        return instructionBytes;
    }

    public override string GetLogEntry()
    {
        return $"A=58, B={RegAddressTarget}, C={MemoryAddress}";
    }
}