﻿using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AoC18.Day03
{
    public class Test_UnionClaim
    {
        [Theory, AutoData]
        public void Is_idempotent(Claim claim)
        {
            var union = new UnionClaim(claim);

            union.Should().BeEquivalentTo(claim);
        }

        [Theory, AutoData]
        public void Has_minimal_x_offset(Claim[] claims)
        {
            new UnionClaim(claims)
                .XOffset
                .Should().Be(
                    claims.Select(c => c.XOffset).Min());
        }

        [Theory, AutoData]
        public void Has_minimal_y_offset(Claim[] claims)
        {
            new UnionClaim(claims)
                .YOffset
                .Should().Be(
                    claims.Select(c => c.YOffset).Min());
        }

        [Theory, AutoData]
        public void Has_maximum_width(Claim[] claims)
        {
            new UnionClaim(claims)
                .Width
                .Should().Be(
                    claims.Select(c => c.Width).Max());
        }

        [Theory, AutoData]
        public void Has_maximum_height(Claim[] claims)
        {
            new UnionClaim(claims)
                .Height
                .Should().Be(
                    claims.Select(c => c.Height).Max());
        }

        [Theory, AutoData]
        public void Is_the_same_as_composed_claims(
            Claim[] claims,
            int x1, int y1,
            int x2, int y2,
            int x3, int y3)
        {
            x1 = x1.OrHorizontallyInboundOf(claims[0]);
            y1 = y1.OrVerticallyInboundOf(claims[0]);

            x2 = x1.OrHorizontallyInboundOf(claims[1]);
            y2 = y1.OrVerticallyInboundOf(claims[1]);

            var union = new UnionClaim(claims);

            using (new AssertionScope())
            {
                union[x1, y1].Should().Be(
                    claims.Select(c => c[x1, y1]).Sum());
                union[x2, y2].Should().Be(
                    claims.Select(c => c[x2, y2]).Sum());
                union[x3, y3].Should().Be(
                    claims.Select(c => c[x3, y3]).Sum());
            }
        }

        /*   0  1  2  3  4  5
         *   C1 A1 B1 C2 A2 B2
         *      A--------*     A1 0
         *      |  B-----+--*  B1 1
         *      |  |     |  |
         *   C--+--+--*  |  |  C1 2
         *   |  |  |X |  |  |
         *   |  |  *--+--+--B  B2 3
         *   |  |     |  |
         *   |  |     |  |
         *   *--+-----C  |     C2 4
         *      *--------A     A2 5
         */
        [Theory, AutoData]
        public void Is_the_sum_of_composed_claims(IFixture fixture)
        {
            var xs = fixture.CreateMany<int>(6).OrderBy(i => i).ToArray();
            var ys = fixture.CreateMany<int>(6).OrderBy(i => i).ToArray();

            var a1 = (xs[1], ys[0]);
            var a2 = (xs[4], ys[5]);

            var b1 = (xs[2], ys[1]);
            var b2 = (xs[5], ys[3]);

            var c1 = (xs[0], ys[2]);
            var c2 = (xs[3], ys[4]);

            var claimA = new Claim(a1, a2);
        }
    }
}