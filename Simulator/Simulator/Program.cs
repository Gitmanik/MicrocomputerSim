namespace Simulator;

class Program
{
    static void Main(string[] args)
    {
        var executor = new Executor(new CPU());
        
        var program = File.ReadAllLines("simple.asm");
        
        while (executor.Cpu.IP < program.Length)
            executor.Execute(program[executor.Cpu.IP]);
        
        executor.Cpu.Dump();

    }
}