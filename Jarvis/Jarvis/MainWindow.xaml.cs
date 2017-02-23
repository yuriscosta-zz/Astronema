using System;
using System.Collections.Generic;
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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace Astronema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer astronema = new SpeechSynthesizer();

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                speechRecognitionEngine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(engine_AudioLevelUpdated);
                speechRecognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(engine_speechRecognition);

                LoadGrammarAndCommands();

                speechRecognitionEngine.SetInputToDefaultAudioDevice();

                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                astronema.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(astronema_SpeakCompleted);

                if (astronema.State == SynthesizerState.Speaking)
                {
                    astronema.SpeakAsyncCancelAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Voice recognition failed");
            }
        }

        private void LoadGrammarAndCommands()
        {
            try
            {
                Choices Text = new Choices();
                string[] Lines = File.ReadAllLines(Environment.CurrentDirectory + "\\Commands.txt");
                Text.Add(Lines);
                Grammar WordsList = new Grammar(new GrammarBuilder(Text));
                speechRecognitionEngine.LoadGrammar(WordsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void astronema_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (astronema.State == SynthesizerState.Speaking)
            {
                astronema.SpeakAsyncCancelAll();
            }
        }

        private void engine_speechRecognition(object sender, SpeechRecognizedEventArgs e)
        {
            string Speech = e.Result.Text;
            switch (Speech)
            {
                case "hello astronema":
                    Commands.Text += "Yuri: Hello, Astronema!\nAstronema: Hello, my master! How are you?\n";
                    astronema.SpeakAsync("Hello, my master! How are you?");
                    break;
                case "hey":
                    Commands.Text += "Yuri: Hey!...\nAstronema: I`m here!\n";
                    astronema.SpeakAsync("I`m here!");
                    break;
                case "i am fine":
                    Commands.Text += "Yuri: I`m fine!\nAstronema: Great! What can I do for you?\n";
                    astronema.SpeakAsync("Great! What can I do for you?");
                    break;
                case "where am i":
                    Commands.Text += "Yuri: Where am I?\nAstronema: I don't know... I am not a GPS. Huehue\n";
                    astronema.SpeakAsync("I don't know... I am not a GPS. Huehue");
                    break;
                case "tell me something good":
                    Commands.Text += "Yuri: Tell me something good\nAstronema: The dolar is almost 3 reais.\n";
                    astronema.SpeakAsync("The dolar is almost 3 reais");
                    break;
                case "are you a human?":
                    Commands.Text += "Yuri: Are you a human?\nAstronema: Yes, I am.\n";
                    astronema.SpeakAsync("Yes, I am.");
                    break;
                case "you are not":
                    Commands.Text += "Yuri: You are not?\nAstronema: Can you prove it?\n";
                    astronema.SpeakAsync("Can you prove it?");
                    break;
                case "i cant":
                    Commands.Text += "Yuri: I can`t\nAstronema: Of course not\n";
                    astronema.SpeakAsync("Of course not");
                    break;
                case "open google":
                    System.Diagnostics.Process.Start("https://google.com.br");
                    break;
                case "open twitter":
                    System.Diagnostics.Process.Start("https://twitter.com");
                    break;
                case "open facebook":
                    System.Diagnostics.Process.Start("https://facebook.com");
                    break;
                case "play a good song":
                    System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=heZQVYvHPsc");
                    break;
                case "bye bye":
                    astronema.SpeakAsync("Bye Bye!");
                    this.Close();
                    break;
                default:
                    Commands.Text += "I didn't understand!\n";
                    astronema.SpeakAsync("I didn't understand!\n");
                    break;
            }
        }

        private void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            Progress.Value = e.AudioLevel;

        }

        private void Commands_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
