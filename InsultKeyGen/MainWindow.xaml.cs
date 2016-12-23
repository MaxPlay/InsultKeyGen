using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InsultKeyGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string output;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Output { get { return output; } set { output = value; } }

        public MainWindow()
        {
            output = string.Empty;
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            GeneratorSettings settings = GenerateSettings();
            Generate(settings);
        }

        private void Generate(GeneratorSettings settings)
        {
            string input = tbInput.Text;
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                if (char.IsPunctuation(currentChar) || char.IsWhiteSpace(currentChar) || char.IsSymbol(currentChar) || char.IsSeparator(currentChar))
                {
                    input = input.Remove(i, 1);
                    i--;
                }
            }

            input = input.ToUpper();
            input = input.Replace("ß", "55");
            input = input.Replace("A", "4");
            input = input.Replace("E", "3");
            input = input.Replace("I", "1");
            input = input.Replace("O", "0");
            input = input.Replace("S", "5");

            List<string> segments = new List<string>();
            int segmentCount = input.Length / settings.Blocklength;
            int segmentOverhang = input.Length - segmentCount * settings.Blocklength;
            for (int i = 0; i < segmentCount; i++)
            {
                if (i == segmentCount - 1)
                    segments.Add(input.Substring(i * settings.Blocklength, settings.Blocklength + segmentOverhang));
                else
                    segments.Add(input.Substring(i * settings.Blocklength, settings.Blocklength));
            }
            
            List<string[]> lines = new List<string[]>();
            int lineCount = segmentCount / settings.Blockcount;
            int lineOverhang = segments.Count - lineCount * settings.Blockcount;
            for (int i = 0; i < lineCount; i++)
            {
                lines.Add(segments.GetRange(i * settings.Blockcount, settings.Blockcount).ToArray());
            }
            if (lineOverhang > 0)
                lines.Add(segments.GetRange(lineCount * settings.Blockcount, lineOverhang).ToArray());

            List<string> output = new List<string>();

            foreach (string[] s in lines)
            {
                switch (settings.Separator)
                {
                    case Separator.FreeSpace:
                        output.Add(string.Join(" ", s));
                        break;
                    case Separator.Minus:
                        output.Add(string.Join("-", s));
                        break;
                    case Separator.Underscore:
                        output.Add(string.Join("_", s));
                        break;
                }
            }

            this.output = string.Join("\n", output.ToArray());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Output"));
        }

        private GeneratorSettings GenerateSettings()
        {
            GeneratorSettings settings = new GeneratorSettings();
            settings.Blockcount = int.Parse((string)pnlBlockcount.Children.OfType<RadioButton>().FirstOrDefault(f => f.IsChecked == true).Content);
            settings.Blocklength = int.Parse((string)pnlBlockLength.Children.OfType<RadioButton>().FirstOrDefault(f => f.IsChecked == true).Content);
            settings.Separator = (Separator)Enum.Parse(typeof(Separator), ((string)pnlSeparator.Children.OfType<RadioButton>().FirstOrDefault(f => f.IsChecked == true).Content).Replace(" ", string.Empty));
            return settings;
        }
    }
}
