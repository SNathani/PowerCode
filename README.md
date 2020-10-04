# PowerCode
An intuitive code specification for generating source code.

Type what really matters to declare members. The following is a simple **PowerCode** specification for code generation.

## The Specification
The PowerCode specification must be wrapped in a multiline comment in order to work in Visual Studio. Press CTRL+(dot) to invoke refactoring actions and select "Execute from PowerCode" to generate code inserted after the multiline comment.

It's ok to have comment lines begin with an astrisk (*), it will be ignored.

Each line must start with a type specifier to tell the PowerCode parser what kind of tokens are expected next.

|Specifier | Description | Example  |
|:---------:|--------------|-----------|
|c             | class            |   `c Person, .name`             |
|i              | inteface       |   `i IRepository<T>, Add:T item:T, Update:T item:T, Delete id:int`|
|s             | struct          |  `s Point, X:int, Y:int, m Clone:Point`     |
|e            | enum         |    `e Gender, Female=0, Male`      |
|p[n]            | property  | `p Id, Name,DateOfBirth`, This is optional. `n` represents the number of properties to produce with the same name by appending the index. i.e. `p2 UserDefined` will generate 2 UserDefined properties `public string UserDefined0 {get;set;}` and `public string UserDefined1 {get;set;}` . See the examples below to get a better idea.           |
|m           | method    |    `m Add:int, a:int, b:int`         |

### Contextual symbols
|Symbol   | Description |
|:---------:|--------------|
| comma   |  method, property, argument seperator                  |
| pipe        | CRLF. Line separator used to represent the specification in a single line              |
| space      | initial separator to separate type specifier and the template body. Also used to separate method and arguments                   |
| colon      | separates identifier and type                 |
| dot / period    | denotes constructor arguments for class `c`   |


#### NOTE: 
- `p` is optional for class and struct. 
- `m` is optional for interface. For interface, methods are separated by comma and the method arguments are separated by space.
- Any line without a type specifier is considered as `p`. You can simply add comma separated `<Identifier>[:<type>]` to generate properties.
- Properties `p` and methods `m` on separate lines will be added to the nearest class, struct or interface.
- Enum `e` should be represented in a single line.


---
# Examples
## Example 1 - Classes
```
/*
c Person, .name
Id, Name, DateOfBirth, p5 UserDefinedField
m GetAge:int
*/
```
The code generated for the above **PowerCode** specification is as below:

```cs
public class Person
{
    public Person(string name)
    {
    }
    //type inferred automatically
    public int Id { get; set; }

    public string Name { get; set; }

    //type inferred automatically
    public DateTime DateOfBirth { get; set; }

    public string UserDefinedField0 { get; set; }

    public string UserDefinedField1 { get; set; }

    public string UserDefinedField2 { get; set; }

    public string UserDefinedField3 { get; set; }

    public string UserDefinedField4 { get; set; }

    public string UserDefinedField5 { get; set; }

    public int GetAge()
    {
        return default;
    }
}
```

## Example 2 - Interfaces
```
/*
i Repository<T>:IEntity<T>, GetAll:IEnumerable<T>, Get:T id:int
m Add:T, item:T
m Update:T, item:T
m Delete ,id:int
*/
```

... converted to

```cs
public interface Repository<T> : IEntity<T>
{
    T Add(T item);
    T Update(T item);
    void Delete(int id);
    IEnumerable<T> GetAll();
    T Get(int id);
}
```

## Example 3 - Structures

```
/*
s Point, X:int, Y:int, m Clone:Point
*/
```

... converted to

```cs
public struct Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point Clone()
    {
        return default;
    }
}
```

## Example 4 - Enums

```
/*
e Color, Red=0, Blue, Green, Yellow=10
*/
```

... converted to

```cs
public enum Color
{
    Red = 0,
    Blue,
    Green,
    Yellow = 10
}
```

NOTE:
You can also combine all the example specification together and generate the code at once.

---
# NuGet Extension
You can install the NuGet extension to generate code for all your coding purposes by using *C# Interactive* or *LinqPad*.

Install the extension:
```
Install-Package PowerCode.CodeGeneration.Extensions -Version 1.0.0
``` 

# Visual Studio Extension
You can execute the PowerCode specification directly from within Visual Studio. This extension installs a PowerCode refactoring.

Press CTRL+. in a multi-comment and you will get an option **"Execute from PowerCode"** refactoring context menu.

Install the editor extension from the Visual Studio *Extensions* dialog and search for **PowerCode.CodeGeneration.EditorExtensions**