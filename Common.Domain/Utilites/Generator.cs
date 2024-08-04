
using System;
using System.Security.Cryptography;

namespace Common.Domain.Utilites
{
    public class Generator<TId>
    {
        private static RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();

        /// <summary>
        /// This method will generate AutoId For int,long,string,Guid types
        /// </summary>
        /// <exception cref="InvalidOperationException">will throw exception on passing other types</exception>
        public static TId GenerateId()
        {

            byte[] buffer;
            var result = default(TId);

            if (typeof(TId) == typeof(int) ||
                typeof(TId) == typeof(long))
            {
                buffer = new byte[5];
                randomNumberGenerator.GetBytes(buffer);

                if (typeof(TId) == typeof(int))
                    result = (TId)Convert.ChangeType(BitConverter.ToInt32(buffer), typeof(TId));
                else
                    result = (TId)Convert.ChangeType(BitConverter.ToInt32(buffer), typeof(TId));

            }
            else if (typeof(TId) == typeof(string))
            {
                buffer = new byte[8];
                randomNumberGenerator.GetBytes(buffer);

                result = (TId)Convert.ChangeType(BitConverter.ToString(buffer), typeof(TId));
            }
            else if (typeof(TId) == typeof(Guid))
            {
                result = (TId)Convert.ChangeType(Guid.NewGuid(), typeof(TId));
            }
            else
                throw new InvalidOperationException($"Can not create Identity with current Type type : ({typeof(TId)})");

            return result;
        }
    }
}
