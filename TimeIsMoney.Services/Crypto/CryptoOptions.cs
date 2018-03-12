﻿namespace TimeIsMoney.Services.Crypto
{
    public sealed class CryptoOptions
    {
        public int DerivedKeyIterations { get; set; }
        public int DerivedKeySizeBits { get; set; }
        public int SaltSizeBits { get; set; }
    }
}