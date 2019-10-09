# GOST.Core
GOST 28147-89 cipher. Шифр ГОСТ 28147-89.

# What is it?
GOST 28147-89 (Magma) - is a Russian government standard symmetric key block cipher.
GOST has a 64-bit block size and a key length of 256 bits.

**Supports only netcore3.**

# Usage
XOR encoding/decoding:
```cs
var encoded = Xor.Encode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);
var decoded = Xor.Decode("12345678901234567890123456789012", "12345678", "message", SBlockTypes.GOST);
```

All modes:
* Substitution cipher (only 64 bit blocks).
* XOR cipher.
* CFB cipher (XOR with feedback mode).
* MAC generator (for check authenticity of message).

Warning: substitution cipher supports only 64 bit blocks. MAC is not cipher.
