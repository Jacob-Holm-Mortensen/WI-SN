using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork
{
    class Sentiment
    {
        List<string> vocabolary = new List<string>() { "cute", "house", "fat", "is", "small", "with", "a", "tree", "cat", "ogre" };
        List<Review> reviews = new List<Review>() { new Review(Review.sentiment.veryPositive, "cute small house cat"),
                                                    new Review(Review.sentiment.negative, "fat house cat"),
                                                    new Review(Review.sentiment.neutral, "a small tree"),
                                                    new Review(Review.sentiment.veryNegative, "a fat ogre"),
                                                    new Review(Review.sentiment.positive, "small tree house"),
                                                    new Review(Review.sentiment.positive, "a small cat"),
                                                    new Review(Review.sentiment.neutral, "house cat"),
                                                    new Review(Review.sentiment.negative, "house is small"),
                                                    new Review(Review.sentiment.negative, "ogre with a cat"),
                                                    new Review(Review.sentiment.positive, "house with a tree"),
                                                    new Review(Review.sentiment.veryNegative, "small ogre house"),
                                                    new Review(Review.sentiment.neutral, "a tree"),
                                                    new Review(Review.sentiment.neutral, "a cat"),
                                                    new Review(Review.sentiment.neutral, "small ogre"),
                                                    new Review(Review.sentiment.negative, "ogre is a cat"),
                                                    new Review(Review.sentiment.positive, "cute small ogre"),
                                                    new Review(Review.sentiment.veryPositive, "small tree cat"),
                                                    new Review(Review.sentiment.positive, "house with small tree"),
                                                    new Review(Review.sentiment.veryNegative, "tree cat is fat"),
                                                    new Review(Review.sentiment.veryNegative, "fat cat ogre") };
        public void calculateSentiment()
        {

        }
    }
}
