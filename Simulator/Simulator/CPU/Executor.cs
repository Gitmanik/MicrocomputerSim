namespace Simulator;

public class Executor
{
    public CPU Cpu { get; private set; }
    
    public Executor(CPU cpu)
    {
        Cpu = cpu;
    }

    public void Execute(string line, int line_no)
    {
        if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine($"Skipping {line_no}: {line}");
            Cpu.IP += 1;
            return;
        }
        Console.WriteLine($"Executing {line_no}: {line}");
        
        
        // Usuwamy białe znaki i zamieniamy na wielkie litery dla spójności
        line = line.Trim().ToUpper();
        
        // Usuwamy komentarze
        line = line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[0];
        
        // Rozdzielamy linię na części: rozkaz i argumenty
        string[] parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 1)
            throw new ArgumentException($"Invalid line {line_no}: '{line}'");
        
        string instruction = parts[0];
        
        switch (instruction)
        {
            case "INT":
                if (parts.Length != 2)
                    throw new ArgumentException($"{line_no}: Argument count invalid, expected 3, got {parts.Length}");
                Cpu.HandleInt(parts[1]);
                break;
            case "MOV":
                if (parts.Length != 3)
                    throw new ArgumentException($"{line_no}: Argument count invalid, expected 3, got {parts.Length}");
                Cpu.HandleMov(parts[1], parts[2]);
                break;
            case "ADD":
                if (parts.Length != 3)
                    throw new ArgumentException($"{line_no}: Argument count invalid, expected 3, got {parts.Length}");
                Cpu.HandleAdd(parts[1], parts[2]);
                break;
            case "SUB":
                if (parts.Length != 3)
                    throw new ArgumentException($"{line_no}: Argument count invalid, expected 3, got {parts.Length}");
                Cpu.HandleSub(parts[1], parts[2]);
                break;
            default:
                throw new ArgumentException($"{line_no}: Unknown instruction: {instruction}");
        }
        
        Cpu.IP += 1;
    }
}
