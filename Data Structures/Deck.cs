using System;
using System.Collections.Generic;

public class Deck<T>
{
    private LinkedList<T> _cards;


    public Deck()
    {
        _cards = new LinkedList<T>();
    }
    public Deck(LinkedList<T> cards)
    {
        _cards = cards;
    }
    public Deck(params T[] cards)
    {
        _cards = new LinkedList<T>(cards);
    }

    public LinkedList<T> Cards { get { return _cards; } }
    public int Size { get { return _cards.Count; } }

    public List<T> ToList()
    {
        return new List<T>(_cards);
    }
    // return the number of instances of removals,
    // if strict is true then only remove if the number of instances are at least the amount specified
    public int Remove(T cardToRemove, int amount=1, bool strict=false)
    {
        int counter = 0;
        if (strict)
        {
            // only remove from deck if deck contains at least the amount specified otherwise do nothing
            List<T> deckList = this.ToList();
            foreach(T card in deckList)
            {
                if (cardToRemove.Equals(card))
                {
                    counter++;
                }
            }
            if (counter >= amount)
            {
                // Remove the specified amount from the list
                return this.Remove(cardToRemove, amount, strict: false);
            }
            else
            {
                return 0;
            }
        }
        else
        {
            // remove specified amount of instances of card and return the number of instances removed be it 0 or otherwise
            for (int i = 0; i < amount; i++)
            {
                if (_cards.Contains(cardToRemove))
                {
                    counter++;
                    _cards.Remove(cardToRemove);
                }
            }
            return counter;
        }
    }
    public void Add(params T[] cards)
    {
        foreach(T card in cards)
        {
            _cards.AddLast(card);
        }
    }
    public T[] Draw(int numCards=1)
    {
        if (_cards.Count > 0)
        {
            T[] cardArray = new T[numCards];
            for(int i = 0; i < numCards; i++)
            {
                cardArray[i] = _cards.First.Value;
                _cards.RemoveFirst();
            }
            return cardArray;
        }
        return null;
    }
    public void Shuffle(int? seed=null)
    {
        Random random;
        if (seed == null)
        {
            random = new Random();
        }
        else
        {
            random = new Random(seed.Value);
        }
        LinkedList<T> shuffled = new LinkedList<T>();
        List<T> deckList = this.ToList();
        for (int i = 0; i < _cards.Count; i++)
        {
            int index = random.Next(0, _cards.Count);
            shuffled.AddLast(deckList[index]);
        }
        _cards = shuffled;
    }
}
