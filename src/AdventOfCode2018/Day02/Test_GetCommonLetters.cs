﻿using FluentAssertions;
using Xunit;
using static AoC18.Day02.Part2;

namespace AoC18.Day02
{
    public class GetCommonLetters
    {
        [Fact]
        public void Returns_empty_for_two_different_strings()
        {
            var result = GetCommonLetters("abcde", "fghij");

            result.Should().Be(string.Empty);
        }

        [Fact]
        public void Returns_common_letters_for_strings_differing_in_some_letters()
        {
            var result = GetCommonLetters("abcde", "aBcdE");

            result.Should().Be("acd");
        }
    }
}