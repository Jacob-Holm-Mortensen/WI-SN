using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork
{
    class Person
    {
        public string name = "";
        public List<Person> friends = new List<Person>();
        List<string> summaries = new List<string>();
        List<string> reviews = new List<string>();
        int sentimentalScore = 0;
        bool purchaseDecision = false;

        public Person(string _name, List<string> _summaries, List<string> _reviews)
        {
            name = _name;
            summaries = _summaries;
            reviews = _reviews;
        }

        public void AddFriends(List<Person> _friends)
        {
            foreach (var friend in _friends)
            {
                if (!friends.Contains(friend))
                    AddFriend(friend);
                if (!friend.friends.Contains(this))
                    friend.AddFriend(this);
            }
        }

        public void AddFriend(Person _friend)
        {
            friends.Add(_friend);
        }
    }
}
