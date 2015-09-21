# GoofyCoin2015
Centralized digital currency

GoofyCoin
Is about the simplest cryptocurrency we can imagine. There
are just two rules of GoofyCoin. The first rule is that a designated entity, Goofy, can create new coins
whenever he wants and these newly created coins belong to him.
To create a coin, Goofy generates a unique coin ID uniqueCoinID that he’s never generated before
and constructs the string “CreateCoin [uniqueCoinID]”. He then computes the digital signature of
this string with his secret signing key. The string, together with Goofy’s signature, is a coin. Anyone
can verify that the coin contains Goofy’s valid signature of a CreateCoin statement, and is therefore a
valid coin.
The second rule of GoofyCoin is that whoever owns a coin can transfer it on to someone else.
Transferring a coin is not simply a matter of sending the coin data structure to the recipient — it’s
done using cryptographic operations.
Let’s say Goofy wants to transfer a coin that he created to Alice. To do this he creates a new
statement that says “Pay this to Alice” where “this” is a hash pointer that references the coin in
question. And as we saw earlier, identities are really just public keys, so “Alice” refers to Alice’s public
key. Finally, Goofy signs the string representing the statement. Since Goofy is the one who originally
owned that coin, he has to sign any transaction that spends the coin. Once this data structure
representing Goofy’s transaction signed by him exists, Alice owns the coin. She can prove to anyone
that she owns the coin, because she can present the data structure with Goofy’s valid signature.
Furthermore, it points to a valid coin that was owned by Goofy. So the validity and ownership of coins
are self-evident in the system.

![Alt text](https://github.com/vinils/GoofyCoin2015/blob/master/GoofyCoin.jpg "Figure 1.10")

Figure 1.10 GoofyCoin coin. Shown here is a coin that’s been created (bottom) and spent twice
(middle and top).
Once Alice owns the coin, she can spend it in turn. To do this she creates a statement that says, “Pay
this coin to Bob’s public key” where “this” is a hash pointer to the coin that was owned by her. And of
course, Alice signs this statement. Anyone, when presented with this coin, can verify that Bob is the
owner. They would follow the chain of hash pointers back to the coin’s creation and verify that at
each step, the rightful owner signed a statement that says “pay this coin to [new owner]”.
To summarize, the rules of GoofyCoin are:

● Goofy can create new coins by simply signing a statement that he’s making a new coin with a
unique coin ID.

● Whoever owns a coin can pass it on to someone else by signing a statement that saying, “Pass
on this coin to X” (where X is specified as a public key)

● Anyone can verify the validity of a coin by following the chain of hash pointers back to its
creation by Goofy, verifying all of the signatures along the way.

Of course, there’s a fundamental security problem with GoofyCoin. Let’s say Alice passed her coin on
to Bob by sending her signed statement to Bob but didn’t tell anyone else. She could create another
signed statement that pays the very same coin to Chuck. To Chuck, it would appear that it is perfectly
valid transaction, and now he’s the owner of the coin. Bob and Chuck would both have valid-looking
claims to be the owner of this coin. This is called a double-spending attack — Alice is spending the
same coin twice. Intuitively, we know coins are not supposed to work that way.
In fact, double-spending attacks are one of the key problems that any cryptocurrency has to solve.
GoofyCoin does not solve the double-spending attack and therefore it’s not secure. GoofyCoin is
simnple, and its mechanism for transferring coins is actually very similar to Bitcoin, but because it it is
insecure it won’t cut it as a cryptocurrency.
