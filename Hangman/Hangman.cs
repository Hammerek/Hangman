using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangman
{
    public class Hangman
    {
        private const int NUMBEROFTRIES = 5;
        private readonly MainPage _mainPage;
        private readonly Random _random = new Random(); //gör nytt random klass
        private readonly List<char> _usedLetters = new List<char>(); //listan för bokstaver som man skrev i textboxen

        private readonly string[] _words = new string[10] //array
        {
            "hentai", "tangentbord", "yuri", "oppai", "loli", "gothic", "programming", "witcher", "anime", "radio"
        };

        private char[] _currentWord; //ord som man gissar
        private char[] _currentWordShow; //ord som man visar i labeln        
        private int _missTries; //missade försök

        public Hangman(MainPage mainPage)
        {
            _mainPage = mainPage;
        }

        private bool IsWin => !_currentWordShow.Contains('_');
        public int LeftTries => NUMBEROFTRIES - _missTries;
        public bool IsGameOver => _missTries == NUMBEROFTRIES;
        public string HiddenWord => new string(_currentWordShow);

        public void Clear()
        {
            _usedLetters.Clear();
            _missTries = 0;
        }

        public void GenerateNewWord()
        {
            var index = _random.Next(0, _words.Length - 1);
                //väljer en random ord från arrayen mellan 0 och words.Length
            _currentWord = _words[index].ToCharArray(); //en random ord konverterad till Char
            _currentWordShow = new char[_currentWord.Length]; //gör en ny char array till variabeln som man visar
            for (var i = 0; i < _currentWord.Length; i++) //för varje tecken i currentWord
            {
                _currentWordShow[i] = '_'; //sätter undertecken i chararrayen
            }
        }

        public void CheckLetters(string txt)
        {
            foreach (var c in txt) //för varje tecken i textboxen
            {
                if (_usedLetters.Contains(c)) //om listan innehåller tecken från texboxen
                {
                    continue; //då kör man programmet vidare
                }
                //man lägger till bokstaven från textboxen till listan, sen konventerar man det till string för att returnera det som en liten bokstav
                _usedLetters.Add(c.ToString().ToLower().ToCharArray()[0]); //sen konverterar man det igen till CharArray
                _mainPage.ShowUsedLetters(new string(_usedLetters.ToArray())); //man skriver ut bokstäver till labeln
                if (_currentWord.Contains(c)) //om ord innehåller tecken från textbox
                {
                    ReplaceChar(c); //man gör replacement på _currentWordShow = byter '_' för rätt tecken i textboxen
                }
                else
                {
                    _missTries++; //ökar antalet försök
                }
                _mainPage.UpdateUI(); //metod för Update
                if (IsWin) //om metoden är true
                {
                    _mainPage.MarkWin();
                }
                if (_missTries >= NUMBEROFTRIES) //om man hade gissat fel 5 gånger 
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
    }
}