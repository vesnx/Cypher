// ***********************************************************************
// Assembly         : Walter.Web.CypherTests
// Author           : Walter Verhoeven
// Created          : Fri 01-Mar-2024
//
// Last Modified By : Walter Verhoeven
// Last Modified On : Fri 01-Mar-2024
// ***********************************************************************
// <copyright file="PfxCertificateGenerator.cs" company="VESNX SA, Walter Verhoeven">
//     © 2019 - 2024 Walter Verhoeven, Lambert Snellinx, all rights reserved.
// </copyright>
// <summary>
// PFX certificate generator
// </summary>
// ***********************************************************************
using System;
using System.IO;
using Org.BouncyCastle.Math;

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;

internal class PfxCertificateGenerator
{
    public static void GeneratePfxCertificate(string outputPath, string password)
    {
        // Generate RSA key pair
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
        AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

        // Generate a self-signed certificate
        var generator = new X509V3CertificateGenerator();
        var issuerName = new X509Name("CN=Self-Signed Certificate");
        var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(long.MaxValue), new SecureRandom());
        generator.SetSerialNumber(serialNumber);
        generator.SetIssuerDN(issuerName);
        generator.SetNotBefore(DateTime.UtcNow);
        generator.SetNotAfter(DateTime.UtcNow.AddYears(1));
        generator.SetSubjectDN(issuerName); // Self-signed, so issuer and subject are the same
        generator.SetPublicKey(keyPair.Public);

        var signatureFactory = new Asn1SignatureFactory(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id, keyPair.Private);
        var certificate = generator.Generate(signatureFactory);

        // Create the PFX (PKCS#12) store
        var store = new Pkcs12StoreBuilder().Build();
        var certificateEntry = new X509CertificateEntry(certificate);
        store.SetCertificateEntry(issuerName + " Certificate", certificateEntry);
        store.SetKeyEntry(issuerName + " Key", new AsymmetricKeyEntry(keyPair.Private), new[] { certificateEntry });

        // Write the PFX to a file
        using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        {
            store.Save(fileStream, password.ToCharArray(), new SecureRandom());
        }

        Console.WriteLine($"PFX certificate generated at {outputPath}");
    }


}
