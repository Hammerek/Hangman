using System;
using System.Windows.Forms;

namespace Hangman
{
    /// <summary>
    /// </summary>
    public partial class MainPage : Form
    {
        private readonly Hangman _hangman;

        public MainPage()
        {
            InitializeComponent();
            _hangman = new Hangman(this);
            checkWord.Enabled = false; //button är stängt från början
            textBox1.KeyDown += TextBox1OnKeyDown; //med varje bokstav i textboxen använder metoden
            textBox1.ReadOnly = true; //man får inte skriva i textboxen från början
        }

        private void TextBox1OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Enter) //om man trycker enter då behöver man inte clicka med musen
            {
                checkWord_Click(sender, keyEventArgs); //använder enter som mus click
            }
            var a = _hangman.LeftTries; //tilldelar uträkning till en variabel så att man behöver inte skriva så mycket

            if (a == 0) //gör så att man får inte MsgBox när man får inte gissa längre
            {
                return;
            }
            if (textBox1.Text.Length == a) //om antal tecken i texboxen är samma som NUMBEROFTRIES minus _misstries
            {
                MessageBox.Show("You can't write more letters."); //då får man inte skriva mer bokstäver
            }
            if (textBox1.Text.Length > a) //om antal tecken i texboxen är större än a
            {
                textBox1.Text = textBox1.Text.Substring(0, a);
                    //tar bort tecken i texboxen om man försöker skriva mer än 5 
            }
        }

        /// <summary>
        ///     Button gör så att väljer nytt ord och sätter det i labeln.
        /// </summary>
        private void newWord_Click(object sender, EventArgs e)
        {
            Clear(); //Clear metod
            _hangman.GenerateNewWord();
            UpdateUI(); //metod för update
        }

        /// <summary>
        ///     Button som checkar om ordet innehåller tecken från textboxen.
        /// </summary>
        private void checkWord_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) //om textbox är empty
            {
                return; //returnerar ingenting
            }
            var txt = textBox1.Text.ToLower(); //variabel till textboxen och metod som byter bokstaver till småa
            textBox1.Text = ""; //tömmer textbox
            _hangman.CheckLetters(txt);
        }

        private void Clear() //metod som tömmer variablar, textboxen, öppnar button och gör Update metoden
        {
            textBox1.ReadOnly = false; //man får skriva i texboxen
            label4.Text = "";
            _hangman.Clear();
            checkWord.Enabled = true;
            UpdateUI();
        }

        public void UpdateUI() //uppdaterar label
        {
            label2.Text = "Antal försök kvar: " + _hangman.LeftTries; //räknar antal försök
            if (_hangman.IsGameOver) //om fel gissade försök är lika med fem
            {
                checkWord.Enabled = false; //man stänger ner knappen
                textBox1.ReadOnly = true;
            }
            label1.Text = _hangman.HiddenWord; //visal ord med undertecken i labeln
        }

        public void ShowUsedLetters(string usedLetters)
        {
            label4.Text = usedLetters;
        }

        public void MarkWin()
        {
            MessageBox.Show("Du vann!");
            checkWord.Enabled = false;
            textBox1.ReadOnly = true;
        }
    }
}