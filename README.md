# lavoro-collaborativo-be
Backend of a collaborative work system with text chat.
Based on 

## Prerequisites

## Getting started


## How to create a comment

1. Add `///` before a Class, Method, Property, Function... and a c# style comment (or better a XML doucumentation comment) will be generate from Visual Studio 2017 Enterprise
2. The kind of tags available see the [c# style comment] https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/xml-documentation-comments

## How to generate the Documentation

0. We use `Sandcastle NuGet package` to generate the documentation.
1. Open the project, the file `lavoro-collaborativo.sln` with Visual Studio 2017 Enterprise.
2. Right click on the project `lavoro-collaborativo` then `Properties` and open the tab `Build`. On the section `Output` then `XML documentation file` must be checked.
3. Right click on the solution `lavoro-collaborativo` then `Properties`, select `Configuration Properties` -> `Configuration` and uncheck the coloumn `Build` for the project `Documentation`.
4. From the Visual Studio menu build the solution from `Build` -> `Rebuild solution`, so the xml file about the documentation is generated
5. Right click on the project `Documentation` then `Build`, so into the folder `lavoro-collaborativo\Documentation\Help` we will find finally the complete Docuemntation.
6. You can see the documentation by open the index.html
7. To use the search you must insert the folder `lavoro-collaborativo\Documentation\Help` on a web server like `Apache` or `IIS`. For example for a quick view we use XAMPP.




# WApiChat
# WApiChat
