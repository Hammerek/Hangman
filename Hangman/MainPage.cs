using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainPage : Form
    {
        string[] _words = new string[10] //array
      {
            "hentai", "tangentbord", "yuri", "oppai", "loli", "gothic", "programming", "witcher", "anime", "radio"
      };
        List<char> usedLetters = new List<char>(); //listan för bokstaver som man skrev i textboxen
        Random _random = new Random(); //gör nytt random klass
        char[] _currentWord; //ord som man gissar
        char[] _currentWordShow; //ord som man visar i labeln
        int _allTries = 0; //alla försök
        int _missTries = 0; //missade försök
        const int NUMBEROFTRIES = 5; //konstant variabel

        public MainPage()
        {
            InitializeComponent();
            checkWord.Enabled = false; //button är stängt från början
            textBox1.KeyDown += TextBox1OnKeyDown;//med varje bokstav i textboxen använder metoden
            textBox1.ReadOnly = true; //man får inte skriva i textboxen från början
        }

        private void TextBox1OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Enter) //om man trycker enter då behöver man inte clicka med musen
            {
                checkWord_Click(sender, keyEventArgs); //använder enter som mus click
            }
            var a = NUMBEROFTRIES - _missTries; //tilldelar uträkning till en variabel så att man behöver inte skriva så mycket
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
                textBox1.Text = textBox1.Text.Substring(0, a); //tar bort tecken i texboxen om man försöker skriva mer än 5 
            }
        }

        /// <summary>
        /// Button gör så att väljer nytt ord och sätter det i labeln. 
        /// </summary>
        private void newWord_Click(object sender, EventArgs e)
        {

            Clear(); //Clear metod
            var index = _random.Next(0, _words.Length - 1); //väljer en random ord från arrayen mellan 0 och words.Length
            _currentWord = _words[index].ToCharArray(); //en random ord konverterad till Char
            _currentWordShow = new char[_currentWord.Length]; //gör en ny char array till variabeln som man visar
            for (var i = 0; i < _currentWord.Length; i++) //för varje tecken i currentWord
            {
                _currentWordShow[i] = '_';  //sätter undertecken i chararrayen
            }

            UpdateUI(); //metod för update
        }

        /// <summary>
        /// Button som checkar om ordet innehåller tecken från textboxen. 
        /// </summary>
        private void checkWord_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) //om textbox är empty
            {
                return; //returnerar ingenting
            }
            var txt = textBox1.Text.ToLower(); //variabel till textboxen och metod som byter bokstaver till småa

            textBox1.Text = ""; //tömmer textbox

            foreach (var c in txt) //för varje tecken i textboxen
            {
                _allTries++; //ökar alla försök
                if (usedLetters.Contains(c)) //om listan innehåller tecken från texboxen
                {
                    continue; //då kör man programmet vidare
                }
                //man lägger till bokstaven från textboxen till listan, sen konventerar man det till string för att returnera det som en liten bokstav
                usedLetters.Add(c.ToString().ToLower().ToCharArray()[0]); //sen konverterar man det igen till CharArray
                label4.Text = new string(usedLetters.ToArray()); //man skriver ut bokstäver till labeln
                if (_currentWord.Contains(c)) //om ord innehåller tecken från textbox
                {
                    ReplaceChar(c); //man gör replacement på _currentWordShow = byter '_' för rätt tecken i textboxen
                }
                else
                {
                    _missTries++; //ökar antalet försök
                }
                UpdateUI(); //metod för Update
                if (IsWin()) //om metoden är true
                {
                    MessageBox.Show("Du vann!");
                    checkWord.Enabled = false;
                    textBox1.ReadOnly = true;
                }
                if (_missTries >= NUMBEROFTRIES) //om man hade gissat fel 5 gånger 
                {
                    break; //avslutar foreach
                }
            }
        }

        private void Clear() //metod som tömmer variablar, textboxen, öppnar button och gör Update metoden
        {
            textBox1.ReadOnly = false; //man får skriva i texboxen
            label4.Text = "";
            usedLetters.Clear();
            _allTries = 0; //sätter värde på en variabel som 0 
            _missTries = 0;
            checkWord.Enabled = true;
            UpdateUI();
        }

        private bool IsWin() //win metod som sen visar MessageBox med meddelande i if-satsen att man gissade rätt orden 
        {
            return !_currentWordShow.Contains('_'); //om variabel inte innehåller undertecken då är det win
        }

        private void UpdateUI() //uppdaterar label
        {
            label2.Text = "Antal försök kvar: " + (NUMBEROFTRIES - _missTries); //räknar antal försök
            if (_missTries == NUMBEROFTRIES) //om fel gissade försök är lika med fem
            {
                checkWord.Enabled = false; //man stänger ner knappen
                textBox1.ReadOnly = true;
            }
            label1.Text = new string(_currentWordShow); //visal ord med undertecken i labeln
        }

        private void ReplaceChar(char c) //metod för att byta undertecken till tecken som finns i orden
        {
            for (var i = 0; i < _currentWordShow.Length; i++) //går igenom ord med undertecken
            {
                if (_currentWord[i] == c) //om tecken i orden är samma som i texboxen
                {
                    _currentWordShow[i] = c; //ändrar undertecken till rätt tecken 
                }
            }
        }
    }
}
