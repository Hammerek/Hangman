using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hangman
{
    public class Hangman
    {
        private string[] _words = new string[10] //array
        {
            "hentai", "tangentbord", "yuri", "oppai", "loli", "gothic", "programming", "witcher", "anime", "radio"
        };

        private List<char> usedLetters = new List<char>(); //listan för bokstaver som man skrev i textboxen
        private Random _random = new Random(); //gör nytt random klass
        private char[] _currentWord; //ord som man gissar
        private char[] _currentWordShow; //ord som man visar i labeln
        private int _allTries = 0; //alla försök
        private int _missTries = 0; //missade försök
        private const int NUMBEROFTRIES = 5;
        private MainPage mainPage;

        public Hangman(MainPage mainPage)
        {
            this.mainPage = mainPage;
        }

        public int LeftTries()
        {
            return NUMBEROFTRIES - _missTries;
        }

        public void Clear()
        {
            usedLetters.Clear();
            _allTries = 0; //sätter värde på en variabel som 0 
            _missTries = 0;
        }

        public bool IsGameOver()
        {
            return _missTries == NUMBEROFTRIES;
        }

        internal string HiddenWord()
        {
            return new string(_currentWordShow);
        }

        public void GenerateNewWord()
        {
            var index = _random.Next(0, _words.Length - 1); //väljer en random ord från arrayen mellan 0 och words.Length
            _currentWord = _words[index].ToCharArray(); //en random ord konverterad till Char
            _currentWordShow = new char[_currentWord.Length]; //gör en ny char array till variabeln som man visar
            for (var i = 0; i < _currentWord.Length; i++) //för varje tecken i currentWord
            {
                _currentWordShow[i] = '_';  //sätter undertecken i chararrayen
            }
        }

        public void CheckLetters(string txt)
        {
            foreach (var c in txt) //för varje tecken i textboxen
            {
                _allTries++; //ökar alla försök
                if (usedLetters.Contains(c)) //om listan innehåller tecken från texboxen
                {
                    continue; //då kör man programmet vidare
                }
                //man lägger till bokstaven från textboxen till listan, sen konventerar man det till string för att returnera det som en liten bokstav
                usedLetters.Add(c.ToString().ToLower().ToCharArray()[0]); //sen konverterar man det igen till CharArray
                mainPage.ShowUsedLetters(new string(usedLetters.ToArray())); //man skriver ut bokstäver till labeln
                if (_currentWord.Contains(c)) //om ord innehåller tecken från textbox
                {
                    ReplaceChar(c); //man gör replacement på _currentWordShow = byter '_' för rätt tecken i textboxen
                }
                else
                {
                    _missTries++; //ökar antalet försök
                }
                mainPage.UpdateUI(); //metod för Update
                if (IsWin()) //om metoden är true
                {
                    mainPage.MarkWin();
                }
                if (_missTries >= Hangman.NUMBEROFTRIES) //om man hade gissat fel 5 gånger 
                {
                    break; //avslutar foreach
                }
            }
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
        private bool IsWin() //win metod som sen visar MessageBox med meddelande i if-satsen att man gissade rätt orden 
        {
            return !_currentWordShow.Contains('_'); //om variabel inte innehåller undertecken då är det win
        }

    }
}
