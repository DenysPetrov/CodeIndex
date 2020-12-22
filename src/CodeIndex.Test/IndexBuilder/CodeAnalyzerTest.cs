﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CodeIndex.IndexBuilder;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;
using NUnit.Framework;

namespace CodeIndex.Test
{
    [ExcludeFromCodeCoverage]
    public class CodeAnalyzerTest
    {
        [Test]
        public void TestAnalyzer()
        {
            var content = " LucenePool.SaveResultsAndClearLucenePool(TempIndexDir);";
            var result = GetTokens(new CodeAnalyzer(Constants.AppLuceneVersion, false), content);
            CollectionAssert.AreEquivalent(new[]
            {
                "LucenePool",
                ".",
                "SaveResultsAndClearLucenePool",
                "(",
                "TempIndexDir",
                ")",
                ";"
            }, result);

            result = GetTokens(new CodeAnalyzer(Constants.AppLuceneVersion, true), content);
            CollectionAssert.AreEquivalent(new[]
            {
                "lucenepool",
                ".",
                "saveresultsandclearlucenepool",
                "(",
                "tempindexdir",
                ")",
                ";"
            }, result);

            result = GetTokens(new CodeAnalyzer(Constants.AppLuceneVersion, false), @"Line One
Line Two

Line Four");

            CollectionAssert.AreEquivalent(new[]
            {
                "Line",
                "One",
                "Line",
                "Two",
                "Line",
                "Four"
            }, result);
        }

        [Test]
        public void TestGetWords()
        {
            var content = "It's a content for test" + Environment.NewLine + "这是一个例句,我知道了";
            CollectionAssert.AreEquivalent(new[] { "It", "s", "a", "content", "for", "test", "这是一个例句", "我知道了" }, WordSegmenter.GetWords(content));
            CollectionAssert.AreEquivalent(new[] { "It", "for", "test", "我知道了" }, WordSegmenter.GetWords(content, 2, 4));
            CollectionAssert.IsEmpty(WordSegmenter.GetWords("a".PadRight(201, 'b')));

            Assert.Throws<ArgumentException>(() => WordSegmenter.GetWords(null));
            Assert.Throws<ArgumentException>(() => WordSegmenter.GetWords(content, 0));
            Assert.Throws<ArgumentException>(() => WordSegmenter.GetWords(content, 200));
            Assert.Throws<ArgumentException>(() => WordSegmenter.GetWords(content, 3, 1));
            Assert.Throws<ArgumentException>(() => WordSegmenter.GetWords(content, 3, -1));
            Assert.Throws<ArgumentException>(() => WordSegmenter.GetWords(content, 3, 1001));
        }

        List<string> GetTokens(Analyzer analyzer, string content)
        {
            var tokens = new List<string>();
            var tokenStream = analyzer.GetTokenStream("A", content);
            var termAttr = tokenStream.GetAttribute<ICharTermAttribute>();
            tokenStream.Reset();

            while (tokenStream.IncrementToken())
            {
                tokens.Add(termAttr.ToString());
            }

            return tokens;
        }
    }
}
