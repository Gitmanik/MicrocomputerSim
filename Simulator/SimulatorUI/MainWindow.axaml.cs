using System;
using Avalonia.Controls;
using Simulator;

namespace SimulatorUI;

public partial class MainWindow : Window
{
    private readonly CPU _cpu;
    private readonly Executor _executor;
    private string[] _programLines;
    private int _currentLine;

    public MainWindow()
    {
        InitializeComponent();
        _cpu = new CPU();
        _executor = new Executor(_cpu);
        _currentLine = 0;
    }

    // Kliknięcie przycisku Run
    private void OnRunClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoadProgram();
        _currentLine = 0;
        _cpu.Reset();
        while (_currentLine < _programLines.Length)
        {
            ExecuteCurrentLine();
        }
    }

    // Kliknięcie przycisku Step
    private void OnStepClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoadProgram();
        if (_currentLine < _programLines.Length)
        {
            ExecuteCurrentLine();
        }
    }

    // Wczytanie i parsowanie programu
    private void LoadProgram()
    {
        _programLines = CodeEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    // Wykonanie bieżącej linii kodu
    private void ExecuteCurrentLine()
    {
        try
        {
            var line = _programLines[_currentLine];
            _executor.Execute(line);
            UpdateCpuState();
            _currentLine++;
        }
        catch (Exception ex)
        {
            ConsoleOutput.Text += $"Error: {ex.Message}\n";
        }
    }

    // Aktualizacja wartości rejestrów w UI
    private void UpdateCpuState()
    {
        AxValue.Text = _cpu.AX.ToString("X4");
        BxValue.Text = _cpu.BX.ToString("X4");
        CxValue.Text = _cpu.CX.ToString("X4");
        DxValue.Text = _cpu.DX.ToString("X4");
        IpValue.Text = _cpu.IP.ToString("X4");
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
            CodeEditor.Text = System.IO.File.ReadAllText(result[0]);
        }
    }
}