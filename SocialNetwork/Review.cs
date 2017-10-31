using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork
{
    class Review
    {
        public enum sentiment
        {
            veryNegative, negative, neutral, positive, veryPositive
        }

        sentiment pos;
        string review;

        public Review(sentiment _pos, string _review)
        {
            pos = _pos;
            review = _review;
        }
    }
}
