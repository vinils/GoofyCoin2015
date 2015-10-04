//-----------------------------------------------------------------------
// <copyright file="Global.cs" company="VLS">
//     Copyright (c) VLS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GoofyCoin2015
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;

    /// <summary>
    /// Global properties and functions
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Hash Algorithm
        /// </summary>
        public static readonly CngAlgorithm HashAlgorithm = CngAlgorithm.Sha256;

        /// <summary>
        /// Goofy public key
        /// </summary>
        private static byte[] goofyPk;

        /// <summary>
        /// Gets or sets goofy public key
        /// </summary>
        public static byte[] GoofyPk
        {
            get { return goofyPk; }
            set { goofyPk = value; }
        }

        /// <summary>
        /// Serialize a transfer
        /// </summary>
        /// <param name="trans">transfer instance</param>
        /// <returns>Serialized transfer</returns>
        public static byte[] SerializeObject(TransferInfo trans)
        {
            return SerializeObject((object)trans);
        }

        /// <summary>
        /// Serialize a coin
        /// </summary>
        /// <param name="coin">Coin instance</param>
        /// <returns>Serialized coin</returns>
        public static byte[] SerializeObject(int coin)
        {
            return SerializeObject((object)coin);
        }

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Serialized object</returns>
        private static byte[] SerializeObject(object obj)
        {
            byte[] ret;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ret = ms.ToArray();
            }

            return ret;
        }

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <param name="byts">Serialized object</param>
        /// <returns>deserialized object</returns>
        private static object DeserializeObject(byte[] byts)
        {
            object ret;

            using (var memStream = new System.IO.MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(byts, 0, byts.Length);
                memStream.Seek(0, System.IO.SeekOrigin.Begin);
                ret = binForm.Deserialize(memStream);
            }

            return ret;
        }
    }
}
