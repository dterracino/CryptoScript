# CryptoScript
Script language with crypto function in .NET

# Example

```
// comment here
$password = TremendousTiger
$$sha256hash = sha256($password)
print Hash of "TremendousTiger": 
println $sha256hash
println overwriting file "hash.txt" with hash...
write $sha256hash hash.txt -nl
$$ionhash = sha256(ionix)
print hash of "ionix": 
println $ionhash
println appending hash to "hash.txt"
append $ionhash hash.txt -nl
$$crc32hash = crc32(HelloWorld)
print crc32 hash of "HelloWorld": 
// crc stuff
println $crc32hash
println appending crc32 hash to "hash.txt"
append $crc32hash hash.txt
println Press any key to close...
read
```
