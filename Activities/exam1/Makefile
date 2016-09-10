all : wordcount.exe binary.exe taylor

wordcount.exe : question1.cs
	mcs -out:wordcount.exe question1.cs

binary.exe : question2.cs
	mcs -out:binary.exe question2.cs

taylor : question3.f
	f77 -o taylor question3.f