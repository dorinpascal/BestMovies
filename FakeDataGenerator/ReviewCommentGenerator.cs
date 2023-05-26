using Bogus;
using Bogus.Distributions.Gaussian;

namespace FakeDataGenerator;

public static class ReviewCommentGenerator
{
    public static string? Generate(Faker faker, int rating)
    {
        if (Math.Abs(faker.Random.GaussianDecimal(0, 1)) > 1.7m)
        {
            // if outside of 1.7∂ -> set comment as null
            return null;
        }
        
        var reviewDescription = rating switch
        {
            < 3 => faker.PickRandom(NegativeDescriptions),
            3 => faker.PickRandom(NeutralDescriptions),
            > 3 => faker.PickRandom(PositiveDescriptions)
        };
        
        var phrase = rating switch
        {
            < 3 => faker.PickRandom(NegativePhrases),
            3 => faker.PickRandom(NeutralPhrases),
            > 3 => faker.PickRandom(PositivePhrases)
        };
           
        return string.Format(phrase, reviewDescription);
      
    }
    
    private static readonly string[] PositivePhrases = {
        "I absolutely loved it!",
        "The acting was superb.",
        "The plot was {0} and kept me engaged throughout.",
        "The cinematography was {0}.",
        "The character development was {0}.",
        "The movie had a {0} balance of action and emotion.",
        "I would highly recommend it to anyone.",
        "The story was {0} and touched my heart.",
        "The performances were {0} and brought the characters to life.",
        "The visuals were {0} and created a stunning atmosphere.",
        "The soundtrack was {0} and added depth to the movie.",
        "The dialogue was {0} and filled with memorable lines."
    };
    
    private static readonly string[] NeutralPhrases = {
        "It was an {0} movie.",
        "The movie had its ups and downs.",
        "The overall experience was {0}.",
        "It didn't leave a strong impression on me. It was {0}",
        "It was {0}.",
        "It had some enjoyable moments."
    };
    
    private static readonly string[] NegativePhrases = {
        "I was disappointed by it.",
        "The acting felt {0} and unnatural.",
        "The plot was {0} and hard to follow.",
        "The cinematography was {0}.",
        "The characters were {0} developed.",
        "The movie lacked {0} and substance.",
        "I would not recommend it to others.",
        "The story was {0} and failed to captivate me.",
        "The performances were {0} and unconvincing.",
        "The visuals were {0} and didn't enhance the experience.",
        "The pacing was {0} and made the movie drag.",
        "The ending was {0} and left me unsatisfied."
    };
        
    private static readonly string[] PositiveDescriptions = {
        "captivating",
        "intriguing",
        "mesmerizing",
        "brilliantly executed",
        "visually stunning",
        "emotionally gripping",
        "thought-provoking",
        "engaging",
        "inspiring",
        "riveting",
        "masterfully crafted",
        "heartwarming"
    };
    
    private static readonly string[] NeutralDescriptions = {
        "decent",
        "average",
        "balanced",
        "adequate",
        "moderate",
        "passable",
        "unremarkable",
        "indifferent"
    };
        
    private static readonly string[] NegativeDescriptions = {
        "disappointing",
        "underwhelming",
        "confusing",
        "mediocre",
        "lackluster",
        "poorly developed",
        "uninspiring",
        "forgettable",
        "clichéd",
        "tedious",
        "contrived",
        "unremarkable"
    };
}