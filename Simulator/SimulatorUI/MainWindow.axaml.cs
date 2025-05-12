using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Simulator;

namespace SimulatorUI;

public partial class MainWindow : Window
{
    private readonly CPU _cpu;
    private readonly Executor _executor;
    private string[] _programLines;

    public MainWindow()
    {
        InitializeComponent();
        _cpu = new CPU();
        _executor = new Executor(_cpu);
        _programLines = new string[0];
        CodeEditor.Text = "";
        UpdateCpuState();
    }

    // Kliknięcie przycisku Run
    private void OnRunClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoadProgram();
        _cpu.Reset();
        while (_cpu.IP < _programLines.Length)
        {
            ExecuteCurrentLine();
        }
    }

    // Kliknięcie przycisku Step
    private void OnStepClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoadProgram();
        if (_cpu.IP < _programLines.Length)
        {
            ExecuteCurrentLine();
        }
    }

    // Wczytanie i parsowanie programu
    private void LoadProgram()
    {
        _programLines = CodeEditor.Text.Replace("\r", "").Split(new[] { '\n' }, StringSplitOptions.None);
    }

    // Wykonanie bieżącej linii kodu
    private void ExecuteCurrentLine()
    {
        try
        {
            var line = _programLines[_cpu.IP];
            _executor.Execute(line);
            UpdateCpuState();
            ConsoleOutput.Text = $"Executed {line}\n";
            ConsoleOutput.Foreground = Brushes.Green;
        }
        catch (Exception ex)
        {
            ConsoleOutput.Text = $"Error: {ex.Message}\n";
            ConsoleOutput.Foreground = Brushes.Red;
        }
    }

    // Aktualizacja wartości rejestrów w UI
    private void UpdateCpuState()
    {
        AxValue.Text = $"0x{_cpu.AX:X4} (AH: 0x{_cpu.AH:X2}, AL: 0x{_cpu.AL:X2})";
        BxValue.Text = $"0x{_cpu.BX:X4} (BH: 0x{_cpu.BH:X2}, BL: 0x{_cpu.BL:X2})";
        CxValue.Text = $"0x{_cpu.CX:X4} (CH: 0x{_cpu.CH:X2}, CL: 0x{_cpu.CL:X2})";
        DxValue.Text = $"0x{_cpu.DX:X4} (DH: 0x{_cpu.DH:X2}, DL: 0x{_cpu.DL:X2})";
        IpValue.Text = _cpu.IP.ToString("X4");
        SpValue.Text = _cpu.SP.ToString("X4");
        CodeEditor_TextChanged(null, null);
    }

    // Zapis programu do pliku
    private async void OnSaveClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog();
        dialog.DefaultExtension = "asm";
        dialog.Filters.Add(new FileDialogFilter { Extensions = { "asm" } });

        var path = await dialog.ShowAsync(this);
        if (path != null)
        {
            System.IO.File.WriteAllText(path, CodeEditor.Text);
        }
    }

    // Wczytanie programu z pliku
    private async void OnLoadClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.AllowMultiple = false;
        dialog.Filters.Add(new FileDialogFilter { Extensions = { "asm" } });
        var result = await dialog.ShowAsync(this);
        if (result != null && result.Length > 0)
        {
            CodeEditor.Text = await System.IO.File.ReadAllTextAsync(result[0]);
            _cpu.Reset();
            ConsoleOutput.Text = "";
            UpdateCpuState();
        }
    }

    private void OnResetClicked(object? sender, RoutedEventArgs e)
    {
        _cpu.Reset();
        UpdateCpuState();
        ConsoleOutput.Text = "";
    }
    
    private void CodeEditor_TextChanged(object? sender, RoutedEventArgs e)
    {
        // Liczba linii w `TextBox` (liczone na podstawie wierszy tekstu)
        var lineCount = CodeEditor.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;

        // Tworzenie ciągu z numerami linii
        var lineNumbers = string.Join(Environment.NewLine, Enumerable.Range(1, lineCount)
            .Select(i => i == _cpu.IP ? ">" : i.ToString()));
        
        // Wyświetlanie numerów linii w `TextBlock`
        LineNumbers.Text = lineNumbers;
    }
}