using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace WalmartApiClient
{
    class SignatureGenerator
    {
        public static string PRIVATE_KEY_VERSION = "4";
        public static string CONSUMER_ID_PROD = "fac77a69-e4e0-4642-a7cb-a266baee1f4e";
        public static string PRIVATE_KEY = "MIIEowIBAAKCAQEAtkHuPQnpxp8Pknd2uQ2/WnSwhWjcrBmB0Vw2NWvjxxK26ZwIEcKvuqZUBhbyJF3zSvCqoz1T32/S2rTnMO3B/9WfRKyR5XiaLgTaceSWdBpflzClyuUHFyCSAgYiziy9FZY4BbPHovoHUVnpORPGVa3evBvXYNCpbQka15NzDJfhG0pdBW/2Coojom+9iG5iaJlcuOA9VveNybNutUmgAP5pA9C1E5s0j+BaE7GjzjUes3hs2giUm/Fd3GY3rbnfHGZaYbYROG1DP5Al/Hs7HkMAaNk0TBg/reg4h2zQo8ciUsFGayFRTi21fhbiLcV1jAXc322hdRy8wA9Au7OFawIDAQABAoIBAEGmmCGGi54PN7XDsJFSIWZ8+ATmU+7uNTPr6l7t4RuZYyfdG48COPib51JWO5zb9sI81Mp+UdL/Xc2IMmsOib4U/gznuJoXCjqfZux1sbhas6orTN08CITNJ6rw+OKZfPbkqINtUfEj7TThlUAJHn1IZx4NTVTDdPQgvnl+1IdpJnvZnWe2wcaRfvI4Q2NbnV7JXVckBsfpmb01Q9kpFFARItu0gOSbayvo2fJUuEvi8ZHfu5npeTAy3DUiQPKJykQ8fisV85KUnR56YMNJ0h7zI5kXcuN8074z2GSYtjHQnZ9bAJ1q7JPYxpj9XNmq+NOV6hyCcbNXj1mL5MHRJzkCgYEA20mpvxJag+sCf7yRchMSVNpQILHqhECMTObP0+AkUxu6TnXiZFrgYixit415wdghpYgHA64jtVA+1+j6dXkHX3cURyTjZWrFadG1g5N78KSBMw6XRLQcN6sokyBGAY5s/O0eVIcie593WVF6m7yAaWkaxLJfGLMbBbRjHXwoFU0CgYEA1MU1rd2GDtRrcIrtH4LGHNH0/rrk9Wos/Vbu6izgW8Am9IlGWm0OJsDWditbpqiJgggLS9o8cDuHL2y6/yGMltooBxMuBA8Q7CUxNlRHRUMT2sZ1/Q1VRuKkrk0f0Q/r1W6QvNuTEI7vVLSCyv3FBpmL2k4VEi2qXjVa46vsSZcCgYA1uhmd1+hXc+f71f6ovWV7ZrXFHPJBjvMREGgRGFSuDBgciyN5xQ4+33McV8xiIwszXF2jmDHlhZCwI8PhrlKRIELWn4IbYgqWP6xCXYs7TYLeOI/1ZE+ZkBTz3y9nyDaJzarluOpI8awzNRRePdQUf4zwbmeATLmtLyItojLL7QKBgG3zl4DIqD1Dol3fPRh6kPxVt4BnfmggPS2R3hbfp/Vh//+AbP3Pu29lWa5pS4x9LcondAb3uSHacUqdqqirYqaWB/dX/wCT9I/DzIGh3c66EimNQ23IlRfziVyVeGqmCp4Z6+vcoEv/QUiJm8lgiI+Xi4D9tr/VH2RduCFNN+bzAoGBAJnZaHNgpQagxvjUgpQCVqsB7zvlpbasxsKZZw2YMWT+C/3NSX1+SbunCBXn9wFPkP4zEZZWKATeb75ezSjOsWzXMLTewOfC5E2TFZsr1OJQg4dV4GLW6GA05jFJaYqbAMwUh4WDN9houYKPAJ6mPn3GmUGJY1dl1CS6dq5w62hD";

        public static string CONSUMERIDKEY = "WM_CONSUMER.ID";
        public static string CONSUMERINTIMESTAMPKEY = "WM_CONSUMER.INTIMESTAMP";
        public static string PRIVATEKEYVERSIONKEY = "WM_SEC.KEY_VERSION";
        public static string AUTHSIGNATUREKEY = "WM_SEC.AUTH_SIGNATURE";

        public static string generateSignature(string timeStamp)
        {
            string signature = "";

            Hashtable headersToSign = new Hashtable();
            headersToSign.Add(CONSUMERIDKEY, CONSUMER_ID_PROD);
            headersToSign.Add(CONSUMERINTIMESTAMPKEY, timeStamp);
            headersToSign.Add(PRIVATEKEYVERSIONKEY, PRIVATE_KEY_VERSION);

            string[] canonicalizedStr = canonicalize(headersToSign);
            signature = generate(PRIVATE_KEY, canonicalizedStr[1]);
            return signature;
        }

        private static string[] canonicalize(Hashtable headersToSign)
        {
            string[] canonicalizedStr = new string[2] { "", "" };
            foreach (DictionaryEntry de in headersToSign)
            {
                canonicalizedStr[0] += (((string)(de.Key)).Trim() + ";");
                canonicalizedStr[1] += (((string)(de.Value)).Trim() + "\n");
            }
            return canonicalizedStr;
        }

        private static string generate(string privateKey, string stringToSign)
        {
            try
            {
                byte[] signature;
                RSAParameters RSAParam = DecodeRSAPrivateKeyToRSAParam(DecodeBase64(privateKey));
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(RSAParam);

                // Hash and sign the data. Pass a new instance of SHA256
                // to specify the hashing algorithm.
                signature = RSAalg.SignData(System.Text.Encoding.UTF8.GetBytes(stringToSign), SHA256.Create());
                return EncodeBase64(signature);
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }

        private static string EncodeBase64(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        private static byte[] DecodeBase64(string value)
        {
            return Convert.FromBase64String(value);
        }

        private static RSAParameters DecodeRSAPrivateKeyToRSAParam(byte[] privkey)
        {
            
            RSAParameters RSAparams = new RSAParameters();
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem); byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return RSAparams;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    return RSAparams;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return RSAparams;


                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                return RSAparams;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hello World");
                Console.WriteLine(e.ToString());
                return RSAparams;
            }
            finally { binr.Close(); }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02) return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current); return count;
        }

    }
}
