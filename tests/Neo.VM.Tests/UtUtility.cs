// Copyright (C) 2015-2024 The Neo Project.
//
// UtUtility.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.VM;
using System;
using System.Numerics;

namespace Neo.Test
{
    [TestClass]
    public class UtUtility
    {
        [TestMethod]
        public void SqrtTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => BigInteger.MinusOne.Sqrt());

            Assert.AreEqual(BigInteger.Zero, BigInteger.Zero.Sqrt());
            Assert.AreEqual(new BigInteger(1), new BigInteger(1).Sqrt());
            Assert.AreEqual(new BigInteger(1), new BigInteger(2).Sqrt());
            Assert.AreEqual(new BigInteger(1), new BigInteger(3).Sqrt());
            Assert.AreEqual(new BigInteger(2), new BigInteger(4).Sqrt());
            Assert.AreEqual(new BigInteger(9), new BigInteger(81).Sqrt());
        }

        private static byte[] GetRandomByteArray(Random random, bool isSmall)
        {
            byte[] value;
            int byteValue;

            if (isSmall)
            {
                byteValue = random.Next(0, 32);
                value = new byte[byteValue];
            }
            else
            {
                byteValue = random.Next(32, 128);
                value = new byte[byteValue];
            }

            random.NextBytes(value);
            return value;
        }

        private void VerifyGetBitLength(BigInteger value, long expected)
        {
            Assert.AreEqual(expected, value.GetBitLength());
            Assert.AreEqual(expected, Utility.GetBitLength(value));
        }

        [TestMethod]
        public void GetBitLengthTest()
        {
            var random = new Random();

            // Trivial cases
            //                     sign bit|shortest two's complement
            //                              string w/o sign bit
            VerifyGetBitLength(0, 0);  // 0|
            VerifyGetBitLength(1, 1);  // 0|1
            VerifyGetBitLength(-1, 0); // 1|
            VerifyGetBitLength(2, 2);  // 0|10
            VerifyGetBitLength(-2, 1); // 1|0
            VerifyGetBitLength(3, 2);  // 0|11
            VerifyGetBitLength(-3, 2); // 1|01
            VerifyGetBitLength(4, 3);  // 0|100
            VerifyGetBitLength(-4, 2); // 1|00
            VerifyGetBitLength(5, 3);  // 0|101
            VerifyGetBitLength(-5, 3); // 1|011
            VerifyGetBitLength(6, 3);  // 0|110
            VerifyGetBitLength(-6, 3); // 1|010
            VerifyGetBitLength(7, 3);  // 0|111
            VerifyGetBitLength(-7, 3); // 1|001
            VerifyGetBitLength(8, 4);  // 0|1000
            VerifyGetBitLength(-8, 3); // 1|000

            // Random cases
            for (uint i = 0; i < 1000; i++)
            {
                var bi = new BigInteger(GetRandomByteArray(random, true));
                Assert.AreEqual(bi.GetBitLength(), Utility.GetBitLength(bi), message: $"Error comparing: {bi}");
                bi = new BigInteger(GetRandomByteArray(random, false));
                Assert.AreEqual(bi.GetBitLength(), Utility.GetBitLength(bi), message: $"Error comparing: {bi}");
            }

            foreach (var bi in new[] { BigInteger.Zero, BigInteger.One, BigInteger.MinusOne, new BigInteger(ulong.MaxValue), new BigInteger(long.MinValue) })
            {
                Assert.AreEqual(bi.GetBitLength(), Utility.GetBitLength(bi), message: $"Error comparing: {bi}");
            }
        }

        [TestMethod]
        public void ModInverseTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigInteger.One.ModInverse(BigInteger.Zero));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigInteger.One.ModInverse(BigInteger.One));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigInteger.Zero.ModInverse(BigInteger.Zero));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigInteger.Zero.ModInverse(BigInteger.One));
            Assert.ThrowsException<InvalidOperationException>(() => new BigInteger(ushort.MaxValue).ModInverse(byte.MaxValue));

            Assert.AreEqual(new BigInteger(52), new BigInteger(19).ModInverse(141));
        }
    }
}
