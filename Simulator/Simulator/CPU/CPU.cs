namespace Simulator;

public class CPU
{
    // Rejestry 16-bitowe
    public ushort AX { get; set; }
    public ushort BX { get; set; }
    public ushort CX { get; set; }
    public ushort DX { get; set; }
    
    // Dostęp do 8-bitowych części rejestrów
    public byte AH
    {
        get => (byte)(AX >> 8);
        set => AX = (ushort)((AX & 0x00FF) | (value << 8));
    }
    
    public byte AL
    {
        get => (byte)(AX & 0xFF);
        set => AX = (ushort)((AX & 0xFF00) | value);
    }
    
    // Implementacja dla rejestru BX
    public byte BH
    {
        get => (byte)(BX >> 8);
        set => BX = (ushort)((BX & 0x00FF) | (value << 8));
    }
    
    public byte BL
    {
        get => (byte)(BX & 0xFF);
        set => BX = (ushort)((BX & 0xFF00) | value);
    }
    
    // Implementacja dla rejestru CX
    public byte CH
    {
        get => (byte)(CX >> 8);
        set => CX = (ushort)((CX & 0x00FF) | (value << 8));
    }
    
    public byte CL
    {
        get => (byte)(CX & 0xFF);
        set => CX = (ushort)((CX & 0xFF00) | value);
    }
    
    // Implementacja dla rejestru DX
    public byte DH
    {
        get => (byte)(DX >> 8);
        set => DX = (ushort)((DX & 0x00FF) | (value << 8));
    }
    
    public byte DL
    {
        get => (byte)(DX & 0xFF);
        set => DX = (ushort)((DX & 0xFF00) | value);
    }
    
    // Rejestr IP
    public ushort IP { get; set; }
    
    // Rejestr SP
    public ushort SP { get; set; }
    
    public CPU()
    {
    }
    
    public void HandleMov(string destination, string source)
    {
        ushort sourceValue = GetValue(source);
        SetValue(destination, sourceValue);
    }

    public void HandleAdd(string destination, string source)
    {
        ushort destValue = GetValue(destination);
        ushort sourceValue = GetValue(source);
        SetValue(destination, (ushort)(destValue + sourceValue));
    }

    public void HandleSub(string destination, string source)
    {
        ushort destValue = GetValue(destination);
        ushort sourceValue = GetValue(source);
        SetValue(destination, (ushort)(destValue - sourceValue));
    }

    public ushort GetValue(string register)
    {
        // Sprawdzamy czy wartość jest liczbą szesnastkową
        if (register.StartsWith("0X") || register.StartsWith("0x"))
        {
            return Convert.ToUInt16(register.Substring(2), 16);
        }
        // Sprawdzamy czy wartość jest liczbą dziesiętną
        if (ushort.TryParse(register, out ushort number))
        {
            return number;
        }
        
        // Jeśli nie jest liczbą, traktujemy jako rejestr
        return register switch
        {
            "AX" => AX,
            "BX" => BX,
            "CX" => CX,
            "DX" => DX,
            "AH" => AH,
            "AL" => AL,
            "BH" => BH,
            "BL" => BL,
            "CH" => CH,
            "CL" => CL,
            "DH" => DH,
            "DL" => DL,
            _ => throw new ArgumentException($"Invalid register or value: {register}")
        };
    }

    private void SetValue(string register, ushort value)
    {
        switch (register)
        {
            case "AX":
                AX = value;
                break;
            case "BX":
                BX = value;
                break;
            case "CX":
                CX = value;
                break;
            case "DX":
                DX = value;
                break;
            case "AH":
                AH = (byte)(value & 0xFF);
                break;
            case "AL":
                AL = (byte)(value & 0xFF);
                break;
            case "BH":
                BH = (byte)(value & 0xFF);
                break;
            case "BL":
                BL = (byte)(value & 0xFF);
                break;
            case "CH":
                CH = (byte)(value & 0xFF);
                break;
            case "CL":
                CL = (byte)(value & 0xFF);
                break;
            case "DH":
                DH = (byte)(value & 0xFF);
                break;
            case "DL":
                DL = (byte)(value & 0xFF);
                break;
            default:
                throw new ArgumentException($"Invalid register: {register}");
        }
    }
    
    public void Dump()
    {
        Console.WriteLine("=== CPU Registers ===");
        Console.WriteLine($"AX: 0x{AX:X4} (AH: 0x{AH:X2}, AL: 0x{AL:X2})");
        Console.WriteLine($"BX: 0x{BX:X4} (BH: 0x{BH:X2}, BL: 0x{BL:X2})");
        Console.WriteLine($"CX: 0x{CX:X4} (CH: 0x{CH:X2}, CL: 0x{CL:X2})");
        Console.WriteLine($"DX: 0x{DX:X4} (DH: 0x{DH:X2}, DL: 0x{DL:X2})");
        Console.WriteLine($"IP: 0x{IP:X4}");
        Console.WriteLine($"SP: 0x{SP:X4}");
        Console.WriteLine("=================");
    }

    public void Reset()
    {
        AX = 0;
        BX = 0;
        CX = 0;
        DX = 0;
        IP = 0;
        SP = 0;
    }
}