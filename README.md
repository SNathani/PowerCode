# PowerCode
An intuitive code specification for generating source code.

Type what really matters to declare members. The following is a simple **PowerCode** specification to generate class code.

## Example 1 - Classes
```
c Person, .name
Id, Name, DateOfBirth, p5 UserDefinedField
m GetAge:int
```
The code generated for the above **PowerCode** specification is as below:

```cs
public class Person
{
    public Person(string name)
    {
    }

    public int Id { get; set; }

    public string Name { get; set; }

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
i Repository<T>:IEntity<T>, GetAll:IEnumerable<T>, Get:T id:int
m Add:T, item:T
m Update:T, item:T
m Delete ,id:int
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
s Point, X:int, Y:int, m Clone:Point
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
e Color, Red=0, Blue, Green, Yellow=10
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
