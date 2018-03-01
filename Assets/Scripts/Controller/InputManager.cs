using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FunnyPoker.Extensions;
using FunnyPoker.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyPoker.Scripts.Controller
{
    public class InputManager : MonoBehaviour
    {
        public GameObject panel;
        private InputField inputBid;
        private Button btnBid;
        private bool isButtonClicked;
        private GameObject gameObjectTextInvalid;
        private GameObject gameObjectTextBid;
        private Game _game;
        private CardsFigureComparer _comparer;

        private void Start()
        {
            var c = FindObjectOfType<Canvas>();
            _game = FindObjectOfType<Game>();
            panel = Instantiate(Resources.Load("Prefabs\\BidInput")) as GameObject;
            panel.transform.SetParent(c.transform, false);
            inputBid = GameObject.Find("inputBid").GetComponent<InputField>();
            btnBid = GameObject.Find("btnEnterBid").GetComponent<Button>();
            gameObjectTextInvalid = GameObject.Find("txtInvalid");
            gameObjectTextInvalid.SetActive(false);
            gameObjectTextBid = GameObject.Find("txtBid");
            panel.SetActive(false);
            btnBid.onClick.AddListener(() => isButtonClicked = true);
        }

        /// <summary>
        /// Activates input window and waits for input field to be filled and enter button to be pressed.
        /// When button is pressed checkes validity of input. If input is valid closes panel and increments
        /// games current player number (I blame coroutines for games ref in this class....). 
        /// Else waits for proper input.
        /// </summary>
        public IEnumerator OpenInputWindowAndProcessInputData(StringBuilder input)
        {

            if (panel.activeSelf == false)
            {
                panel.SetActive(true);
                if(gameObjectTextInvalid.activeSelf == true)
                {
                    gameObjectTextInvalid.SetActive(false);
                    gameObjectTextBid.SetActive(true);
                }
            }

            // Waits for button to be clicked
            do
            {
                yield return null;
            } while (!isButtonClicked);

            input.Clear();
            input.Append(inputBid.text);

            if(!IsProperCardsFigure(input))
            {
                gameObjectTextBid.SetActive(false);
                gameObjectTextInvalid.SetActive(true);
                isButtonClicked = false;
                StartCoroutine(OpenInputWindowAndProcessInputData(input));
            }
            else
            {
                isButtonClicked = false;
                panel.SetActive(false);
                _game.IsCallBidExecuted = true;
            }
        }

        /// <summary>
        /// Checks validity of input first by proper data format (input contains '-') except "RAMPAMPAMPARADAM" easteregg.
        /// Then checks if input string can be turned into proper Cards figure. If so sets games current bid and previous
        /// bid to proper values and increments current player variable.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsProperCardsFigure(StringBuilder input)
        {
            var str = input.ToString().ToUpper();
            _comparer = FindObjectOfType<CardsFigureComparer>();

            if (!str.Contains("-"))
            {
                if (str != "RAMPAMPAMPARADAM")
                {
                    return false;
                }
            }

            var cardsFigure = InputToCardsFigure(str);

            if(cardsFigure == null)
            {
                return false;
            }

            if(_comparer.Compare(cardsFigure, _game.CurrentBid) <= 0)
            {
                return false;
            }

            _game.PreviousBid = _game.CurrentBid;
            _game.CurrentBid = cardsFigure;
            return true;
        }

        private CardsFigure InputToCardsFigure(string str)
        {

            if (str == "RAMPAMPAMPARADAM")
            {
                return new CardsFigure(new Card(Symbol.Clubs, Value.Queen), new Card(Symbol.Spades, Value.Queen));
            }
            
            CardsFigure cf;
            string[] splitInput = str.Split('-');
            var firstPart = splitInput[0];
            var secPart = splitInput[1];

            // Disgusting
            switch (firstPart.Length)
            {
                case 1:
                    switch (firstPart[0])
                    {
                        case 'P':
                            if (secPart.Length != 1) return null; // Proper ex. P-2
                            var c1a = secPart.ToCard();
                            var c1b = secPart.ToCard();
                            c1a.Symbol = Symbol.Diamonds;
                            cf = new CardsFigure(c1a, c1b);
                            break;
                        case 'H':
                            if (secPart.Length != 1) return null; // Proper H-A
                            var c2a = secPart.ToCard();
                            cf = new CardsFigure(c2a);
                            break;
                        case 'T':
                            if (secPart.Length != 1) return null; // Proper ex. T-4
                            var c3a = secPart.ToCard();
                            var c3b = secPart.ToCard();
                            var c3c = secPart.ToCard();
                            c3b.Symbol = Symbol.Diamonds;
                            c3c.Symbol = Symbol.Hearts;
                            cf = new CardsFigure(c3a, c3b, c3c);
                            break;
                        case 'F':
                            if (secPart.Length != 1) return null; // Proper ex. F-Q
                            var c4a = secPart.ToCard();
                            var c4b = secPart.ToCard();
                            var c4c = secPart.ToCard();
                            var c4d = secPart.ToCard();
                            c4b.Symbol = Symbol.Diamonds;
                            c4c.Symbol = Symbol.Hearts;
                            c4d.Symbol = Symbol.Spades;
                            cf = new CardsFigure(c4a, c4b, c4c, c4d);
                            break;
                        case 'S': // TODO: Probably change sth
                            if (secPart.Length != 1) return null; // Proper ex. S-A
                            var startingCard = secPart.ToCard();
                            if (startingCard.Value == Value.Jack || startingCard.Value == Value.Queen || startingCard.Value == Value.King) return null;
                            var straightCards = new List<Card>();

                            straightCards.Add(startingCard);
                            Card nextCard;
                            var nextVal = startingCard.Value + 1;

                            switch (startingCard.Value)
                            {
                                case Value.Ace:
                                    nextCard = new Card(Symbol.Spades, Value.Two);
                                    nextVal = Value.Two;
                                    break;
                                default:
                                    nextCard = new Card(Symbol.Spades, nextVal);
                                    break;
                            }
                            straightCards.Add(nextCard);

                            for (int i = 0; i < 3; i++)
                            {
                                nextVal++;
                                nextCard = new Card(Symbol.Hearts, nextVal);
                                straightCards.Add(nextCard);
                            }
                            cf = new CardsFigure(straightCards.ToArray());
                            break;
                        default:
                            return null;
                    }
                    break;
                case 2:
                    switch (firstPart)
                    {
                        case "FH":
                            if (secPart.Length != 2) return null; // Proper ex. FH-AK
                            var c1aA = secPart[0].ToString().ToCard();
                            var c1aB = secPart[0].ToString().ToCard();
                            var c1aC = secPart[0].ToString().ToCard();
                            c1aB.Symbol = Symbol.Diamonds;
                            c1aC.Symbol = Symbol.Hearts;
                            var c1bA = secPart[1].ToString().ToCard();
                            var c1bB = secPart[1].ToString().ToCard();
                            c1bB.Symbol = Symbol.Diamonds;
                            cf = new CardsFigure(c1aA, c1aB, c1aC, c1bA, c1bB);
                            break;
                        case "SF":
                            if (secPart.Length != 2) return null; // Proper ex. SF-D2
                            // TODO later, nobody calls it anyway Kappa
                            cf = new CardsFigure();
                            break;
                        case "TP":
                            if (secPart.Length != 2) return null; // Proper ex. TP-2A
                            var c3aA = secPart[0].ToString().ToCard();
                            var c3aB = secPart[0].ToString().ToCard();
                            c3aA.Symbol = Symbol.Diamonds;
                            c3aB.Symbol = Symbol.Hearts;
                            var c3bA = secPart[1].ToString().ToCard();
                            var c3bB = secPart[1].ToString().ToCard();
                            c3bA.Symbol = Symbol.Diamonds;
                            c3bB.Symbol = Symbol.Hearts;
                            cf = new CardsFigure(c3aA, c3aA, c3bB, c3bB);
                            break;
                        case "FL":
                            if (secPart.Length != 1) return null; // Proper ex. FL-D
                            cf = new CardsFigure();
                            cf.Type = HandType.Flush;
                            switch(secPart[0])
                            {
                                case 'D':
                                    cf.FlushSymbol = Symbol.Diamonds;
                                    break;
                                case 'H':
                                    cf.FlushSymbol = Symbol.Hearts;
                                    break;
                                case 'S':
                                    cf.FlushSymbol = Symbol.Spades;
                                    break;
                                case 'C':
                                    cf.FlushSymbol = Symbol.Clubs;
                                    break;
                                default:
                                    return null;
                            }
                            break;
                        default:
                            return null;
                    }
                    break;
                default:
                    return null;
            }
            return cf;

        }
    }
}