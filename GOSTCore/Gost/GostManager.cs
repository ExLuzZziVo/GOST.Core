using System;
using System.Collections.Generic;
using System.Linq;
using GOSTCore.Core.Ciphers;
using GOSTCore.Core.Generators.Mac;
using GOSTCore.Core.SBlocks.IBlock;
using GOSTCore.Gost.Factories;
using GOSTCore.Gost.Types;

namespace GOSTCore.Gost
{
    internal class GostManager : IDisposable
    {
        /// <summary>
        /// 256 bit key;
        /// </summary>
        private byte[] _key;

        /// <summary>
        /// Message.
        /// </summary>
        private byte[] _message;

        /// <summary>
        /// 64 bit IV.
        /// </summary>
        private byte[] _iv;

        /// <summary>
        /// Subkeys.
        /// </summary>
        private List<uint> _subKeys;

        private ISBlock _sBlock;
        private ICipher _cipher;

        /// <summary>
        /// Check key length.
        /// </summary>
        /// <exception cref="ArgumentException">Key must have 256 bit length.</exception>
        private byte[] Key
        {
            set
            {
                if (value.Length != 32)
                {
                    throw new ArgumentException("Wrong key. Try to use 256 bit key.");
                }
                else if (value.Length == 32)
                {
                    _key = value;
                }
            }
        }

        /// <summary>
        /// Check message.
        /// </summary>
        /// <exception cref="ArgumentException">Empty message.</exception>
        private byte[] Message
        {
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentException("Empty message!");
                }
                else
                {
                    _message = value;
                }
            }
        }

        /// <summary>
        /// Check IV.
        /// </summary>
        /// <exception cref="ArgumentException">IV must have 256 bit length.</exception>
        private byte[] IV
        {
            set
            {
                if (value is null)
                {
                    _iv = null;
                }

                else if (value.Length != 8)
                {
                    throw new ArgumentException("Wrong IV. Try to use 64 bit IV.");
                }
                else if (value.Length == 8)
                {
                    _iv = value;
                }
            }
        }

        /// <summary>
        /// IDisposable flag;
        /// </summary>
        private bool released;

        public GostManager(byte[] key, byte[] iv, byte[] message, CipherTypes cipherType = CipherTypes.Substitution,
            SBlockTypes sBlockType = SBlockTypes.GOST)
        {
            released = false;
            _subKeys = new List<uint>();

            Key = key;
            IV = iv;
            Message = message;

            GenerateSubKeys();

            _sBlock = SBlockFactory.SBlock(sBlockType);
            _cipher = CipherFactory.Cipher(_iv, _subKeys, cipherType, _sBlock);
        }

        /// <summary>
        /// Encode process.
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            var res = new byte[_message.Length];

            var i = -1;

            foreach (var item in ReadByChunk())
            {
                Array.Copy(_cipher.Encode(item, _subKeys), 0, res, i + 1, item.Length);

                i += item.Length;
            }

            return res;
        }

        /// <summary>
        /// Decode process.
        /// </summary>
        /// <returns></returns>
        public byte[] Decode()
        {
            var res = new byte[_message.Length];

            var i = -1;

            foreach (var item in ReadByChunk())
            {
                Array.Copy(_cipher.Decode(item, _subKeys), 0, res, i + 1, item.Length);

                i += item.Length;
            }

            return res;
        }

        /// <summary>
        /// Mac generator.
        /// </summary>
        /// <returns></returns>
        public byte[] Generate()
        {
            var res = new byte[8];

            foreach (var item in ReadByChunk().Select((chunk, i) => new {i, chunk}))
            {
                res = new MacGenerator(_sBlock).Generate(item.chunk, _subKeys);
            }

            return res;
        }

        /// <summary>
        /// Generate subkeys.
        /// </summary>
        private void GenerateSubKeys()
        {
            var res = new byte[4];

            // Stage 1.
            var j = 0;

            for (var i = 0; i != _key.Length; i++)
            {
                res[j] = _key[i];

                if (j % 3 == 0 && j != 0)
                {
                    _subKeys.Add(BitConverter.ToUInt32(res, 0));
                    j = 0;
                }
                else
                {
                    j++;
                }
            }

            // Stage 2.
            for (var i = 0; i != 16; i++)
            {
                _subKeys.Add(_subKeys[i]);
            }

            // Stage 3.
            for (var i = 7; i != -1; i--)
            {
                _subKeys.Add(_subKeys[i]);
            }
        }

        /// <summary>
        /// Read message by chunks.
        /// </summary>
        /// <returns>At least 64 bit block.</returns>
        private IEnumerable<byte[]> ReadByChunk()
        {
            for (var i = 0; i < _message.Length; i += 8)
            {
                var min = Math.Min(8, _message.Length - i);

                var res = new byte[min];

                Array.Copy(_message, i, res, 0, min);

                yield return res;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!released)
            {
                if (disposing)
                {
                    _sBlock = null;
                    _cipher = null;
                    _iv = null;
                    _key = null;
                    _message = null;
                    _subKeys = null;
                }

                released = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}