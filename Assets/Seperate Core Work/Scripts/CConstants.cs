using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts.TherapyItemRecSys
//{
static class CConstants
{
    public enum g_LinguisticCategory
    {
        noun_1syll_word, noun_1syll_phrase, noun_2syll_word, noun_2syll_phrase, noun_3syll_word, noun_3syll_phrase,
        noun_attributive, noun_predicative, noun_intransitive, noun_intransitivePPbeg, noun_intransitivePPend,
        noun_transitive_beg, noun_transitive_end, noun_transitivePP, noun_ditransitive, prep_phrase, prep_sentence, prn_possessive_sentences, prn_possessive, prn_personal,
        tense_simple, tense_progressive, verb_intransitive, verb_intransitivePP, verb_transitive, verb_ditransitive, verb_transitivePP, adj_phrase, adj_sentence, adj_intrans_sent
    };

    public enum g_LinguisticType { Word, EasySentence, HardSentence };

    // noise1 = -5db, noise2 = 0db, noise3 = 5db, noise4 = 10db, noise5 = 15db
    public enum g_NoiseLevel { NoNoise, PhoneVoice, Noise1, Noise2, Noise3, Noise4, Noise5 };
    //public enum g_NoiseLevel { NoNoise };

    public const int g_intItemNumPerBlock = 15;
    public const int g_intEasyItemNumPerBlock = 13;
    public const int g_intDifficultItemNumPerBlock = 2;

    public const int g_intItemMaxExposureCtr = 100;
    public const int g_intItemComplexityDecrementFactor = 20;

    public const int g_intBlockNumPerWindow = 50;

    public const double g_dUserAccuracyThresholdMax = 0.7;
    public const double g_dUserAccuracyThresholdMin = 0.4;
    public const double g_dUserAccuracyFactor = 0.05;

    public const int g_intBlockNumBefNoiseStart = 50;
    public const int g_intNoiseBlockInterval = 5; // every five blocks should have one noise block
    public const int g_intNoiseBlockNumPerWindow = 5;
}
//}
